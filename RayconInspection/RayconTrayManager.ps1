# ==============================================================================
# Raycon X-Ray Remote Inspection - System Tray Manager
# ==============================================================================
param(
    [switch]$NoBrowser
)

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $scriptDir

# Load Windows Forms & Drawing for System Tray
Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing

# Clean up any lingering processes on ports 5000 and 5173
Write-Host "Dang kiem tra va don dep tien trinh cu..." -ForegroundColor Cyan
Get-Process -Name "Raycon.Api" -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue

$logDir = Join-Path $scriptDir "logs"
if (-not (Test-Path $logDir)) {
    New-Item -ItemType Directory -Path $logDir -Force | Out-Null
}

$backendLog = Join-Path $logDir "backend.log"
$frontendLog = Join-Path $logDir "frontend.log"

# Global process variables
$global:backendProc = $null
$global:frontendProc = $null

function Start-BackendService {
    Write-Host "[1/3] Dang khoi dong Backend API (.NET x86)..." -ForegroundColor Yellow
    $backendDir = Join-Path $scriptDir "backend\src\Raycon.Api"
    
    $psi = New-Object System.Diagnostics.ProcessStartInfo
    $psi.FileName = "dotnet"
    $psi.Arguments = "run"
    $psi.WorkingDirectory = $backendDir
    $psi.UseShellExecute = $false
    $psi.CreateNoWindow = $true
    $psi.RedirectStandardOutput = $true
    $psi.RedirectStandardError = $true

    $global:backendProc = New-Object System.Diagnostics.Process
    $global:backendProc.StartInfo = $psi
    $global:backendProc.Start() | Out-Null

    # Async log redirection
    Register-ObjectEvent -InputObject $global:backendProc -EventName OutputDataReceived -Action {
        if ($EventArgs.Data) { Add-Content -Path $using:backendLog -Value $EventArgs.Data }
    } | Out-Null
    Register-ObjectEvent -InputObject $global:backendProc -EventName ErrorDataReceived -Action {
        if ($EventArgs.Data) { Add-Content -Path $using:backendLog -Value $EventArgs.Data }
    } | Out-Null
    $global:backendProc.BeginOutputReadLine()
    $global:backendProc.BeginErrorReadLine()

    # Wait until Backend responds on port 5000 before proceeding!
    Write-Host "[2/3] Dang cho Backend san sang tren port 5000..." -ForegroundColor Yellow
    $ready = $false
    $retries = 0
    while (-not $ready -and $retries -lt 40) {
        Start-Sleep -Milliseconds 500
        $retries++
        try {
            $resp = Invoke-WebRequest -Uri "http://localhost:5000/api/status" -UseBasicParsing -TimeoutSec 1 -ErrorAction Stop
            if ($resp.StatusCode -eq 200) {
                $ready = $true
            }
        }
        catch {
            # Still waiting
        }
    }

    if ($ready) {
        Write-Host "Backend API da san sang (Port 5000)!" -ForegroundColor Green
    } else {
        Write-Host "Canh bao: Backend khoi dong cham, tiep tuc bat Frontend..." -ForegroundColor DarkYellow
    }
}

function Start-FrontendService {
    Write-Host "[3/3] Dang khoi dong Frontend UI (Vite)..." -ForegroundColor Yellow
    $frontendDir = Join-Path $scriptDir "frontend"

    $psi = New-Object System.Diagnostics.ProcessStartInfo
    $psi.FileName = "cmd.exe"
    $psi.Arguments = "/c pnpm dev"
    $psi.WorkingDirectory = $frontendDir
    $psi.UseShellExecute = $false
    $psi.CreateNoWindow = $true
    $psi.RedirectStandardOutput = $true
    $psi.RedirectStandardError = $true

    $global:frontendProc = New-Object System.Diagnostics.Process
    $global:frontendProc.StartInfo = $psi
    $global:frontendProc.Start() | Out-Null

    Register-ObjectEvent -InputObject $global:frontendProc -EventName OutputDataReceived -Action {
        if ($EventArgs.Data) { Add-Content -Path $using:frontendLog -Value $EventArgs.Data }
    } | Out-Null
    Register-ObjectEvent -InputObject $global:frontendProc -EventName ErrorDataReceived -Action {
        if ($EventArgs.Data) { Add-Content -Path $using:frontendLog -Value $EventArgs.Data }
    } | Out-Null
    $global:frontendProc.BeginOutputReadLine()
    $global:frontendProc.BeginErrorReadLine()

    Write-Host "Frontend da san sang (Port 5173)!" -ForegroundColor Green
}

