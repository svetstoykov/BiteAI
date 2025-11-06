import { useState, useEffect } from "react";

const MealPlanPreparationSpinner = () => {
  const [messageIndex, setMessageIndex] = useState(0);

  const funnyMessages = [
    "Slicing up some fresh ingredients...",
    "Preheating the oven...",
    "Whisking everything together...",
    "Letting the flavors meld...",
    "Simmering to perfection...",
    "Flipping pancakes with style...",
    "Baking something delicious...",
    "Adding a pinch of seasoning...",
    "Plating with extra flair...",
    "Serving up something tasty...",
  ];  

  useEffect(() => {
    const interval = setInterval(() => {
      setMessageIndex((prevIndex) => (prevIndex + 1) % funnyMessages.length);
    }, 3500);

    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex flex-col justify-center items-center h-96">
      {/* Larger spinner with beige/gray color palette */}
      <div className="relative">
        <div className="animate-spin rounded-full h-20 w-20 border-4 border-stone-200"></div>
        <div className="animate-spin rounded-full h-20 w-20 border-t-4 border-b-4 border-stone-400 absolute top-0 left-0"></div>

        {/* Inner spinner with complementary neutral tone */}
        <div
          className="absolute top-2 left-2 animate-spin rounded-full h-16 w-16 border-t-2 border-r-2 border-stone-300"
          style={{ animationDirection: "reverse", animationDuration: "1.5s" }}
        ></div>
      </div>

      {/* Message display with animation */}
      <div className="mt-8 text-2xl font-thin text-stone-700 text-center min-h-10">
        <div className="animate-pulse">{funnyMessages[messageIndex]}</div>
      </div>
    </div>
  );
};

export default MealPlanPreparationSpinner;
