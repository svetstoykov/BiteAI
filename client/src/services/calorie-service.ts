import {
  CALCULATE_CALORIE_GOALS,
  CALCULATE_TARGET_DAILY_CALORIES,
} from "../constants/endpoints";
import {
  CaloricIntakeForWeightGoalsDto,
  CalorieCalculationResult,
  MetabolicProfileDto,
} from "../models/calorie";
import apiClient from "./api-client";
import { ResultWithData } from "../models/result";

export class CalorieService {
  public async calculateCalories(
    formData: MetabolicProfileDto,
    calculationType: "calories" | "weight"
  ): Promise<ResultWithData<CalorieCalculationResult>> {
    try {
      let caloricGoalsDto: CaloricIntakeForWeightGoalsDto | null = null;
      let targetIntakeCalories: number | null = null;

      if (calculationType === "calories") {
        const response = await apiClient.get<CaloricIntakeForWeightGoalsDto>(
          CALCULATE_CALORIE_GOALS,
          { params: formData }
        );

        if (!response.success) {
          return ResultWithData.failure<CalorieCalculationResult>(response.message!);
        }

        caloricGoalsDto = response.data;
      } else {
        const response = await apiClient.get<number>(CALCULATE_TARGET_DAILY_CALORIES, {
          params: formData,
        });

        if (!response.success) {
          return ResultWithData.failure<CalorieCalculationResult>(response.message!);
        }

        targetIntakeCalories = response.data;
      }

      return ResultWithData.success<CalorieCalculationResult>({
        caloricGoalsDto,
        targetIntakeCalories
      });
    } catch (err: any) {
      return ResultWithData.failure<CalorieCalculationResult>(
        err.response?.data?.message || "Something went wrong. Please try again."
      );
    }
  }
}
