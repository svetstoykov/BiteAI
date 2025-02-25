import { useState } from "react";
import { ChevronDown, ChevronRight, ChevronUp } from "lucide-react";
import { useNavigate } from "react-router-dom";

export default function TargetSetup() {
  const [selectedSection, setSelectedSection] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleSectionClick = (section: string) => {
    if (selectedSection === section) {
      setSelectedSection(null);
    } else {
      setSelectedSection(section);
    }
  };

  const handleNextClick = () => {
    if (!selectedSection) {
      // If no section is selected, prompt the user to select one
      alert("Please select a calculation method before continuing.");
      return;
    }
    
    // Navigate to calculate route with the selected calculation type as a query parameter
    navigate(`/calculate?type=${selectedSection}`);
  };

  return (
    <div className="max-w-3xl mx-auto p-4 sm:p-6 bg-gradient-to-b from-amber-50 to-orange-50 rounded-xl shadow-md">
      <h1 className="text-3xl sm:text-4xl font-thin text-center mb-1 sm:mb-2 text-amber-900">
        Choose Calculation Method
      </h1>
      <p className="text-center text-amber-800 mb-4 sm:mb-6 text-sm sm:text-base font-thin">
        Select how you'd like us to determine your daily calorie targets
      </p>

      {/* Option 1: Multiple Calorie Scenarios */}
      <div className="mb-3 sm:mb-4">
        <div 
          className={`bg-white rounded-xl shadow-sm border overflow-hidden
            ${selectedSection === "calories" 
              ? "border-amber-400 ring-1 ring-amber-400" 
              : "border-amber-100 hover:border-amber-300"}`}
        >
          {/* Header - Always visible */}
          <div
            className="p-4 sm:p-5 flex justify-between items-center cursor-pointer"
            onClick={() => handleSectionClick("calories")}
          >
            <div className="flex items-center gap-3">
              <div
                className={`w-8 h-8 sm:w-10 sm:h-10 rounded-full flex items-center justify-center
                  ${selectedSection === "calories"
                    ? "bg-amber-900 text-white"
                    : "bg-amber-100 text-amber-700"}`}
              >
                <span className="font-semibold">1</span>
              </div>
              <h2 className="text-xl sm:text-2xl font-thin text-amber-900">Multiple Calorie Scenarios</h2>
            </div>
            <div className="flex-shrink-0">
              {selectedSection === "calories" ? (
                <ChevronUp className="w-5 h-5 sm:w-6 sm:h-6 text-amber-500" />
              ) : (
                <ChevronDown className="w-5 h-5 sm:w-6 sm:h-6 text-amber-400" />
              )}
            </div>
          </div>
          
          {/* Content - Conditionally visible */}
          <div 
            className={`overflow-hidden transition-[max-height,opacity] duration-300 ease-in-out
              ${selectedSection === "calories" 
                ? "max-h-48 sm:max-h-64 opacity-100" 
                : "max-h-0 opacity-0"}`}
          >
            <div className="px-4 pb-4 sm:px-5 sm:pb-5 pl-12 sm:pl-16">
              <p className="text-sm sm:text-base text-amber-800 leading-relaxed">
                We'll provide you with <strong>5 different daily calorie targets</strong> ranging from weight loss to weight gain. Compare these options side by side to see which aligns best with your health goals.
              </p>
            </div>
          </div>
        </div>
      </div>

      {/* Option 2: Target Weight Calculator */}
      <div className="mb-6">
        <div 
          className={`bg-white rounded-xl shadow-sm border overflow-hidden
            ${selectedSection === "weight" 
              ? "border-amber-400 ring-1 ring-amber-400" 
              : "border-amber-100 hover:border-amber-300"}`}
        >
          {/* Header - Always visible */}
          <div
            className="p-4 sm:p-5 flex justify-between items-center cursor-pointer"
            onClick={() => handleSectionClick("weight")}
          >
            <div className="flex items-center gap-3">
              <div
                className={`w-8 h-8 sm:w-10 sm:h-10 rounded-full flex items-center justify-center
                  ${selectedSection === "weight"
                    ? "bg-amber-900 text-white"
                    : "bg-amber-100 text-amber-700"}`}
              >
                <span className="font-semibold">2</span>
              </div>
              <h2 className="text-xl sm:text-2xl font-thin text-amber-900">Target Weight Calculator</h2>
            </div>
            <div className="flex-shrink-0">
              {selectedSection === "weight" ? (
                <ChevronUp className="w-5 h-5 sm:w-6 sm:h-6 text-amber-500" />
              ) : (
                <ChevronDown className="w-5 h-5 sm:w-6 sm:h-6 text-amber-400" />
              )}
            </div>
          </div>
          
          {/* Content - Conditionally visible */}
          <div 
            className={`overflow-hidden transition-[max-height,opacity] duration-300 ease-in-out
              ${selectedSection === "weight" 
                ? "max-h-48 sm:max-h-64 opacity-100" 
                : "max-h-0 opacity-0"}`}
          >
            <div className="px-4 pb-4 sm:px-5 sm:pb-5 pl-12 sm:pl-16">
              <p className="text-sm sm:text-base text-amber-800 leading-relaxed mb-2">
                You'll specify your exact target weight and desired timeframe.
              </p>
              <p className="text-sm sm:text-base text-amber-800 leading-relaxed">
                We'll calculate <strong>one precise daily calorie target</strong> specifically calibrated to help you achieve your goal in your timeframe.
              </p>
            </div>
          </div>
        </div>
      </div>

      {/* Continue Button */}
      <div className="flex justify-center">
        <button
          onClick={handleNextClick}
          className="group bg-amber-900 transition-all duration-300 hover:bg-amber-900/90 text-white px-6 sm:px-8 py-2.5 sm:py-3 rounded-full flex items-center gap-2 text-base sm:text-lg font-medium"
        >
          <span>Continue</span>
          <ChevronRight className="w-4 h-4 sm:w-5 sm:h-5 group-hover:translate-x-1 transition-transform duration-300" />
        </button>
      </div>
    </div>
  );
}