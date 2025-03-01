import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import LoginForm from "./components/authentication/LoginForm";
import RegisterForm from "./components/authentication/RegisterForm";
import CalorieCalculationForm from "./components/calories/CalorieCalculationForm";
import CalorieResults from "./components/calories/CalorieResults";
import TargetSetup from "./components/TargetSetup";
import NotFoundPage from "./components/NotFoundPage";
import TransitionRoutes from "./components/common/TransitionRoutes";
import Layout from "./components/Layout";

export default function App() {
  return (
   <BrowserRouter>
     <Layout>
       <TransitionRoutes>
         <Route path="/" element={<Navigate to="/setup" replace />} />
         <Route path="/login" element={<LoginForm />} />
         <Route path="/register" element={<RegisterForm />} />
         <Route path="/setup" element={<TargetSetup />} />
         <Route path="/calculate" element={<CalorieCalculationForm />} />
         <Route path="/results" element={<CalorieResults />} />
         <Route path="*" element={<NotFoundPage />} />
       </TransitionRoutes>
     </Layout>
   </BrowserRouter>
  );
 }