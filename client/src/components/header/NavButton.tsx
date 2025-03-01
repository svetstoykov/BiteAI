// NavButton.tsx
import React from "react";

interface NavButtonProps {
  label: string;
  onClick: () => void;
  className?: string;
}

const NavButton: React.FC<NavButtonProps> = ({ label, onClick, className = "" }) => {
  const baseClasses = "text-xl font-thin transition-colors duration-300";

  // Determine if this is for mobile or desktop based on class names
  const isMobile = className.includes("w-full");

  // Mobile styling vs desktop styling
  const styleClasses = isMobile
    ? "w-full py-3 text-2xl pl-4 text-start hover:bg-gray-200"
    : "px-4 py-2 hover:bg-gray-200/20 rounded-xl";

  return (
    <button className={`${baseClasses} ${styleClasses} ${className}`} onClick={onClick}>
      {label}
    </button>
  );
};

export default NavButton;
