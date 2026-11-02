import React, { useState, useEffect } from 'react'
import type { MachineStatus } from '../types/inspection'
import { 
  Settings as SettingsIcon, Bell, 
  RefreshCw, Save, CheckCircle2, AlertCircle, 
  FolderSearch, Sparkles, Server, HelpCircle
} from 'lucide-react'
import { toast } from 'sonner'

interface SettingsPageProps {
  status: MachineStatus | null
  isConnected: boolean
  onConfigUpdated: () => void
}

export const SettingsPage: React.FC<SettingsPageProps> = ({
  status,
  isConnected,
  onConfigUpdated,
}) => {
  // Config inputs
  const [watchDirectory, setWatchDirectory] = useState('')
  const [machineName, setMachineName] = useState('')
  const [machineId, setMachineId] = useState('')

  // State
  const [isTesting, setIsTesting] = useState(false)
  const [isSaving, setIsSaving] = useState(false)
  const [isRescanning, setIsRescanning] = useState(false)
  const [testResult, setTestResult] = useState<{ success: boolean; message: string } | null>(null)

  // Alert settings
  const [audioEnabled, setAudioEnabled] = useState(true)
  const [consecutiveThreshold, setConsecutiveThreshold] = useState(3)

  // Sync inputs with status when loaded
  useEffect(() => {
    if (status) {
      setWatchDirectory(status.watchDirectory || '')
      setMachineName(status.machineName || '')
      setMachineId(status.machineId || '')
    }
  }, [status])

  // Test directory connection
  const handleTestConnection = async () => {
    if (!watchDirectory.trim()) {
      toast.error('Vui lòng nhập đường dẫn thư mục!')
      return
    }

    setIsTesting(true)
    setTestResult(null)

    try {
      const res = await fetch('/api/config/test', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          watchDirectory: watchDirectory.trim(),
          machineName,
          machineId,
        }),
      })

      const data = await res.json()
      setIsTesting(false)
      setTestResult(data)

      if (data.success) {
        toast.success('Kết nối thành công!', {
          description: data.message,
        })
      } else {
        toast.error('Kiểm tra thất bại!', {
          description: data.message,
        })
      }
    } catch (err: any) {
      setIsTesting(false)
      const errorMsg = 'Không thể gửi yêu cầu kiểm tra tới máy chủ.'
      setTestResult({ success: false, message: errorMsg })
      toast.error(errorMsg)
    }
  }

  // Save new configuration to backend and persist to appsettings.json
  const handleSaveConfig = async () => {
    if (!watchDirectory.trim()) {
      toast.error('Đường dẫn thư mục không được để trống!')
      return
    }

    setIsSaving(true)
    try {
      const res = await fetch('/api/config', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          watchDirectory: watchDirectory.trim(),
          machineName: machineName.trim() || 'Raycon EX2',
          machineId: machineId.trim() || 'RAYCON_EX2',
        }),
      })

      const data = await res.json()
      setIsSaving(false)

      if (res.ok && data.success) {
        toast.success('Cập nhật cấu hình thành công!', {
          description: `Đã nạp ${data.totalLoaded} ảnh từ thư mục mới. Cấu hình đã được lưu tự động!`,
        })
        onConfigUpdated()
      } else {
        toast.error('Lưu cấu hình thất bại!', {
          description: data.message || 'Vui lòng kiểm tra lại đường dẫn.',
        })
      }
    } catch (err: any) {
      setIsSaving(false)
      toast.error('Lỗi khi lưu cấu hình', {
        description: err.message,
      })
    }
  }

  // Rescan current directory
  const handleRescan = () => {
    setIsRescanning(true)
    fetch('/api/rejects')
      .then(() => {
        setTimeout(() => {
          setIsRescanning(false)
          toast.success('Đã đồng bộ lại toàn bộ danh mục ảnh từ máy X-Ray!')
          onConfigUpdated()
        }, 500)
      })
      .catch(() => {
        setIsRescanning(false)
        toast.error('Lỗi khi đồng bộ danh mục ảnh')
      })
  }

  // Apply quick preset path
  const applyPreset = (path: string, name: string, id: string) => {
    setWatchDirectory(path)
    setMachineName(name)
    setMachineId(id)
    setTestResult(null)
    toast.info(`Đã điền mẫu: ${name}`)
  }

  return (
    <div className="flex-1 overflow-y-auto p-6 space-y-6 bg-slate-950 max-w-4xl">
      <div className="pb-3 border-b border-slate-800 flex flex-col md:flex-row md:items-center justify-between gap-4">
        <div>
          <h2 className="text-2xl font-black tracking-tight text-white flex items-center gap-2.5">
            <SettingsIcon className="w-6 h-6 text-rose-500" />
            <span>Cài Đặt Hệ Thống & Cấu Hình Đường Dẫn</span>
          </h2>
          <p className="text-sm text-slate-400 mt-1">
            Điều chỉnh trực tiếp đường dẫn thư mục máy X-Ray và thiết lập cảnh báo mà không cần sửa file cấu hình
          </p>
        </div>

        <button
          type="button"
          onClick={handleRescan}
          disabled={isRescanning}
          className="flex items-center gap-2 px-4 py-2 rounded-xl bg-slate-900 border border-slate-700 hover:border-slate-600 text-slate-200 hover:text-white transition text-xs font-semibold self-start md:self-auto disabled:opacity-50"
        >
          <RefreshCw className={`w-4 h-4 ${isRescanning ? 'animate-spin text-sky-400' : ''}`} />
          <span>Quét lại thư mục</span>
        </button>
      </div>

      <div className="space-y-6 text-sm">
        {/* Main Section: Dynamic Path Configuration Form */}
        <div className="p-5 rounded-2xl bg-slate-900/70 border border-slate-800 space-y-4 shadow-lg shadow-black/20">
          <div className="flex items-center justify-between border-b border-slate-800 pb-3">
            <h3 className="text-sm font-bold text-slate-100 flex items-center gap-2">
              <FolderSearch className="w-4 h-4 text-rose-400" />
              <span>Cấu Hình Đường Dẫn Máy Soi Tia X (Watch Directory)</span>
            </h3>
            <span
              className={`px-2 py-0.5 rounded-full text-[10px] font-bold ${
                isConnected
                  ? 'bg-emerald-500/10 text-emerald-400 border border-emerald-500/20'
                  : 'bg-amber-500/10 text-amber-400 border border-amber-500/20'
              }`}
            >
              {isConnected ? 'REALTIME ACTIVE' : 'RECONNECTING'}
            </span>
          </div>

          {/* Form Fields */}
          <div className="space-y-3.5">
            {/* Watch Directory Input */}
            <div>
              <label className="block text-slate-300 font-semibold mb-1">
                Đường Dẫn Thư Mục Chứa Ảnh Reject (*.xri):
              </label>
              <div className="flex gap-2">
                <input
                  type="text"
                  value={watchDirectory}
                  onChange={(e) => {
                    setWatchDirectory(e.target.value)
                    setTestResult(null)
                  }}
                  placeholder="Ví dụ: \\192.168.1.150\RayconRejectImages hoặc E:\Project\Archive\Image\Reject"
                  className="flex-1 px-3.5 py-2.5 rounded-xl bg-slate-950 border border-slate-700/80 font-mono text-xs text-slate-200 placeholder-slate-600 focus:outline-none focus:border-rose-500 focus:ring-1 focus:ring-rose-500"
                />
                <button
                  type="button"
                  onClick={handleTestConnection}
                  disabled={isTesting}
                  className="px-3.5 py-2 rounded-xl bg-slate-800 hover:bg-slate-700 text-slate-200 font-medium transition flex items-center gap-1.5 shrink-0 disabled:opacity-50"
                >
                  <Server className="w-3.5 h-3.5 text-sky-400" />
                  <span>{isTesting ? 'Đang thử...' : 'Kiểm Tra Thử'}</span>
                </button>
              </div>
              <p className="text-[11px] text-slate-500 mt-1">
                Nhập đường dẫn ổ đĩa cục bộ (ví dụ <code className="text-slate-400 font-mono">D:\GUI_XRS\Archive\Image\Reject</code>) hoặc đường dẫn chia sẻ mạng SMB (<code className="text-slate-400 font-mono">\\192.168.1.150\RayconRejectImages</code>).
              </p>
            </div>

            {/* Test Connection Status Banner */}
            {testResult && (
              <div
                className={`p-3 rounded-xl border flex items-start gap-2.5 text-xs ${
                  testResult.success
                    ? 'bg-emerald-500/10 border-emerald-500/30 text-emerald-300'
                    : 'bg-rose-500/10 border-rose-500/30 text-rose-300'
                }`}
              >
                {testResult.success ? (
                  <CheckCircle2 className="w-4 h-4 text-emerald-400 shrink-0 mt-0.5" />
                ) : (
                  <AlertCircle className="w-4 h-4 text-rose-400 shrink-0 mt-0.5" />
                )}
                <div>
                  <div className="font-bold">{testResult.success ? 'Hợp lệ!' : 'Lỗi truy cập:'}</div>
                  <div className="text-[11px] opacity-90 mt-0.5">{testResult.message}</div>
                </div>
              </div>
            )}

            {/* Machine Name & ID */}
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-3 pt-1">
              <div>
                <label className="block text-slate-400 font-medium mb-1">Tên Dây Chuyền / Thiết Bị:</label>
                <input
                  type="text"
                  value={machineName}
                  onChange={(e) => setMachineName(e.target.value)}
                  placeholder="Ví dụ: Máy Soi Tia X Dây Chuyền 1"
                  className="w-full px-3 py-2 rounded-xl bg-slate-950 border border-slate-800 text-xs text-slate-200 placeholder-slate-600 focus:outline-none focus:border-rose-500/50"
                />
              </div>

              <div>
                <label className="block text-slate-400 font-medium mb-1">Mã Định Danh (Machine ID):</label>
                <input
                  type="text"
                  value={machineId}
                  onChange={(e) => setMachineId(e.target.value)}
                  placeholder="Ví dụ: RAYCON_EX2_01"
                  className="w-full px-3 py-2 rounded-xl bg-slate-950 border border-slate-800 font-mono text-xs text-slate-200 placeholder-slate-600 focus:outline-none focus:border-rose-500/50"
                />
              </div>
            </div>

            {/* Quick Presets */}
            <div className="pt-2 border-t border-slate-800/80">
              <div className="text-[11px] font-semibold text-slate-400 mb-2 flex items-center gap-1.5">
                <Sparkles className="w-3.5 h-3.5 text-amber-400" />
                <span>Mẫu cấu hình nhanh:</span>
              </div>
              <div className="flex flex-wrap gap-2">
                <button
                  type="button"
                  onClick={() =>
                    applyPreset(
                      'E:\\VisualStudioCode\\MyProjects\\RayconEX2\\Source\\GUI_XRS\\Archive\\Image\\Reject',
                      'Máy Soi Raycon EX2 (Thử Nghiệm Nội Bộ)',
                      'RAYCON_EX2_LOCAL'
                    )
                  }
                  className="px-2.5 py-1.5 rounded-lg bg-slate-950 hover:bg-slate-800 border border-slate-800 text-slate-300 text-[11px] transition"
                >
                  🏢 Thư mục mẫu nội bộ (50 ảnh)
                </button>

                <button
                  type="button"
                  onClick={() =>
                    applyPreset(
                      '\\\\192.168.1.150\\RayconRejectImages',
                      'Máy Soi Raycon EX2 - Dây Chuyền 1 (Mạng LAN)',
                      'RAYCON_EX2_LINE1'
                    )
                  }
                  className="px-2.5 py-1.5 rounded-lg bg-slate-950 hover:bg-slate-800 border border-slate-800 text-slate-300 text-[11px] transition"
                >
                  🌐 Đường dẫn chia sẻ LAN mẫu (\\\\192.168.1.150)
                </button>
              </div>
            </div>
          </div>

          {/* Action Button: Save & Apply */}
          <div className="pt-3 border-t border-slate-800 flex justify-end">
            <button
              onClick={handleSaveConfig}
              disabled={isSaving}
              className="flex items-center gap-2 px-5 py-2.5 rounded-xl bg-rose-500 hover:bg-rose-600 text-white font-bold transition shadow-lg shadow-rose-500/25 disabled:opacity-50 text-xs"
            >
              <Save className="w-4 h-4" />
              <span>{isSaving ? 'Đang áp dụng...' : 'Lưu & Áp Dụng Ngay'}</span>
            </button>
          </div>
        </div>

        {/* Realtime Alert Settings */}
        <div className="p-5 rounded-2xl bg-slate-900/60 border border-slate-800 space-y-4">
          <h3 className="text-sm font-bold text-slate-200 flex items-center gap-2">
            <Bell className="w-4 h-4 text-amber-400" />
            <span>Cấu Hình Âm Thanh & Báo Động</span>
          </h3>

          <div className="space-y-3">
            <div className="flex items-center justify-between p-3.5 rounded-xl bg-slate-950/80 border border-slate-800/80">
              <div>
                <div className="font-semibold text-slate-200">Âm thanh Thông báo khi có Lỗi (Beep Alert)</div>
                <div className="text-[11px] text-slate-500 mt-0.5">
                  Tự động phát chuông thông báo mỗi khi có sản phẩm dị vật rơi vào thùng phế phẩm
                </div>
              </div>
              <input
                type="checkbox"
                checked={audioEnabled}
                onChange={(e) => setAudioEnabled(e.target.checked)}
                className="w-5 h-5 accent-rose-500 rounded cursor-pointer"
              />
            </div>

            <div className="flex items-center justify-between p-3.5 rounded-xl bg-slate-950/80 border border-slate-800/80">
              <div>
                <div className="font-semibold text-slate-200">Ngưỡng Báo động Hàng Lỗi Liên Tiếp</div>
                <div className="text-[11px] text-slate-500 mt-0.5">
                  Bật còi báo động khẩn nếu phát hiện từ {consecutiveThreshold} sản phẩm lỗi liên tiếp trong vòng 1 phút
                </div>
              </div>
              <div className="flex items-center gap-2">
                <input
                  type="number"
                  min="2"
                  max="10"
                  value={consecutiveThreshold}
                  onChange={(e) => setConsecutiveThreshold(Number(e.target.value))}
                  className="w-16 px-2.5 py-1.5 rounded-lg bg-slate-900 border border-slate-700 text-center font-mono font-bold text-slate-100"
                />
                <span className="text-slate-400">gói</span>
              </div>
            </div>
          </div>
        </div>

        {/* Step-by-Step Instructions Card */}
        <div className="p-5 rounded-2xl bg-gradient-to-br from-slate-900/90 to-slate-950/80 border border-slate-800 space-y-3">
          <div className="flex items-center gap-2 text-xs font-bold text-sky-400">
            <HelpCircle className="w-4 h-4" />
            <span>Cách cấu hình sang máy X-Ray khác trong xưởng</span>
          </div>

          <div className="space-y-2 text-[11px] text-slate-400 leading-relaxed">
            <p>
              Để đổi sang theo dõi một máy soi Raycon khác trong dây chuyền, bạn thực hiện 3 bước đơn giản:
            </p>
            <ol className="list-decimal list-inside space-y-1.5 pl-1 text-slate-300">
              <li>
                <strong className="text-white">Lấy địa chỉ IP máy soi:</strong> Trên máy X-Ray, mở CMD và gõ lệnh <code className="text-rose-400 font-mono bg-slate-900 px-1 py-0.5 rounded">ipconfig</code> để xem địa chỉ IP (ví dụ: <code className="text-sky-300 font-mono">192.168.1.150</code>).
              </li>
              <li>
                <strong className="text-white">Chia sẻ thư mục ảnh lỗi:</strong> Trên máy X-Ray, nhấp chuột phải vào thư mục <code className="text-slate-200 font-mono">Archive\Image\Reject</code> chọn <strong className="text-white">Properties → Sharing → Advanced Sharing</strong>, bật chia sẻ với tên <code className="text-rose-400 font-mono">RayconRejectImages</code> và cấp quyền Read.
              </li>
              <li>
                <strong className="text-white">Nhập vào form bên trên:</strong> Nhập đường dẫn <code className="text-emerald-400 font-mono">\\192.168.1.150\RayconRejectImages</code> vào ô <em>Đường Dẫn Thư Mục</em>, bấm <strong className="text-sky-400">Kiểm Tra Thử</strong> rồi bấm <strong className="text-rose-400">Lưu & Áp Dụng Ngay</strong>.
              </li>
            </ol>
            <p className="text-slate-500 text-[10px] pt-1">
              * Hệ thống sẽ tự động chuyển bộ theo dõi và lưu đè cấu hình vào <code className="font-mono text-slate-400">backend/src/Raycon.Api/appsettings.json</code> mà bạn không cần phải mở file sửa thủ công.
            </p>
          </div>
        </div>
      </div>
    </div>
  )
}
