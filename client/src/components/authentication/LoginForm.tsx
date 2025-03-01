import { useEffect, useState } from "react";
import { EyeIcon, EyeOffIcon } from "lucide-react";
import Input from "../common/Input";
import Checkbox from "../common/Checkbox";
import { clearFieldErrors } from "../../helpers/component-helper";

interface LoginFormProps {
  onSubmit?: (email: string, password: string) => void;
}

export default function LoginForm({ onSubmit }: LoginFormProps = {}) {
  const [formData, setFormData] = useState({
    email: "",
    password: "",
  });
  const [showPassword, setShowPassword] = useState<boolean>(false);
  const [rememberMe, setRememberMe] = useState<boolean>(false);
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

    if (!formData.email) newErrors.email = "Email is required";
    else if (!/\S+@\S+\.\S+/.test(formData.email))
      newErrors.email = "Please enter a valid email";
    
    if (!formData.password) newErrors.password = "Password is required";
    else if (formData.password.length < 6)
      newErrors.password = "Password must be at least 6 characters";

    if (Object.keys(newErrors).length > 0) {
      setErrors(newErrors);
      return;
    }

    setErrors({});
    onSubmit?.(formData.email, formData.password);
  };

  return (
    <div className="min-w-md mx-auto p-6 bite-container">
      <h2 className="text-4xl font-thin text-center text-gray-800 mb-6">Log In</h2>
      <form onSubmit={handleSubmit} className="space-y-6">
        <Input
          id="email"
          type="email"
          required={true}
          value={formData.email}
          onChange={(e) => handleChange("email", e.target.value)}
          label="Email"
          placeholder="you@domain.com"
          error={errors.email}
        />
        <Input
          id="password"
          required={true}
          type={showPassword ? "text" : "password"}
          value={formData.password}
          onChange={(e) => handleChange("password", e.target.value)}
          label="Password"
          placeholder="••••••••"
          error={errors.password}
          icon={
            showPassword ? (
              <EyeOffIcon className="h-5 w-5" />
            ) : (
              <EyeIcon className="h-5 w-5" />
            )
          }
          onIconClick={() => setShowPassword(!showPassword)}
        />
        <Checkbox
          id="remember-me"
          checked={rememberMe}
          onChange={(e) => setRememberMe(e.target.checked)}
          label="Remember me"
        />
        <div>
          <button
            type="submit"
            className="cursor-pointer hover:bg-eggshell/80 flex justify-center mx-auto py-2 px-10 border rounded-2xl border-gray-300 shadow-sm text-sm transition duration-300"
          >
            Sign in
          </button>
        </div>
        <div className="text-center text-sm">
          Don't have an account?{" "}
          <a
            href="#"
            className="text-moss-green hover:text-moss-green/80 transition-colors duration-300"
          >
            Create an account
          </a>
          <div className="mt-2">
            <a
              href="#"
              className="text-moss-green hover:text-moss-green/80 transition-colors duration-300"
            >
              Forgot your password?
            </a>
          </div>
        </div>
      </form>
    </div>
  );
}
