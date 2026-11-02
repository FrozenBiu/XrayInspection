import React, { useRef, useEffect } from 'react'
import type { RejectItem } from '../types/inspection'
import { AlertCircle, Clock, Zap } from 'lucide-react'

interface FilmstripProps {
  items: RejectItem[]
  selectedItem: RejectItem | null
  onSelectItem: (item: RejectItem) => void
}

export const Filmstrip: React.FC<FilmstripProps> = ({ items, selectedItem, onSelectItem }) => {
  const scrollRef = useRef<HTMLDivElement>(null)
  const activeItemRef = useRef<HTMLButtonElement>(null)

  // Scroll active item into view when selected
  useEffect(() => {
    if (activeItemRef.current) {
      activeItemRef.current.scrollIntoView({ behavior: 'smooth', block: 'nearest', inline: 'center' })
    }
  }, [selectedItem?.id])

  if (items.length === 0) {
    return (
      <div className="h-36 border-t border-slate-800 bg-slate-900/60 flex items-center justify-center text-sm text-slate-500">
        Chưa có ảnh lỗi nào được ghi nhận. Hệ thống đang chờ dữ liệu từ máy X-Ray...
      </div>
    )
  }

  return (
    <div className="h-40 border-t border-slate-800 bg-slate-950/80 backdrop-blur p-3 flex flex-col shrink-0 select-none">
      <div className="flex items-center justify-between mb-2 px-1">
        <span className="text-xs font-bold uppercase tracking-wider text-slate-300 flex items-center gap-2">
          <Zap className="w-3.5 h-3.5 text-amber-400" />
          <span>Filmstrip Danh sách Ảnh lỗi ({items.length})</span>
        </span>
        <span className="text-xs text-slate-400">Mới nhất nằm bên trái</span>
      </div>

      <div 
        ref={scrollRef}
        className="flex gap-3 overflow-x-auto pb-1.5 scrollbar-thin scrollbar-thumb-slate-700"
      >
        {items.map((item) => {
          const isSelected = selectedItem?.id === item.id
          const date = new Date(item.createdAt)
          const timeStr = date.toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit', second: '2-digit' })

          return (
            <button
              type="button"
              key={item.id}
              ref={isSelected ? activeItemRef : null}
              onClick={() => onSelectItem(item)}
              className={`group relative flex flex-col shrink-0 w-40 rounded-xl border transition-all text-left overflow-hidden bg-slate-900 ${
                isSelected 
                  ? 'border-rose-500 ring-2 ring-rose-500/30 shadow-[0_0_15px_rgba(244,63,94,0.25)]' 
                  : 'border-slate-800 hover:border-slate-700 opacity-80 hover:opacity-100'
              }`}
            >
              {/* Thumbnail Container */}
              <div className="relative h-18 w-full bg-black/70 flex items-center justify-center overflow-hidden">
                <img
                  src={item.thumbnailUrl}
                  alt={item.id}
                  className="max-h-full max-w-full object-contain filter contrast-125 transition-transform group-hover:scale-105"
                  loading="lazy"
                />
                
                {/* Defect Badge */}
                {item.isContaminant && (
                  <span className="absolute top-1.5 right-1.5 px-2 py-0.5 rounded text-[10px] font-bold bg-rose-500 text-white flex items-center gap-1 shadow">
                    <AlertCircle className="w-3 h-3" /> Dị vật
                  </span>
                )}
              </div>

              {/* Item Info */}
              <div className="p-2 flex flex-col justify-between flex-1 bg-slate-900/90">
                <p className="text-xs font-bold text-slate-100 truncate" title={item.productName}>
                  {item.productName}
                </p>
                <div className="flex items-center justify-between text-xs text-slate-400 mt-0.5">
                  <span className="flex items-center gap-1 font-mono">
                    <Clock className="w-3 h-3 text-slate-500" />
                    {timeStr}
                  </span>
                  <span className="font-mono text-xs font-semibold text-amber-400">
                    {item.sourceVoltage}kV
                  </span>
                </div>
              </div>
            </button>
          )
        })}
      </div>
    </div>
  )
}
