import React, { useState, useMemo } from 'react'
import type { RejectItem } from '../types/inspection'
import { 
  Search, Filter, LayoutGrid, List, 
  AlertTriangle, Download, ExternalLink, 
  RotateCcw, Eye, X, ArrowUpDown
} from 'lucide-react'
import { ShadcnSelect, type SelectOption } from '../components/ui/ShadcnSelect'
import { ShadcnDatePicker } from '../components/ui/ShadcnDatePicker'

interface HistoryArchivePageProps {
  rejects: RejectItem[]
  onOpenLiveInspect: (item: RejectItem) => void
}

export const HistoryArchivePage: React.FC<HistoryArchivePageProps> = ({
  rejects,
  onOpenLiveInspect,
}) => {
  // Filters
  const [searchTerm, setSearchTerm] = useState('')
  const [selectedProduct, setSelectedProduct] = useState<string>('all')
  const [selectedDate, setSelectedDate] = useState<string>('')
  const [defectFilter, setDefectFilter] = useState<string>('all')
  const [sortOrder, setSortOrder] = useState<string>('desc')
  const [viewMode, setViewMode] = useState<'grid' | 'table'>('grid')
  
  // Quick View Modal
  const [quickViewItem, setQuickViewItem] = useState<RejectItem | null>(null)

  // Unique product names for dropdown
  const productOptions = useMemo(() => {
    const set = new Set<string>()
    rejects.forEach((r) => {
      if (r.productName) set.add(r.productName)
    })
    return Array.from(set).sort()
  }, [rejects])

  const productSelectOptions: SelectOption[] = useMemo(() => {
    return [
      { 
        value: 'all', 
        label: `Tất cả sản phẩm (${productOptions.length})` 
      },
      ...productOptions.map((p) => ({ 
        value: p, 
        label: p 
      })),
    ]
  }, [productOptions])

  const defectOptions: SelectOption[] = [
    { value: 'all', label: 'Tất cả phân loại' },
    { 
      value: 'contaminant', 
      label: 'Chỉ Dị vật', 
      badge: 'DỊ VẬT', 
      badgeColor: 'bg-rose-500/20 text-rose-400 border-rose-500/30' 
    },
    { 
      value: 'defect', 
      label: 'Chỉ Khuyết tật', 
      badge: 'KHUYẾT TẬT', 
      badgeColor: 'bg-sky-500/20 text-sky-400 border-sky-500/30' 
    },
  ]

  const sortOptions: SelectOption[] = [
    { 
      value: 'desc', 
      label: 'Mới nhất trước',
      icon: <ArrowUpDown className="w-3.5 h-3.5 text-slate-400" />
    },
    { 
      value: 'asc', 
      label: 'Cũ nhất trước',
      icon: <ArrowUpDown className="w-3.5 h-3.5 text-slate-400" />
    },
  ]

  // Filtered & Sorted items
  const filteredItems = useMemo(() => {
    return rejects
      .filter((item) => {
        // Search term
        if (searchTerm.trim()) {
          const term = searchTerm.toLowerCase()
          const matchName = item.productName.toLowerCase().includes(term)
          const matchId = item.id.toLowerCase().includes(term)
          const matchFile = item.fileName.toLowerCase().includes(term)
          if (!matchName && !matchId && !matchFile) return false
        }

        // Product filter
        if (selectedProduct !== 'all' && item.productName !== selectedProduct) {
          return false
        }

        // Date filter (YYYY-MM-DD)
        if (selectedDate) {
          const itemDate = new Date(item.createdAt).toISOString().split('T')[0]
          if (itemDate !== selectedDate) return false
        }

        // Defect type filter
        if (defectFilter === 'contaminant' && !item.isContaminant) return false
        if (defectFilter === 'defect' && (item.isContaminant || !item.hasErrors)) return false

        return true
      })
      .sort((a, b) => {
        const timeA = new Date(a.createdAt).getTime()
        const timeB = new Date(b.createdAt).getTime()
        return sortOrder === 'desc' ? timeB - timeA : timeA - timeB
      })
  }, [rejects, searchTerm, selectedProduct, selectedDate, defectFilter, sortOrder])

  const handleResetFilters = () => {
    setSearchTerm('')
    setSelectedProduct('all')
    setSelectedDate('')
    setDefectFilter('all')
    setSortOrder('desc')
  }

  // Quick Date Setters
  const setQuickDate = (type: 'today' | 'yesterday' | 'all') => {
    if (type === 'all') {
      setSelectedDate('')
      return
    }
    const d = new Date()
    if (type === 'yesterday') {
      d.setDate(d.getDate() - 1)
    }
    setSelectedDate(d.toISOString().split('T')[0])
  }

  return (
    <div className="flex-1 flex flex-col h-full overflow-hidden bg-slate-950">
      {/* Top Filter Bar */}
      <div className="p-4 border-b border-slate-800 bg-slate-900/60 backdrop-blur space-y-3.5 shrink-0 relative z-30">
        <div className="flex flex-col lg:flex-row lg:items-center justify-between gap-3">
          {/* Search Box with increased font size */}
          <div className="relative flex-1 max-w-md">
            <Search className="w-4 h-4 absolute left-3.5 top-1/2 -translate-y-1/2 text-slate-400" />
            <input
              type="text"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              placeholder="Tìm theo tên sản phẩm, mã ID, tên file..."
              className="w-full pl-10 pr-3.5 py-2.5 rounded-xl bg-slate-950 border border-slate-700/80 text-sm text-slate-100 placeholder-slate-400 focus:outline-none focus:border-rose-500 focus:ring-1 focus:ring-rose-500/20 transition"
            />
          </div>

          {/* Quick Date Pills & View Switcher */}
          <div className="flex items-center gap-2.5">
            <div className="flex items-center p-1 rounded-xl bg-slate-950 border border-slate-800 text-xs">
              <button
                type="button"
                onClick={() => setQuickDate('all')}
                className={`px-3 py-1.5 rounded-lg text-xs font-semibold transition ${
                  !selectedDate ? 'bg-slate-800 text-white shadow-sm' : 'text-slate-400 hover:text-slate-200'
                }`}
              >
                Tất cả ngày
              </button>
              <button
                type="button"
                onClick={() => setQuickDate('today')}
                className={`px-3 py-1.5 rounded-lg text-xs font-semibold transition ${
                  selectedDate === new Date().toISOString().split('T')[0] ? 'bg-slate-800 text-white shadow-sm' : 'text-slate-400 hover:text-slate-200'
                }`}
              >
                Hôm nay
              </button>
              <button
                type="button"
                onClick={() => setQuickDate('yesterday')}
                className={`px-3 py-1.5 rounded-lg text-xs font-semibold transition ${
                  selectedDate === new Date(Date.now() - 86400000).toISOString().split('T')[0] ? 'bg-slate-800 text-white shadow-sm' : 'text-slate-400 hover:text-slate-200'
                }`}
              >
                Hôm qua
              </button>
            </div>

            <div className="flex items-center p-1 rounded-xl bg-slate-950 border border-slate-800">
              <button
                type="button"
                onClick={() => setViewMode('grid')}
                className={`p-2 rounded-lg transition ${
                  viewMode === 'grid' ? 'bg-slate-800 text-white shadow-sm' : 'text-slate-500 hover:text-slate-300'
                }`}
                title="Dạng Lưới (Grid)"
              >
                <LayoutGrid className="w-4 h-4" />
              </button>
              <button
                type="button"
                onClick={() => setViewMode('table')}
                className={`p-2 rounded-lg transition ${
                  viewMode === 'table' ? 'bg-slate-800 text-white shadow-sm' : 'text-slate-500 hover:text-slate-300'
                }`}
                title="Dạng Bảng (Table)"
              >
                <List className="w-4 h-4" />
              </button>
            </div>
          </div>
        </div>

        {/* Detailed Modern Shadcn Filter Controls */}
        <div className="flex flex-wrap items-center gap-3 pt-1">
          {/* Shadcn Product Select */}
          <ShadcnSelect
            value={selectedProduct}
            onChange={setSelectedProduct}
            options={productSelectOptions}
            label="Sản phẩm"
          />

          {/* Shadcn Date Picker */}
          <ShadcnDatePicker
            value={selectedDate}
            onChange={setSelectedDate}
            placeholder="Lọc theo ngày..."
          />

          {/* Shadcn Defect Filter Select */}
          <ShadcnSelect
            value={defectFilter}
            onChange={setDefectFilter}
            options={defectOptions}
            label="Phân loại"
          />

          {/* Shadcn Sort Select */}
          <ShadcnSelect
            value={sortOrder}
            onChange={setSortOrder}
            options={sortOptions}
          />

          {/* Reset Filters Button */}
          {(searchTerm || selectedProduct !== 'all' || selectedDate || defectFilter !== 'all') && (
            <button
              type="button"
              onClick={handleResetFilters}
              className="flex items-center gap-1.5 px-3 py-2 rounded-xl text-slate-300 hover:text-white bg-slate-800/80 hover:bg-slate-800 border border-slate-700 transition text-xs font-medium"
            >
              <RotateCcw className="w-3.5 h-3.5" /> Đặt lại lọc
            </button>
          )}

          <div className="ml-auto text-xs text-slate-400 font-mono">
            Hiển thị <span className="font-bold text-white text-sm">{filteredItems.length}</span> / {rejects.length} ảnh
          </div>
        </div>
      </div>

      {/* Main Content Area */}
      <div className="flex-1 overflow-y-auto p-4 lg:p-6">
        {filteredItems.length === 0 ? (
          <div className="h-64 flex flex-col items-center justify-center text-slate-400 space-y-3">
            <Filter className="w-10 h-10 text-slate-600" />
            <p className="text-sm font-medium">Không tìm thấy ảnh lỗi nào phù hợp với bộ lọc</p>
            <button
              type="button"
              onClick={handleResetFilters}
              className="text-sm font-semibold text-rose-400 hover:text-rose-300 underline underline-offset-4"
            >
              Bấm vào đây để đặt lại bộ lọc
            </button>
          </div>
        ) : viewMode === 'grid' ? (
          /* Grid Cards View with enlarged typography */
          <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6 gap-4">
            {filteredItems.map((item) => {
              const date = new Date(item.createdAt)
              const timeStr = date.toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit', second: '2-digit' })
              const dateStr = date.toLocaleDateString('vi-VN', { day: '2-digit', month: '2-digit' })

              return (
                <div
                  key={item.id}
                  className="group relative flex flex-col rounded-2xl border border-slate-800 bg-slate-900/80 hover:border-slate-700 hover:bg-slate-900 transition-all overflow-hidden shadow-sm hover:shadow-lg hover:shadow-black/50"
                >
                  {/* Thumbnail */}
                  <div 
                    onClick={() => setQuickViewItem(item)}
                    className="relative h-40 w-full bg-black/90 flex items-center justify-center cursor-pointer overflow-hidden"
                  >
                    <img
                      src={item.thumbnailUrl}
                      alt={item.id}
                      className="max-h-full max-w-full object-contain filter contrast-125 group-hover:scale-105 transition-transform duration-200"
                      loading="lazy"
                    />

                    {/* Defect Indicator Badge */}
                    {item.isContaminant ? (
                      <span className="absolute top-2.5 right-2.5 px-2 py-0.5 rounded-lg text-[10px] font-bold bg-rose-500 text-white flex items-center gap-1 shadow-md shadow-rose-950/50">
                        <AlertTriangle className="w-3 h-3" /> DỊ VẬT
                      </span>
                    ) : item.hasErrors ? (
                      <span className="absolute top-2.5 right-2.5 px-2 py-0.5 rounded-lg text-[10px] font-bold bg-sky-500 text-white shadow-md shadow-sky-950/50">
                        KHUYẾT TẬT
                      </span>
                    ) : null}

                    {/* Hover Overlay Button */}
                    <div className="absolute inset-0 bg-black/40 opacity-0 group-hover:opacity-100 transition-opacity flex items-center justify-center gap-2">
                      <span className="p-2.5 rounded-full bg-slate-900/90 text-white shadow-lg border border-slate-700">
                        <Eye className="w-5 h-5" />
                      </span>
                    </div>
                  </div>

                  {/* Card Details */}
                  <div className="p-3.5 flex flex-col justify-between flex-1 space-y-2.5">
                    <div>
                      <div className="text-sm font-bold text-slate-100 truncate" title={item.productName}>
                        {item.productName}
                      </div>
                      <div className="flex items-center justify-between text-xs text-slate-400 mt-1 font-mono">
                        <span>{dateStr} {timeStr}</span>
                        <span className="text-amber-400 font-semibold">{item.sourceVoltage}kV</span>
                      </div>
                    </div>

                    {/* Action Bar */}
                    <div className="pt-2.5 border-t border-slate-800 flex items-center justify-between">
                      <button
                        type="button"
                        onClick={() => onOpenLiveInspect(item)}
                        className="text-xs text-rose-400 hover:text-rose-300 font-semibold flex items-center gap-1.5 transition"
                      >
                        <ExternalLink className="w-3.5 h-3.5" /> Soi trực tiếp
                      </button>
                      <a
                        href={item.imageUrl}
                        download={item.fileName.replace('.xri', '.png')}
                        className="p-1.5 rounded-lg hover:bg-slate-800 text-slate-400 hover:text-slate-200 transition"
                        title="Tải ảnh PNG"
                      >
                        <Download className="w-4 h-4" />
                      </a>
                    </div>
                  </div>
                </div>
              )
            })}
          </div>
        ) : (
          /* Table List View with enlarged typography */
          <div className="rounded-2xl border border-slate-800 bg-slate-900/60 overflow-hidden shadow-sm">
            <table className="w-full text-left text-sm">
              <thead className="bg-slate-950 text-slate-400 border-b border-slate-800 text-xs font-bold uppercase tracking-wider">
                <tr>
                  <th className="p-3.5 w-20 text-center">Ảnh</th>
                  <th className="p-3.5">Thời gian</th>
                  <th className="p-3.5">Tên sản phẩm</th>
                  <th className="p-3.5">Phân loại lỗi</th>
                  <th className="p-3.5">Điện áp kV</th>
                  <th className="p-3.5">Dòng mA</th>
                  <th className="p-3.5">Tốc độ</th>
                  <th className="p-3.5 text-right">Thao tác</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-800 text-slate-200">
                {filteredItems.map((item) => (
                  <tr key={item.id} className="hover:bg-slate-800/40 transition">
                    <td className="p-2.5 text-center">
                      <img
                        src={item.thumbnailUrl}
                        alt={item.id}
                        onClick={() => setQuickViewItem(item)}
                        className="w-12 h-12 object-contain rounded-xl bg-black/70 mx-auto cursor-pointer hover:opacity-80 transition border border-slate-800"
                      />
                    </td>
                    <td className="p-3.5 font-mono text-xs text-slate-400">
                      {new Date(item.createdAt).toLocaleString('vi-VN')}
                    </td>
                    <td className="p-3.5 font-bold text-white text-sm">{item.productName}</td>
                    <td className="p-3.5">
                      {item.isContaminant ? (
                        <span className="px-2.5 py-1 rounded-lg text-xs font-bold bg-rose-500/20 text-rose-400 border border-rose-500/30">
                          DỊ VẬT
                        </span>
                      ) : (
                        <span className="px-2.5 py-1 rounded-lg text-xs font-bold bg-sky-500/20 text-sky-400 border border-sky-500/30">
                          Khuyết tật
                        </span>
                      )}
                    </td>
                    <td className="p-3.5 font-mono font-medium text-amber-400">{item.sourceVoltage} kV</td>
                    <td className="p-3.5 font-mono font-medium">{item.sourceCurrent} mA</td>
                    <td className="p-3.5 font-mono">{item.conveyingSpeed} m/s</td>
                    <td className="p-3.5 text-right space-x-2">
                      <button
                        type="button"
                        onClick={() => onOpenLiveInspect(item)}
                        className="px-3 py-1.5 rounded-xl bg-rose-500/10 text-rose-400 hover:bg-rose-500/20 font-semibold text-xs transition border border-rose-500/20"
                      >
                        Soi ảnh
                      </button>
                      <a
                        href={item.imageUrl}
                        download={item.fileName.replace('.xri', '.png')}
                        className="px-3 py-1.5 rounded-xl bg-slate-800 hover:bg-slate-700 text-slate-200 transition text-xs font-medium border border-slate-700"
                      >
                        Tải PNG
                      </a>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>

      {/* Quick View Modal with enlarged layout & typography */}
      {quickViewItem && (
        <div 
          onClick={() => setQuickViewItem(null)}
          className="fixed inset-0 z-[100] bg-black/80 backdrop-blur-sm flex items-center justify-center p-4 animate-in fade-in duration-200"
        >
          <div 
            onClick={(e) => e.stopPropagation()}
            className="bg-slate-900 border border-slate-700/80 rounded-3xl max-w-2xl w-full p-6 shadow-2xl relative space-y-4"
          >
            <div className="flex items-center justify-between border-b border-slate-800 pb-3">
              <div>
                <h3 className="text-base font-bold text-white">{quickViewItem.productName}</h3>
                <p className="text-xs text-slate-400 font-mono mt-0.5">
                  {new Date(quickViewItem.createdAt).toLocaleString('vi-VN')}
                </p>
              </div>
              <button
                type="button"
                onClick={() => setQuickViewItem(null)}
                className="p-2 rounded-xl hover:bg-slate-800 text-slate-400 hover:text-white transition"
              >
                <X className="w-5 h-5" />
              </button>
            </div>

            {/* Modal Image */}
            <div className="h-88 w-full bg-black/90 rounded-2xl flex items-center justify-center overflow-hidden p-2 border border-slate-800">
              <img
                src={quickViewItem.imageUrl}
                alt={quickViewItem.productName}
                className="max-h-full max-w-full object-contain filter contrast-125"
              />
            </div>

            {/* Modal Footer Controls */}
            <div className="flex items-center justify-between pt-2">
              <div className="flex items-center gap-3 text-xs text-slate-300 font-mono">
                <span className="text-amber-400 font-semibold">{quickViewItem.sourceVoltage}kV</span>
                <span>•</span>
                <span>{quickViewItem.sourceCurrent}mA</span>
                <span>•</span>
                <span>Tốc độ: {quickViewItem.conveyingSpeed} m/s</span>
              </div>

              <div className="flex items-center gap-2">
                <button
                  type="button"
                  onClick={() => {
                    const item = quickViewItem
                    setQuickViewItem(null)
                    onOpenLiveInspect(item)
                  }}
                  className="flex items-center gap-2 px-4 py-2.5 rounded-xl bg-rose-500 hover:bg-rose-600 text-white font-bold text-sm transition shadow-lg shadow-rose-500/25"
                >
                  <ExternalLink className="w-4 h-4" /> Mở trong Live Inspector
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  )
}
