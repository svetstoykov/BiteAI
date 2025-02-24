import { useState } from "react";
import { ChevronDown, ChevronRight, ChevronUp } from "lucide-react";
import { redirect, useNavigate } from "react-router-dom";

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
    console.log("redirectedddd")
    navigate("/calculate");
  };

  return (
    <div className="max-w-2xl mx-auto p-6 bg-pastel-green/40 rounded-xl">
      <h1 className="text-4xl font-bold flex items-center justify-center mb-3.5 ">Choose your program
      </h1>
      <section 
        className={`mb-4 bg-off-white rounded-lg shadow-md cursor-pointer overflow-hidden transition-all duration-300 ease-in-out ${
          selectedSection === 'calories' ? 'ring-1 ring-pastel-dark-green/50' : ''
        }`}
      >
        <div 
          className="p-6 flex justify-between items-center "
          onClick={() => handleSectionClick('calories')}
        >
          <h2 className="text-2xl font-bold">Calorie Goals</h2>
          {selectedSection === 'calories' ? (
            <ChevronUp className="w-6 h-6 text-gray-600 transition-transform duration-300" />
          ) : (
            <ChevronDown className="w-6 h-6 text-gray-600 transition-transform duration-300" />
          )}
        </div>
        
        <div 
          className={`transition-all duration-400 ease-in-out origin-top ${
            selectedSection === 'calories' 
              ? 'max-h-96 opacity-100' 
              : 'max-h-0 opacity-0'
          }`}
        >
          <div className="p-6 pt-0">
            <p className="mb-4">
              Get a comprehensive breakdown of your daily calorie targets based on your
              personal profile. We'll calculate multiple calorie scenarios to help you make
              informed decisions about your nutrition. Perfect for understanding your calorie
              needs across different fitness scenarios.
            </p>
          </div>
        </div>
      </section>

      <section 
        className={`mb-4 bg-off-white rounded-lg shadow-md cursor-pointer overflow-hidden transition-all duration-300 ease-in-out ${
          selectedSection === 'weight' ? 'ring-1 ring-pastel-dark-green/50' : ''
        }`}
      >
        <div 
          className="p-6 flex justify-between items-center "
          onClick={() => handleSectionClick('weight')}
        >
          <h2 className="text-2xl font-bold">Target Weight</h2>
          {selectedSection === 'weight' ? (
            <ChevronUp className="w-6 h-6 text-gray-600 transition-transform duration-300" />
          ) : (
            <ChevronDown className="w-6 h-6 text-gray-600 transition-transform duration-300" />
          )}
        </div>

        <div 
          className={`transition-all duration-400 ease-in-out origin-top ${
            selectedSection === 'weight' 
              ? 'max-h-96 opacity-100' 
              : 'max-h-0 opacity-0'
          }`}
        >
          <div className="p-6 pt-0">
            <p className="mb-4">
              Get a personalized daily calorie target specifically designed to help you reach
              your goal weight within your desired timeframe.
            </p>
            <p className="mb-4">In addition to your personal profile, you'll provide:</p>
            <ul className="list-disc ml-6 mb-4">
              <li>Your target weight</li>
              <li>Your desired timeframe to reach this goal</li>
            </ul>
            <p className="mb-4">
              We'll calculate a precise daily calorie target that aligns with your specific
              weight goal and timeline.
            </p>
          </div>
        </div>
      </section>

      <div className="flex justify-center mt-6">
        <button
          onClick={handleNextClick}
          className="bg-pastel-dark-green text-white pr-3 pl-6 py-2 rounded-full hover:bg-pastel-dark-green/90 transition-colors duration-300 flex items-center gap-1 cursor-pointer text-2xl"
        >
          Next
          <ChevronRight size={30} />
        </button>
      </div>
    </div>
  );
}
