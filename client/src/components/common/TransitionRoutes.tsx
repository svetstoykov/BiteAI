import { ReactNode } from "react";
import { Routes, useLocation } from "react-router-dom";
import { motion, AnimatePresence } from "framer-motion";

type TransitionRoutesProps = {
  children: ReactNode;
};

export default function TransitionRoutes({ children }: TransitionRoutesProps) {
  const location = useLocation();
  
  return (
    <AnimatePresence mode="wait">
      <motion.div
        key={location.pathname}
        initial={{ opacity: 0, x: 20 }}
        animate={{ opacity: 1, x: 0 }}
        exit={{ opacity: 0, x: -20 }}
        transition={{ 
          duration: 0.3,
          ease: "easeInOut"
        }}
        style={{ width: '100%', display: 'flex' }} // Fix width issue
        className="w-full" // Additional safeguard with Tailwind
      >
        <Routes location={location}>
          {children}
        </Routes>
      </motion.div>
    </AnimatePresence>
  );
}