import React, { useEffect, useState } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import { MealService } from "../../services/meal-service";
import { GroceryList, GroceryItem, GroceryCategory } from "../../models/meals";
import { toast } from "react-toastify";
import { ChevronDown, Share2, Printer, RotateCcw, CheckSquare, Square } from "lucide-react";
import { motion, AnimatePresence } from "framer-motion";
import GroceryListPreparationSpinner from "./GroceryListPreparationSpinner";

const GroceryListView = () => {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const [groceryList, setGroceryList] = useState<GroceryList | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [expandedCategories, setExpandedCategories] = useState<{ [key: string]: boolean }>({});
  const [checkedItems, setCheckedItems] = useState<{ [key: string]: boolean }>({});

  const mealPlanId = searchParams.get("mealPlanId");

  const mealService = new MealService();

  useEffect(() => {
    if (!mealPlanId) {
      navigate("/meal-plan");
      return;
    }

    const fetchGroceryList = async () => {
      setIsLoading(true);
      try {
        const result = await mealService.generateGroceryList(mealPlanId);
        if (result.success) {
          setGroceryList(result.data!);
          // Initialize all categories as expanded
          const initialExpanded: { [key: string]: boolean } = {};
          result.data!.categories.forEach(cat => {
            initialExpanded[cat.categoryName] = true;
          });
          setExpandedCategories(initialExpanded);
          // Load checked state from localStorage
          loadCheckedState(result.data!.weekMealPlanId);
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
  }, [mealPlanId, navigate]);

  const loadCheckedState = (weekMealPlanId: string) => {
    const stored = localStorage.getItem(`grocery-checked-${weekMealPlanId}`);
    if (stored) {
      setCheckedItems(JSON.parse(stored));
    }
  };

  const saveCheckedState = (newCheckedItems: { [key: string]: boolean }) => {
    if (groceryList) {
      localStorage.setItem(`grocery-checked-${groceryList.weekMealPlanId}`, JSON.stringify(newCheckedItems));
    }
  };

  const toggleItemChecked = (itemId: string) => {
    const newCheckedItems = {
      ...checkedItems,
      [itemId]: !checkedItems[itemId]
    };
    setCheckedItems(newCheckedItems);
    saveCheckedState(newCheckedItems);
  };

  const toggleCategoryExpanded = (categoryName: string) => {
    setExpandedCategories(prev => ({
      ...prev,
      [categoryName]: !prev[categoryName]
    }));
  };

  const checkAllInCategory = (category: GroceryCategory) => {
    const newCheckedItems = { ...checkedItems };
    category.items.forEach(item => {
      newCheckedItems[item.id] = true;
    });
    setCheckedItems(newCheckedItems);
    saveCheckedState(newCheckedItems);
  };

  const resetList = () => {
    const newCheckedItems: { [key: string]: boolean } = {};
    setCheckedItems(newCheckedItems);
    saveCheckedState(newCheckedItems);
  };

  const shareList = async () => {
    if (!groceryList) return;

    const text = generateShareText();

    if (navigator.share) {
      try {
        await navigator.share({
          title: 'Weekly Grocery List',
          text: text
        });
      } catch (err) {
        // Fallback to clipboard
        copyToClipboard(text);
      }
    } else {
      copyToClipboard(text);
    }
  };

  const copyToClipboard = (text: string) => {
    navigator.clipboard.writeText(text).then(() => {
      toast.success("Grocery list copied to clipboard!");
    }).catch(() => {
      toast.error("Failed to copy to clipboard");
    });
  };

  const generateShareText = (): string => {
    if (!groceryList) return "";

    let text = "Weekly Grocery List\n\n";
    groceryList.categories.forEach(category => {
      text += `${category.categoryName}:\n`;
      category.items.forEach(item => {
        const check = checkedItems[item.id] ? "✓" : "☐";
        text += `${check} ${item.quantity} ${item.unit} ${item.name}\n`;
      });
      text += "\n";
    });
    return text;
  };

  const printList = () => {
    window.print();
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
      <div className="text-center py-8">
        <p>Failed to load grocery list</p>
      </div>
    );
  }

  return (
    <div className="mx-auto md:min-w-2xl min-w-[370px] p-2 md:p-6 bite-container">
      <h1 className="text-3xl md:text-4xl font-thin mb-4 md:mb-6 text-center md:text-start">
        Grocery List
      </h1>

      {/* Action Bar */}
      <div className="fixed bottom-4 left-4 right-4 md:static md:bottom-auto md:left-auto md:right-auto bg-white border border-gray-300 rounded-lg p-4 shadow-lg md:shadow-none md:border-0 md:p-0 md:bg-transparent z-10">
        <div className="flex justify-between items-center gap-2">
          <button
            onClick={shareList}
            className="flex items-center gap-2 bg-blue-500 text-white px-4 py-2 rounded-lg hover:bg-blue-600 transition-colors min-h-[44px] flex-1 justify-center"
          >
            <Share2 size={20} />
            <span className="hidden md:inline">Share</span>
          </button>
          <button
            onClick={printList}
            className="flex items-center gap-2 bg-green-500 text-white px-4 py-2 rounded-lg hover:bg-green-600 transition-colors min-h-[44px] flex-1 justify-center"
          >
            <Printer size={20} />
            <span className="hidden md:inline">Print</span>
          </button>
          <button
            onClick={resetList}
            className="flex items-center gap-2 bg-gray-500 text-white px-4 py-2 rounded-lg hover:bg-gray-600 transition-colors min-h-[44px] flex-1 justify-center"
          >
            <RotateCcw size={20} />
            <span className="hidden md:inline">Reset</span>
          </button>
        </div>
      </div>

      {/* Categories */}
      <div className="space-y-4 mb-20 md:mb-8">
        {groceryList.categories.map((category) => {
          const isExpanded = expandedCategories[category.categoryName] || false;
          const checkedCount = category.items.filter(item => checkedItems[item.id]).length;
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
                    className="text-blue-500 hover:text-blue-700 text-sm font-medium"
                  >
                    Check All
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
                      {category.items.map((item) => (
                        <GroceryListItem
                          key={item.id}
                          item={item}
                          isChecked={checkedItems[item.id] || false}
                          onToggle={() => toggleItemChecked(item.id)}
                          onSwipe={(direction) => handleSwipe(item.id, direction)}
                        />
                      ))}
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