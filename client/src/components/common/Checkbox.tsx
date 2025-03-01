import React from "react";

interface CheckboxProps {
  id: string;
  checked: boolean;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  label: string;
}

const Checkbox: React.FC<CheckboxProps> = ({ id, checked, onChange, label }) => {
  return (
    <div className="flex items-center justify-center">
      <div className="relative">
        <input
          id={id}
          name={id}
          type="checkbox"
          checked={checked}
          onChange={onChange}
          className="absolute w-0 h-0 opacity-0" // Hide the actual checkbox
        />
        <label htmlFor={id} className="flex items-center cursor-pointer">
          <div
            className={`w-4 h-4 border border-gray-400 rounded flex items-center justify-center transition-all duration-200 ${
              checked ? "bg-gray-800" : "bg-white"
            }`}
          >
            {checked && (
              <svg
                xmlns="http://www.w3.org/2000/svg"
                className="h-3 w-3 text-white"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={3}
                  d="M5 13l4 4L19 7"
                />
              </svg>
            )}
          </div>
          <span className="ml-2 block text-sm text-gray-700">{label}</span>
        </label>
      </div>
    </div>
  );
};

export default Checkbox;