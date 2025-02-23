import { ChangeEvent, FormEvent, useState } from "react";
import {
  CaloricIntakeForWeightGoalsDto,
  CalorieGoalsFormData,
} from "../../models/calorie";
import CalorieService from "../../services/calorie-service";
import React from "react";
import { BaseCalorieForm } from "./BaseCalorieForm";


export function CalorieGoalsForm() {
  const [formData, setFormData] = useState<CalorieGoalsFormData>({
    weightKg: 0,
    heightCm: 0,
    age: 0,
    exerciseDaysPerWeek: 0,
    isMale: true,
  });

  const [isLoading, setIsLoading] = useState(false);
  const [result, setResult] = useState<CaloricIntakeForWeightGoalsDto | null>(null);
  const calorieService = new CalorieService();

  const handleInputChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: type === "checkbox" ? checked : value,
    }));
  };

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setIsLoading(true);

    const params = {
      weightKg: Number(formData.weightKg),
      heightCm: Number(formData.heightCm),
      age: Number(formData.age),
      exerciseDaysPerWeek: Number(formData.exerciseDaysPerWeek),
      isMale: formData.isMale,
    };

    const response = await calorieService.calculateCalorieGoals(params);
    if (response) {
      setResult(response);
    }

    setIsLoading(false);
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <BaseCalorieForm formData={formData} onInputChange={handleInputChange} />

      <button
        type="submit"
        className="w-full px-4 py-2 font-medium text-white bg-blue-600 rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed"
        disabled={isLoading}
      >
        {isLoading ? "Calculating..." : "Calculate Goals"}
      </button>

      {result && (
        <div className="p-4 mt-4 rounded-md bg-gray-50">
          <h3 className="mb-2 font-medium">Your Calorie Goals:</h3>
          <p>Maintenance: {result.maintainCalories} calories</p>
          <p>Weight Loss (O.5kg per week): {result.loseHalfKgCalories} calories</p>
          <p>Weight Loss (1kg per week): {result.loseOneKgCalories} calories</p>
          <p>Weight Gain (O.5kg per week): {result.gainHalfKgCalories} calories</p>
          <p>Weight Gain (1kg per week): {result.gainOneKgCalories} calories</p>
        </div>
      )}
    </form>
  );
}
