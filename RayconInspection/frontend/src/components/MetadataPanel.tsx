import React from 'react'
import type { RejectItem } from '../types/inspection'
import { 
  Info, AlertTriangle, CheckCircle2, 
  Gauge, Zap, Sliders, Calendar, Package, FileCode 
} from 'lucide-react'

interface MetadataPanelProps {
  item: RejectItem | null
}

export const MetadataPanel: React.FC<MetadataPanelProps> = ({ item }) => {
  if (!item) {
    return (
      <aside className="w-80 border-l border-slate-800 bg-slate-900/60 p-6 flex flex-col justify-center items-center text-sm text-slate-500 text-center">
        Chưa có sản phẩm nào được chọn
      </aside>
    )
  }

  const date = new Date(item.createdAt)
  const formattedDate = date.toLocaleString('vi-VN', {
    day: '2-digit', month: '2-digit', year: 'numeric',
    hour: '2-digit', minute: '2-digit', second: '2-digit'
  })

  return (
    <aside className="w-84 border-l border-slate-800 bg-slate-900/80 p-5 flex flex-col justify-between shrink-0 overflow-y-auto">
      <div className="space-y-5">
        {/* Title */}
        <div className="flex items-center justify-between border-b border-slate-800 pb-3.5">
          <h2 className="text-sm font-bold uppercase tracking-wider text-slate-300 flex items-center gap-2">
            <Info className="w-4 h-4 text-sky-400" />
            <span>Thông số Kỹ thuật</span>
          </h2>
          <span className="text-xs px-2.5 py-1 rounded-lg bg-rose-500/10 text-rose-400 border border-rose-500/25 font-bold tracking-wide">
            REJECTED
          </span>
        </div>

        {/* Product Details */}
        <div className="space-y-3">
          <div className="bg-slate-950/60 p-3.5 rounded-xl border border-slate-800">
            <div className="text-xs text-slate-400 mb-1 flex items-center gap-1.5 font-medium">
              <Package className="w-4 h-4 text-slate-400" /> Tên Sản Phẩm / Công thức
            </div>
            <div className="text-base font-bold text-white">{item.productName}</div>
          </div>

          <div className="bg-slate-950/60 p-3.5 rounded-xl border border-slate-800">
            <div className="text-xs text-slate-400 mb-1 flex items-center gap-1.5 font-medium">
              <Calendar className="w-4 h-4 text-slate-400" /> Thời gian Phát hiện
            </div>
            <div className="text-sm font-mono text-slate-200">{formattedDate}</div>
          </div>
        </div>

        {/* Inspection Criteria */}
        <div>
          <div className="text-xs font-bold text-slate-400 uppercase tracking-wider mb-2.5">
            Kết quả Đánh giá
          </div>
          <div className="space-y-2">
            <div className="flex items-center justify-between p-3 rounded-xl bg-slate-950/40 border border-slate-800 text-sm">
              <span className="text-slate-300 font-medium">Phát hiện Dị vật:</span>
              {item.isContaminant ? (
                <span className="flex items-center gap-1.5 text-rose-400 font-bold">
                  <AlertTriangle className="w-4 h-4" /> CÓ DỊ VẬT
                </span>
              ) : (
                <span className="flex items-center gap-1.5 text-slate-400 font-medium">
                  <CheckCircle2 className="w-4 h-4 text-emerald-500" /> Không
                </span>
              )}
            </div>

            <div className="flex items-center justify-between p-3 rounded-xl bg-slate-950/40 border border-slate-800 text-sm">
              <span className="text-slate-300 font-medium">Lỗi Khuyết tật:</span>
              <span className="font-semibold text-slate-300">
                {item.hasErrors ? 'Có lỗi' : 'Bình thường'}
              </span>
            </div>
          </div>
        </div>

        {/* X-Ray Machine Parameters */}
        <div>
          <div className="text-xs font-bold text-slate-400 uppercase tracking-wider mb-2.5 flex items-center gap-1.5">
            <Sliders className="w-3.5 h-3.5 text-amber-400" />
            <span>Thông số Máy lúc quét</span>
          </div>
          <div className="grid grid-cols-2 gap-2.5 text-sm">
            <div className="p-3 rounded-xl bg-slate-950/40 border border-slate-800">
              <div className="text-xs text-slate-400 flex items-center gap-1 mb-1">
                <Zap className="w-3.5 h-3.5 text-amber-400" /> Điện áp kV
              </div>
              <div className="font-mono font-bold text-slate-100 text-base">
                {item.sourceVoltage} <span className="text-xs font-normal text-slate-400">kV</span>
              </div>
            </div>

            <div className="p-3 rounded-xl bg-slate-950/40 border border-slate-800">
              <div className="text-xs text-slate-400 flex items-center gap-1 mb-1">
                <Zap className="w-3.5 h-3.5 text-cyan-400" /> Dòng điện mA
              </div>
              <div className="font-mono font-bold text-slate-100 text-base">
                {item.sourceCurrent} <span className="text-xs font-normal text-slate-400">mA</span>
              </div>
            </div>

            <div className="p-3 rounded-xl bg-slate-950/40 border border-slate-800 col-span-2">
              <div className="text-xs text-slate-400 flex items-center gap-1 mb-1">
                <Gauge className="w-3.5 h-3.5 text-emerald-400" /> Tốc độ Băng chuyền
              </div>
              <div className="font-mono font-bold text-slate-100 text-base">
                {item.conveyingSpeed} <span className="text-xs font-normal text-slate-400">m/s</span>
              </div>
            </div>
          </div>
        </div>

        {/* Raw File info */}
        <div className="p-3 rounded-xl bg-slate-950/30 border border-slate-800/60 text-xs text-slate-400 space-y-1 font-mono">
          <div className="flex items-center gap-1.5">
            <FileCode className="w-3.5 h-3.5 text-slate-500" />
            <span className="truncate">{item.fileName}</span>
          </div>
          <div>Dung lượng: {(Number(item.fileSizeBytes) / 1024 / 1024).toFixed(2)} MB</div>
        </div>
      </div>
    </aside>
  )
}
