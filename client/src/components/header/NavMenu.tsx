// NavMenu.tsx
import React from "react";
import NavButton from "./NavButton";

interface NavItem {
  label: string;
  onClick: () => void;
}

interface NavMenuProps {
  items: NavItem[];
  orientation?: "horizontal" | "vertical";
  className?: string;
}

const NavMenu: React.FC<NavMenuProps> = ({ 
  items, 
  orientation = "horizontal", 
  className = "" 
}) => {
  // Set appropriate container classes based on orientation
  const containerClasses = orientation === "horizontal" 
    ? "flex items-center" 
    : "flex flex-col";
  
  // Set appropriate button classes based on orientation
  const buttonClasses = orientation === "vertical" 
    ? "w-full" 
    : "";

  return (
    <div className={`${containerClasses} ${className}`}>
      {items.map((item, index) => (
        <NavButton
          key={index}
          label={item.label}
          onClick={item.onClick}
          className={buttonClasses}
        />
      ))}
    </div>
  );
};

export default NavMenu;