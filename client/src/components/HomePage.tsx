import { useState } from "react";
import React from "react";
import { CalorieGoals } from "./calories/CalorieGoals";
import { TargetCalories } from "./calories/TargetCalorie";
import { CalculationType } from "../models/calorie";

export default function HomePage() {
  const [calculationType, setCalculationType] = useState<CalculationType>("goals");

  return (
    <div className="flex items-center justify-center min-h-screen p-4 bg-gray-50">
      <div className="w-full max-w-md p-6 bg-white rounded-lg shadow-lg">
        <div className="mb-6 text-center">
          <h1 className="text-2xl font-bold text-gray-900">Calorie Calculator</h1>
          <p className="mt-2 text-gray-500">Calculate your daily calorie needs</p>
        </div>

        <div className="flex gap-4">
          <button
            type="button"
            className="flex-1 px-4 py-2 text-white transition-colors bg-blue-600 rounded-md hover:bg-blue-700"
            onClick={() => setCalculationType("goals")}
          >
            Calculate Goals
          </button>
          <button
            type="button"
            className="flex-1 px-4 py-2 text-white transition-colors bg-blue-600 rounded-md hover:bg-blue-700"
            onClick={() => setCalculationType("target")}
          >
            Target Calculator
          </button>
        </div>

        {calculationType === "goals" ? <CalorieGoals /> : <TargetCalories />}
      </div>
    </div>
  );
}
