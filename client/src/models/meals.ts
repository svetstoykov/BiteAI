export enum DietTypes {
  Standard = 0,
  Vegetarian = 1,
  Vegan = 2,
  Keto = 3,
  LowCarb = 4,
  Paleo = 5,
  Mediterranean = 6,
  GlutenFree = 7,
  DairyFree = 8,
}

export enum MealTypes {
  Breakfast = 0,
  Lunch = 1,
  Dinner = 2,
  Snack = 3,
}

export interface Meal {
  id: string;
  name: string;
  recipe: string;
  calories: number;
  mealOrder: number;
  proteinInGrams: number;
  carbsInGrams: number;
  fatInGrams: number;
  mealType: MealTypes;
}

export interface MealDay {
  id: string;
  dayNumber: number;
  totalCalories: number;
  dailyMeals: Meal[];
}

export interface MealPlan {
  id: string;
  createdDate: string;
  dailyCalories: number;
  dietType: DietTypes;
  durationDays: number;
  mealDays: MealDay[];
}

export interface MealPlanRequest {
  dailyTargetCalories: number;
  dietType: DietTypes;
}


// Descriptions for enum values
export const DietTypeDescriptions: Record<DietTypes, string> = {
  [DietTypes.Standard]: "Standard",
  [DietTypes.Vegetarian]: "Vegetarian",
  [DietTypes.Vegan]: "Vegan",
  [DietTypes.Keto]: "Keto",
  [DietTypes.LowCarb]: "Low Carb",
  [DietTypes.Paleo]: "Paleo",
  [DietTypes.Mediterranean]: "Mediterranean",
  [DietTypes.GlutenFree]: "Gluten Free",
  [DietTypes.DairyFree]: "Dairy Free",
};
