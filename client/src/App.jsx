import { ToastContainer } from "react-toastify";
import { CalculationForm } from "./components/CalculationForm";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { LoginForm } from "./components/LoginForm";
import NotFoundPage from "./components/NotFoundPage";
import Layout from "./components/Layout";

export default function App() {
  return (
    <BrowserRouter>
      <Layout>
        <Routes>
          <Route path="/" element={<Navigate to="/calculate" replace />} />
          <Route path="/login" element={<LoginForm />} />
          <Route path="/calculate" element={<CalculationForm />} />
          <Route path="*" element={<NotFoundPage />} />
        </Routes>
        <ToastContainer />
      </Layout>
    </BrowserRouter>
  );
}
