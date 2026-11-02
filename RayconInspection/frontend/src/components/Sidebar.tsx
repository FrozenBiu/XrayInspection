import React from "react";
import {
  LayoutDashboard,
  Radio,
  FolderArchive,
  Settings,
  ShieldAlert,
  Wifi,
  WifiOff,
} from "lucide-react";

export type TabType = "overview" | "live" | "history" | "settings";

interface SidebarProps {
  activeTab: TabType;
  onTabChange: (tab: TabType) => void;
  isConnected: boolean;
  totalRejects: number;
}

export const Sidebar: React.FC<SidebarProps> = ({
  activeTab,
  onTabChange,
  isConnected,
  totalRejects,
}) => {
  const navItems = [
    {
      id: "overview" as TabType,
      label: "Tổng quan Dashboard",
      icon: LayoutDashboard,
      badge: null,
    },
    {
      id: "live" as TabType,
      label: "Soi ảnh Trực tiếp",
      icon: Radio,
      badge: isConnected ? "LIVE" : null,
      badgeColor: "bg-emerald-500/20 text-emerald-400 border-emerald-500/30",
    },
    {
      id: "history" as TabType,
      label: "Thư viện & Lịch sử",
      icon: FolderArchive,
      badge: totalRejects.toString(),
      badgeColor: "bg-rose-500/20 text-rose-300 border-rose-500/30",
    },
    {
      id: "settings" as TabType,
      label: "Cài đặt Hệ thống",
      icon: Settings,
      badge: null,
    },
  ];

  return (
    <aside className="w-64 border-r border-slate-800 bg-slate-950 flex flex-col justify-between shrink-0 select-none">
      <div>
        {/* Brand Header */}
        <div className="h-16 border-b border-slate-800/80 px-4 flex items-center gap-3">
          <div className="p-2.5 rounded-xl bg-gradient-to-br from-rose-500/20 to-orange-500/20 border border-rose-500/30 text-rose-400 shadow-[0_0_15px_rgba(244,63,94,0.15)]">
            <ShieldAlert className="w-5 h-5" />
          </div>
          <div>
            <h1 className="text-base font-black tracking-tight text-white flex items-center gap-1.5">
              <span>Xray Inspection</span>
            </h1>
          </div>
        </div>

        {/* Navigation List */}
        <nav className="p-3 space-y-1.5">
          <div className="px-3 py-1.5 text-xs font-bold uppercase tracking-wider text-slate-500">
            Menu Quản lý
          </div>
          {navItems.map((item) => {
            const Icon = item.icon;
            const isActive = activeTab === item.id;
            return (
              <button
                key={item.id}
                onClick={() => onTabChange(item.id)}
                className={`w-full flex items-center justify-between px-3.5 py-2.5 rounded-xl text-sm font-semibold transition-all ${
                  isActive
                    ? "bg-rose-500/10 text-rose-400 border border-rose-500/25 shadow-[0_0_12px_rgba(244,63,94,0.1)]"
                    : "text-slate-400 hover:text-slate-200 hover:bg-slate-900 border border-transparent"
                }`}
              >
                <div className="flex items-center gap-3">
                  <Icon
                    className={`w-4 h-4 ${isActive ? "text-rose-400" : "text-slate-500"}`}
                  />
                  <span>{item.label}</span>
                </div>
                {item.badge && (
                  <span
                    className={`text-xs font-mono font-bold px-2 py-0.5 rounded-full border ${item.badgeColor || "bg-slate-800 text-slate-300 border-slate-700"}`}
                  >
                    {item.badge}
                  </span>
                )}
              </button>
            );
          })}
        </nav>
      </div>

      {/* Connection & Machine Status Footer */}
      <div className="p-3 border-t border-slate-800/80 bg-slate-950/60">
        <div className="p-3 rounded-xl bg-slate-900/60 border border-slate-800">
          <div className="flex items-center justify-between text-xs mb-1">
            <span className="text-xs text-slate-400 font-medium">
              Trạng thái Kết nối
            </span>
            {isConnected ? (
              <span className="flex items-center gap-1.5 text-xs font-bold text-emerald-400">
                <Wifi className="w-4 h-4 animate-pulse" /> Online
              </span>
            ) : (
              <span className="flex items-center gap-1.5 text-xs font-bold text-amber-400">
                <WifiOff className="w-4 h-4" /> Thử lại...
              </span>
            )}
          </div>
          <div className="text-xs text-slate-500 font-mono truncate">
            {isConnected
              ? "SignalR WebSocket Connected"
              : "Đang kết nối lại tới Port 5000"}
          </div>
        </div>
      </div>
    </aside>
  );
};
