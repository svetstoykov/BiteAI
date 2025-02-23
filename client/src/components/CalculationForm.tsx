import { useState } from "react";
import { CalculationType } from "../models/calorie";
import { CalorieGoalsForm } from "./calories/CalorieGoalsForm";
import { TargetCaloriesForm } from "./calories/TargetCalorieForm";
import CalculationTabs from "./calories/CalculationTabs";

export function CalculationForm() {
  const [calculationType, setCalculationType] = useState<CalculationType>("goals");
  const [isLoading, setIsLoading] = useState(false);

  return (
    <div className="flex items-center justify-center min-h-screen p-4">
      <div className="w-full max-w-md p-6 bg-white rounded-lg shadow-lg">
        <div className="mb-6 text-center">
          <h1 className="text-2xl font-bold text-gray-900">Calorie Calculator</h1>
          <p className="mt-2 text-gray-500">Calculate your daily calorie needs</p>
        </div>

        <CalculationTabs
          calculationType={calculationType}
          setCalculationType={setCalculationType}
        />

        {calculationType === "goals" ? (
          <CalorieGoalsForm setIsLoading={setIsLoading} />
        ) : (
          <TargetCaloriesForm setIsLoading={setIsLoading} />
        )}

        <button
          type="submit"
          className="w-full p-4 py-2 my-4 font-medium text-white rounded-md bg-pastel-green hover:bg-pastel-green focus:outline-none focus:ring-1 focus:ring-offset-1 focus:ring-pastel-green disabled:opacity-50 disabled:cursor-not-allowed"
          disabled={isLoading}
        >
          {isLoading ? "Calculating..." : "Calculate"}
        </button>
      </div>
    </div>
  );
}
