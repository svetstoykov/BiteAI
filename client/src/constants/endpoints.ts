// Auth endpoints
export const REGISTER_USER = "/api/Auth/register";
export const LOGIN_USER = "/api/Auth/login";

// Calories endpoints
export const CALCULATE_CALORIE_GOALS = "/api/Calories/calculate-goals";
export const CALCULATE_TARGET_DAILY_CALORIES = "/api/Calories/calculate-target-daily-calories";

// Operation Errors endpoints
export const NOT_FOUND = "/api/OperationErrors/not-found";
export const VALIDATION_ERROR = "/api/OperationErrors/validation-error";
export const UNAUTHORIZED = "/api/OperationErrors/unauthorized";
export const BAD_REQUEST = "/api/OperationErrors/bad-request";
export const CONFLICT = "/api/OperationErrors/conflict";
export const TIMEOUT = "/api/OperationErrors/timeout";
export const EXTERNAL_ERROR = "/api/OperationErrors/external-error";
export const INTERNAL_ERROR = "/api/OperationErrors/internal-error";
export const INVALID_OPERATION = "/api/OperationErrors/invalid-operation";
export const SUCCESS = "/api/OperationErrors/success";
export const SUCCESS_DATA = "/api/OperationErrors/success-data";

// Meals endpoints
export const WEEKLY_MEAL_PLAN = "/api/Meals/weekly-meal-plan";
export const DAILY_MEAL_PLAN = "/api/Meals/daily-meal-plan";
export const LATEST_MEAL_PLAN = "/api/Meals/latest-meal-plan";