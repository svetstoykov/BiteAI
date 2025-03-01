/**
 * Helper function to clear errors for non-empty form fields
 * @param formData The current form data object
 * @param setErrors The state setter function for errors
 */
export function clearFieldErrors<T extends Record<string, any>>(
  formData: T, 
  setErrors: React.Dispatch<React.SetStateAction<Record<string, string>>>
): void {
  Object.keys(formData).forEach((field) => {
    if (formData[field as keyof typeof formData]) {
      setErrors((prev) => {
        const newErrors = { ...prev };
        delete newErrors[field];
        return newErrors;
      });
    }
  });
}