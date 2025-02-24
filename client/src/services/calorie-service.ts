import { toast } from "react-toastify";
import {
  CaloricIntakeForWeightGoalsDto,
  CalorieGoalsFormData,
  TargetCalorieFormData,
} from "../models/calorie";
import apiClient from "./api-client";

export default class CalorieService {
  private readonly baseUrl = "http://localhost:5036";

  private async makeRequest<T>(
    endpoint: string,
    params: object,
    errorMessage: string
  ): Promise<T | undefined> {
    try {
      const queryParams = new URLSearchParams(
        Object.entries(params).map(([key, value]) => [key, String(value)])
      );
      const url = `${this.baseUrl}${endpoint}?${queryParams}`;

      const response = await apiClient.get<T>(url);

      if (!response.isSuccess) {
        toast.error(`${errorMessage} ${response.message}`);
        return;
      }

      return response.data;
    } catch (error: unknown) {
      const errorMessage = error instanceof Error ? error.message : "Operation error";
      toast.error(errorMessage);
    }
  }

  public async calculateTargetDailyCalories(
    request: TargetCalorieFormData
  ): Promise<number | undefined> {
    return this.makeRequest<number>(
      "/api/calories/calculate-target-daily-calories",
      request,
      "Failed to calculate calories."
    );
  }

  public async calculateCalorieGoals(
    request: CalorieGoalsFormData
  ): Promise<CaloricIntakeForWeightGoalsDto | undefined> {
    return this.makeRequest<CaloricIntakeForWeightGoalsDto>(
      "/api/calories/calculate-goals",
      request,
      "Failed to calculate calorie goals."
    );
  }
}
