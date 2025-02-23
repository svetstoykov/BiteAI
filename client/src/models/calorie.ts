export type CalculationType = 'goals' | 'target';

export interface MetabolicProfileDto {
    weightKg: string;
    heightCm: string;
    age: string;
    exerciseDaysPerWeek: string;
    isMale: boolean;
}

export interface TargetCalorieFormData extends MetabolicProfileDto {
    targetWeightKg: string;
    targetWeeks: string;
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

// Form state type that can handle both types of calculations
export type CalorieFormData = MetabolicProfileDto & Partial<Pick<TargetCalorieFormData, 'targetWeightKg' | 'targetWeeks'>>;