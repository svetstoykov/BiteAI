export enum Genders
{
    Male = 1,
    Female = 2
} 

export enum ActivityLevels
{
    NotSpecified = 0,
    Sedentary = 1,     // Little to no exercise
    LightlyActive = 2, // Light exercise 1-3 days/week
    ModeratelyActive = 3, // Moderate exercise 3-5 days/week
    VeryActive = 4,    // Hard exercise 6-7 days/week
    ExtraActive = 5    // Very hard exercise & physical job or 2x training
}

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