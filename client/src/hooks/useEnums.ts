import { useState, useEffect } from 'react';
import { EnumService, EnumValue } from '../services/enum-service';

export const useEnums = () => {
  const [activityLevels, setActivityLevels] = useState<EnumValue[]>([]);
  const [dietTypes, setDietTypes] = useState<EnumValue[]>([]);
  const [genders, setGenders] = useState<EnumValue[]>([]);
  const [mealTypes, setMealTypes] = useState<EnumValue[]>([]);
  const [allergies, setAllergies] = useState<EnumValue[]>([]);
  const [foodDislikes, setFoodDislikes] = useState<EnumValue[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchEnums = async () => {
      try {
        setLoading(true);
        setError(null);

        const [
          activityResult,
          dietResult,
          genderResult,
          mealResult,
          allergyResult,
          foodDislikeResult
        ] = await Promise.all([
          EnumService.getActivityLevels(),
          EnumService.getDietTypes(),
          EnumService.getGenders(),
          EnumService.getMealTypes(),
          EnumService.getAllergies(),
          EnumService.getFoodDislikes()
        ]);

        if (activityResult.success && activityResult.data) setActivityLevels(activityResult.data);
        if (dietResult.success && dietResult.data) setDietTypes(dietResult.data);
        if (genderResult.success && genderResult.data) setGenders(genderResult.data);
        if (mealResult.success && mealResult.data) setMealTypes(mealResult.data);
        if (allergyResult.success && allergyResult.data) setAllergies(allergyResult.data);
        if (foodDislikeResult.success && foodDislikeResult.data) setFoodDislikes(foodDislikeResult.data);

        // Set error if any failed
        const errors = [
          activityResult, dietResult, genderResult, mealResult, allergyResult, foodDislikeResult
        ].filter(result => !result.success).map(result => result.message);

        if (errors.length > 0) {
          setError(errors.join('; '));
        }
      } catch {
        setError('Failed to load enum values');
      } finally {
        setLoading(false);
      }
    };

    fetchEnums();
  }, []);

  return {
    activityLevels,
    dietTypes,
    genders,
    mealTypes,
    allergies,
    foodDislikes,
    loading,
    error
  };
};