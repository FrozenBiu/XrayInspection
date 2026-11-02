import React from 'react'
import type { RejectItem, MachineStatus } from '../types/inspection'
import { 
  ShieldAlert, AlertTriangle, CheckCircle2, 
  Zap, Clock, ArrowRight, Activity, HardDrive 
} from 'lucide-react'

interface OverviewPageProps {
  rejects: RejectItem[]
  status: MachineStatus | null
  onOpenLiveInspect: (item: RejectItem) => void
}

export const OverviewPage: React.FC<OverviewPageProps> = ({
  rejects,
  status,
  onOpenLiveInspect,
}) => {
  const totalRejects = rejects.length
  const contaminants = rejects.filter((r) => r.isContaminant).length
  const defects = rejects.filter((r) => r.hasErrors && !r.isContaminant).length
  
  // Calculate average kV & mA
  const avgVoltage = totalRejects > 0 
    ? (rejects.reduce((acc, curr) => acc + curr.sourceVoltage, 0) / totalRejects).toFixed(1)
    : '0'
  const avgCurrent = totalRejects > 0
    ? (rejects.reduce((acc, curr) => acc + curr.sourceCurrent, 0) / totalRejects).toFixed(1)
    : '0'

  // Group by product name
  const productCounts = rejects.reduce((acc, curr) => {
    acc[curr.productName] = (acc[curr.productName] || 0) + 1
    return acc
  }, {} as Record<string, number>)

  const sortedProducts = Object.entries(productCounts)
    .sort(([, a], [, b]) => b - a)
    .slice(0, 5)

  // Recent 6 rejects
  const recentRejects = rejects.slice(0, 6)

  return (
    <div className="flex-1 overflow-y-auto p-6 space-y-6 bg-slate-950">
      {/* Page Title & Status Banner */}
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 pb-3 border-b border-slate-800">
        <div>
          <h2 className="text-2xl font-black tracking-tight text-white flex items-center gap-2.5">
            <span>Bảng Điều Khiển Giám Sát</span>
            <span className="text-xs px-2.5 py-0.5 rounded-full bg-slate-800 text-slate-300 font-semibold font-mono border border-slate-700">
              RAYCON EX2
            </span>
          </h2>
          <p className="text-sm text-slate-400 mt-1">
            Tổng quan tình trạng kiểm tra tạp chất và phân loại sản phẩm lỗi thời gian thực
          </p>
        </div>

        <div className="flex items-center gap-2 text-xs">
          <div className="px-3.5 py-2 rounded-xl bg-slate-900 border border-slate-800 text-slate-300 flex items-center gap-2">
            <Clock className="w-4 h-4 text-slate-400" />
            <span className="font-mono text-xs">Cập nhật: {new Date().toLocaleTimeString('vi-VN')}</span>
          </div>
        </div>
      </div>

      {/* KPI Stats Grid */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        {/* KPI 1: Total Rejects */}
        <div className="p-5 rounded-2xl bg-gradient-to-br from-slate-900 to-slate-950 border border-slate-800/90 relative overflow-hidden shadow-sm">
          <div className="flex items-center justify-between">
            <span className="text-sm font-semibold text-slate-300">Tổng Số Ảnh Lỗi</span>
            <div className="p-2.5 rounded-xl bg-rose-500/10 text-rose-400 border border-rose-500/20">
              <ShieldAlert className="w-5 h-5" />
            </div>
          </div>
          <div className="mt-3 flex items-baseline gap-2">
            <span className="text-4xl font-black font-mono text-white tracking-tight">{totalRejects}</span>
            <span className="text-xs text-rose-400 font-semibold">sản phẩm</span>
          </div>
          <p className="text-xs text-slate-400 mt-1.5">Đã lưu trữ trong bộ nhớ tạm</p>
        </div>

        {/* KPI 2: Contaminants */}
        <div className="p-5 rounded-2xl bg-gradient-to-br from-slate-900 to-slate-950 border border-slate-800/90 relative overflow-hidden shadow-sm">
          <div className="flex items-center justify-between">
            <span className="text-sm font-semibold text-slate-300">Dị Vật Ngoại Lai</span>
            <div className="p-2.5 rounded-xl bg-amber-500/10 text-amber-400 border border-amber-500/20">
              <AlertTriangle className="w-5 h-5" />
            </div>
          </div>
          <div className="mt-3 flex items-baseline gap-2">
            <span className="text-4xl font-black font-mono text-amber-400 tracking-tight">{contaminants}</span>
            <span className="text-xs text-amber-400/90 font-semibold">
              ({totalRejects > 0 ? Math.round((contaminants / totalRejects) * 100) : 0}%)
            </span>
          </div>
          <p className="text-xs text-slate-400 mt-1.5">Kim loại, Thủy tinh, Đá, Xương</p>
        </div>

        {/* KPI 3: Defects */}
        <div className="p-5 rounded-2xl bg-gradient-to-br from-slate-900 to-slate-950 border border-slate-800/90 relative overflow-hidden shadow-sm">
          <div className="flex items-center justify-between">
            <span className="text-sm font-semibold text-slate-300">Lỗi Khuyết Tật Bao Bì</span>
            <div className="p-2.5 rounded-xl bg-sky-500/10 text-sky-400 border border-sky-500/20">
              <CheckCircle2 className="w-5 h-5" />
            </div>
          </div>
          <div className="mt-3 flex items-baseline gap-2">
            <span className="text-4xl font-black font-mono text-sky-400 tracking-tight">{defects}</span>
            <span className="text-xs text-sky-400/90 font-semibold">sản phẩm</span>
          </div>
          <p className="text-xs text-slate-400 mt-1.5">Thiếu sản phẩm, nứt vỡ bao gói</p>
        </div>

        {/* KPI 4: X-Ray Tube Parameters */}
        <div className="p-5 rounded-2xl bg-gradient-to-br from-slate-900 to-slate-950 border border-slate-800/90 relative overflow-hidden shadow-sm">
          <div className="flex items-center justify-between">
            <span className="text-sm font-semibold text-slate-300">Thông Số Bóng Tia X</span>
            <div className="p-2.5 rounded-xl bg-emerald-500/10 text-emerald-400 border border-emerald-500/20">
              <Zap className="w-5 h-5" />
            </div>
          </div>
          <div className="mt-3 flex items-baseline gap-3">
            <div>
              <span className="text-3xl font-black font-mono text-emerald-400">{avgVoltage}</span>
              <span className="text-xs font-semibold text-slate-400 ml-1">kV</span>
            </div>
            <span className="text-slate-600 font-bold">/</span>
            <div>
              <span className="text-3xl font-black font-mono text-emerald-400">{avgCurrent}</span>
              <span className="text-xs font-semibold text-slate-400 ml-1">mA</span>
            </div>
          </div>
          <p className="text-xs text-slate-400 mt-1.5">Mức phát tia trung bình</p>
        </div>
      </div>

      {/* Middle Section: Product Distribution + Machine Info */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Product Distribution */}
        <div className="lg:col-span-2 p-5 rounded-2xl bg-slate-900/60 border border-slate-800">
          <div className="flex items-center justify-between mb-4">
            <h3 className="text-base font-bold text-white">Phân Bổ Lỗi Theo Sản Phẩm</h3>
            <span className="text-xs text-slate-400">Top 5 sản phẩm lỗi nhiều nhất</span>
          </div>

          {sortedProducts.length === 0 ? (
            <div className="h-36 flex items-center justify-center text-sm text-slate-500">
              Chưa có dữ liệu phân loại sản phẩm
            </div>
          ) : (
            <div className="space-y-4">
              {sortedProducts.map(([product, count]) => {
                const percentage = Math.round((count / totalRejects) * 100)
                return (
                  <div key={product} className="space-y-1.5">
                    <div className="flex justify-between text-sm">
                      <span className="font-semibold text-slate-200">{product}</span>
                      <span className="font-mono text-slate-400 text-xs">
                        {count} lỗi ({percentage}%)
                      </span>
                    </div>
                    <div className="h-2.5 w-full bg-slate-800 rounded-full overflow-hidden">
                      <div
                        className="h-full bg-gradient-to-r from-rose-500 to-amber-500 rounded-full transition-all duration-500"
                        style={{ width: `${percentage}%` }}
                      />
                    </div>
                  </div>
                )
              })}
            </div>
          )}
        </div>

        {/* Machine Hardware & Watcher Status */}
        <div className="p-5 rounded-2xl bg-slate-900/60 border border-slate-800 flex flex-col justify-between">
          <div>
            <h3 className="text-base font-bold text-white mb-4 flex items-center gap-2">
              <Activity className="w-4 h-4 text-emerald-400" />
              <span>Thông Tin Hệ Thống</span>
            </h3>

            <div className="space-y-3">
              <div className="p-3 rounded-xl bg-slate-950/60 border border-slate-800/80">
                <span className="text-xs text-slate-400 block mb-0.5">Tên Thiết Bị</span>
                <span className="text-sm font-semibold text-slate-200">{status?.machineName || 'Raycon EX2'}</span>
              </div>

              <div className="p-3 rounded-xl bg-slate-950/60 border border-slate-800/80">
                <span className="text-xs text-slate-400 block mb-0.5">Mã Định Danh (ID)</span>
                <span className="font-mono text-sm font-semibold text-slate-200">{status?.machineId || 'RAYCON_EX2'}</span>
              </div>

              <div className="p-3 rounded-xl bg-slate-950/60 border border-slate-800/80">
                <span className="text-xs text-slate-400 block mb-0.5 flex items-center gap-1.5">
                  <HardDrive className="w-3.5 h-3.5 text-slate-500" /> Thư Mục Đang Giám Sát
                </span>
                <span className="font-mono text-xs text-slate-300 break-all">
                  {status?.watchDirectory || 'Archive\\Image\\Reject'}
                </span>
              </div>
            </div>
          </div>

          <div className="pt-4 border-t border-slate-800/80 mt-4 text-xs text-slate-400 flex items-center justify-between">
            <span>Dịch vụ Engine:</span>
            <span className="text-emerald-400 font-mono font-bold">ACTIVE (.NET 10 x86)</span>
          </div>
        </div>
      </div>

      {/* Recent Rejects Gallery Preview */}
      <div className="p-5 rounded-2xl bg-slate-900/60 border border-slate-800">
        <div className="flex items-center justify-between mb-4">
          <div>
            <h3 className="text-base font-bold text-white">Sản Phẩm Lỗi Mới Nhất</h3>
            <p className="text-xs text-slate-400">Các mẫu vừa được máy X-Ray phát hiện và lưu lại</p>
          </div>
        </div>

        {recentRejects.length === 0 ? (
          <div className="h-32 flex items-center justify-center text-sm text-slate-500">
            Chưa có ảnh lỗi nào được ghi nhận
          </div>
        ) : (
          <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-6 gap-3.5">
            {recentRejects.map((item) => (
              <div
                key={item.id}
                onClick={() => onOpenLiveInspect(item)}
                className="group relative flex flex-col rounded-xl border border-slate-800 hover:border-rose-500/60 bg-slate-950 p-2.5 cursor-pointer transition-all hover:shadow-[0_0_15px_rgba(244,63,94,0.15)] overflow-hidden"
              >
                <div className="h-32 w-full bg-black/70 rounded-lg flex items-center justify-center overflow-hidden mb-2.5">
                  <img
                    src={item.thumbnailUrl}
                    alt={item.id}
                    className="max-h-full max-w-full object-contain filter contrast-125 group-hover:scale-105 transition-transform"
                    loading="lazy"
                  />
                  {item.isContaminant && (
                    <span className="absolute top-3.5 right-3.5 px-2 py-0.5 rounded text-[10px] font-bold bg-rose-500 text-white shadow">
                      DỊ VẬT
                    </span>
                  )}
                </div>

                <div className="text-xs font-bold text-slate-100 truncate">{item.productName}</div>
                <div className="text-xs text-slate-400 font-mono mt-0.5">
                  {new Date(item.createdAt).toLocaleTimeString('vi-VN')}
                </div>

                <div className="mt-2.5 pt-2 border-t border-slate-800/80 flex items-center justify-between text-xs text-rose-400 group-hover:text-rose-300 font-semibold">
                  <span>Soi ảnh</span>
                  <ArrowRight className="w-3.5 h-3.5 group-hover:translate-x-0.5 transition-transform" />
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  )
}
