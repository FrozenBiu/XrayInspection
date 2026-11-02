import React, { useState, useRef, useEffect } from 'react'
import type { RejectItem } from '../types/inspection'
import { 
  ZoomIn, ZoomOut, RotateCcw, 
  Sun, Contrast, Layers, Download
} from 'lucide-react'

interface GalleryViewerProps {
  item: RejectItem | null
}

export const GalleryViewer: React.FC<GalleryViewerProps> = ({ item }) => {
  const [scale, setScale] = useState(1)
  const [position, setPosition] = useState({ x: 0, y: 0 })
  const [isDragging, setIsDragging] = useState(false)
  const [dragStart, setDragStart] = useState({ x: 0, y: 0 })
  
  // Image adjustments
  const [brightness, setBrightness] = useState(100)
  const [contrast, setContrast] = useState(120)
  const [isInverted, setIsInverted] = useState(false)

  const containerRef = useRef<HTMLDivElement>(null)

  // Reset transforms when image changes
  useEffect(() => {
    setScale(1)
    setPosition({ x: 0, y: 0 })
  }, [item?.id])

  if (!item) {
    return (
      <div className="flex-1 flex flex-col items-center justify-center text-slate-500 p-8">
        <Layers className="w-14 h-14 stroke-[1.2] mb-3 text-slate-600" />
        <p className="text-base font-medium">Chọn một sản phẩm từ Filmstrip bên dưới để kiểm tra chi tiết</p>
      </div>
    )
  }

  const handleZoom = (delta: number) => {
    setScale(prev => Math.min(Math.max(prev + delta, 0.2), 6))
  }

  const handleReset = () => {
    setScale(1)
    setPosition({ x: 0, y: 0 })
    setBrightness(100)
    setContrast(120)
    setIsInverted(false)
  }

  const handleMouseDown = (e: React.MouseEvent) => {
    if (e.button === 0) {
      setIsDragging(true)
      setDragStart({ x: e.clientX - position.x, y: e.clientY - position.y })
    }
  }

  const handleMouseMove = (e: React.MouseEvent) => {
    if (isDragging) {
      setPosition({
        x: e.clientX - dragStart.x,
        y: e.clientY - dragStart.y
      })
    }
  }

  const handleMouseUp = () => {
    setIsDragging(false)
  }

  const handleWheel = (e: React.WheelEvent) => {
    e.preventDefault()
    const zoomFactor = e.deltaY < 0 ? 0.15 : -0.15
    handleZoom(zoomFactor)
  }

  return (
    <div className="relative flex-1 flex flex-col overflow-hidden bg-black/90 select-none">
      {/* Floating Toolbar */}
      <div className="absolute top-4 left-4 z-20 flex items-center gap-2 px-3.5 py-2 rounded-2xl bg-slate-900/90 border border-slate-700/80 backdrop-blur-md shadow-2xl text-sm">
        {/* Zoom Controls */}
        <button 
          type="button"
          onClick={() => handleZoom(-0.2)}
          className="p-1.5 rounded-lg hover:bg-slate-800 text-slate-300 hover:text-white transition"
          title="Thu nhỏ (-)"
        >
          <ZoomOut className="w-4 h-4" />
        </button>
        <span className="font-mono text-xs font-semibold text-slate-200 w-14 text-center">
          {Math.round(scale * 100)}%
        </span>
        <button 
          type="button"
          onClick={() => handleZoom(0.2)}
          className="p-1.5 rounded-lg hover:bg-slate-800 text-slate-300 hover:text-white transition"
          title="Phóng to (+)"
        >
          <ZoomIn className="w-4 h-4" />
        </button>
        <button 
          type="button"
          onClick={handleReset}
          className="p-1.5 rounded-lg hover:bg-slate-800 text-slate-400 hover:text-white transition ml-1"
          title="Đặt lại góc nhìn"
        >
          <RotateCcw className="w-4 h-4" />
        </button>

        <div className="w-[1px] h-5 bg-slate-700 mx-1" />

        {/* Invert Filter */}
        <button 
          type="button"
          onClick={() => setIsInverted(!isInverted)}
          className={`p-1.5 rounded-lg transition ${
            isInverted 
              ? 'bg-amber-500/20 text-amber-300 border border-amber-500/40 shadow-sm' 
              : 'hover:bg-slate-800 text-slate-400 hover:text-white'
          }`}
          title="Đảo màu X-ray (Negative View)"
        >
          <Contrast className="w-4 h-4" />
        </button>

        {/* Direct Download */}
        <a 
          href={item.imageUrl}
          download={item.fileName.replace('.xri', '.png')}
          className="p-1.5 rounded-lg hover:bg-slate-800 text-slate-400 hover:text-white transition"
          title="Tải ảnh PNG về máy"
        >
          <Download className="w-4 h-4" />
        </a>
      </div>

      {/* Sliders for Brightness & Contrast */}
      <div className="absolute top-4 right-4 z-20 flex flex-col gap-2.5 p-3 rounded-2xl bg-slate-900/90 border border-slate-700/80 backdrop-blur-md shadow-2xl text-xs w-52">
        <div>
          <div className="flex justify-between text-xs text-slate-300 mb-1.5 font-medium">
            <span className="flex items-center gap-1.5"><Sun className="w-3.5 h-3.5 text-amber-400" /> Độ sáng</span>
            <span className="font-mono text-slate-400">{brightness}%</span>
          </div>
          <input 
            type="range" 
            min="50" 
            max="180" 
            value={brightness} 
            onChange={(e) => setBrightness(Number(e.target.value))}
            className="w-full accent-rose-500 h-1.5 bg-slate-700 rounded-lg cursor-pointer"
          />
        </div>
        <div>
          <div className="flex justify-between text-xs text-slate-300 mb-1.5 font-medium">
            <span className="flex items-center gap-1.5"><Contrast className="w-3.5 h-3.5 text-sky-400" /> Tương phản</span>
            <span className="font-mono text-slate-400">{contrast}%</span>
          </div>
          <input 
            type="range" 
            min="60" 
            max="250" 
            value={contrast} 
            onChange={(e) => setContrast(Number(e.target.value))}
            className="w-full accent-rose-500 h-1.5 bg-slate-700 rounded-lg cursor-pointer"
          />
        </div>
      </div>

      {/* Main Interactive Canvas / Image Display */}
      <div 
        ref={containerRef}
        onMouseDown={handleMouseDown}
        onMouseMove={handleMouseMove}
        onMouseUp={handleMouseUp}
        onWheel={handleWheel}
        className={`flex-1 flex items-center justify-center cursor-grab active:cursor-grabbing overflow-hidden`}
      >
        <div
          style={{
            transform: `translate(${position.x}px, ${position.y}px) scale(${scale})`,
            transition: isDragging ? 'none' : 'transform 0.1s ease-out'
          }}
          className="relative max-w-full max-h-full flex items-center justify-center"
        >
          <img
            src={item.imageUrl}
            alt={item.productName}
            draggable={false}
            style={{
              filter: `brightness(${brightness}%) contrast(${contrast}%) ${isInverted ? 'invert(1)' : ''}`,
            }}
            className="max-h-[75vh] max-w-[70vw] object-contain shadow-2xl select-none"
          />
        </div>
      </div>

      {/* Bottom Bar Info */}
      <div className="absolute bottom-3 left-4 z-20 px-3.5 py-1.5 rounded-xl bg-slate-900/80 border border-slate-800 text-xs text-slate-300 backdrop-blur font-mono flex items-center gap-3">
        <span>Độ phân giải: {item.width} × {item.height} px</span>
        <span className="text-slate-600">•</span>
        <span>File: {item.fileName}</span>
      </div>
    </div>
  )
}
