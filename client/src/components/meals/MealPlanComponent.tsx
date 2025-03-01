import React, { useEffect, useState } from 'react';
import { format, addDays } from 'date-fns';
import { MealService } from '../../services/meal-service';
import { DietTypes, MealPlan } from '../../models/meals';
import { toast } from 'react-toastify';
import { AuthenticationService } from '../../services/authentication-service';
import { useNavigate } from 'react-router-dom';

interface MealPlanComponentProps {
  targetCalories: number;
  dietType: DietTypes;
}

const MealPlanComponent: React.FC<MealPlanComponentProps> = ({ targetCalories, dietType }) => {
  const [mealPlan, setMealPlan] = useState<MealPlan | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [activeDay, setActiveDay] = useState<number>(0);
  const navigate = useNavigate();

  const mealService = new MealService();
  const authService = new AuthenticationService();

  useEffect(() => {
    if (!authService.isAuthenticated()) {
      return;
    }

    const fetchMealPlan = async () => {
      setIsLoading(true);
      setError(null);

      try {
        const result = await mealService.generateWeeklyMealPlan(3000, DietTypes.Standard);
        
        if (result.isSuccess && result.data) {
          setMealPlan(result.data);
        } else {
          toast.error(result.message || 'Failed to load meal plan');
        }
      } catch (err) {
        setError('An unexpected error occurred');
        console.error(err);
      } finally {
        setIsLoading(false);
      }
    };

    fetchMealPlan();
  }, [targetCalories, dietType]);

  // Get the current day data
  const currentDay = mealPlan?.mealDays[activeDay] || null;

  // Calculate nutrition summary for the current day
  const nutritionSummary = React.useMemo(() => {
    if (!currentDay) return null;

    let totalCalories = 0;
    let totalProtein = 0;
    let totalCarbs = 0;
    let totalFat = 0;
    let totalFiber = 0; // Assume we estimate fiber based on recipes
    
    currentDay.meals.forEach(meal => {
      totalCalories += meal.calories;
      totalProtein += meal.proteinInGrams;
      totalCarbs += meal.carbsInGrams;
      totalFat += meal.fatInGrams;
      
      // For fiber, we could estimate based on meal type or use a default ratio
      // This is just a placeholder as fiber wasn't in your meal model
      totalFiber += Math.round(meal.carbsInGrams * 0.1); // Estimating fiber as 10% of carbs
    });

    return {
      calories: totalCalories,
      protein: totalProtein,
      carbs: totalCarbs,
      fat: totalFat,
      fiber: totalFiber
    };
  }, [currentDay]);

  const handleGenerateMealPlan = async () => {
    setIsLoading(true);
    try {
      const result = await mealService.generateWeeklyMealPlan(targetCalories, dietType);
      if (result.isSuccess && result.data) {
        setMealPlan(result.data);
      } else {
        setError(result.message || 'Failed to generate new meal plan');
      }
    } catch (err) {
      setError('An unexpected error occurred');
    } finally {
      setIsLoading(false);
    }
  };

  if (isLoading) {
    return <div className="flex justify-center items-center h-64">
      <div className="animate-spin rounded-full h-10 w-10 border-b-2 border-gray-900"></div>
    </div>;
  }

  if (error) {
    return <div className="text-red-600 p-4">{error}</div>;
  }

  // Generate tab dates
  const generateTabDates = () => {
    const today = new Date();
    const tabs = [];
    
    // Add tab for each day in the meal plan (usually 7 days)
    for (let i = 0; i < (mealPlan?.durationDays || 7); i++) {
      const date = addDays(today, i);
      let label = i === 0 ? 'Today' : i === 1 ? 'Tomorrow' : format(date, 'EEE, MMM d');
      
      tabs.push(
        <button
          key={i}
          className={`px-4 py-2 ${activeDay === i ? 'border-b-2 border-black font-medium' : 'text-gray-600'}`}
          onClick={() => setActiveDay(i)}
        >
          {label}
        </button>
      );
    }
    
    // Add "Add a Meal" tab
    tabs.push(
      <button
        key="add"
        className="px-4 py-2 text-gray-600"
        onClick={() => {/* Handle add meal functionality */}}
      >
        Add a Meal
      </button>
    );
    
    return tabs;
  };

  return (
    <div className="max-w-4xl mx-auto p-6">
      <h1 className="text-2xl font-bold mb-6">Meal Plan</h1>
      
      {/* Tabs Navigation */}
      <div className="flex overflow-x-auto border-b mb-6 pb-1">
        {generateTabDates()}
      </div>
      
      {/* Meal List */}
      <div className="mb-8">
        {currentDay?.meals.map((meal, index) => (
          <div key={meal.id || index} className="flex items-center mb-6">
            {/* Time and meal info with circle */}
            <div className="flex items-center bg-gray-100 p-4 rounded-lg flex-grow mr-4">
              <div className="w-8 h-8 rounded-full bg-white flex items-center justify-center mr-4">
                <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5 text-gray-500" viewBox="0 0 20 20" fill="currentColor">
                  <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm1-12a1 1 0 10-2 0v4a1 1 0 00.293.707l2.828 2.829a1 1 0 101.415-1.415L11 9.586V6z" clipRule="evenodd" />
                </svg>
              </div>
              <div>
                <p className="font-medium">{getMealTimeByType(meal.mealType)}</p>
                <p className="text-gray-600">{meal.name}</p>
                <p className="text-gray-500 text-sm">{meal.mealType.toString()}</p>
              </div>
            </div>
            
            {/* Edit button */}
            <button className="text-gray-700 font-medium">Edit</button>
          </div>
        ))}
        
        {/* Bullet point summary section */}
        <div className="mt-6 mb-8">
          {currentDay?.meals.map((meal, index) => (
            <div key={`summary-${meal.id || index}`} className="flex items-start mb-4">
              <span className="mt-1.5 h-2 w-2 rounded-full bg-black block mr-4"></span>
              <div>
                <p className="font-medium">{getMealTimeByType(meal.mealType)}</p>
                <p className="text-gray-600">{meal.name}</p>
              </div>
            </div>
          ))}
        </div>
        
        {/* Generate Meal Plan Button */}
        <div className="flex justify-center mb-8">
          <button 
            className="bg-gray-200 hover:bg-gray-300 text-gray-800 font-medium py-2 px-6 rounded"
            onClick={handleGenerateMealPlan}
          >
            Generate Meal Plan
          </button>
        </div>
      </div>
      
      {/* Nutrition Summary */}
      <div>
        <h2 className="text-xl font-bold mb-4 border-b pb-2">Nutrition Summary</h2>
        
        <div className="space-y-4">
          <div className="flex justify-between border-b pb-2">
            <span>Calories</span>
            <span>{nutritionSummary?.calories || 0}</span>
          </div>
          
          <div className="flex justify-between border-b pb-2">
            <span>Protein</span>
            <span>{nutritionSummary?.protein || 0}g</span>
          </div>
          
          <div className="flex justify-between border-b pb-2">
            <span>Carbs</span>
            <span>{nutritionSummary?.carbs || 0}g</span>
          </div>
          
          <div className="flex justify-between border-b pb-2">
            <span>Fat</span>
            <span>{nutritionSummary?.fat || 0}g</span>
          </div>
          
          <div className="flex justify-between border-b pb-2">
            <span>Fiber</span>
            <span>{nutritionSummary?.fiber || 0}g</span>
          </div>
        </div>
      </div>
    </div>
  );
};

// Helper function to get meal time based on meal type
function getMealTimeByType(mealType: number): string {
  switch (mealType) {
    case 0: // Breakfast
      return "8:00 AM";
    case 1: // Lunch
      return "12:00 PM";
    case 2: // Dinner
      return "6:00 PM";
    case 3: // Snack
      return "3:00 PM";
    default:
      return "12:00 PM";
  }
}

export default MealPlanComponent;