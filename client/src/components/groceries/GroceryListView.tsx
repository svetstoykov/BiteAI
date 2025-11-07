import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { GroceryService } from "../../services/grocery-service";
import { GroceryList, GroceryItem } from "../../models/groceries";
import { toast } from "react-toastify";
import { ChevronDown, CheckSquare, Square } from "lucide-react";
import { motion, AnimatePresence } from "framer-motion";
import GroceryListPreparationSpinner from "./GroceryListPreparationSpinner";
import ErrorPage from "../common/ErrorPage";

const GroceryListView = () => {
  const navigate = useNavigate();
  const [groceryList, setGroceryList] = useState<GroceryList | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [expandedCategories, setExpandedCategories] = useState<{ [key: string]: boolean }>({});
  const [checkedItems, setCheckedItems] = useState<{ [key: string]: boolean }>({});
  const [isToggling, setIsToggling] = useState<{ [key: string]: boolean }>({});

  // Group items by category for UI rendering
  const getCategoriesFromItems = (items: GroceryItem[]): CategoryGroup[] => {
    const categoryMap: { [key: string]: GroceryItem[] } = {};
    items.forEach(item => {
      if (!categoryMap[item.category]) {
        categoryMap[item.category] = [];
      }
      categoryMap[item.category].push(item);
    });
    return Object.entries(categoryMap).map(([categoryName, items]) => ({
      categoryName,
      items
    }));
  };

  const groceryService = new GroceryService();

  useEffect(() => {
    const fetchGroceryList = async () => {
      setIsLoading(true);
      try {
        const getResult = await groceryService.getGroceryList();
        let result = getResult;

        if (!getResult.success) {
          result = await groceryService.generateGroceryList();
        }

        if (result.success) {
          setGroceryList(result.data!);
          const initialExpanded: { [key: string]: boolean } = {};
          const categories = getCategoriesFromItems(result.data!.items);
          categories.forEach(cat => {
            initialExpanded[cat.categoryName] = true;
          });
          setExpandedCategories(initialExpanded);
          const initialChecked: { [key: string]: boolean } = {};
          result.data!.items.forEach((item: GroceryItem) => {
            initialChecked[item.id] = item.checked;
          });
          setCheckedItems(initialChecked);
        } else {
          toast.error(result.message);
          navigate("/meal-plan");
        }
      } catch (err) {
        toast.error("Failed to load grocery list");
        navigate("/meal-plan");
      } finally {
        setIsLoading(false);
      }
    };

    fetchGroceryList();
  }, [navigate]);


  const toggleItemChecked = async (itemId: string) => {
    if (isToggling[itemId]) return; // Prevent multiple simultaneous calls

    setIsToggling(prev => ({ ...prev, [itemId]: true }));

    try {
      const result = await groceryService.toggleGroceryItemCheck(itemId);
      if (result.success) {
        // Update local state
        setCheckedItems(prev => ({
          ...prev,
          [itemId]: !prev[itemId]
        }));
      } else {
        toast.error(result.message);
      }
    } catch (error) {
      toast.error("Failed to toggle item check");
    } finally {
      setIsToggling(prev => ({ ...prev, [itemId]: false }));
    }
  };

  const toggleCategoryExpanded = (categoryName: string) => {
    setExpandedCategories(prev => ({
      ...prev,
      [categoryName]: !prev[categoryName]
    }));
  };

  const checkAllInCategory = async (category: CategoryGroup) => {
    const allChecked = category.items.every(item => checkedItems[item.id]);
    let itemIdsToToggle: string[];

    if (allChecked) {
      // Uncheck all
      itemIdsToToggle = category.items.map(item => item.id);
    } else {
      // Check all unchecked
      itemIdsToToggle = category.items.filter(item => !checkedItems[item.id]).map(item => item.id);
    }

    if (itemIdsToToggle.length === 0) return;

    try {
      const result = await groceryService.toggleGroceryItemsCheck(itemIdsToToggle);
      if (result.success) {
        const newCheckedItems = { ...checkedItems };
        itemIdsToToggle.forEach(itemId => {
          newCheckedItems[itemId] = !newCheckedItems[itemId];
        });
        setCheckedItems(newCheckedItems);
      } else {
        toast.error(result.message);
      }
    } catch (error) {
      toast.error("Failed to toggle items in category");
    }
  };


  const handleSwipe = (itemId: string, direction: 'left' | 'right') => {
    if (direction === 'right') {
      toggleItemChecked(itemId);
    }
  };

  if (isLoading) {
    return <GroceryListPreparationSpinner />;
  }

  if (!groceryList) {
    return (
      <ErrorPage
        message="Failed to load grocery list"
        primaryButton={{
          text: "Go to Meal Plan",
          onClick: () => navigate("/meal-plan")
        }}
        secondaryButton={{
          text: "Try Again",
          onClick: () => window.location.reload()
        }}
      />
    );
  }

  const categories = getCategoriesFromItems(groceryList.items);
  if (!categories || categories.length === 0) {
    return (
      <ErrorPage
        message="No grocery items found in your list"
        primaryButton={{
          text: "Go to Meal Plan",
          onClick: () => navigate("/meal-plan")
        }}
        secondaryButton={{
          text: "Try Again",
          onClick: () => window.location.reload()
        }}
      />
    );
  }

  return (
    <div className="mx-auto md:min-w-[773px] min-w-[426px] p-2 md:p-6 bite-container">
      <h1 className="text-3xl md:text-4xl font-thin mb-4 md:mb-6 text-center md:text-start">
        Grocery List
      </h1>

      {/* Categories */}
      <div className="space-y-4 mb-8">
        {categories.map((category: CategoryGroup) => {
          const isExpanded = expandedCategories[category.categoryName] || false;
          const checkedCount = category.items.filter((item: GroceryItem) => checkedItems[item.id]).length;
          const totalCount = category.items.length;

          return (
            <div key={category.categoryName} className="border border-gray-200 rounded-lg overflow-hidden">
              {/* Category Header */}
              <div
                className="bg-gray-50 p-4 cursor-pointer hover:bg-gray-100 transition-colors sticky top-0 z-20"
                onClick={() => toggleCategoryExpanded(category.categoryName)}
              >
                <div className="flex items-center justify-between">
                  <div className="flex items-center gap-3">
                    <ChevronDown
                      size={20}
                      className={`text-gray-500 transition-transform duration-300 ${
                        isExpanded ? "rotate-180" : "rotate-0"
                      }`}
                    />
                    <h3 className="font-medium text-lg">{category.categoryName}</h3>
                    <span className="text-sm text-gray-500">
                      ({checkedCount}/{totalCount})
                    </span>
                  </div>
                  <button
                    onClick={(e) => {
                      e.stopPropagation();
                      checkAllInCategory(category);
                    }}
                    className="bg-dark-charcoal hover:bg-dark-charcoal/90 text-white text-sm font-medium px-3 py-1 rounded-md transition-colors"
                  >
                    {category.items.every(item => checkedItems[item.id]) ? "Uncheck All" : "Check All"}
                  </button>
                </div>
              </div>

              {/* Category Items */}
              <AnimatePresence>
                {isExpanded && (
                  <motion.div
                    initial={{ height: 0, opacity: 0 }}
                    animate={{ height: "auto", opacity: 1 }}
                    exit={{ height: 0, opacity: 0 }}
                    transition={{ duration: 0.3 }}
                    className="overflow-hidden"
                  >
                    <div className="divide-y divide-gray-100">
                      {category.items.map((item: GroceryItem) => {
                        return (
                          <GroceryListItem
                            key={item.id}
                            item={item}
                            isChecked={checkedItems[item.id] || false}
                            onToggle={() => toggleItemChecked(item.id)}
                            onSwipe={(direction) => handleSwipe(item.id, direction)}
                          />
                        );
                      })}
                    </div>
                  </motion.div>
                )}
              </AnimatePresence>
            </div>
          );
        })}
      </div>

    </div>
  );
};

