import { ChangeEvent } from "react";
import { MetabolicProfileDto } from "../../models/calorie";

export interface BaseFormProps {
  formData: MetabolicProfileDto;
  onInputChange: (e: ChangeEvent<HTMLInputElement>) => void;
  onSelectChange: (e: ChangeEvent<HTMLSelectElement>) => void;
}

export const BaseCalorieForm = ({ formData, onInputChange, onSelectChange }: BaseFormProps) => {
  return (
    <>
      <div className="grid grid-cols-2 gap-4">
        <div>
          <label className="block mb-1 text-sm font-medium text-gray-700">
            Weight (kg)
          </label>
          <input
            type="number"
            name="weightKg"
            value={formData.weightKg}
            onChange={onInputChange}
            className="w-full p-2 border border-gray-300 rounded-md focus:ring-1 focus:ring-pastel-green focus:border-pastel-green focus:outline-none"
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
            onChange={onInputChange}
            className="w-full p-2 border border-gray-300 rounded-md focus:ring-1 focus:ring-pastel-green focus:border-pastel-green focus:outline-none"
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
            onChange={onInputChange}
            className="w-full p-2 border border-gray-300 rounded-md focus:ring-1 focus:ring-pastel-green focus:border-pastel-green focus:outline-none"
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
            onChange={onInputChange}
            className="w-full p-2 border border-gray-300 rounded-md focus:ring-1 focus:ring-pastel-green focus:border-pastel-green focus:outline-none"
            required
          />
        </div>
      </div>

      <div className="flex flex-col space-y-1">
        <label htmlFor="gender" className="text-sm font-medium text-gray-700">
          Gender
        </label>
        <select
          id="gender"
          name="gender"
          onChange={onSelectChange}
          className="w-full p-2 text-sm border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-pastel-green focus:border-transparent"
        >
          <option value="male">Male</option>
          <option value="female">Female</option>
        </select>
      </div>
    </>
  );
};
