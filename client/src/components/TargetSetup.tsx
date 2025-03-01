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
      alert("Please select a calculation method before continuing.");
      return;
    }
    
    // Navigate to calculate route with the selected calculation type as a query parameter
    navigate(`/calculate?type=${selectedSection}`);
  };

  return (
    <div className="max-w-3xl mx-auto p-4 sm:p-6 bg-white rounded-xl shadow-sm">
      <h1 className="text-3xl sm:text-4xl font-thin text-center mb-1 sm:mb-2 text-gray-800">
        Choose Calculation Method
      </h1>
      <p className="text-center text-gray-500 mb-4 sm:mb-6 text-sm sm:text-base font-light">
        Select how you'd like us to determine your daily calorie targets
      </p>

      <div className="mb-3 sm:mb-4">
        <div 
          className={`bg-gray-50 rounded-xl shadow-sm border overflow-hidden
            ${selectedSection === "calories" 
              ? "border-gray-300 ring-1 ring-gray-300" 
              : "border-gray-200 hover:border-gray-300"}`}
        >
          <div
            className="p-4 sm:p-5 flex justify-between items-center cursor-pointer"
            onClick={() => handleSectionClick("calories")}
          >
            <div className="flex items-center gap-3">
              <div
                className={`w-8 h-8 sm:w-10 sm:h-10 rounded-full flex items-center justify-center
                  ${selectedSection === "calories"
                    ? "bg-gray-800 text-white"
                    : "bg-gray-100 text-gray-500"}`}
              >
                <span className="font-medium">1</span>
              </div>
              <h2 className="text-xl sm:text-2xl font-thin text-gray-800">Multiple Calorie Scenarios</h2>
            </div>
            <div className="flex-shrink-0">
              {selectedSection === "calories" ? (
                <ChevronUp className="w-5 h-5 sm:w-6 sm:h-6 text-gray-400" />
              ) : (
                <ChevronDown className="w-5 h-5 sm:w-6 sm:h-6 text-gray-400" />
              )}
            </div>
          </div>
          
          <div 
            className={`overflow-hidden transition-[max-height,opacity] duration-300 ease-in-out
              ${selectedSection === "calories" 
                ? "max-h-48 sm:max-h-64 opacity-100" 
                : "max-h-0 opacity-0"}`}
          >
            <div className="px-4 pb-4 sm:px-5 sm:pb-5 pl-12 sm:pl-16">
              <p className="text-sm sm:text-base text-gray-600 leading-relaxed">
                We'll provide you with <strong>5 different daily calorie targets</strong> ranging from weight loss to weight gain. Compare these options side by side to see which aligns best with your health goals.
              </p>
            </div>
          </div>
        </div>
      </div>

      <div className="mb-6">
        <div 
          className={`bg-gray-50 rounded-xl shadow-sm border overflow-hidden
            ${selectedSection === "weight" 
              ? "border-gray-300 ring-1 ring-gray-300" 
              : "border-gray-200 hover:border-gray-300"}`}
        >
          <div
            className="p-4 sm:p-5 flex justify-between items-center cursor-pointer"
            onClick={() => handleSectionClick("weight")}
          >
            <div className="flex items-center gap-3">
              <div
                className={`w-8 h-8 sm:w-10 sm:h-10 rounded-full flex items-center justify-center
                  ${selectedSection === "weight"
                    ? "bg-gray-800 text-white"
                    : "bg-gray-100 text-gray-500"}`}
              >
                <span className="font-medium">2</span>
              </div>
              <h2 className="text-xl sm:text-2xl font-thin text-gray-800">Target Weight Calculator</h2>
            </div>
            <div className="flex-shrink-0">
              {selectedSection === "weight" ? (
                <ChevronUp className="w-5 h-5 sm:w-6 sm:h-6 text-gray-400" />
              ) : (
                <ChevronDown className="w-5 h-5 sm:w-6 sm:h-6 text-gray-400" />
              )}
            </div>
          </div>
          
          <div 
            className={`overflow-hidden transition-[max-height,opacity] duration-300 ease-in-out
              ${selectedSection === "weight" 
                ? "max-h-48 sm:max-h-64 opacity-100" 
                : "max-h-0 opacity-0"}`}
          >
            <div className="px-4 pb-4 sm:px-5 sm:pb-5 pl-12 sm:pl-16">
              <p className="text-sm sm:text-base text-gray-600 leading-relaxed mb-2">
                You'll specify your exact target weight and desired timeframe.
              </p>
              <p className="text-sm sm:text-base text-gray-600 leading-relaxed">
                We'll calculate <strong>one precise daily calorie target</strong> specifically calibrated to help you achieve your goal in your timeframe.
              </p>
            </div>
          </div>
        </div>
      </div>

      <div className="flex justify-center">
        <button
          onClick={handleNextClick}
          className="group bg-gray-900 transition-all duration-300 hover:bg-black text-white px-6 sm:px-8 py-2.5 sm:py-3 rounded-full flex items-center gap-2 text-base sm:text-lg font-medium"
        >
          <span>Continue</span>
          <ChevronRight className="w-4 h-4 sm:w-5 sm:h-5 group-hover:translate-x-1 transition-transform duration-300" />
        </button>
      </div>
    </div>
  );
}