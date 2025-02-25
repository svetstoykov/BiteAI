import { ToastContainer } from "react-toastify";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import LoginForm from "./components/LoginForm";
import CalorieCalculationForm from "./components/CalorieCalculationForm";
import TargetSetup from "./components/TargetSetup";
import NotFoundPage from "./components/NotFoundPage";
import Layout from "./components/Layout";

export default function App() {
  return (
    <BrowserRouter>
      <Layout>
        <Routes>
          <Route path="/" element={<Navigate to="/setup" replace />} />
          <Route path="/login" element={<LoginForm />} />
          <Route path="/setup" element={<TargetSetup />} />
          <Route path="/calculate" element={<CalorieCalculationForm />} />
          <Route path="*" element={<NotFoundPage />} />
        </Routes>
        <ToastContainer />
      </Layout>
    </BrowserRouter>
  );
}