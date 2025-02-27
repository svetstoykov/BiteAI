import { ChevronLeft } from "lucide-react";

interface BackButtonProps {
  handleBack: () => void;
}

const BackButton = ({handleBack}: BackButtonProps) => {
  return (
    <button
      type="button"
      onClick={handleBack}
      className="group inline-flex items-center text-gray-700 pr-4 pl-3 py-2 border border-gray-300 rounded-full hover:bg-gray-100 transition-colors duration-300 text-sm"
    >
      <ChevronLeft className="w-4 h-4 mr-1 group-hover:-translate-x-1 transition-transform duration-300" />
      Back
    </button>
  );
};

export default BackButton;
