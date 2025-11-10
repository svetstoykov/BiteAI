import { Genders } from './calorie';
import { ActivityLevels } from './calorie';
import { DietTypes } from './meals';

export interface UserProfile {
  id: string;
  userId: string;
  gender: Genders;
  age: number;
  weightInKg: number;
  heightInCm: number;
  activityLevel: ActivityLevels;
  preferredDietType?: DietTypes;
  allergies?: number[];
  foodDislikes?: number[];
  createdAt: string; // ISO format
  updatedAt: string; // ISO format
}

export interface CreateUserProfile {
  gender: Genders;
  age: number;
  weightInKg: number;
  heightInCm: number;
  activityLevel: ActivityLevels;
  preferredDietType?: DietTypes;
  allergies?: number[];
  foodDislikes?: number[];
}

export interface UpdateUserProfile extends Partial<UserProfile> {}