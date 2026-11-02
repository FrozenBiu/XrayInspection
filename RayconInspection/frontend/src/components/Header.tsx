import React from 'react'
import { Activity, ShieldAlert } from 'lucide-react'
import type { MachineStatus } from '../types/inspection'

interface HeaderProps {
  status: MachineStatus | null
  isConnected: boolean
  totalRejects: number
}

export const Header: React.FC<HeaderProps> = ({ status, isConnected, totalRejects }) => {
  return (
    <header className="h-14 border-b border-slate-800 bg-slate-900/80 backdrop-blur px-4 flex items-center justify-between shrink-0">
      <div className="flex items-center gap-3">
        <div className="p-2 rounded-lg bg-rose-500/10 border border-rose-500/20 text-rose-400">
          <ShieldAlert className="w-5 h-5" />
        </div>
        <div>
          <h1 className="text-sm font-bold tracking-tight text-slate-100 flex items-center gap-2">
            <span>RAYCON X-RAY INSPECTION</span>
            <span className="text-[10px] px-2 py-0.5 rounded bg-slate-800 text-slate-400 font-normal">EX2 SYSTEM</span>
          </h1>
          <p className="text-xs text-slate-400 flex items-center gap-2">
            <span>{status?.machineName || 'Đang kết nối tới máy...'}</span>
            <span className="text-slate-600">•</span>
            <span className="font-mono text-[11px] text-slate-400">{status?.machineId || 'RAYCON_EX2'}</span>
          </p>
        </div>
      </div>

      <div className="flex items-center gap-4">
        {/* Total Rejects Badge */}
        <div className="flex items-center gap-2 px-3 py-1.5 rounded-lg bg-slate-800/80 border border-slate-700/50 text-xs">
          <Activity className="w-3.5 h-3.5 text-rose-400 animate-pulse" />
          <span className="text-slate-400">Đã phát hiện:</span>
          <span className="font-bold text-rose-400 font-mono text-sm">{totalRejects}</span>
        </div>

        {/* Connection status */}
        <div className="flex items-center gap-2 px-3 py-1.5 rounded-lg bg-slate-800/50 border border-slate-700/50 text-xs">
          <span className={`w-2 h-2 rounded-full ${isConnected ? 'bg-emerald-400 shadow-[0_0_8px_rgba(52,211,153,0.6)] animate-ping' : 'bg-amber-400'}`} />
          <span className={isConnected ? 'text-emerald-300' : 'text-amber-300 font-medium'}>
            {isConnected ? 'Realtime Kết nối' : 'Đang thử lại...'}
          </span>
        </div>
      </div>
    </header>
  )
}
