import React, { useState, useEffect } from 'react';
import { Genders, ActivityLevels } from '../../models/calorie';
import { CreateUserProfile } from '../../models/profile';
import { useProfile } from '../../hooks/useProfile';
import { useEnums } from '../../hooks/useEnums';
import Input from '../common/Input';
import Select, { SelectOption } from '../common/Select';
import MultiSelect, { MultiSelectOption } from '../common/MultiSelect';

interface ProfileFormProps {
  mode: 'create' | 'edit';
  onSuccess?: () => void;
  onCancel?: () => void;
}

const ProfileForm: React.FC<ProfileFormProps> = ({ mode, onSuccess, onCancel }) => {
  const { profile, createProfile, updateProfile, loading, error } = useProfile();
  const {
    genders,
    activityLevels,
    dietTypes,
    allergies,
    foodDislikes
  } = useEnums();

  const [formData, setFormData] = useState<CreateUserProfile>({
    gender: Genders.Male,
    age: 0,
    weightInKg: 0,
    heightInCm: 0,
    activityLevel: ActivityLevels.Sedentary,
    preferredDietType: undefined,
    allergies: [],
    foodDislikes: [],
  });

  const [formErrors, setFormErrors] = useState<Record<string, string>>({});

  useEffect(() => {
    if (mode === 'edit' && profile) {
      setFormData({
        gender: profile.gender,
        age: profile.age,
        weightInKg: profile.weightInKg,
        heightInCm: profile.heightInCm,
        activityLevel: profile.activityLevel,
        preferredDietType: profile.preferredDietType,
        allergies: profile.allergies || [],
        foodDislikes: profile.foodDislikes || [],
      });
    }
  }, [mode, profile]);

  const genderOptions: SelectOption[] = genders.map(g => ({ value: g.key, label: g.value }));

  const activityOptions: SelectOption[] = activityLevels.map(a => ({ value: a.key, label: a.value }));

  const dietOptions: SelectOption[] = dietTypes.map(d => ({ value: d.key, label: d.value }));

  const allergyOptions: MultiSelectOption[] = allergies.map(a => ({ value: a.key, label: a.value }));

  const foodDislikeOptions: MultiSelectOption[] = foodDislikes.map(f => ({ value: f.key, label: f.value }));

  const validateForm = (): boolean => {
    const errors: Record<string, string> = {};

    if (formData.age <= 0 || formData.age > 120) {
      errors.age = 'Please enter a valid age (1-120)';
    }
    if (formData.weightInKg <= 0 || formData.weightInKg > 500) {
      errors.weightInKg = 'Please enter a valid weight (1-500 kg)';
    }
    if (formData.heightInCm <= 0 || formData.heightInCm > 250) {
      errors.heightInCm = 'Please enter a valid height (1-250 cm)';
    }

    setFormErrors(errors);
    return Object.keys(errors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!validateForm()) return;

    const data = formData;

    let result;
    if (mode === 'create') {
      result = await createProfile(data);
    } else {
      result = await updateProfile(data);
    }

    if (result.success) {
      onSuccess?.();
    }
  };

  const handleInputChange = (field: keyof CreateUserProfile, value: unknown) => {
    setFormData(prev => ({ ...prev, [field]: value }));
    if (formErrors[field]) {
      setFormErrors(prev => ({ ...prev, [field]: '' }));
    }
  };

  return (
    <div className="max-w-md mx-auto bg-white p-6 rounded-lg shadow-md">
      <h2 className="text-2xl font-bold mb-6 text-center">
        {mode === 'create' ? 'Create Your Profile' : 'Edit Profile'}
      </h2>

      {error && (
        <div className="mb-4 p-3 bg-red-100 border border-red-400 text-red-700 rounded">
          {error}
        </div>
      )}

      <form onSubmit={handleSubmit} className="space-y-4">
        <Select
          id="gender"
          label="Gender"
          value={formData.gender.toString()}
          onChange={(e) => handleInputChange('gender', parseInt(e.target.value))}
          options={genderOptions}
          required
        />

        <Input
          id="age"
          type="number"
          label="Age"
          value={formData.age.toString()}
          onChange={(e) => handleInputChange('age', parseInt(e.target.value) || 0)}
          placeholder="Enter your age"
          error={formErrors.age}
          required
        />

        <Input
          id="weight"
          type="number"
          label="Weight (kg)"
          value={formData.weightInKg.toString()}
          onChange={(e) => handleInputChange('weightInKg', parseFloat(e.target.value) || 0)}
          placeholder="Enter your weight in kg"
          error={formErrors.weightInKg}
          required
        />

        <Input
          id="height"
          type="number"
          label="Height (cm)"
          value={formData.heightInCm.toString()}
          onChange={(e) => handleInputChange('heightInCm', parseInt(e.target.value) || 0)}
          placeholder="Enter your height in cm"
          error={formErrors.heightInCm}
          required
        />

        <Select
          id="activity"
          label="Activity Level"
          value={formData.activityLevel.toString()}
          onChange={(e) => handleInputChange('activityLevel', parseInt(e.target.value))}
          options={activityOptions}
          required
        />

        <Select
          id="diet"
          label="Preferred Diet Type (Optional)"
          value={formData.preferredDietType?.toString() || ''}
          onChange={(e) => handleInputChange('preferredDietType', e.target.value ? parseInt(e.target.value) : undefined)}
          options={dietOptions}
        />

        <MultiSelect
          id="allergies"
          label="Allergies (Optional)"
          value={formData.allergies?.map(a => a) || []}
          onChange={(values) => handleInputChange('allergies', values)}
          options={allergyOptions}
          placeholder="Select your allergies..."
        />

        <MultiSelect
          id="dislikes"
          label="Food Dislikes (Optional)"
          value={formData.foodDislikes?.map(d => d) || []}
          onChange={(values) => handleInputChange('foodDislikes', values)}
          options={foodDislikeOptions}
          placeholder="Select foods you dislike..."
        />

        <div className="flex space-x-4 pt-4">
          <button
            type="submit"
            disabled={loading}
            className="flex-1 bg-blue-600 text-white py-2 px-4 rounded-md hover:bg-blue-700 disabled:opacity-50"
          >
            {loading ? 'Saving...' : mode === 'create' ? 'Create Profile' : 'Update Profile'}
          </button>
          {onCancel && (
            <button
              type="button"
              onClick={onCancel}
              className="flex-1 bg-gray-300 text-gray-700 py-2 px-4 rounded-md hover:bg-gray-400"
            >
              Cancel
            </button>
          )}
        </div>
      </form>
    </div>
  );
};

export default ProfileForm;