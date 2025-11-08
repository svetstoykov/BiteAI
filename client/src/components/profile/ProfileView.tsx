import React from 'react';
import { UserProfile } from '../../models/profile';
import { Genders, ActivityLevels } from '../../models/calorie';
import { DietTypes, DietTypeDescriptions } from '../../models/meals';

interface ProfileViewProps {
  profile: UserProfile;
  onEdit?: () => void;
}

const ProfileView: React.FC<ProfileViewProps> = ({ profile, onEdit }) => {
  const getGenderLabel = (gender: Genders) => {
    return gender === Genders.Male ? 'Male' : 'Female';
  };

  const getActivityLabel = (activity: ActivityLevels) => {
    switch (activity) {
      case ActivityLevels.Sedentary: return 'Sedentary';
      case ActivityLevels.LightlyActive: return 'Lightly Active';
      case ActivityLevels.ModeratelyActive: return 'Moderately Active';
      case ActivityLevels.VeryActive: return 'Very Active';
      case ActivityLevels.ExtraActive: return 'Extra Active';
      default: return 'Unknown';
    }
  };

  const getDietLabel = (diet?: DietTypes) => {
    return diet ? DietTypeDescriptions[diet] : 'Not specified';
  };

  const calculateCompleteness = () => {
    const requiredFields = ['gender', 'age', 'weightInKg', 'heightInCm', 'activityLevel'];
    const optionalFields = ['preferredDietType', 'allergies', 'foodDislikes'];

    const requiredComplete = requiredFields.every(field => {
      const value = profile[field as keyof UserProfile];
      return value !== null && value !== undefined && value !== '';
    });

    const optionalComplete = optionalFields.some(field => {
      const value = profile[field as keyof UserProfile];
      return value !== null && value !== undefined && (Array.isArray(value) ? value.length > 0 : value !== '');
    });

    let completeness = 0;
    if (requiredComplete) completeness += 70;
    if (optionalComplete) completeness += 30;

    return completeness;
  };

  const completeness = calculateCompleteness();

  return (
    <div className="max-w-md mx-auto bg-white p-6 rounded-lg shadow-md">
      <div className="flex justify-between items-center mb-6">
        <h2 className="text-2xl font-bold">Your Profile</h2>
        {onEdit && (
          <button
            onClick={onEdit}
            className="bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700"
          >
            Edit Profile
          </button>
        )}
      </div>

      {/* Profile Completeness Indicator */}
      <div className="mb-6">
        <div className="flex justify-between items-center mb-2">
          <span className="text-sm font-medium text-gray-700">Profile Completeness</span>
          <span className="text-sm text-gray-600">{completeness}%</span>
        </div>
        <div className="w-full bg-gray-200 rounded-full h-2">
          <div
            className="bg-green-600 h-2 rounded-full transition-all duration-300"
            style={{ width: `${completeness}%` }}
          ></div>
        </div>
        {completeness < 100 && (
          <p className="text-xs text-gray-500 mt-1">
            Complete your profile for better personalized recommendations
          </p>
        )}
      </div>

      {/* Profile Details */}
      <div className="space-y-4">
        <div className="grid grid-cols-2 gap-4">
          <div>
            <label className="block text-sm font-medium text-gray-700">Gender</label>
            <p className="text-gray-900">{getGenderLabel(profile.gender)}</p>
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700">Age</label>
            <p className="text-gray-900">{profile.age} years</p>
          </div>
        </div>

        <div className="grid grid-cols-2 gap-4">
          <div>
            <label className="block text-sm font-medium text-gray-700">Weight</label>
            <p className="text-gray-900">{profile.weightInKg} kg</p>
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700">Height</label>
            <p className="text-gray-900">{profile.heightInCm} cm</p>
          </div>
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700">Activity Level</label>
          <p className="text-gray-900">{getActivityLabel(profile.activityLevel)}</p>
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700">Preferred Diet</label>
          <p className="text-gray-900">{getDietLabel(profile.preferredDietType)}</p>
        </div>

        {profile.allergies && profile.allergies.length > 0 && (
          <div>
            <label className="block text-sm font-medium text-gray-700">Allergies</label>
            <p className="text-gray-900">{profile.allergies.join(', ')}</p>
          </div>
        )}

        {profile.foodDislikes && profile.foodDislikes.length > 0 && (
          <div>
            <label className="block text-sm font-medium text-gray-700">Food Dislikes</label>
            <p className="text-gray-900">{profile.foodDislikes.join(', ')}</p>
          </div>
        )}

        <div className="pt-4 border-t">
          <div className="text-xs text-gray-500">
            Last updated: {new Date(profile.updatedAt).toLocaleDateString()}
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProfileView;