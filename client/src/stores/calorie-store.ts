// src/stores/calorieStore.ts
import { create } from 'zustand';
import { CaloricIntakeForWeightGoalsDto } from '../models/calorie';

interface CalorieState {
  caloricIntakeGoals: CaloricIntakeForWeightGoalsDto | null;
  targetCaloricIntake: number | null;
  calculationType: 'calories' | 'weight' | null;
  targetWeightKg: number | null;
  targetWeeks: number | null;
  
  // Actions
  setResults: (
    caloricIntakeGoals: CaloricIntakeForWeightGoalsDto | null, 
    targetCaloricIntake: number | null,
    calculationType: 'calories' | 'weight', 
    targetWeightKg: number | null,
    targetWeeks: number | null
  ) => void;
  
  resetResults: () => void;
}

export const useCalorieStore = create<CalorieState>((set) => ({
  caloricIntakeGoals: null,
  targetCaloricIntake: null,
  calculationType: null,
  targetWeightKg: null,
  targetWeeks: null,
  
  setResults: (caloricIntakeGoals, targetCaloricIntake, calculationType, targetWeightKg = null, targetWeeks = null) => set({
    caloricIntakeGoals,
    targetCaloricIntake,
    calculationType,
    targetWeightKg,
    targetWeeks
  }),
  
  resetResults: () => set({
    caloricIntakeGoals: null,
    targetCaloricIntake: null,
    calculationType: null,
    targetWeightKg: null,
    targetWeeks: null
  })
}));