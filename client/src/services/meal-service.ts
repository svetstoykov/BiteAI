import { LATEST_MEAL_PLAN, WEEKLY_MEAL_PLAN } from "../constants/endpoints";
import { DietTypes, MealPlan, MealPlanRequest } from "../models/meals";
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
        WEEKLY_MEAL_PLAN,
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
      const response = await apiClient.get<MealPlan>(LATEST_MEAL_PLAN);

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
}
