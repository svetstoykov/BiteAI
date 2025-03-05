import { ActivityLevels, Genders } from "./calorie";

// Login DTO
export interface LoginDto {
  email: string;
  password: string;
}

// Register DTO
export interface RegisterDto {
  firstName: string;
  lastName: string;
  email: string;
  username: string;
  password: string;
  confirmPassword: string;
  gender?: Genders;
  age?: number;
  weightInKg?: number;
  heightInCm?: number;
  activityLevel?: ActivityLevels;
}

export interface LoginResponse {
  token: string;
  refreshToken?: string;
  firstName: string;
  lastName: string;
  email: string;
  userName: string;
  tokenExpiration: Date;
  userId: string; // Guid in C# → string in TS
}