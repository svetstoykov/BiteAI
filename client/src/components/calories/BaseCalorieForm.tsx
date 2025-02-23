import React from "react";
import { ChangeEvent } from "react";
import { MetabolicProfileDto } from "../../models/calorie";

export interface BaseFormProps {
  formData: MetabolicProfileDto;
  onInputChange: (e: ChangeEvent<HTMLInputElement>) => void;
}

export const BaseCalorieForm = ({ formData, onInputChange }: BaseFormProps) => {
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
            onChange={onInputChange}
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
            onChange={onInputChange}
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
            onChange={onInputChange}
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
          onChange={onInputChange}
          className="w-4 h-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
        />
        <label htmlFor="isMale" className="text-sm font-medium text-gray-700">
          Male
        </label>
      </div>
    </>
  );
};