import { CREATE_GROCERY_LIST, GET_GROCERY_LIST, CHECK_GROCERY_ITEM } from "../constants/endpoints";
import { GroceryList } from "../models/groceries";
import { ResultWithData } from "../models/result";
import apiClient from "./api-client";

export class GroceryService {
  /**
   * Get existing grocery list for a meal plan.
   * @param mealPlanId The ID of the meal plan
   * @returns Result with grocery list data or error message
   */
  public async getGroceryList(mealPlanId: string): Promise<ResultWithData<GroceryList>> {
    try {
      const endpoint = GET_GROCERY_LIST.replace("{mealPlanId}", mealPlanId);
      const response = await apiClient.get<GroceryList>(endpoint);

      if (response.success && response.data) {
        return ResultWithData.success(response.data);
      }
      return ResultWithData.failure(response.message || "Failed to retrieve grocery list");
    } catch (err: any) {
      return ResultWithData.failure(
        err.response?.data?.message || "Failed to retrieve grocery list. Please try again."
      );
    }
  }

  /**
   * Generate grocery list for a meal plan.
   * @param mealPlanId The ID of the meal plan
   * @returns Result with grocery list data or error message
   */
  public async generateGroceryList(mealPlanId: string): Promise<ResultWithData<GroceryList>> {
    try {
      const endpoint = CREATE_GROCERY_LIST.replace("{mealPlanId}", mealPlanId);
      const response = await apiClient.post<GroceryList, {}>(endpoint, {});

      if (response.success && response.data) {
        return ResultWithData.success(response.data);
      }
      return ResultWithData.failure(response.message || "Failed to generate grocery list");
    } catch (err: any) {
      return ResultWithData.failure(
        err.response?.data?.message || "Failed to generate grocery list. Please try again."
      );
    }
  }

  /**
   * Toggle the checked state of a grocery list item.
   * @param groceryListItemId The ID of the grocery list item
   * @returns Result with success or error message
   */
  public async toggleGroceryItemCheck(groceryListItemId: string): Promise<ResultWithData<boolean>> {
    try {
      const endpoint = CHECK_GROCERY_ITEM.replace("{groceryListItemId}", groceryListItemId);
      const response = await apiClient.post<boolean, {}>(endpoint, {});

      if (response.success) {
        return ResultWithData.success(response.data || true);
      }
      return ResultWithData.failure(response.message || "Failed to toggle item check");
    } catch (err: any) {
      return ResultWithData.failure(
        err.response?.data?.message || "Failed to toggle item check. Please try again."
      );
    }
  }
}