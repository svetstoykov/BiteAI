import { useState, useEffect } from "react";
import { ChevronRight } from "lucide-react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { CalorieCalculationGoalsFormData } from "../../models/calorie";
import { useCalorieStore } from "../../stores/calorie-store";
import { CalorieService } from "../../services/calorie-service";
import { toast } from "react-toastify";
import BackButton from "../common/BackButton";
import Input from "../common/Input";
import Select from "../common/Select";

export default function CalorieCalculationForm() {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const calculationType = searchParams.get("type") as "calories" | "weight";
  const setResults = useCalorieStore((state) => state.setResults);

  // Dropdown options
  const exerciseOptions = [
    { value: "0", label: "Sedentary (0 days)" },
    { value: "1", label: "Very Light (1 day)" },
    { value: "2", label: "Light (2 days)" },
    { value: "3", label: "Moderate (3 days)" },
    { value: "4", label: "Active (4 days)" },
    { value: "5", label: "Very Active (5 days)" },
    { value: "6", label: "Extra Active (6 days)" },
    { value: "7", label: "Athlete (7 days)" },
  ];

  const timeframeOptions = [
    { value: "4", label: "1 month (4 weeks)" },
    { value: "8", label: "2 months (8 weeks)" },
    { value: "12", label: "3 months (12 weeks)" },
    { value: "16", label: "4 months (16 weeks)" },
    { value: "20", label: "5 months (20 weeks)" },
    { value: "24", label: "6 months (24 weeks)" },
    { value: "36", label: "9 months (36 weeks)" },
    { value: "48", label: "1 year (48 weeks)" },
  ];

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
  const calorieService = new CalorieService();

  const handleBack = () => {
    navigate("/setup");
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    setError(null);

    try {
      const isWeightTarget = calculationType === "weight";
      const targetWeeksValue: number | null = isWeightTarget
        ? parseInt(targetWeeks)
        : null;
      const targetWeightKgValue: number | null = isWeightTarget
        ? parseInt(targetWeightKg)
        : null;

      const formData: CalorieCalculationGoalsFormData = {
        weightKg: parseFloat(weightKg),
        heightCm: parseFloat(heightCm),
        age: parseInt(age),
        exerciseDaysPerWeek: parseInt(exerciseDaysPerWeek),
        isMale,
        targetWeeks: targetWeeksValue,
        targetWeightKg: targetWeightKgValue,
      };

      const calculationResult = await calorieService.calculateCalories(
        formData,
        calculationType
      );

      if (calculationResult.isSuccess === false) {
        toast.error(calculationResult.message);
        setIsSubmitting(false);
        return;
      }

      const data = calculationResult.data!;

      setResults(
        data.caloricGoalsDto,
        data.targetIntakeCalories,
        calculationType,
        targetWeightKgValue,
        targetWeeksValue
      );

      navigate("/results");
    } catch (err: any) {
      setError(err.response?.data?.message || "Something went wrong. Please try again.");
      setIsSubmitting(false);
    }
  };

  return (
    <div className={`w-full max-w-3xl mx-auto p-5 sm:p-6 bite-container ${calculationType === "weight" ? "mt-2" : "mt-8"}`}>
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
        <div className="bg-gray-100 p-4 rounded-lg shadow-sm">
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
            <Input
              id="weight"
              type="number"
              required={true}
              value={weightKg}
              onChange={(e) => setWeightKg(e.target.value)}
              label="Current Weight (kg)"
              placeholder="e.g. 80"
              error={undefined}
            />

            <Input
              id="height"
              type="number"
              required={true}
              value={heightCm}
              onChange={(e) => setHeightCm(e.target.value)}
              label="Height (cm)"
              placeholder="e.g. 175"
              error={undefined}
            />

            {/* Second row: Age and Gender */}
            <Input
              id="age"
              type="number"
              value={age}
              required={true}
              onChange={(e) => setAge(e.target.value)}
              label="Age"
              placeholder="e.g. 35"
              error={undefined}
            />

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">Gender</label>
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
            <Select
              id="exercise"
              value={exerciseDaysPerWeek}
              onChange={(e) => setExerciseDaysPerWeek(e.target.value)}
              label="Exercise Days Per Week"
              options={exerciseOptions}
              required={true}
              className="col-span-2"
            />
          </div>
        </div>

        {/* Target Weight Form Fields - Only shown for Target Weight calculation */}
        {calculationType === "weight" && (
          <div className="bg-gray-100 p-4 rounded-lg shadow-sm">
            <div className="flex items-center justify-between mb-3">
              <h2 className="text-xl font-thin text-gray-800">Your Goal</h2>
              <div className="text-xs px-2 py-1 bg-gray-200 rounded-full text-gray-700">
                Step 2 of 2
              </div>
            </div>

            <div className="grid grid-cols-2 gap-3 sm:gap-4">
              {/* Target Weight */}
              <Input
                id="targetWeight"
                type="number"
                required={calculationType === "weight" ? true : false}
                value={targetWeightKg}
                onChange={(e) => setTargetWeightKg(e.target.value)}
                label="Target Weight (kg)"
                placeholder="e.g. 65"
                error={undefined}
              />

              {/* Target Timeframe */}
              <Select
                id="targetWeeks"
                value={targetWeeks}
                onChange={(e) => setTargetWeeks(e.target.value)}
                label="Target Timeframe"
                options={timeframeOptions}
                required={true}
              />
            </div>
          </div>
        )}

        {/* Form Buttons */}
        <div className="flex justify-between items-center mt-5">
          <BackButton handleBack={handleBack} />

          <button
            type="submit"
            disabled={isSubmitting}
            className="group bg-black transition-all duration-300 hover:bg-black/90 text-white px-5 py-2 rounded-full flex items-center gap-1 text-base font-medium disabled:opacity-70"
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