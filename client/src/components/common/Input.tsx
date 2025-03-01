import React, { useState, useEffect } from "react";

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
  required?: boolean; // Add required prop
  validateOnBlur?: boolean; // Optional prop to control validation timing
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
  required = false, // Default to false
  validateOnBlur = true, // Default to true
}) => {
  const [localError, setLocalError] = useState<string>("");
  const [touched, setTouched] = useState(false);
  
  // Validate on value change if the field has been touched
  useEffect(() => {
    if (touched && required && value.trim() === "") {
      setLocalError("This field is required");
    } else {
      setLocalError("");
    }
  }, [value, required, touched]);

  const displayError = error || localError;

  const handleBlur = () => {
    setTouched(true);
  };

  return (
    <div className="space-y-2">
      <label htmlFor={id} className="block text-sm font-medium text-gray-700">
        {label}
        {required && <span className="text-red-500 ml-1">*</span>}
      </label>
      <div className="relative">
        <input
          type={type}
          id={id}
          value={value}
          onChange={onChange}
          onBlur={validateOnBlur ? handleBlur : undefined}
          className={`w-full px-3 py-2 transition-all duration-300 border rounded-md shadow-sm focus:outline-none focus:ring-1 h-10 box-border ${
            displayError ? "border-red-500 focus:ring-red-500" : "border-gray-300 focus:ring-gray-400"
          }`}
          placeholder={placeholder}
          required={required}
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
      {displayError && <p className="text-red-500 text-xs mt-1">{displayError}</p>}
    </div>
  );
};

export default Input;