import apiClient from './api-client';
import { ResultWithData } from '../models/result';

export interface EnumValue {
  key: number;
  value: string;
}

export class EnumService {
  static async getActivityLevels(): Promise<ResultWithData<EnumValue[]>> {
    try {
      const response = await apiClient.get<EnumValue[]>('/api/enums/activity-levels');
      return ResultWithData.success(response.data);
    } catch (error: unknown) {
      const message = (error as { response?: { data?: { message?: string } } })?.response?.data?.message || 'Failed to fetch activity levels';
      return ResultWithData.failure(message);
    }
  }

  static async getDietTypes(): Promise<ResultWithData<EnumValue[]>> {
    try {
      const response = await apiClient.get<EnumValue[]>('/api/enums/diet-types');
      return ResultWithData.success(response.data);
    } catch (error: unknown) {
      const message = (error as { response?: { data?: { message?: string } } })?.response?.data?.message || 'Failed to fetch diet types';
      return ResultWithData.failure(message);
    }
  }

  static async getGenders(): Promise<ResultWithData<EnumValue[]>> {
    try {
      const response = await apiClient.get<EnumValue[]>('/api/enums/genders');
      return ResultWithData.success(response.data);
    } catch (error: unknown) {
      const message = (error as { response?: { data?: { message?: string } } })?.response?.data?.message || 'Failed to fetch genders';
      return ResultWithData.failure(message);
    }
  }

  static async getMealTypes(): Promise<ResultWithData<EnumValue[]>> {
    try {
      const response = await apiClient.get<EnumValue[]>('/api/enums/meal-types');
      return ResultWithData.success(response.data);
    } catch (error: unknown) {
      const message = (error as { response?: { data?: { message?: string } } })?.response?.data?.message || 'Failed to fetch meal types';
      return ResultWithData.failure(message);
    }
  }

  static async getAllergies(): Promise<ResultWithData<EnumValue[]>> {
    try {
      const response = await apiClient.get<EnumValue[]>('/api/enums/allergies');
      return ResultWithData.success(response.data);
    } catch (error: unknown) {
      const message = (error as { response?: { data?: { message?: string } } })?.response?.data?.message || 'Failed to fetch allergies';
      return ResultWithData.failure(message);
    }
  }

  static async getFoodDislikes(): Promise<ResultWithData<EnumValue[]>> {
    try {
      const response = await apiClient.get<EnumValue[]>('/api/enums/food-dislikes');
      return ResultWithData.success(response.data);
    } catch (error: unknown) {
      const message = (error as { response?: { data?: { message?: string } } })?.response?.data?.message || 'Failed to fetch food dislikes';
      return ResultWithData.failure(message);
    }
  }
}