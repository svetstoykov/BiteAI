import PreparationSpinner from "./PreparationSpinner";

const MealPlanPreparationSpinner = () => {
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

  return <PreparationSpinner messages={funnyMessages} />;
};

export default MealPlanPreparationSpinner;
