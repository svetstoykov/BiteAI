import { useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { useCalorieStore } from "../stores/calorie-store";

export default function CalorieResults() {
  const navigate = useNavigate();

  const { results, calculationType, targetWeightKg, targetWeeks } = useCalorieStore();

  useEffect(() => {
    if (!results) {
      navigate("/calculate");
    }
  }, [results, navigate]);

  const handleEditInformation = () => {
    navigate(`/calculate?type=${calculationType}`);
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
        <h2 className="text-xl font-thin text-gray-800 mb-4">Results</h2>

        {calculationType === "calories" ? (
          <div className="space-y-3">
            <div className="grid grid-cols-2 gap-3">
              <div className="p-3 border border-gray-200 bg-white rounded-lg">
                <div className="text-xs text-gray-500">Maintain Weight</div>
                <div className="text-xl font-bold text-gray-800">
                  {results.maintainCalories} cal
                </div>
              </div>
              <div className="p-3 border border-gray-200 bg-white rounded-lg">
                <div className="text-xs text-gray-500">Lose 0.5kg/week</div>
                <div className="text-xl font-bold text-gray-800">
                  {results.loseHalfKgCalories} cal
                </div>
              </div>
              <div className="p-3 border border-gray-200 bg-white rounded-lg">
                <div className="text-xs text-gray-500">Lose 1kg/week</div>
                <div className="text-xl font-bold text-gray-800">
                  {results.loseOneKgCalories} cal
                </div>
              </div>
              <div className="p-3 border border-gray-200 bg-white rounded-lg">
                <div className="text-xs text-gray-500">Gain 0.5kg/week</div>
                <div className="text-xl font-bold text-gray-800">
                  {results.gainHalfKgCalories} cal
                </div>
              </div>
              <div className="p-3 border border-gray-200 bg-white rounded-lg col-span-2">
                <div className="text-xs text-gray-500">Gain 1kg/week</div>
                <div className="text-xl font-bold text-gray-800">
                  {results.gainOneKgCalories} cal
                </div>
              </div>
            </div>
            <div className="mt-4">
              <p className="text-gray-600 text-sm mb-2">Click to generate a meal plan:</p>
              <div className="grid grid-cols-2 gap-2 sm:grid-cols-3">
                <button className="p-2 bg-gray-100 hover:bg-gray-200 rounded-lg transition-colors text-gray-800 font-medium text-sm">
                  Plan for {results.maintainCalories} cal
                </button>
                <button className="p-2 bg-gray-100 hover:bg-gray-200 rounded-lg transition-colors text-gray-800 font-medium text-sm">
                  Plan for {results.loseHalfKgCalories} cal
                </button>
                <button className="p-2 bg-gray-100 hover:bg-gray-200 rounded-lg transition-colors text-gray-800 font-medium text-sm">
                  Plan for {results.loseOneKgCalories} cal
                </button>
              </div>
            </div>
          </div>
        ) : (
          <div className="space-y-3 text-center">
            <p className="text-gray-600 text-sm">
              To reach your goal weight of {targetWeightKg}kg in {targetWeeks} weeks:
            </p>
            <div className="p-4 border-2 border-gray-300 rounded-lg inline-block mx-auto bg-white">
              <div className="text-xs text-gray-500">Your Daily Calorie Target</div>
              <div className="text-3xl font-bold text-gray-800">{results} cal</div>
            </div>
            <div className="mt-4">
              <button className="p-2 bg-gray-100 hover:bg-gray-200 rounded-lg transition-colors text-gray-800 font-medium text-sm">
                Generate Meal Plan for {results} cal
              </button>
            </div>
          </div>
        )}

        <div className="flex justify-center items-center mt-6">
          <button
            type="button"
            onClick={handleEditInformation}
            className="text-gray-700 px-4 py-2 border border-gray-300 rounded-full hover:bg-gray-100 transition-colors duration-300 text-sm"
          >
            Edit Information
          </button>
        </div>
      </div>
    </div>
  );
}
