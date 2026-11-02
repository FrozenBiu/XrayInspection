@echo off
for /f "delims=" %%i in ('type C:\info.txt') do set WholeLine=%%i
set ImageVersion=%WholeLine:~-2%
if %ImageVersion% LSS 20 (
taskkill /f /im GUI_XRS.exe >nul 2>&1
echo Image version too low.
echo Min version: 20
echo Current version: %ImageVersion%
echo Application will restart...
timeout 5
del "Autostart.bat"
start "" "GUI_XRS.exe"
)