function Stop-AllServices {
    Write-Host "Dang dung toan bo tien trinh..." -ForegroundColor Red
    try {
        if ($global:backendProc -and -not $global:backendProc.HasExited) {
            $global:backendProc.Kill()
        }
        if ($global:frontendProc -and -not $global:frontendProc.HasExited) {
            $global:frontendProc.Kill()
        }
        # Clean up any child node / dotnet processes
        Get-Process -Name "Raycon.Api" -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue
    }
    catch { }
}

# 1. Start Services sequentially
Start-BackendService
Start-FrontendService

# 2. Open browser
if (-not $NoBrowser) {
    Start-Sleep -Seconds 1
    Start-Process "http://localhost:5173"
}

# 3. Setup System Tray Icon (NotifyIcon)
$notify = New-Object System.Windows.Forms.NotifyIcon
$notify.Icon = [System.Drawing.SystemIcons]::Shield
$notify.Text = "Raycon X-Ray Inspection System"
$notify.Visible = $true

# Context Menu
$contextMenu = New-Object System.Windows.Forms.ContextMenuStrip

$menuOpen = New-Object System.Windows.Forms.ToolStripMenuItem
$menuOpen.Text = "Mo Dashboard (http://localhost:5173)"
$menuOpen.Font = New-Object System.Drawing.Font($menuOpen.Font, [System.Drawing.FontStyle]::Bold)
$menuOpen.Add_Click({
    Start-Process "http://localhost:5173"
})

$menuLogs = New-Object System.Windows.Forms.ToolStripMenuItem
$menuLogs.Text = "Xem Nhat Ky He Thong (Logs)"
$menuLogs.Add_Click({
    if (Test-Path $logDir) {
        Start-Process "explorer.exe" $logDir
    }
})

$menuRestart = New-Object System.Windows.Forms.ToolStripMenuItem
$menuRestart.Text = "Khoi Dong Lai Dich Vu"
$menuRestart.Add_Click({
    Stop-AllServices
    Start-Sleep -Seconds 1
    Start-BackendService
    Start-FrontendService
    $notify.ShowBalloonTip(2000, "Raycon Inspection", "Da khoi dong lai toan bo dich vu!", [System.Windows.Forms.ToolTipIcon]::Info)
})

$menuSep = New-Object System.Windows.Forms.ToolStripSeparator

$menuExit = New-Object System.Windows.Forms.ToolStripMenuItem
$menuExit.Text = "Thoat Hoan Toan"
$menuExit.Add_Click({
    Stop-AllServices
    $notify.Visible = $false
    $notify.Dispose()
    [System.Windows.Forms.Application]::Exit()
})

$contextMenu.Items.Add($menuOpen) | Out-Null
$contextMenu.Items.Add($menuLogs) | Out-Null
$contextMenu.Items.Add($menuRestart) | Out-Null
$contextMenu.Items.Add($menuSep) | Out-Null
$contextMenu.Items.Add($menuExit) | Out-Null

$notify.ContextMenuStrip = $contextMenu

# Double click tray icon opens Dashboard
$notify.Add_DoubleClick({
    Start-Process "http://localhost:5173"
})

# Show balloon tip
$notify.ShowBalloonTip(3000, "Raycon X-Ray Inspection", "He thong dang chay ngam trong khay he thong (Tray)! Nhap vao icon de mo Dashboard.", [System.Windows.Forms.ToolTipIcon]::Info)

Write-Host "`n========================================================" -ForegroundColor Cyan
Write-Host " HE THONG DANG CHAY NGAM TRONG KHAY HE THONG (SYSTEM TRAY)!" -ForegroundColor Green
Write-Host " - Ban co the dong cua so nay hoac de ngam o goc dong ho." -ForegroundColor White
Write-Host " - Nhap chuot phai vao icon hinh khien de tuy chon hoac Thoat." -ForegroundColor White
Write-Host "========================================================`n" -ForegroundColor Cyan

# Start message loop for Tray Icon
[System.Windows.Forms.Application]::Run()

