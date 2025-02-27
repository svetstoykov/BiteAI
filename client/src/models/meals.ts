export enum DietTypes {
  Standard = 0,
  Vegetarian = 1,
  Vegan = 2,
  Keto = 3,
  LowCarb = 4,
  Paleo = 5,
  Mediterranean = 6,
  GlutenFree = 7,
  DairyFree = 8
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
  [DietTypes.DairyFree]: "Dairy Free"
};
