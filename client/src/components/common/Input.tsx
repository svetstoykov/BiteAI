import React from "react";

interface InputProps {
  id: string;
  type: string;
  value: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  label: string;
  placeholder?: string;
  error?: string;
  icon?: React.ReactNode;
  onIconClick?: () => void;
}

const Input: React.FC<InputProps> = ({
  id,
  type,
  value,
  onChange,
  label,
  placeholder,
  error,
  icon,
  onIconClick,
}) => {
  return (
    <div className="space-y-2">
      <label htmlFor={id} className="block text-sm font-medium text-gray-700">
        {label}
      </label>
      <div className="relative">
        <input
          type={type}
          id={id}
          value={value}
          onChange={onChange}
          className={`w-full px-3 py-2 transition-all duration-300 border rounded-md shadow-sm focus:outline-none focus:ring-1 h-10 box-border ${
            error ? "border-red-500 focus:ring-red-500" : "border-gray-300 focus:ring-gray-400"
          }`}
          placeholder={placeholder}
        />
        {icon && (
          <button
            type="button"
            onClick={onIconClick}
            className="absolute inset-y-0 right-0 pr-3 flex items-center text-gray-400 hover:text-gray-500"
          >
            {icon}
          </button>
        )}
      </div>
      {error && <p className="text-red-500 text-xs mt-1">{error}</p>}
    </div>
  );
};

export default Input;