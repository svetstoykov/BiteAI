export type CalculationType = "goals" | "target";

export interface MetabolicProfileDto {
  weightKg: number;
  heightCm: number;
  age: number;
  exerciseDaysPerWeek: number;
  isMale: boolean;
}

export interface CalorieCalculationGoalsFormData extends MetabolicProfileDto {
  targetWeightKg: number | null;
  targetWeeks: number | null;
}

export interface WeightGoalPlanDto {
  metabolicProfile: MetabolicProfileDto;
  targetWeightKg: number;
  targetWeeks: number;
}

export interface CaloricIntakeForWeightGoalsDto {
  maintainCalories: number;
  loseOneKgCalories: number;
  loseHalfKgCalories: number;
  gainHalfKgCalories: number;
  gainOneKgCalories: number;
}

export interface CalorieCalculationResult {
  caloricGoalsDto: CaloricIntakeForWeightGoalsDto | null;
  targetIntakeCalories: number | null;
}
