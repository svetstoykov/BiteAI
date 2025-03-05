// src/stores/calorieStore.ts
import { create } from "zustand";
import { CaloricIntakeForWeightGoalsDto } from "../models/calorie";
import { DietTypes } from "../models/meals";

interface CalorieState {
  caloricIntakeGoals: CaloricIntakeForWeightGoalsDto | null;
  targetCaloricIntake: number | null;
  calculationType: "calories" | "weight" | null;
  targetWeightKg: number | null;
  targetWeeks: number | null;
  dailyCalories: number | null;
  dietType: DietTypes | null;

  setResults: (
    caloricIntakeGoals: CaloricIntakeForWeightGoalsDto,
    targetCaloricIntake: number,
    calculationType: "calories" | "weight",
    targetWeightKg: number | null,
    targetWeeks: number | null
  ) => void;

  setMealPlanSpecifics: (dailyCalories: number, dietType: DietTypes) => void;

  resetResults: () => void;

  resetMealPlanSpecifics: () => void;
}

export const useCalorieStore = create<CalorieState>((set) => ({
  caloricIntakeGoals: null,
  targetCaloricIntake: null,
  calculationType: null,
  targetWeightKg: null,
  targetWeeks: null,
  dailyCalories: null,
  dietType: null,

  setResults: (
    caloricIntakeGoals,
    targetCaloricIntake,
    calculationType,
    targetWeightKg = null,
    targetWeeks = null
  ) =>
    set({
      caloricIntakeGoals,
      targetCaloricIntake,
      calculationType,
      targetWeightKg,
      targetWeeks,
    }),
    
  setMealPlanSpecifics: (dailyCalories, dietType) =>
    set({ dailyCalories, dietType }),
    
  resetMealPlanSpecifics: () => set({ dailyCalories: null, dietType: null }),

  resetResults: () =>
    set({
      caloricIntakeGoals: null,
      targetCaloricIntake: null,
      calculationType: null,
      targetWeightKg: null,
      targetWeeks: null,
    }),
}));