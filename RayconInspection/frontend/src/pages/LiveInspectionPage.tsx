import React from 'react'
import type { RejectItem } from '../types/inspection'
import { GalleryViewer } from '../components/GalleryViewer'
import { MetadataPanel } from '../components/MetadataPanel'
import { Filmstrip } from '../components/Filmstrip'
import { ChevronLeft, ChevronRight } from 'lucide-react'

interface LiveInspectionPageProps {
  rejects: RejectItem[]
  selectedReject: RejectItem | null
  onSelectReject: (item: RejectItem) => void
}

export const LiveInspectionPage: React.FC<LiveInspectionPageProps> = ({
  rejects,
  selectedReject,
  onSelectReject,
}) => {
  const currentIndex = rejects.findIndex((r) => r.id === selectedReject?.id)
  const hasNext = currentIndex > 0 // in descending order, next means newer (index - 1)
  const hasPrev = currentIndex < rejects.length - 1 // previous means older (index + 1)

  const handleNext = () => {
    if (hasNext) {
      onSelectReject(rejects[currentIndex - 1])
    }
  }

  const handlePrev = () => {
    if (hasPrev) {
      onSelectReject(rejects[currentIndex + 1])
    }
  }

  return (
    <div className="flex-1 flex flex-col h-full overflow-hidden bg-slate-950">
      {/* Top Action Bar */}
      <div className="h-14 border-b border-slate-800 bg-slate-900/50 backdrop-blur px-5 flex items-center justify-between shrink-0">
        <div className="flex items-center gap-3">
          <span className="flex items-center gap-2 px-3 py-1.5 rounded-xl bg-emerald-500/10 border border-emerald-500/25 text-emerald-400 text-xs font-bold font-mono shadow-sm">
            <span className="w-2.5 h-2.5 rounded-full bg-emerald-400 animate-ping" /> LIVE INSPECTOR
          </span>
          <span className="text-sm text-slate-300">
            {selectedReject ? (
              <span>
                Đang soi: <strong className="text-white font-bold">{selectedReject.productName}</strong> ({selectedReject.id})
              </span>
            ) : (
              'Chưa chọn mẫu'
            )}
          </span>
        </div>

        {/* Quick Prev / Next Buttons */}
        <div className="flex items-center gap-2 text-xs">
          <button
            type="button"
            onClick={handlePrev}
            disabled={!hasPrev}
            className="flex items-center gap-1.5 px-3 py-1.5 rounded-xl border border-slate-700 bg-slate-800 hover:bg-slate-700 text-slate-200 disabled:opacity-30 disabled:pointer-events-none transition font-medium"
            title="Ảnh cũ hơn (Ảnh trước)"
          >
            <ChevronLeft className="w-4 h-4" /> Ảnh trước
          </button>
          <span className="text-xs font-mono font-semibold text-slate-300 px-2.5">
            {currentIndex >= 0 ? `${currentIndex + 1} / ${rejects.length}` : '0 / 0'}
          </span>
          <button
            type="button"
            onClick={handleNext}
            disabled={!hasNext}
            className="flex items-center gap-1.5 px-3 py-1.5 rounded-xl border border-slate-700 bg-slate-800 hover:bg-slate-700 text-slate-200 disabled:opacity-30 disabled:pointer-events-none transition font-medium"
            title="Ảnh mới hơn (Ảnh sau)"
          >
            Ảnh sau <ChevronRight className="w-4 h-4" />
          </button>
        </div>
      </div>

      {/* Main Inspection View: Center Gallery + Right Metadata */}
      <div className="flex-1 flex overflow-hidden">
        <GalleryViewer item={selectedReject} />
        <MetadataPanel item={selectedReject} />
      </div>

      {/* Bottom Filmstrip */}
      <Filmstrip
        items={rejects}
        selectedItem={selectedReject}
        onSelectItem={onSelectReject}
      />
    </div>
  )
}
