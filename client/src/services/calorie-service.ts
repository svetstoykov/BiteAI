import { toast } from "react-toastify";
import { ApiResponse } from "../models/api";
import {
  CaloricIntakeForWeightGoalsDto,
  CalorieGoalsFormData,
  TargetCalorieFormData,
} from "../models/calorie";

export default class CalorieService {
  private readonly baseUrl = "http://localhost:5036";

  private async makeRequest<T>(
    endpoint: string,
    params: Record<string, any>,
    errorMessage: string
  ): Promise<T | undefined> {
    try {
      const queryParams = new URLSearchParams(
        Object.entries(params).map(([key, value]) => [key, String(value)])
      );
      const url = `${this.baseUrl}${endpoint}?${queryParams}`;

      const response = await fetch(url);
      const content: ApiResponse<T> = await response.json();

      if (!response.ok) {
        toast.error(`${errorMessage} ${content.message}`);
        return;
      }

      return content.data;
    } catch (error) {
      toast.error(error ?? `An error occurred while ${errorMessage.toLowerCase()}`);
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
