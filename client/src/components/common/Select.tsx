import React from "react";

export interface SelectOption {
  value: number;
  label: string;
}

export interface SelectProps {
  id: string;
  value: string;
  onChange: (e: React.ChangeEvent<HTMLSelectElement>) => void;
  label: string;
  options: SelectOption[];
  error?: string;
  required?: boolean;
  className?: string;
}

const Select: React.FC<SelectProps> = ({
  id,
  value,
  onChange,
  label,
  options,
  error,
  required = false,
  className = "",
}) => {
  return (
    <div className={`space-y-2 ${className}`}>
      <label htmlFor={id} className="block text-sm font-medium text-gray-700">
        {label}
      </label>
      <div className="relative">
        <div className="relative">
          <select
            id={id}
            value={value}
            onChange={onChange}
            required={required}
            className={`w-full px-3 py-2 border rounded-md shadow-sm focus:outline-none focus:ring-1 h-10 box-border appearance-none ${
              error ? "border-red-500 focus:ring-red-500" : "border-gray-300 focus:ring-gray-400"
            }`}
          >
          {options.map((option) => (
            <option key={option.value} value={option.value}>
              {option.label}
            </option>
          ))}
                  </select>
            <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2 text-gray-700">
              <svg className="fill-current h-4 w-4" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20">
                <path d="M9.293 12.95l.707.707L15.657 8l-1.414-1.414L10 10.828 5.757 6.586 4.343 8z" />
              </svg>
            </div>
          </div>
        </div>
      {error && <p className="text-red-500 text-xs mt-1">{error}</p>}
    </div>
  );
};

export default Select;