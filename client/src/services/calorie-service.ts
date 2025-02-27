import {
  CaloricIntakeForWeightGoalsDto,
  CalorieCalculationResult,
  MetabolicProfileDto,
} from "../models/calorie";
import apiClient from "./api-client";

export class CalorieService {
  public async calculateCalories(
    formData: MetabolicProfileDto,
    calculationType: "calories" | "weight"
  ): Promise<CalorieCalculationResult> {
    try {
      let caloricGoalsDto: CaloricIntakeForWeightGoalsDto | null = null;
      let targetIntakeCalories: number | null = null;

      if (calculationType === "calories") {
        const response = await apiClient.get<CaloricIntakeForWeightGoalsDto>(
          "/calories/calculate-goals",
          { params: formData }
        );

        if (response.success) {
          caloricGoalsDto = response.data;
        } else {
          throw new Error(response.message);
        }
      } else {
        const response = await apiClient.get<number>(
          "/calories/calculate-target-daily-calories",
          { params: formData }
        );

        if (response.success) {
          targetIntakeCalories = response.data;
        } else {
          throw new Error(response.message);
        }
      }

      return {
        caloricGoalsDto,
        targetIntakeCalories,
        error: null,
      };
    } catch (err: any) {
      return {
        caloricGoalsDto: null,
        targetIntakeCalories: null,
        error: err.response.data.message || "Something went wrong. Please try again.",
      };
    }
  }
}
