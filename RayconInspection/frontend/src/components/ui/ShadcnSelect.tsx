import React, { useState, useRef, useEffect } from "react";
import { ChevronDown, Check } from "lucide-react";

export interface SelectOption {
  value: string;
  label: string;
  badge?: string;
  badgeColor?: string;
  icon?: React.ReactNode;
}

interface ShadcnSelectProps {
  value: string;
  onChange: (value: string) => void;
  options: SelectOption[];
  label?: string;
  placeholder?: string;
  className?: string;
}

export const ShadcnSelect: React.FC<ShadcnSelectProps> = ({
  value,
  onChange,
  options,
  label,
  placeholder = "Chọn một mục...",
  className = "",
}) => {
  const [isOpen, setIsOpen] = useState(false);
  const containerRef = useRef<HTMLDivElement>(null);

  const selectedOption = options.find((opt) => opt.value === value);

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

  return (
    <div ref={containerRef} className={`relative inline-block ${isOpen ? 'z-50' : 'z-10'} ${className}`}>
      {/* Trigger Button */}
      <button
        type="button"
        onClick={() => setIsOpen(!isOpen)}
        className={`flex items-center justify-between gap-3 px-3.5 py-2 rounded-xl border transition-all text-sm font-medium ${
          isOpen
            ? "bg-slate-900 border-rose-500/80 ring-2 ring-rose-500/20 text-white shadow-lg shadow-black/40"
            : "bg-slate-950 border-slate-700/80 hover:border-slate-600 text-slate-200"
        }`}
      >
        <div className="flex items-center gap-2 truncate">
          {label && (
            <span className="text-slate-400 font-normal text-xs">{label}:</span>
          )}
          {selectedOption?.icon && (
            <span className="shrink-0">{selectedOption.icon}</span>
          )}
          <span className="truncate">
            {selectedOption ? selectedOption.label : placeholder}
          </span>
        </div>

        <ChevronDown
          className={`w-4 h-4 text-slate-400 transition-transform duration-200 shrink-0 ${
            isOpen ? "rotate-180 text-rose-400" : ""
          }`}
        />
      </button>

      {/* Dropdown Menu Popover */}
      {isOpen && (
        <div className="absolute left-0 mt-2 z-50 min-w-[220px] max-h-72 overflow-y-auto rounded-2xl border border-slate-700/90 bg-slate-900/95 backdrop-blur-xl p-1.5 shadow-2xl shadow-black/70 animate-in fade-in zoom-in-95 duration-150">
          <div className="space-y-1">
            {options.map((option) => {
              const isSelected = option.value === value;
              return (
                <div
                  key={option.value}
                  onClick={() => {
                    onChange(option.value);
                    setIsOpen(false);
                  }}
                  className={`flex items-center justify-between px-3 py-2 rounded-xl text-sm transition cursor-pointer select-none ${
                    isSelected
                      ? "bg-rose-500/15 text-rose-400 font-semibold"
                      : "text-slate-300 hover:text-white hover:bg-slate-800/80"
                  }`}
                >
                  <div className="flex items-center gap-2 truncate">
                    {option.icon && (
                      <span className="shrink-0">{option.icon}</span>
                    )}
                    <span className="truncate">{option.label}</span>
                  </div>

                  <div className="flex items-center gap-1.5 shrink-0 ml-2">
                    {option.badge && (
                      <span
                        className={`text-[10px] font-bold font-mono px-2 py-0.5 rounded-full border ${
                          option.badgeColor ||
                          "bg-slate-800 text-slate-400 border-slate-700"
                        }`}
                      >
                        {option.badge}
                      </span>
                    )}
                    {isSelected && <Check className="w-4 h-4 text-rose-400" />}
                  </div>
                </div>
              );
            })}
          </div>
        </div>
      )}
    </div>
  );
};
