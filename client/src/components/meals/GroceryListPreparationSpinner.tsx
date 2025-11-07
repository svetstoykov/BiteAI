import PreparationSpinner from "./PreparationSpinner";

const GroceryListPreparationSpinner = () => {
  const groceryMessages = [
    "Stocking up the pantry...",
    "Gathering fresh produce...",
    "Checking the dairy aisle...",
    "Picking out the meats...",
    "Grabbing some grains...",
    "Snagging the spices...",
    "Loading up on snacks...",
    "Finding the frozen treats...",
    "Bagging the essentials...",
    "Finalizing your cart...",
  ];

  return <PreparationSpinner messages={groceryMessages} />;
};

export default GroceryListPreparationSpinner;