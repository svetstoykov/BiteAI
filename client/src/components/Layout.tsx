import { ReactNode } from "react";
import backgroundImage from "../assets/backgrounds/7475073_3685696.jpg";

interface LayoutProps {
  children: ReactNode;
}

const Layout = ({ children }: LayoutProps) => {
  return (
    <main>
      <img
        className="object-cover absolute -z-5 w-full h-full min-h-[70svh]"
        alt="Hero Background"
        src={backgroundImage}
      />
      <div className="relative z-10">
        {children}
      </div>
    </main>
  );
};

export default Layout;
