import { useState } from "react";
import React from "react";
import { CalculationType } from "../models/calorie";
import { CalorieGoalsForm } from "./calories/CalorieGoalsForm";
import { TargetCaloriesForm } from "./calories/TargetCalorieForm";
import CalculationTabs from "./calories/CalculationTabs";

export function CalculationForm() {
  const [calculationType, setCalculationType] = useState<CalculationType>("goals");

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
        {calculationType === "goals" ? <CalorieGoalsForm /> : <TargetCaloriesForm />}
      </div>
    </div>
  );
}
