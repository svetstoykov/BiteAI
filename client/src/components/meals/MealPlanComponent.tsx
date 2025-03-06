import React, { useEffect, useState } from "react";
import { MealService } from "../../services/meal-service";
import { DietTypes, MealPlan, MealTypes } from "../../models/meals";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";
import { useCalorieStore } from "../../stores/calorie-store";
import { motion, AnimatePresence } from "framer-motion";
import { Apple, Coffee, Soup, UtensilsCrossed, ChevronDown } from "lucide-react";
import MealPlanPreparationSpinner from "./MealPlanPreparationSpinner";

const MealPlanComponent = () => {
  const [mealPlan, setMealPlan] = useState<MealPlan | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [activeDay, setActiveDay] = useState<number>(0);
  const [expandedMeals, setExpandedMeals] = useState<{ [key: string]: boolean }>({});
  const navigate = useNavigate();

  const { dailyCalories, dietType } = useCalorieStore();

  const mealService = new MealService();

  useEffect(() => {
    const fetchMealPlan = async () => {
      setIsLoading(true);

      try {
        const existingMealPlanResult = await mealService.getLatestMealPlan();
        if (existingMealPlanResult.success) {
          setMealPlan(existingMealPlanResult.data!);
          return;
        }

        toast.error("Failed to retrieve latest meal plan!");

        if (dailyCalories === null || dietType === null) {
          navigate("/setup");
          return;
        }

        const result = await mealService.generateWeeklyMealPlan(dailyCalories, dietType);

        if (result.success) {
          setMealPlan(result.data!);
          return;
        }

        toast.error(result.message);
      } catch (err) {
        toast.error("An unexpected error occurred");
      } finally {
        setIsLoading(false);
      }
    };

    fetchMealPlan();
  }, [dailyCalories, dietType]);

  // Reset expanded meals when changing day
  useEffect(() => {
    setExpandedMeals({});
  }, [activeDay]);

  const handleGenerateNewMealClick = async () => {
    const result = await mealService.generateWeeklyMealPlan(
      mealPlan?.dailyCalories!,
      mealPlan?.dietType!
    );

    if (result.success) {
      setMealPlan(result.data!);
      return;
    }
  };

  // Get the current day data
  const currentDay = mealPlan?.mealDays[activeDay] || null;

  const toggleMealExpand = (mealId: string) => {
    setExpandedMeals((prev) => ({
      ...prev,
      [mealId]: !prev[mealId],
    }));
  };

  // Calculate nutrition summary for the current day
  const nutritionSummary = React.useMemo(() => {
    if (!currentDay) return null;

    let totalCalories = 0;
    let totalProtein = 0;
    let totalCarbs = 0;
    let totalFat = 0;
    let totalFiber = 0; // Assume we estimate fiber based on recipes

    currentDay.dailyMeals.forEach((meal) => {
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
      fiber: totalFiber,
    };
  }, [currentDay]);

  if (isLoading) {
    return <MealPlanPreparationSpinner />;
  }

  // Map meal types to Lucide icons
  const getMealIcon = (mealType: MealTypes) => {
    switch (mealType) {
      case MealTypes.Breakfast:
        return <Coffee size={20} className="text-gray-500" />;
      case MealTypes.Lunch:
        return <Soup size={20} className="text-gray-500" />;
      case MealTypes.Dinner:
        return <UtensilsCrossed size={20} className="text-gray-500" />;
      case MealTypes.Snack:
        return <Apple size={20} className="text-gray-500" />;
      default:
        return <UtensilsCrossed size={20} className="text-gray-500" />;
    }
  };

  // Generate tab dates
  const generateTabDates = () => {
    const tabs = [];

    // Add tab for each day in the meal plan (usually 7 days)
    for (let i = 0; i < (mealPlan?.durationDays || 7); i++) {
      const dayLabel = i + 1;

      tabs.push(
        <button
          key={i}
          className={`py-2 text-center md:min-w-16  ${
            activeDay === i ? "border-b-2 border-black font-medium" : "text-gray-600"
          }`}
          onClick={() => setActiveDay(i)}
        >
          <div className="whitespace-nowrap">{dayLabel}</div>
          <div className="text-sm whitespace-nowrap">Day</div>
        </button>
      );
    }

    return tabs;
  };

  return (
    <div className="mx-auto md:min-w-2xl min-w-[370px] p-2 md:p-6 bite-container">
      <h1 className="text-3xl md:text-4xl font-thin mb-4 md:mb-6 text-center md:text-start">
        Meal Plan
      </h1>

      {/* Tabs Navigation */}
      <div className="flex justify-between pb-1 md:gap-2 border-b mb-4 md:mb-6">
        {generateTabDates()}
      </div>

      {/* Meal List with simple fade animation */}
      <div className="mb-8 relative">
        <AnimatePresence mode="wait" initial={false}>
          <motion.div
            key={activeDay}
            initial={{ opacity: 0, scale: 0.95 }}
            animate={{ opacity: 1, scale: 1 }}
            exit={{ opacity: 0, scale: 0.95 }}
            transition={{ duration: 0.4 }}
            className=""
          >
            {currentDay?.dailyMeals.map((meal, index) => {
              const mealId = meal.id || `meal-${index}`;
              const isExpanded = expandedMeals[mealId] || false;

              return (
                <div key={mealId} className="mb-4 md:mb-6">
                  {/* Clickable meal card */}
                  <div
                    onClick={() => toggleMealExpand(mealId)}
                    className="flex items-center bg-gray-200 p-3 md:p-4 rounded-lg cursor-pointer hover:bg-gray-300 "
                  >
                    <div className="w-8 h-8 rounded-full bg-white flex items-center justify-center mr-3 md:mr-4 flex-shrink-0">
                      {getMealIcon(meal.mealType)}
                    </div>
                    <div className="flex-grow min-w-0">
                      <p className="font-bold text-sm md:text-base truncate">
                        {MealTypes[meal.mealType]}
                      </p>
                      <p className="text-gray-600 font-thin text-xs md:text-base truncate">
                        {meal.name}
                      </p>
                    </div>

                    {/* Edit button and expand icon */}
                    <div className="flex items-center flex-shrink-0">
                      <ChevronDown
                        size={20}
                        className={`text-gray-500 transition-transform duration-300 ${
                          isExpanded ? "rotate-180" : "rotate-0"
                        } flex-shrink-0`}
                      />
                    </div>
                  </div>

                  {/* Expandable recipe section with Framer Motion */}
                  <AnimatePresence>
                    {isExpanded && (
                      <motion.div
                        initial={{ height: 0, opacity: 0 }}
                        animate={{ height: "auto", opacity: 1 }}
                        exit={{ height: 0, opacity: 0 }}
                        transition={{ duration: 0.3 }}
                        className="overflow-hidden rounded-b-lg bg-gray-100 mt-2 "
                      >
                        <div className="p-4">
                          {/* Nutrition information */}
                          <div className="mb-4 bg-white p-3 rounded-lg">
                            <h3 className="font-medium mb-2 text-sm border-b pb-1">
                              Nutritional Information
                            </h3>
                            <div className="grid grid-cols-2 md:grid-cols-4 gap-2 text-sm">
                              <div className="flex flex-col items-center p-2 bg-gray-50 rounded">
                                <span className="text-gray-500 text-xs">Calories</span>
                                <span className="font-medium">{meal.calories}</span>
                              </div>
                              <div className="flex flex-col items-center p-2 bg-gray-50 rounded">
                                <span className="text-gray-500 text-xs">Protein</span>
                                <span className="font-medium">
                                  {meal.proteinInGrams}g
                                </span>
                              </div>
                              <div className="flex flex-col items-center p-2 bg-gray-50 rounded">
                                <span className="text-gray-500 text-xs">Carbs</span>
                                <span className="font-medium">{meal.carbsInGrams}g</span>
                              </div>
                              <div className="flex flex-col items-center p-2 bg-gray-50 rounded">
                                <span className="text-gray-500 text-xs">Fat</span>
                                <span className="font-medium">{meal.fatInGrams}g</span>
                              </div>
                            </div>
                          </div>

                          {/* Recipe */}
                          <h3 className="font-medium mb-2 text-sm border-b pb-1">
                            Recipe
                          </h3>
                          <div className="text-sm md:text-base">
                            {meal.recipe || "Recipe details not available."}
                          </div>
                        </div>
                      </motion.div>
                    )}
                  </AnimatePresence>
                </div>
              );
            })}
          </motion.div>
        </AnimatePresence>
      </div>

      {/* Nutrition Summary */}
      <div className="">
        <h2 className="text-lg md:text-xl font-bold mb-3 md:mb-4 border-b pb-2">
          Nutrition Summary
        </h2>

        <div className="space-y-3 md:space-y-4">
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
        {mealPlan && (
          <div className="text-center text-xl font-thin cursor-pointer hover:bg-eggshell/80 mt-4 p-4 border rounded-full border-gray-300 shadow-sm transition duration-300">
            <button onClick={handleGenerateNewMealClick}>Generate new weekly meal</button>
          </div>
        )}
      </div>
    </div>
  );
};

export default MealPlanComponent;
