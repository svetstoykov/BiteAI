import { ReactNode } from "react";
import backgroundImage from "../assets/backgrounds/28563543_background_flower_3_07.jpg";
import { ToastContainer } from "react-toastify";
import Header from "./header/Header";

interface LayoutProps {
  children: ReactNode;
}

const Layout = ({ children }: LayoutProps) => {
  return (
    <main className="font-atkinson min-h-screen flex flex-col relative items-center overflow-x-hidden">
      <img
        className="object-cover fixed inset-0 -z-5 w-full h-full min-h-[100vh] blur-[3px]"
        alt="Hero Background"
        src={backgroundImage}
      />
      <Header/>
      <div className="z-10 pt-8 pb-8 px-4">
        {children}
      </div>
      {/* <Footer/> */}
      <ToastContainer />
    </main>
  );
};

export default Layout;