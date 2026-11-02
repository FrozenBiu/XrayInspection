import React, { useState, useRef, useEffect } from "react";
import {
  Calendar as CalendarIcon,
  ChevronLeft,
  ChevronRight,
  X,
} from "lucide-react";

interface ShadcnDatePickerProps {
  value: string; // YYYY-MM-DD or empty
  onChange: (value: string) => void;
  placeholder?: string;
  className?: string;
}

export const ShadcnDatePicker: React.FC<ShadcnDatePickerProps> = ({
  value,
  onChange,
  placeholder = "Chọn ngày...",
  className = "",
}) => {
  const [isOpen, setIsOpen] = useState(false);
  const containerRef = useRef<HTMLDivElement>(null);

  // Current view month & year (defaults to selected date or today)
  const initialDate = value ? new Date(value) : new Date();
  const [viewYear, setViewYear] = useState(initialDate.getFullYear());
  const [viewMonth, setViewMonth] = useState(initialDate.getMonth()); // 0-indexed

  // Close when clicking outside
  useEffect(() => {
    const handleClickOutside = (e: MouseEvent) => {
      if (
        containerRef.current &&
        !containerRef.current.contains(e.target as Node)
      ) {
        setIsOpen(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);

  // Sync view when value changes from outside
  useEffect(() => {
    if (value) {
      const d = new Date(value);
      if (!isNaN(d.getTime())) {
        setViewYear(d.getFullYear());
        setViewMonth(d.getMonth());
      }
    }
  }, [value]);

  const handlePrevMonth = () => {
    if (viewMonth === 0) {
      setViewMonth(11);
      setViewYear((y) => y - 1);
    } else {
      setViewMonth((m) => m - 1);
    }
  };

  const handleNextMonth = () => {
    if (viewMonth === 11) {
      setViewMonth(0);
      setViewYear((y) => y + 1);
    } else {
      setViewMonth((m) => m + 1);
    }
  };

  // Generate calendar days
  const daysInMonth = new Date(viewYear, viewMonth + 1, 0).getDate();
  // Day of week for 1st day of month (0: Sunday, 1: Monday, ...). Convert to Monday as 0:
  const firstDayOfWeek = (new Date(viewYear, viewMonth, 1).getDay() + 6) % 7;

  const days: {
    dayNumber: number;
    dateString: string;
    isCurrentMonth: boolean;
  }[] = [];

  // Previous month trailing days
  const prevMonthDays = new Date(viewYear, viewMonth, 0).getDate();
  for (let i = firstDayOfWeek - 1; i >= 0; i--) {
    const day = prevMonthDays - i;
    const m = viewMonth === 0 ? 12 : viewMonth;
    const y = viewMonth === 0 ? viewYear - 1 : viewYear;
    const mStr = String(m).padStart(2, "0");
    const dStr = String(day).padStart(2, "0");
    days.push({
      dayNumber: day,
      dateString: `${y}-${mStr}-${dStr}`,
      isCurrentMonth: false,
    });
  }

  // Current month days
  for (let i = 1; i <= daysInMonth; i++) {
    const mStr = String(viewMonth + 1).padStart(2, "0");
    const dStr = String(i).padStart(2, "0");
    days.push({
      dayNumber: i,
      dateString: `${viewYear}-${mStr}-${dStr}`,
      isCurrentMonth: true,
    });
  }

  // Format display text
  const displayLabel = value
    ? new Date(value).toLocaleDateString("vi-VN", {
        day: "2-digit",
        month: "2-digit",
        year: "numeric",
      })
    : placeholder;

  const todayStr = new Date().toISOString().split("T")[0];

  return (
    <div ref={containerRef} className={`relative inline-block ${isOpen ? 'z-50' : 'z-10'} ${className}`}>
      {/* Trigger Button */}
      <div
        className={`flex items-center justify-between gap-2.5 px-3.5 py-2 rounded-xl border transition-all text-sm font-medium ${
          isOpen
            ? "bg-slate-900 border-rose-500/80 ring-2 ring-rose-500/20 text-white shadow-lg shadow-black/40"
            : "bg-slate-950 border-slate-700/80 hover:border-slate-600 text-slate-200"
        }`}
      >
        <button
          type="button"
          onClick={() => setIsOpen(!isOpen)}
          className="flex items-center gap-2 cursor-pointer focus:outline-none"
        >
          <CalendarIcon className="w-4 h-4 text-rose-400 shrink-0" />
          <span
            className={value ? "text-white font-semibold" : "text-slate-400"}
          >
            {displayLabel}
          </span>
        </button>

        {value ? (
          <button
            type="button"
            onClick={(e) => {
              e.stopPropagation();
              onChange("");
            }}
            className="p-0.5 rounded-md hover:bg-slate-800 text-slate-400 hover:text-white transition"
            title="Xóa chọn ngày"
          >
            <X className="w-3.5 h-3.5" />
          </button>
        ) : null}
      </div>

      {/* Popover Calendar */}
      {isOpen && (
        <div className="absolute left-0 mt-2 z-50 w-72 rounded-2xl border border-slate-700/90 bg-slate-900/95 backdrop-blur-xl p-3.5 shadow-2xl shadow-black/80 animate-in fade-in zoom-in-95 duration-150 select-none">
          {/* Calendar Header */}
          <div className="flex items-center justify-between mb-3 px-1">
            <span className="text-sm font-bold text-white tracking-wide">
              Tháng {viewMonth + 1}, {viewYear}
            </span>
            <div className="flex items-center gap-1">
              <button
                type="button"
                onClick={handlePrevMonth}
                className="p-1.5 rounded-lg hover:bg-slate-800 text-slate-400 hover:text-white transition"
              >
                <ChevronLeft className="w-4 h-4" />
              </button>
              <button
                type="button"
                onClick={handleNextMonth}
                className="p-1.5 rounded-lg hover:bg-slate-800 text-slate-400 hover:text-white transition"
              >
                <ChevronRight className="w-4 h-4" />
              </button>
            </div>
          </div>

          {/* Weekday Names */}
          <div className="grid grid-cols-7 gap-1 text-center text-xs font-bold text-slate-500 mb-2">
            <span>T2</span>
            <span>T3</span>
            <span>T4</span>
            <span>T5</span>
            <span>T6</span>
            <span>T7</span>
            <span>CN</span>
          </div>

          {/* Days Grid */}
          <div className="grid grid-cols-7 gap-1 text-center">
            {days.map((d, index) => {
              const isSelected = value === d.dateString;
              const isToday = todayStr === d.dateString;

              return (
                <button
                  key={index}
                  type="button"
                  onClick={() => {
                    onChange(d.dateString);
                    setIsOpen(false);
                  }}
                  className={`h-8 w-8 mx-auto rounded-lg text-xs font-medium transition flex items-center justify-center ${
                    isSelected
                      ? "bg-rose-500 text-white font-bold shadow-md shadow-rose-500/40 scale-105"
                      : isToday
                        ? "border border-rose-500/50 text-rose-400 hover:bg-slate-800"
                        : d.isCurrentMonth
                          ? "text-slate-200 hover:bg-slate-800"
                          : "text-slate-600 hover:bg-slate-800/40"
                  }`}
                >
                  {d.dayNumber}
                </button>
              );
            })}
          </div>

          {/* Quick Presets Footer */}
          <div className="mt-3 pt-3 border-t border-slate-800 flex items-center justify-between text-xs">
            <button
              type="button"
              onClick={() => {
                onChange(todayStr);
                setIsOpen(false);
              }}
              className="text-rose-400 hover:text-rose-300 font-semibold transition"
            >
              Hôm nay
            </button>
            <button
              type="button"
              onClick={() => {
                const yDate = new Date(Date.now() - 86400000)
                  .toISOString()
                  .split("T")[0];
                onChange(yDate);
                setIsOpen(false);
              }}
              className="text-slate-400 hover:text-slate-200 transition"
            >
              Hôm qua
            </button>
            <button
              type="button"
              onClick={() => {
                onChange("");
                setIsOpen(false);
              }}
              className="text-slate-500 hover:text-slate-300 transition"
            >
              Xóa ngày
            </button>
          </div>
        </div>
      )}
    </div>
  );
};
