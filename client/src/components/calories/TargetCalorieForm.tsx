import { ChangeEvent, FormEvent, useState } from "react";
import { TargetCalorieFormData } from "../../models/calorie";
import CalorieService from "../../services/calorie-service";
import { BaseCalorieForm } from "./BaseCalorieForm";

interface TargetCalorieFormProps {
  setIsLoading: (loading: boolean) => void;
}

export function TargetCaloriesForm({ setIsLoading }: TargetCalorieFormProps) {
  const [formData, setFormData] = useState<TargetCalorieFormData>({
    weightKg: 0,
    heightCm: 0,
    age: 0,
    exerciseDaysPerWeek: 0,
    isMale: true,
    targetWeightKg: 0,
    targetWeeks: 0,
  });

  const [result, setResult] = useState<number | null>(null);
  const calorieService = new CalorieService();

  const handleInputChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: type === "checkbox" ? checked : value,
    }));
  };

  const handleSelectChange = (e: ChangeEvent<HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
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
      targetWeightKg: Number(formData.targetWeightKg),
      targetWeeks: Number(formData.targetWeeks),
    };

    const response = await calorieService.calculateTargetDailyCalories(params);
    if (response) {
      setResult(response);
    }

    setIsLoading(false);
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <BaseCalorieForm
        formData={formData}
        onInputChange={handleInputChange}
        onSelectChange={handleSelectChange}
      />

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
            className="w-full p-2 border border-gray-300 rounded-md focus:ring-1 focus:ring-pastel-green focus:border-pastel-green focus:outline-none"
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
            className="w-full p-2 border border-gray-300 rounded-md focus:ring-1 focus:ring-pastel-green focus:border-pastel-green focus:outline-none"
            required
          />
        </div>
      </div>

      {result && (
        <div className="p-4 mt-4 rounded-md bg-gray-50">
          <h3 className="mb-2 font-medium">Your Target Daily Calories:</h3>
          <p>{result} calories per day</p>
        </div>
      )}
    </form>
  );
}
