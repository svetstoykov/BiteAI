import { useEffect, useState } from "react";
import Input from "../common/Input";
import { clearFieldErrors } from "../../helpers/component-helper";

interface RegisterFormProps {
  onSubmit?: (
    firstName: string,
    lastName: string,
    email: string,
    username: string
  ) => void;
}

export default function RegisterForm({ onSubmit }: RegisterFormProps = {}) {
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    username: "",
  });
  const [errors, setErrors] = useState<Record<string, string>>({});

  useEffect(() => {
    clearFieldErrors(formData, setErrors);
  }, [formData]);

  const handleChange = (field: string, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    const newErrors: Record<string, string> = {};

    if (!formData.firstName) newErrors.firstName = "First name is required";
    if (!formData.lastName) newErrors.lastName = "Last name is required";
    if (!formData.email) newErrors.email = "Email is required";
    else if (!/\S+@\S+\.\S+/.test(formData.email))
      newErrors.email = "Please enter a valid email";
    if (!formData.username) newErrors.username = "Username is required";
    else if (formData.username.length < 3)
      newErrors.username = "Username must be at least 3 characters";

    if (Object.keys(newErrors).length > 0) {
      setErrors(newErrors);
      return;
    }

    setErrors({});
    onSubmit?.(formData.firstName, formData.lastName, formData.email, formData.username);
  };

  return (
    <div className="min-w-md mx-auto p-6 rounded-lg bg-white shadow-md">
      <h2 className="text-3xl font-thin text-center text-gray-800 mb-6">
        Create Account
      </h2>

      <form onSubmit={handleSubmit} className="space-y-6">
        {Object.keys(formData).map((field) => (
          <Input
            key={field}
            id={field}
            type={field === "email" ? "email" : "text"}
            value={formData[field as keyof typeof formData]}
            onChange={(e) => handleChange(field, e.target.value)}
            label={field.charAt(0).toUpperCase() + field.slice(1)}
            placeholder={field === "email" ? "you@domain.com" : ""}
            error={errors[field]}
          />
        ))}

        <div>
          <button
            type="submit"
            className="hover:bg-gray-100 flex justify-center mx-auto py-2 px-10 border rounded-2xl border-gray-300 shadow-sm text-sm transition duration-300"
          >
            Register
          </button>
        </div>

        <div className="text-center text-sm">
          Already have an account?{" "}
          <a
            href="#"
            className="text-moss-green hover:text-moss-green/80 transition-colors duration-300"
          >
            Login
          </a>
        </div>
      </form>
    </div>
  );
}
