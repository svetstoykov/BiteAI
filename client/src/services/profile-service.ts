import apiClient from './api-client';
import { Result, ResultWithData } from '../models/result';
import { UserProfile, CreateUserProfile, UpdateUserProfile } from '../models/profile';
import { GET_MY_PROFILE, CREATE_PROFILE, UPDATE_PROFILE, DELETE_PROFILE, CHECK_PROFILE_EXISTS } from '../constants/endpoints';

export class ProfileService {
  static async getMyProfile(): Promise<ResultWithData<UserProfile>> {
    try {
      const response = await apiClient.get<UserProfile>(GET_MY_PROFILE);
      return ResultWithData.success(response.data);
    } catch (error: unknown) {
      const message = (error as { response?: { data?: { message?: string } } })?.response?.data?.message || 'Failed to fetch profile';
      return ResultWithData.failure(message);
    }
  }

  static async createProfile(data: CreateUserProfile): Promise<ResultWithData<UserProfile>> {
    try {
      const response = await apiClient.post<UserProfile, CreateUserProfile>(CREATE_PROFILE, data);
      return ResultWithData.success(response.data);
    } catch (error: unknown) {
      const message = (error as { response?: { data?: { message?: string } } })?.response?.data?.message || 'Failed to create profile';
      return ResultWithData.failure(message);
    }
  }

  static async updateProfile(data: UpdateUserProfile): Promise<ResultWithData<UserProfile>> {
    try {
      const response = await apiClient.put<UserProfile, UpdateUserProfile>(UPDATE_PROFILE, data);
      return ResultWithData.success(response.data);
    } catch (error: unknown) {
      const message = (error as { response?: { data?: { message?: string } } })?.response?.data?.message || 'Failed to update profile';
      return ResultWithData.failure(message);
    }
  }

  static async deleteProfile(): Promise<Result> {
    try {
      await apiClient.delete(DELETE_PROFILE);
      return Result.success('Profile deleted successfully');
    } catch (error: unknown) {
      const message = (error as { response?: { data?: { message?: string } } })?.response?.data?.message || 'Failed to delete profile';
      return Result.failure(message);
    }
  }

  static async hasProfile(): Promise<ResultWithData<boolean>> {
    try {
      const response = await apiClient.get<boolean>(CHECK_PROFILE_EXISTS);
      return ResultWithData.success(response.data);
    } catch (error: unknown) {
      const message = (error as { response?: { data?: { message?: string } } })?.response?.data?.message || 'Failed to check profile existence';
      return ResultWithData.failure(message);
    }
  }
}