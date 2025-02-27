// src/stores/calorieStore.ts
import { create } from 'zustand';

interface CalorieState {
  results: any;
  calculationType: 'calories' | 'weight' | null;
  targetWeightKg: string | null;
  targetWeeks: string | null;
  
  // Actions
  setResults: (
    results: any, 
    calculationType: 'calories' | 'weight', 
    targetWeightKg: string | null,
    targetWeeks: string | null
  ) => void;
  
  resetResults: () => void;
}

export const useCalorieStore = create<CalorieState>((set) => ({
  results: null,
  calculationType: null,
  targetWeightKg: null,
  targetWeeks: null,
  
  setResults: (results, calculationType, targetWeightKg = null, targetWeeks = null) => set({
    results,
    calculationType,
    targetWeightKg,
    targetWeeks
  }),
  
  resetResults: () => set({
    results: null,
    calculationType: null,
    targetWeightKg: null,
    targetWeeks: null
  })
}));