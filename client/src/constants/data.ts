import { v4 as uuidv4 } from 'uuid';
import { DietTypes, MealPlan, MealTypes } from '../models/meals';


// Generate a date string for a specific day offset from today
const getDateString = (dayOffset: number): string => {
  const date = new Date();
  date.setDate(date.getDate() + dayOffset);
  return date.toISOString().split('T')[0];
};

// Create dummy meal plan data
export const weeklyMealPlanData: MealPlan = {
  id: uuidv4(),
  name: "7-Day Mediterranean Diet Plan",
  createdDate: new Date().toISOString(),
  dailyCalories: 2200,
  dietType: DietTypes.Mediterranean,
  durationDays: 7,
  userId: "user123",
  mealDays: [
    // DAY 1
    {
      id: uuidv4(),
      dayNumber: 1,
      date: getDateString(0),
      totalCalories: 2187,
      meals: [
        {
          id: uuidv4(),
          name: "Greek Yogurt with Honey and Berries",
          description: "Creamy Greek yogurt topped with fresh berries and a drizzle of honey",
          recipe: "Mix 1 cup of Greek yogurt with 1 tablespoon of honey, top with 1/2 cup mixed berries and 1 tablespoon of chopped almonds.",
          calories: 320,
          proteinInGrams: 22,
          carbsInGrams: 42,
          fatInGrams: 8,
          mealType: MealTypes.Breakfast
        },
        {
          id: uuidv4(),
          name: "Mediterranean Chickpea Salad",
          description: "Refreshing salad with chickpeas, vegetables, and feta cheese",
          recipe: "Combine 1 cup chickpeas, 1 diced cucumber, 1 diced bell pepper, 1/4 cup red onion, 1/4 cup feta cheese, 2 tbsp olive oil, 1 tbsp lemon juice, and herbs.",
          calories: 450,
          proteinInGrams: 15,
          carbsInGrams: 48,
          fatInGrams: 22,
          mealType: MealTypes.Lunch
        },
        {
          id: uuidv4(),
          name: "Hummus and Veggie Snack Pack",
          description: "Homemade hummus with fresh cut vegetables",
          recipe: "Serve 1/4 cup hummus with 1 cup of mixed vegetables like carrots, bell peppers, and cucumber sticks.",
          calories: 197,
          proteinInGrams: 6,
          carbsInGrams: 22,
          fatInGrams: 10,
          mealType: MealTypes.Snack
        },
        {
          id: uuidv4(),
          name: "Grilled Salmon with Roasted Vegetables",
          description: "Wild-caught salmon fillet with a medley of roasted vegetables",
          recipe: "Season a 6oz salmon fillet with herbs, lemon, and olive oil. Grill for 4 minutes each side. Serve with 2 cups of roasted vegetables tossed in olive oil and herbs.",
          calories: 520,
          proteinInGrams: 40,
          carbsInGrams: 24,
          fatInGrams: 30,
          mealType: MealTypes.Dinner
        },
        {
          id: uuidv4(),
          name: "Mixed Nuts",
          description: "A small handful of unsalted mixed nuts",
          recipe: "1oz of mixed nuts including almonds, walnuts, and pistachios.",
          calories: 170,
          proteinInGrams: 5,
          carbsInGrams: 6,
          fatInGrams: 15,
          mealType: MealTypes.Snack
        }
      ]
    },
    // DAY 2
    {
      id: uuidv4(),
      dayNumber: 2,
      date: getDateString(1),
      totalCalories: 2208,
      meals: [
        {
          id: uuidv4(),
          name: "Avocado Toast with Poached Egg",
          description: "Whole grain toast topped with avocado and a perfectly poached egg",
          recipe: "Toast 2 slices of whole grain bread, spread with 1/2 mashed avocado, top with 2 poached eggs, salt, pepper, and red pepper flakes.",
          calories: 380,
          proteinInGrams: 18,
          carbsInGrams: 30,
          fatInGrams: 22,
          mealType: MealTypes.Breakfast
        },
        {
          id: uuidv4(),
          name: "Lentil Soup with Whole Grain Bread",
          description: "Hearty lentil soup with vegetables and a side of whole grain bread",
          recipe: "Simmer 1 cup lentils with diced onion, carrot, celery, garlic, and vegetable broth for 30 minutes. Serve with a slice of whole grain bread.",
          calories: 460,
          proteinInGrams: 24,
          carbsInGrams: 75,
          fatInGrams: 5,
          mealType: MealTypes.Lunch
        },
        {
          id: uuidv4(),
          name: "Apple with Almond Butter",
          description: "Fresh apple slices with a side of almond butter",
          recipe: "Slice 1 medium apple and serve with 2 tablespoons of natural almond butter.",
          calories: 270,
          proteinInGrams: 7,
          carbsInGrams: 30,
          fatInGrams: 15,
          mealType: MealTypes.Snack
        },
        {
          id: uuidv4(),
          name: "Mediterranean Baked Chicken with Quinoa",
          description: "Herb-baked chicken breast with a side of fluffy quinoa and roasted tomatoes",
          recipe: "Season a 6oz chicken breast with Mediterranean herbs, bake at 375°F for 25 minutes. Serve with 1 cup cooked quinoa and 1 cup roasted cherry tomatoes with olive oil.",
          calories: 520,
          proteinInGrams: 42,
          carbsInGrams: 45,
          fatInGrams: 16,
          mealType: MealTypes.Dinner
        },
        {
          id: uuidv4(),
          name: "Greek Yogurt with Honey",
          description: "Plain Greek yogurt drizzled with honey",
          recipe: "Mix 3/4 cup Greek yogurt with 1 teaspoon of honey.",
          calories: 130,
          proteinInGrams: 18,
          carbsInGrams: 8,
          fatInGrams: 0,
          mealType: MealTypes.Snack
        }
      ]
    },
    // DAY 3
    {
      id: uuidv4(),
      dayNumber: 3,
      date: getDateString(2),
      totalCalories: 2195,
      meals: [
        {
          id: uuidv4(),
          name: "Mediterranean Vegetable Omelette",
          description: "Fluffy omelette packed with Mediterranean vegetables and feta cheese",
          recipe: "Whisk 3 eggs, pour into a hot pan with olive oil. Add 1/4 cup each of diced bell peppers, spinach, and 2 tbsp crumbled feta. Fold and serve.",
          calories: 360,
          proteinInGrams: 22,
          carbsInGrams: 10,
          fatInGrams: 25,
          mealType: MealTypes.Breakfast
        },
        {
          id: uuidv4(),
          name: "Tuna Salad Wrap",
          description: "Whole grain wrap filled with tuna salad and fresh vegetables",
          recipe: "Mix 1 can of tuna with 1 tbsp olive oil mayo, diced celery, and red onion. Wrap in a whole grain tortilla with lettuce and tomato.",
          calories: 430,
          proteinInGrams: 30,
          carbsInGrams: 40,
          fatInGrams: 16,
          mealType: MealTypes.Lunch
        },
        {
          id: uuidv4(),
          name: "Fresh Berries with Walnuts",
          description: "Mixed berries topped with crushed walnuts",
          recipe: "Mix 1 cup of mixed berries with 1 oz chopped walnuts.",
          calories: 210,
          proteinInGrams: 5,
          carbsInGrams: 18,
          fatInGrams: 14,
          mealType: MealTypes.Snack
        },
        {
          id: uuidv4(),
          name: "Grilled Mediterranean Vegetable Pasta",
          description: "Whole grain pasta with grilled vegetables and olive oil",
          recipe: "Cook 2oz whole grain pasta. Toss with 2 cups grilled vegetables (zucchini, eggplant, bell peppers), 2 tbsp olive oil, fresh basil, and 2 tbsp grated Parmesan.",
          calories: 480,
          proteinInGrams: 16,
          carbsInGrams: 65,
          fatInGrams: 20,
          mealType: MealTypes.Dinner
        },
        {
          id: uuidv4(),
          name: "Olives and Cheese",
          description: "A small portion of olives and cheese cubes",
          recipe: "10 olives with 1oz cubed feta cheese.",
          calories: 150,
          proteinInGrams: 6,
          carbsInGrams: 2,
          fatInGrams: 12,
          mealType: MealTypes.Snack
        }
      ]
    },
    // DAY 4
    {
      id: uuidv4(),
      dayNumber: 4,
      date: getDateString(3),
      totalCalories: 2150,
      meals: [
        {
          id: uuidv4(),
          name: "Overnight Oats with Almonds and Berries",
          description: "Oats soaked overnight with almond milk, topped with berries and sliced almonds",
          recipe: "Mix 1/2 cup rolled oats with 3/4 cup almond milk, 1 tsp chia seeds. Refrigerate overnight. Top with 1/2 cup berries and 1 tbsp sliced almonds.",
          calories: 340,
          proteinInGrams: 12,
          carbsInGrams: 55,
          fatInGrams: 10,
          mealType: MealTypes.Breakfast
        },
        {
          id: uuidv4(),
          name: "Greek Salad with Grilled Chicken",
          description: "Classic Greek salad with cucumber, tomato, olives, and feta, topped with grilled chicken",
          recipe: "Combine 2 cups mixed greens, 1/2 cucumber, 1/2 cup cherry tomatoes, 10 olives, 1/4 cup feta. Top with 4oz grilled chicken and dress with olive oil and lemon juice.",
          calories: 420,
          proteinInGrams: 35,
          carbsInGrams: 12,
          fatInGrams: 25,
          mealType: MealTypes.Lunch
        },
        {
          id: uuidv4(),
          name: "Tzatziki with Whole Grain Pita",
          description: "Creamy tzatziki sauce with whole grain pita triangles",
          recipe: "Mix 1/2 cup Greek yogurt with grated cucumber, garlic, dill, and lemon juice. Serve with 1 small whole grain pita cut into triangles.",
          calories: 220,
          proteinInGrams: 10,
          carbsInGrams: 30,
          fatInGrams: 5,
          mealType: MealTypes.Snack
        },
        {
          id: uuidv4(),
          name: "Baked White Fish with Herbed Quinoa",
          description: "Lightly seasoned white fish with a side of herb-infused quinoa",
          recipe: "Season 6oz white fish with lemon, herbs, and olive oil. Bake at 375°F for 15-20 minutes. Serve with 1 cup quinoa cooked with herbs and 1 cup steamed vegetables.",
          calories: 490,
          proteinInGrams: 40,
          carbsInGrams: 42,
          fatInGrams: 16,
          mealType: MealTypes.Dinner
        },
        {
          id: uuidv4(),
          name: "Dark Chocolate Square",
          description: "A square of high-quality dark chocolate",
          recipe: "1oz of 70% dark chocolate.",
          calories: 170,
          proteinInGrams: 2,
          carbsInGrams: 13,
          fatInGrams: 12,
          mealType: MealTypes.Snack
        }
      ]
    },
    // DAY 5
    {
      id: uuidv4(),
      dayNumber: 5,
      date: getDateString(4),
      totalCalories: 2220,
      meals: [
        {
          id: uuidv4(),
          name: "Spinach and Feta Omelette with Whole Grain Toast",
          description: "Protein-packed omelette with spinach and feta cheese, served with whole grain toast",
          recipe: "Whisk 3 eggs, cook in olive oil with 1 cup spinach and 2 tbsp feta cheese. Serve with 1 slice whole grain toast.",
          calories: 380,
          proteinInGrams: 25,
          carbsInGrams: 20,
          fatInGrams: 22,
          mealType: MealTypes.Breakfast
        },
        {
          id: uuidv4(),
          name: "Mediterranean Lentil Bowl",
          description: "Warm lentil bowl with roasted vegetables and tahini dressing",
          recipe: "Combine 1 cup cooked lentils with 1.5 cups roasted vegetables. Drizzle with 2 tbsp tahini mixed with lemon juice and herbs.",
          calories: 460,
          proteinInGrams: 18,
          carbsInGrams: 60,
          fatInGrams: 16,
          mealType: MealTypes.Lunch
        },
        {
          id: uuidv4(),
          name: "Orange with Pistachios",
          description: "Fresh orange segments with a small handful of pistachios",
          recipe: "1 medium orange segmented with 1oz shelled pistachios.",
          calories: 190,
          proteinInGrams: 5,
          carbsInGrams: 24,
          fatInGrams: 8,
          mealType: MealTypes.Snack
        },
        {
          id: uuidv4(),
          name: "Eggplant Moussaka with Greek Salad",
          description: "Traditional Greek eggplant dish with a light side salad",
          recipe: "Layer sliced eggplant with seasoned ground lamb (3oz) and béchamel sauce. Bake at 350°F for 45 minutes. Serve with a small Greek salad.",
          calories: 520,
          proteinInGrams: 30,
          carbsInGrams: 30,
          fatInGrams: 30,
          mealType: MealTypes.Dinner
        },
        {
          id: uuidv4(),
          name: "Greek Yogurt with Honey and Cinnamon",
          description: "Greek yogurt topped with honey and a sprinkle of cinnamon",
          recipe: "Mix 1 cup Greek yogurt with 1 tablespoon honey and a dash of cinnamon.",
          calories: 170,
          proteinInGrams: 17,
          carbsInGrams: 18,
          fatInGrams: 5,
          mealType: MealTypes.Snack
        }
      ]
    },
    // DAY 6
    {
      id: uuidv4(),
      dayNumber: 6,
      date: getDateString(5),
      totalCalories: 2190,
      meals: [
        {
          id: uuidv4(),
          name: "Mediterranean Breakfast Bowl",
          description: "Savory breakfast bowl with eggs, vegetables, and feta cheese",
          recipe: "Sauté 1 cup diced vegetables in 1 tbsp olive oil. Add 2 eggs and scramble. Top with 2 tbsp feta cheese and fresh herbs.",
          calories: 350,
          proteinInGrams: 20,
          carbsInGrams: 15,
          fatInGrams: 25,
          mealType: MealTypes.Breakfast
        },
        {
          id: uuidv4(),
          name: "Falafel Wrap with Tahini Sauce",
          description: "Homemade falafel wrapped in whole grain flatbread with tahini sauce",
          recipe: "Wrap 3 small falafels with lettuce, tomato, and cucumber in a whole grain flatbread. Drizzle with 2 tbsp tahini sauce.",
          calories: 480,
          proteinInGrams: 15,
          carbsInGrams: 65,
          fatInGrams: 20,
          mealType: MealTypes.Lunch
        },
        {
          id: uuidv4(),
          name: "Celery with Almond Butter",
          description: "Celery sticks with creamy almond butter",
          recipe: "Serve 3 celery sticks with 1.5 tbsp almond butter.",
          calories: 160,
          proteinInGrams: 5,
          carbsInGrams: 6,
          fatInGrams: 14,
          mealType: MealTypes.Snack
        },
        {
          id: uuidv4(),
          name: "Grilled Shrimp with Mediterranean Vegetables and Couscous",
          description: "Grilled shrimp skewers with a medley of Mediterranean vegetables over couscous",
          recipe: "Grill 6oz shrimp with olive oil and herbs. Serve over 1/2 cup couscous with 1.5 cups grilled Mediterranean vegetables.",
          calories: 420,
          proteinInGrams: 35,
          carbsInGrams: 40,
          fatInGrams: 12,
          mealType: MealTypes.Dinner
        },
        {
          id: uuidv4(),
          name: "Dried Figs and Walnuts",
          description: "Dried figs paired with walnuts",
          recipe: "2 dried figs with 1oz walnuts.",
          calories: 230,
          proteinInGrams: 4,
          carbsInGrams: 24,
          fatInGrams: 14,
          mealType: MealTypes.Snack
        }
      ]
    },
    // DAY 7
    {
      id: uuidv4(),
      dayNumber: 7,
      date: getDateString(6),
      totalCalories: 2175,
      meals: [
        {
          id: uuidv4(),
          name: "Whole Grain Pancakes with Fresh Berries",
          description: "Fluffy whole grain pancakes topped with fresh berry compote",
          recipe: "Cook 3 small whole grain pancakes. Top with 1 cup fresh berries and 1 tbsp maple syrup.",
          calories: 360,
          proteinInGrams: 10,
          carbsInGrams: 65,
          fatInGrams: 8,
          mealType: MealTypes.Breakfast
        },
        {
          id: uuidv4(),
          name: "Mediterranean Quinoa Salad",
          description: "Refreshing quinoa salad with vegetables, olives, and feta cheese",
          recipe: "Mix 1 cup cooked quinoa with 1 cup diced vegetables, 10 olives, 2 tbsp feta cheese, and lemon-olive oil dressing.",
          calories: 420,
          proteinInGrams: 12,
          carbsInGrams: 50,
          fatInGrams: 20,
          mealType: MealTypes.Lunch
        },
        {
          id: uuidv4(),
          name: "Roasted Chickpeas",
          description: "Crispy roasted chickpeas seasoned with Mediterranean spices",
          recipe: "Roast 1/2 cup chickpeas with 1 tsp olive oil and Mediterranean spices at 400°F for 25 minutes.",
          calories: 180,
          proteinInGrams: 8,
          carbsInGrams: 25,
          fatInGrams: 5,
          mealType: MealTypes.Snack
        },
        {
          id: uuidv4(),
          name: "Mediterranean Herb-Roasted Chicken with Vegetables",
          description: "Herb-roasted chicken with a colorful array of roasted vegetables",
          recipe: "Roast 6oz chicken thigh with rosemary, thyme, and garlic. Serve with 2 cups roasted Mediterranean vegetables seasoned with herbs.",
          calories: 480,
          proteinInGrams: 35,
          carbsInGrams: 30,
          fatInGrams: 25,
          mealType: MealTypes.Dinner
        },
        {
          id: uuidv4(),
          name: "Fresh Peach with Ricotta",
          description: "Sliced fresh peach with a dollop of ricotta cheese",
          recipe: "Slice 1 medium peach and top with 2 tbsp ricotta cheese and a drizzle of honey.",
          calories: 145,
          proteinInGrams: 5,
          carbsInGrams: 25,
          fatInGrams: 3,
          mealType: MealTypes.Snack
        }
      ]
    }
  ]
};

export default weeklyMealPlanData;