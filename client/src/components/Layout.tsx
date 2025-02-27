import { ReactNode } from "react";
import backgroundImage from "../assets/backgrounds/15229163_v848-noon-24c.jpg";
import { ToastContainer } from "react-toastify";

interface LayoutProps {
  children: ReactNode;
}

const Layout = ({ children }: LayoutProps) => {
  return (
    <main className="font-atkinson min-h-screen flex flex-col relative items-center overflow-x-hidden">
      <img
        className="object-cover fixed inset-0 -z-5 w-full h-full min-h-[100vh]"
        alt="Hero Background"
        src={backgroundImage}
      />

      {/* BiteAI Header - Fixed at top */}
      <header className="w-full py-6 flex justify-center z-20 bg-gradient-to-b from-amber-50/90 to-transparent">
        <div className="flex items-center">
          <h1 className="text-6xl font-thin">
            <span className="text-amber-900">Bite</span>
            <span className="text-amber-500">AI</span>
          </h1>
        </div>
      </header>

      {/* Content area - scrollable with padding for header */}
      <div className="min-w-[600px] z-10 pt-8 pb-8 px-4">
        {children}
      </div>
      <ToastContainer />
    </main>
  );
};

export default Layout;
