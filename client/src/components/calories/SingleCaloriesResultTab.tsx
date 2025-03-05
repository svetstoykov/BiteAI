interface ISingleCaloriesResultTabProps {
  label: string,
  calories: number;
  onClick: (dailyTargetCalories: number) => void;
}

const SingleCaloriesResultTab = ({
  label,
  calories,
  onClick,
}: ISingleCaloriesResultTabProps) => {
  return (
    <div onClick={() => onClick(calories)} className="cursor-pointer p-3 border hover:bg-gray-300 transition-all duration-300 border-gray-200 bg-white rounded-lg">
      <div className="text-xs text-gray-500">{label}</div>
      <div className="text-xl font-bold text-gray-800">{calories} cal</div>
    </div>
  );
};

export default SingleCaloriesResultTab;
