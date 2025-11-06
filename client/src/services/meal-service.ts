import { CREATE_GROCERY_LIST, GET_LATEST_MEAL_PLAN, CREATE_WEEKLY_MEAL_PLAN } from "../constants/endpoints";
import { DietTypes, GroceryList, MealPlan, MealPlanRequest } from "../models/meals";
import { ResultWithData } from "../models/result";
import apiClient from "./api-client";

export class MealService {
  /**
   * Generate a weekly meal plan based on calories and diet preferences
   * @param dailyTargetCalories Target daily calorie intake
   * @param dietType Type of diet (e.g., Keto, Vegan, etc.)
   * @returns Result with meal plan data or error message
   */
  public async generateWeeklyMealPlan(
    dailyTargetCalories: number,
    dietType: DietTypes
  ): Promise<ResultWithData<MealPlan>> {
    try {
      const request: MealPlanRequest = {
        dailyTargetCalories,
        dietType,
      };

      const response = await apiClient.post<MealPlan, MealPlanRequest>(
        CREATE_WEEKLY_MEAL_PLAN,
        request
      );

      if (response.success && response.data) {
        return ResultWithData.success(response.data);
      }
      return ResultWithData.failure(response.message || "Failed to generate meal plan");
    } catch (err: any) {
      return ResultWithData.failure(
        err.response?.data?.message || "Failed to generate meal plan. Please try again."
      );
    }
  }

  /**
   * Get latest meal plan for logged in user.
   * @returns Result with meal plan data or error message
   */
  public async getLatestMealPlan(): Promise<ResultWithData<MealPlan>> {
    try {
      const response = await apiClient.get<MealPlan>(GET_LATEST_MEAL_PLAN);

      if (response.success && response.data) {
        return ResultWithData.success(response.data);
      }
      return ResultWithData.failure(response.message || "Failed to retrieve meal plan");
    } catch (err: any) {
      return ResultWithData.failure(
        err.response?.data?.message || "Failed to retrieve meal plan. Please try again."
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
}
