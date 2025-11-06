Create a {{days}}-day meal plan for a {{dietType}} diet.
Daily calorie target: {{dailyCalorieTarget}} kcal per day.
Requirements:
- The sum of Calories across all meals in each day must equal the daily target as closely as possible (within +/-1% or <= 30 kcal). If needed, adjust portion sizes to hit the target.
- Include 1 Breakfast, 1 Lunch, 1 Dinner, and 1–2 Snacks per day.
- Provide per-meal Calories and macros (ProteinInGrams, CarbsInGrams, FatInGrams) so that macros approximately sum to Calories.
- Provide variety across days; avoid repeating the same meal more than once.
- Use commonly available ingredients.
- Do not use any default calorie target (e.g., 1650 or 2000); strictly use {{dailyCalorieTarget}} for each day.
