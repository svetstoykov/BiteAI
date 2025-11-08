import apiClient from './api-client';
import { Result, ResultWithData } from '../models/result';
import { UserProfile, CreateUserProfile, UpdateUserProfile } from '../models/profile';

const BASE_URL = '/profile';

export class ProfileService {
  static async getMyProfile(): Promise<ResultWithData<UserProfile>> {
    try {
      const response = await apiClient.get<UserProfile>(`${BASE_URL}/me`);
      return ResultWithData.success(response.data);
    } catch (error: any) {
      const message = error.response?.data?.message || 'Failed to fetch profile';
      return ResultWithData.failure(message);
    }
  }

  static async createProfile(data: CreateUserProfile): Promise<ResultWithData<UserProfile>> {
    try {
      const response = await apiClient.post<UserProfile, CreateUserProfile>(BASE_URL, data);
      return ResultWithData.success(response.data);
    } catch (error: any) {
      const message = error.response?.data?.message || 'Failed to create profile';
      return ResultWithData.failure(message);
    }
  }

  static async updateProfile(data: UpdateUserProfile): Promise<ResultWithData<UserProfile>> {
    try {
      const response = await apiClient.put<UserProfile, UpdateUserProfile>(`${BASE_URL}/me`, data);
      return ResultWithData.success(response.data);
    } catch (error: any) {
      const message = error.response?.data?.message || 'Failed to update profile';
      return ResultWithData.failure(message);
    }
  }

  static async deleteProfile(): Promise<Result> {
    try {
      await apiClient.delete(`${BASE_URL}/me`);
      return Result.success('Profile deleted successfully');
    } catch (error: any) {
      const message = error.response?.data?.message || 'Failed to delete profile';
      return Result.failure(message);
    }
  }

  static async hasProfile(): Promise<ResultWithData<boolean>> {
    try {
      const response = await apiClient.get<boolean>(`${BASE_URL}/exists`);
      return ResultWithData.success(response.data);
    } catch (error: any) {
      const message = error.response?.data?.message || 'Failed to check profile existence';
      return ResultWithData.failure(message);
    }
  }
}