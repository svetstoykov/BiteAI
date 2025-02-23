import { useState, ChangeEvent, FormEvent } from "react";
import {
  CalculationType,
  CaloricIntakeForWeightGoalsDto,
  CalorieFormData,
} from "../models/calorie";
import React from "react";
import { ApiResponse } from "../models/api";
import { toast } from "react-toastify";

export default function HomePage() {
  const [formData, setFormData] = useState<CalorieFormData>({
    weightKg: "",
    heightCm: "",
    age: "",
    exerciseDaysPerWeek: "",
    isMale: true,
    targetWeightKg: "",
    targetWeeks: "",
  });

  const [calculationType, setCalculationType] = useState<CalculationType>("goals");
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [result, setResult] = useState<CaloricIntakeForWeightGoalsDto | number | null>(
    null
  );

  const handleInputChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: type === "checkbox" ? checked : value,
    }));
  };

  const prepareRequestParams = () => {
    const baseParams = {
      weightKg: Number(formData.weightKg),
      heightCm: Number(formData.heightCm),
      age: Number(formData.age),
      exerciseDaysPerWeek: Number(formData.exerciseDaysPerWeek),
      isMale: formData.isMale,
    };

    if (calculationType === "target") {
      return {
        ...baseParams,
        targetWeightKg: Number(formData.targetWeightKg),
        targetWeeks: Number(formData.targetWeeks),
      };
    }

    return baseParams;
  };

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setIsLoading(true);

    try {
      const endpoint =
        calculationType === "goals"
          ? "/api/calories/calculate-goals"
          : "/api/calories/calculate-target-daily-calories";

      const params = prepareRequestParams();
      const queryParams = new URLSearchParams(
        Object.entries(params).map(([key, value]) => [key, String(value)])
      );

      const url = `http://localhost:5036${endpoint}?${queryParams}`;

      const response = await fetch(url);
      const data: ApiResponse<CaloricIntakeForWeightGoalsDto | number> =
        await response.json();

      if (!response.ok) {
        toast.error(`Failed to calculate calories. ${data.message}`);
        return;
      }

      if (data) {
        setResult(data.data);
      }
    } catch (error) {
      toast.error("Error:", error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="flex items-center justify-center min-h-screen p-4 bg-gray-50">
      <div className="w-full max-w-md p-6 bg-white rounded-lg shadow-lg">
        <div className="mb-6 text-center">
          <h1 className="text-2xl font-bold text-gray-900">Calorie Calculator</h1>
          <p className="mt-2 text-gray-500">Calculate your daily calorie needs</p>
        </div>

        <div className="flex gap-4 mb-6">
          <button
            type="button"
            className={`flex-1 py-2 px-4 rounded-md transition-colors ${
              calculationType === "goals"
                ? "bg-blue-600 text-white"
                : "bg-gray-100 text-gray-700 hover:bg-gray-200"
            }`}
            onClick={() => setCalculationType("goals")}
          >
            Calculate Goals
          </button>
          <button
            type="button"
            className={`flex-1 py-2 px-4 rounded-md transition-colors ${
              calculationType === "target"
                ? "bg-blue-600 text-white"
                : "bg-gray-100 text-gray-700 hover:bg-gray-200"
            }`}
            onClick={() => setCalculationType("target")}
          >
            Target Calculator
          </button>
        </div>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block mb-1 text-sm font-medium text-gray-700">
                Weight (kg)
              </label>
              <input
                type="number"
                name="weightKg"
                value={formData.weightKg}
                onChange={handleInputChange}
                className="w-full p-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                required
              />
            </div>
            <div>
              <label className="block mb-1 text-sm font-medium text-gray-700">
                Height (cm)
              </label>
              <input
                type="number"
                name="heightCm"
                value={formData.heightCm}
                onChange={handleInputChange}
                className="w-full p-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                required
              />
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block mb-1 text-sm font-medium text-gray-700">Age</label>
              <input
                type="number"
                name="age"
                value={formData.age}
                onChange={handleInputChange}
                className="w-full p-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                required
              />
            </div>
            <div>
              <label className="block mb-1 text-sm font-medium text-gray-700">
                Exercise Days/Week
              </label>
              <input
                type="number"
                name="exerciseDaysPerWeek"
                min="0"
                max="7"
                value={formData.exerciseDaysPerWeek}
                onChange={handleInputChange}
                className="w-full p-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                required
              />
            </div>
          </div>

          <div className="flex items-center space-x-2">
            <input
              type="checkbox"
              id="isMale"
              name="isMale"
              checked={formData.isMale}
              onChange={handleInputChange}
              className="w-4 h-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
            />
            <label htmlFor="isMale" className="text-sm font-medium text-gray-700">
              Male
            </label>
          </div>

          {calculationType === "target" && (
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="block mb-1 text-sm font-medium text-gray-700">
                  Target Weight (kg)
                </label>
                <input
                  type="number"
                  name="targetWeightKg"
                  value={formData.targetWeightKg}
                  onChange={handleInputChange}
                  className="w-full p-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                  required
                />
              </div>
              <div>
                <label className="block mb-1 text-sm font-medium text-gray-700">
                  Target Weeks
                </label>
                <input
                  type="number"
                  name="targetWeeks"
                  min="4"
                  value={formData.targetWeeks}
                  onChange={handleInputChange}
                  className="w-full p-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                  required
                />
              </div>
            </div>
          )}

          <button
            type="submit"
            className="w-full px-4 py-2 font-medium text-white bg-blue-600 rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed"
            disabled={isLoading}
          >
            {isLoading ? "Calculating..." : "Calculate"}
          </button>
        </form>
      </div>
    </div>
  );
}
