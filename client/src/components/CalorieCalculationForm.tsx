import { useState, useEffect } from "react";
import { ChevronRight } from "lucide-react";
import { useNavigate, useSearchParams } from "react-router-dom";
import apiClient from "../services/api-client";
import { useCalorieStore } from "../stores/calorie-store";

export default function CalorieCalculationForm() {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const calculationType = searchParams.get("type") as "calories" | "weight";
  const setResults = useCalorieStore(state => state.setResults);

  // Redirect if no calculation type is specified
  useEffect(() => {
    if (
      !calculationType ||
      (calculationType !== "calories" && calculationType !== "weight")
    ) {
      navigate("/setup");
    }
  }, [calculationType, navigate]);

  // Form fields
  const [weightKg, setWeightKg] = useState<string>("");
  const [heightCm, setHeightCm] = useState<string>("");
  const [age, setAge] = useState<string>("");
  const [exerciseDaysPerWeek, setExerciseDaysPerWeek] = useState<string>("3");
  const [isMale, setIsMale] = useState<boolean>(true);
  const [targetWeightKg, setTargetWeightKg] = useState<string>("");
  const [targetWeeks, setTargetWeeks] = useState<string>("12");

  // Form state
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const handleBack = () => {
    navigate("/setup");
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    setError(null);

    try {
      // Convert form values to appropriate types
      const formData = {
        weightKg: parseFloat(weightKg),
        heightCm: parseFloat(heightCm),
        age: parseInt(age),
        exerciseDaysPerWeek: parseInt(exerciseDaysPerWeek),
        isMale,
      };

      // Add target weight specific params if applicable
      if (calculationType === "weight") {
        Object.assign(formData, {
          targetWeightKg: parseFloat(targetWeightKg),
          targetWeeks: parseInt(targetWeeks),
        });
      }

      // Determine which endpoint to call
      const endpoint =
        calculationType === "calories"
          ? "/calories/calculate-goals"
          : "/calories/calculate-target-daily-calories";

      // Call the API
      const response = await apiClient.get(endpoint, { params: formData });
      
      setResults(
        response.data, 
        calculationType, 
        calculationType === "weight" ? targetWeightKg : null,
        calculationType === "weight" ? targetWeeks : null
      );

      // Navigate to results page with data
      navigate('/results');
      
    } catch (err: any) {
      setError(err.response?.data?.message || "Something went wrong. Please try again.");
      setIsSubmitting(false);
    }
  };

  return (
    <div className="w-full max-w-3xl mx-auto p-5 sm:p-6 bg-white rounded-xl shadow-sm">
      <h1 className="text-3xl sm:text-4xl font-thin text-center mb-1 sm:mb-2 text-gray-800">
        {calculationType === "calories"
          ? "Calculate Calorie Scenarios"
          : "Calculate Target Calories"}
      </h1>
      <p className="text-center text-gray-500 mb-4 sm:mb-6 text-sm sm:text-base font-light">
        {calculationType === "calories"
          ? "We'll provide 5 different calorie targets for you to choose from"
          : "We'll calculate the exact daily calories needed to reach your goal"}
      </p>

      {error && (
        <div className="mb-4 p-3 bg-red-50 border-l-4 border-red-300 text-red-600 rounded text-sm">
          <p>{error}</p>
        </div>
      )}

      <form onSubmit={handleSubmit} className="space-y-4">
        <div className="bg-gray-50 p-4 rounded-lg shadow-sm">
          <div className="flex items-center justify-between mb-3">
            <h2 className="text-xl font-thin text-gray-800">Your Profile</h2>
            {calculationType === "weight" && (
              <div className="text-xs px-2 py-1 bg-gray-200 rounded-full text-gray-700">
                Step 1 of 2
              </div>
            )}
          </div>

          <div className="grid grid-cols-2 gap-3 sm:gap-4">
            {/* First row: Weight and Height */}
            <div>
              <label htmlFor="weight" className="block text-gray-600 text-sm mb-1">
                Current Weight (kg)
              </label>
              <input
                type="number"
                id="weight"
                value={weightKg}
                onChange={(e) => setWeightKg(e.target.value)}
                min="30"
                max="250"
                required
                className="w-full p-2 border border-gray-200 rounded-lg focus:ring-2 focus:ring-gray-300 focus:border-transparent text-sm"
                placeholder="e.g. 70"
              />
            </div>

            <div>
              <label htmlFor="height" className="block text-gray-600 text-sm mb-1">
                Height (cm)
              </label>
              <input
                type="number"
                id="height"
                value={heightCm}
                onChange={(e) => setHeightCm(e.target.value)}
                min="100"
                max="250"
                required
                className="w-full p-2 border border-gray-200 rounded-lg focus:ring-2 focus:ring-gray-300 focus:border-transparent text-sm"
                placeholder="e.g. 175"
              />
            </div>

            {/* Second row: Age and Gender */}
            <div>
              <label htmlFor="age" className="block text-gray-600 text-sm mb-1">
                Age
              </label>
              <input
                type="number"
                id="age"
                value={age}
                onChange={(e) => setAge(e.target.value)}
                min="18"
                max="100"
                required
                className="w-full p-2 border border-gray-200 rounded-lg focus:ring-2 focus:ring-gray-300 focus:border-transparent text-sm"
                placeholder="e.g. 35"
              />
            </div>

            <div>
              <label className="block text-gray-600 text-sm mb-1">Gender</label>
              <div className="flex gap-3 p-1">
                <label className="flex items-center text-sm text-gray-700">
                  <input
                    type="radio"
                    name="gender"
                    checked={isMale}
                    onChange={() => setIsMale(true)}
                    className="mr-1 text-gray-700 focus:ring-gray-300 h-3.5 w-3.5"
                  />
                  <span>Male</span>
                </label>
                <label className="flex items-center text-sm text-gray-700">
                  <input
                    type="radio"
                    name="gender"
                    checked={!isMale}
                    onChange={() => setIsMale(false)}
                    className="mr-1 text-gray-700 focus:ring-gray-300 h-3.5 w-3.5"
                  />
                  <span>Female</span>
                </label>
              </div>
            </div>

            {/* Third row: Exercise level - spans full width */}
            <div className="col-span-2">
              <label htmlFor="exercise" className="block text-gray-600 text-sm mb-1">
                Exercise Days Per Week
              </label>
              <select
                id="exercise"
                value={exerciseDaysPerWeek}
                onChange={(e) => setExerciseDaysPerWeek(e.target.value)}
                required
                className="w-full p-2 border border-gray-200 rounded-lg focus:ring-2 focus:ring-gray-300 focus:border-transparent text-sm"
              >
                <option value="0">Sedentary (0 days)</option>
                <option value="1">Very Light (1 day)</option>
                <option value="2">Light (2 days)</option>
                <option value="3">Moderate (3 days)</option>
                <option value="4">Active (4 days)</option>
                <option value="5">Very Active (5 days)</option>
                <option value="6">Extra Active (6 days)</option>
                <option value="7">Athlete (7 days)</option>
              </select>
            </div>
          </div>
        </div>

        {/* Target Weight Form Fields - Only shown for Target Weight calculation */}
        {calculationType === "weight" && (
          <div className="bg-gray-50 p-4 rounded-lg shadow-sm">
            <div className="flex items-center justify-between mb-3">
              <h2 className="text-xl font-thin text-gray-800">Your Goal</h2>
              <div className="text-xs px-2 py-1 bg-gray-200 rounded-full text-gray-700">
                Step 2 of 2
              </div>
            </div>

            <div className="grid grid-cols-2 gap-3 sm:gap-4">
              {/* Target Weight */}
              <div>
                <label
                  htmlFor="targetWeight"
                  className="block text-gray-600 text-sm mb-1"
                >
                  Target Weight (kg)
                </label>
                <input
                  type="number"
                  id="targetWeight"
                  value={targetWeightKg}
                  onChange={(e) => setTargetWeightKg(e.target.value)}
                  min="30"
                  max="250"
                  required
                  className="w-full p-2 border border-gray-200 rounded-lg focus:ring-2 focus:ring-gray-300 focus:border-transparent text-sm"
                  placeholder="e.g. 65"
                />
              </div>

              {/* Target Timeframe */}
              <div>
                <label
                  htmlFor="targetWeeks"
                  className="block text-gray-600 text-sm mb-1"
                >
                  Target Timeframe
                </label>
                <select
                  id="targetWeeks"
                  value={targetWeeks}
                  onChange={(e) => setTargetWeeks(e.target.value)}
                  required
                  className="w-full p-2 border border-gray-200 rounded-lg focus:ring-2 focus:ring-gray-300 focus:border-transparent text-sm"
                >
                  <option value="4">1 month (4 weeks)</option>
                  <option value="8">2 months (8 weeks)</option>
                  <option value="12">3 months (12 weeks)</option>
                  <option value="16">4 months (16 weeks)</option>
                  <option value="20">5 months (20 weeks)</option>
                  <option value="24">6 months (24 weeks)</option>
                  <option value="36">9 months (36 weeks)</option>
                  <option value="48">1 year (48 weeks)</option>
                </select>
              </div>
            </div>
          </div>
        )}

        {/* Form Buttons */}
        <div className="flex justify-between items-center mt-5">
          <button
            type="button"
            onClick={handleBack}
            className="text-gray-700 px-4 py-2 border border-gray-300 rounded-full hover:bg-gray-100 transition-colors duration-300 text-sm"
          >
            Back
          </button>

          <button
            type="submit"
            disabled={isSubmitting}
            className="group bg-gray-900 transition-all duration-300 hover:bg-black text-white px-5 py-2 rounded-full flex items-center gap-1 text-base font-medium disabled:opacity-70"
          >
            {isSubmitting ? (
              <>
                <SpinnerIcon className="animate-spin -ml-1 mr-2 h-4 w-4 text-white" />
                <span>Calculating</span>
              </>
            ) : (
              <>
                <span>Calculate</span>
                <ChevronRight className="w-4 h-4 group-hover:translate-x-1 transition-transform duration-300" />
              </>
            )}
          </button>
        </div>
      </form>
    </div>
  );
}

// Modern spinner component
const SpinnerIcon = ({ className }: { className?: string }) => (
  <svg 
    className={className} 
    xmlns="http://www.w3.org/2000/svg" 
    fill="none" 
    viewBox="0 0 24 24"
  >
    <circle 
      className="opacity-25" 
      cx="12" 
      cy="12" 
      r="10" 
      stroke="currentColor" 
      strokeWidth="4"
    />
    <path 
      className="opacity-75" 
      fill="currentColor" 
      d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
    />
  </svg>
);