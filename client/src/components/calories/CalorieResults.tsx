import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { ChevronDown } from "lucide-react";
import { useCalorieStore } from "../../stores/calorie-store";
import SingleCaloriesResultTab from "./SingleCaloriesResultTab";
import { DietTypeDescriptions, DietTypes } from "../../models/meals";
import { enumToArray } from "../../helpers/enum-helper";
import BackButton from "../common/BackButton";

export default function CalorieResults() {
  const navigate = useNavigate();
  const [selectedDietType, setSelectedDietType] = useState<DietTypes>(DietTypes.Standard);

  const {
    caloricIntakeGoals,
    targetCaloricIntake,
    calculationType,
    targetWeeks,
    targetWeightKg,
  } = useCalorieStore();

  useEffect(() => {
    if (!caloricIntakeGoals && !targetCaloricIntake) {
      navigate("/calculate");
    }
  }, [caloricIntakeGoals, targetCaloricIntake, navigate]);

  const handleBack = () => {
    navigate(`/calculate?type=${calculationType}`);
  };

  const handleDietTypeChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedDietType(parseInt(e.target.value));
  };

  const handleClickPlanMealForCalories = () => {
    console.log("clicked");
  };

  return (
    <div className="w-full max-w-3xl mx-auto p-5 sm:p-6 bg-white rounded-xl shadow-sm">
      <h1 className="text-3xl sm:text-4xl font-thin text-center mb-1 sm:mb-2 text-gray-800">
        Your Calorie Results
      </h1>
      <p className="text-center text-gray-500 mb-6 text-sm sm:text-base font-light">
        Based on your information, we've calculated your personalized targets
      </p>

      <div className="bg-gray-50 p-5 rounded-lg shadow-sm">
        <h3 className="font-thin text-md mb-4 text-center">
          Click on any of the results to generate a personalized meal plan
        </h3>
        {calculationType === "calories" ? (
          <div className="space-y-3">
            <div className="grid grid-cols-2 gap-3">
              <SingleCaloriesResultTab
                label="Lose 0.5kg/week"
                calories={caloricIntakeGoals?.loseHalfKgCalories ?? 0}
                onClick={handleClickPlanMealForCalories}
              />
              <SingleCaloriesResultTab
                label="Lose 1kg/week"
                calories={caloricIntakeGoals?.loseOneKgCalories ?? 0}
                onClick={handleClickPlanMealForCalories}
              />
              <SingleCaloriesResultTab
                label="Gain 0.5kg/week"
                calories={caloricIntakeGoals?.gainHalfKgCalories ?? 0}
                onClick={handleClickPlanMealForCalories}
              />
              <SingleCaloriesResultTab
                label="Gain 1kg/week"
                calories={caloricIntakeGoals?.gainOneKgCalories ?? 0}
                onClick={handleClickPlanMealForCalories}
              />
              <div className="p-3 border hover:bg-gray-100 transition-all duration-150 border-gray-200 bg-white rounded-lg col-span-2">
                <div className="text-xs text-gray-500">Maintain Weight</div>
                <div className="text-xl font-bold text-gray-800">
                  {caloricIntakeGoals?.gainOneKgCalories} cal
                </div>
              </div>
            </div>
          </div>
        ) : (
          <div className="space-y-3 text-center">
            <p className="text-gray-600 text-sm italic">
              To reach your goal weight of {targetWeightKg}kg in {targetWeeks} weeks:
            </p>
            <div className="p-4 border-2 cursor-pointer hover:bg-gray-100 transition-all duration-150 border-gray-300 rounded-lg inline-block mx-auto bg-white">
              <div className="text-xs text-gray-500">Your Daily Calorie Target</div>
              <div className="text-3xl font-bold text-gray-800">
                {targetCaloricIntake} cal
              </div>
            </div>
          </div>
        )}

        <div className="mt-8 flex items-center justify-between">
          <BackButton handleBack={handleBack}/>
          <div className="flex items-center gap-2 ">
            <label htmlFor="dietType" className=" text-gray-500">
              Diet Type
            </label>
            <div className="relative">
              <select
                id="dietType"
                value={selectedDietType}
                onChange={handleDietTypeChange}
                className="hover:bg-gray-100 transition-colors duration-300 pr-8 pl-3 py-2 border border-gray-300 rounded-full text-gray-700 text-sm focus:outline-none focus:ring-2 focus:ring-gray-300 appearance-none bg-white"
              >
                {enumToArray(DietTypes, DietTypeDescriptions).map((value) => (
                  <option key={value[0]} value={value[0]}>
                    {value[1]}
                  </option>
                ))}
              </select>
              <ChevronDown className="w-4 h-4 text-gray-500 absolute right-2 top-1/2 -translate-y-1/2 pointer-events-none" />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
