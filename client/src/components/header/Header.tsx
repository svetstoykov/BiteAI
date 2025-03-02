// Header.tsx
import { useState, useEffect } from "react";
import { Menu, X } from "lucide-react";
import NavMenu from "./NavMenu";
import { useNavigate } from "react-router-dom";
import { AuthenticationService } from "../../services/authentication-service";

interface HeaderProps {
  isLoggedIn?: boolean;
  userName?: string;
}

interface NavItem {
  label: string;
  onClick: () => void;
}

const Header: React.FC<HeaderProps> = ({ isLoggedIn = false }) => {
  const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false);
  const navigate = useNavigate();
  const authenticationService = new AuthenticationService();

  useEffect(() => {
    if (isMenuOpen) {
      document.body.style.overflow = "hidden";
    } else {
      document.body.style.overflow = "auto";
    }
    return () => {
      document.body.style.overflow = "auto";
    };
  }, [isMenuOpen]);

  const toggleMenu = (): void => {
    setIsMenuOpen(!isMenuOpen);
  };

  const handleNavigate = (path: string): void => {
    navigate(path);
  };

  const handleLogout = (): void => {
    handleNavigate("/");
  };

  const loggedInNavItems: NavItem[] = [
    { label: "Profile", onClick: () => handleNavigate("/profile") },
    { label: "Meal Plan", onClick: () => handleNavigate("/meal-plan") },
    { label: "Goals", onClick: () => handleNavigate("/goals") },
    { label: "Logout", onClick: handleLogout },
  ];

  const loggedOutNavItems: NavItem[] = [
    { label: "Login", onClick: () => handleNavigate("/login") },
    { label: "Register", onClick: () => handleNavigate("/register") },
  ];

  const navItems: NavItem[] = authenticationService.isAuthenticated()
    ? loggedInNavItems
    : loggedOutNavItems;

  return (
    <header className="w-full z-20">
      <div className="p-10 mx-auto flex justify-between items-center">
        <div
          className="cursor-pointer flex items-center"
          onClick={() => handleNavigate("/")}
        >
          <h1 className="text-6xl">
            <span className="font-thin">Bite</span>
            <span>AI</span>
          </h1>
        </div>

        <div className="hidden md:flex items-center space-x-2">
          <NavMenu items={navItems} orientation="horizontal" />
        </div>

        <div className="md:hidden">
          <MenuToggleButton isOpen={isMenuOpen} onClick={toggleMenu} />
        </div>
      </div>

      {/* Overlay - visible when menu is open */}
      {isMenuOpen && (
        <div
          className="fixed inset-0 bg-black/60 backdrop-blur-sm z-30 md:hidden"
          onClick={toggleMenu}
        />
      )}

      {/* Mobile Navigation Menu - Side drawer */}
      <div
        className={`fixed top-0 right-0 h-full w-4/5 max-w-xs bg-white z-40 shadow-xl transform transition-transform duration-300 ease-in-out md:hidden ${
          isMenuOpen ? "translate-x-0" : "translate-x-full"
        }`}
      >
        <div className="flex flex-col h-full py-20 space-y-6 overflow-y-auto">
          <NavMenu items={navItems} orientation="vertical" />
        </div>
      </div>
    </header>
  );
};

interface MenuToggleButtonProps {
  isOpen: boolean;
  onClick: () => void;
}

// Component for the menu toggle button
const MenuToggleButton: React.FC<MenuToggleButtonProps> = ({ isOpen, onClick }) => (
  <button
    onClick={onClick}
    className={`p-2 hover:${
      isOpen ? "bg-gray-200" : "bg-white/20"
    } transition-colors z-50 relative duration-300`}
  >
    {isOpen ? <X size={25} /> : <Menu size={25} />}
  </button>
);

export default Header;
