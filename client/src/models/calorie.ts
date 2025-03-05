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

export enum Genders
{
    Male = 1,
    Female = 2
} 

export enum ActivityLevels
{
    Sedentary = 1,     // Little to no exercise
    LightlyActive = 2, // Light exercise 1-3 days/week
    ModeratelyActive = 3, // Moderate exercise 3-5 days/week
    VeryActive = 4,    // Hard exercise 6-7 days/week
    ExtraActive = 5    // Very hard exercise & physical job or 2x training
}

export const ActivityLevelsDescriptions: Record<ActivityLevels, string> = {
  [ActivityLevels.Sedentary]: "Little to no exercise",
  [ActivityLevels.LightlyActive]: "Light exercise 1-3 days/week",
  [ActivityLevels.ModeratelyActive]: "Moderate exercise 3-5 days/week",
  [ActivityLevels.VeryActive]: "Hard exercise 6-7 days/week",
  [ActivityLevels.ExtraActive]: "Very hard exercise & physical job or 2x training"
};