interface GroceryListItemProps {
  item: GroceryItem;
  isChecked: boolean;
  onToggle: () => void;
  onSwipe: (direction: 'left' | 'right') => void;
}

interface CategoryGroup {
  categoryName: string;
  items: GroceryItem[];
}

const GroceryListItem: React.FC<GroceryListItemProps> = ({ item, isChecked, onToggle, onSwipe }) => {
  const [touchStart, setTouchStart] = useState<number | null>(null);
  const [touchEnd, setTouchEnd] = useState<number | null>(null);

  const minSwipeDistance = 50;

  const onTouchStart = (e: React.TouchEvent) => {
    setTouchEnd(null);
    setTouchStart(e.targetTouches[0].clientX);
  };

  const onTouchMove = (e: React.TouchEvent) => {
    setTouchEnd(e.targetTouches[0].clientX);
  };

  const onTouchEnd = () => {
    if (!touchStart || !touchEnd) return;

    const distance = touchStart - touchEnd;
    const isRightSwipe = distance < -minSwipeDistance;

    if (isRightSwipe) {
      onSwipe('right');
    }
  };

  return (
    <div
      className={`p-4 hover:bg-gray-50 transition-colors cursor-pointer min-h-[44px] flex items-center gap-3 ${
        isChecked ? 'bg-green-50' : ''
      }`}
      onClick={onToggle}
      onTouchStart={onTouchStart}
      onTouchMove={onTouchMove}
      onTouchEnd={onTouchEnd}
    >
      <div className="flex-shrink-0">
        {isChecked ? (
          <CheckSquare size={24} className="text-green-500" />
        ) : (
          <Square size={24} className="text-gray-400" />
        )}
      </div>
      <div className="flex-grow min-w-0">
        <div className={`font-medium ${isChecked ? 'line-through text-gray-500' : ''}`}>
          {item.name}
        </div>
        <div className="text-sm text-gray-500">
          {item.quantity} {item.unit}
        </div>
      </div>
    </div>
  );
};

export default GroceryListView;