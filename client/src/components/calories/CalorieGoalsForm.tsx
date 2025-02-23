import { ChangeEvent, FormEvent, useState } from "react";
import {
  CaloricIntakeForWeightGoalsDto,
  CalorieGoalsFormData,
} from "../../models/calorie";
import CalorieService from "../../services/calorie-service";
import { BaseCalorieForm } from "./BaseCalorieForm";

interface CalorieGoalsFormProps {
  setIsLoading: (loading: boolean) => void;
}

export function CalorieGoalsForm({ setIsLoading }: CalorieGoalsFormProps) {
  const [formData, setFormData] = useState<CalorieGoalsFormData>({
    weightKg: 0,
    heightCm: 0,
    age: 0,
    exerciseDaysPerWeek: 0,
    isMale: true,
  });

  const [result, setResult] = useState<CaloricIntakeForWeightGoalsDto | null>(null);
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
    };

    const response = await calorieService.calculateCalorieGoals(params);
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
