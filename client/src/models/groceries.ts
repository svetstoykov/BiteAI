export interface GroceryItem {
  id: string;
  name: string;
  quantity: number;
  unit: string;
  category: string;
  checked: boolean;
}

export interface GroceryList {
  mealPlanId: string;
  items: GroceryItem[];
  generatedAt: string;
}

export interface GroceryListResponse {
  data: GroceryList;
  message: string;
  success: boolean;
}