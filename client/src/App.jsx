import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import LoginForm from "./components/authentication/LoginForm";
import RegisterForm from "./components/authentication/RegisterForm";
import CalorieCalculationForm from "./components/calories/CalorieCalculationForm";
import CalorieResults from "./components/calories/CalorieResults";
import TargetSetup from "./components/TargetSetup";
import NotFoundPage from "./components/NotFoundPage";
import LandingPage from "./components/LandingPage";
import TransitionRoutes from "./components/common/TransitionRoutes";
import AuthGuard from "./components/common/AuthGuard";
import Layout from "./components/Layout";
import MealPlanComponent from "./components/meals/MealPlanComponent";
import GroceryListView from "./components/groceries/GroceryListView";

export default function App() {
  return (
   <BrowserRouter>
     <Layout>
       <TransitionRoutes>
         <Route path="/" element={<Navigate to="/home" replace />} />
         <Route path="/login" element={<AuthGuard><LoginForm /></AuthGuard>} />
         <Route path="/home" element={<LandingPage />} />
         <Route path="/register" element={<AuthGuard><RegisterForm /></AuthGuard>} />
         <Route path="/setup" element={<TargetSetup />} />
         <Route path="/calculate" element={<CalorieCalculationForm />} />
         <Route path="/results" element={<CalorieResults />} />
         <Route path="/meal-plan" element={<MealPlanComponent />} />
         <Route path="/grocery-list" element={<GroceryListView />} />
         <Route path="*" element={<NotFoundPage />} />
       </TransitionRoutes>
     </Layout>
   </BrowserRouter>
  );
 }