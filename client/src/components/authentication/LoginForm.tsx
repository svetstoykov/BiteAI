import { useState, useEffect } from "react";
import { EyeIcon, EyeOffIcon } from "lucide-react";
import Input from "../common/Input";
import { AuthenticationService } from "../../services/authentication-service";
import { LoginDto } from "../../models/authentication";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";
import { useProfile } from "../../hooks/useProfile";

export default function LoginForm() {
  const [formData, setFormData] = useState<LoginDto>({
    email: "",
    password: "",
  });
  const [showPassword, setShowPassword] = useState<boolean>(false);
  const navigate = useNavigate();
  const { checkProfile } = useProfile();

  const authenticationService = new AuthenticationService();

  const handleChange = (field: string, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const result = await authenticationService.login(formData);
    if (result.success) {
      toast.success("Login successful");
      navigate("/setup");
      return;
    }
    toast.error(result.message);
  };

  return (
    <div className="min-w-[345px] mx-auto p-6 bite-container">
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
        />
        <Input
          id="password"
          required={true}
          type={showPassword ? "text" : "password"}
          value={formData.password}
          onChange={(e) => handleChange("password", e.target.value)}
          label="Password"
          placeholder="••••••••"
          icon={
            showPassword ? (
              <EyeOffIcon className="h-5 w-5" />
            ) : (
              <EyeIcon className="h-5 w-5" />
            )
          }
          onIconClick={() => setShowPassword(!showPassword)}
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
            href="/register"
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
