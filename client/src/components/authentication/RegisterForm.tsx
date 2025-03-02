import { useState } from "react";
import Input from "../common/Input";
import { AuthenticationService } from "../../services/authentication-service";
import { RegisterDto } from "../../models/authentication";
import { EyeIcon, EyeOffIcon } from "lucide-react";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";

export default function RegisterForm() {
  const [formData, setFormData] = useState<RegisterDto>({
    firstName: "",
    lastName: "",
    email: "",
    username: "",
    password: "",
    confirmPassword: "",
  });
  const [showPassword, setShowPassword] = useState<boolean>(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState<boolean>(false);
  const navigate = useNavigate();

  const authenticationService = new AuthenticationService();

  const handleChange = (field: string, value: string) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const result = await authenticationService.register(formData);

    if (result.success) {
      toast.success("Registration successful");
      navigate("/login");
      return;
    }

    toast.error(result.message);
  };

  return (
    <div className="min-w-md mx-auto p-6 bite-container">
      <h2 className="text-4xl font-thin text-center text-gray-800 mb-6">
        Create Account
      </h2>

      <form onSubmit={handleSubmit} className="space-y-4">
        <Input
          id="firstName"
          required={true}
          type="text"
          value={formData.firstName}
          onChange={(e) => handleChange("firstName", e.target.value)}
          label="First Name"
          placeholder="John"
        />
        <Input
          id="lastName"
          required={true}
          type="text"
          value={formData.lastName}
          onChange={(e) => handleChange("lastName", e.target.value)}
          label="Last Name"
          placeholder="Doe"
        />
        <Input
          id="email"
          required={true}
          type="text"
          value={formData.email}
          onChange={(e) => handleChange("email", e.target.value)}
          label="Email Address"
          placeholder="you@domain.com"
        />
        <Input
          id="username"
          required={true}
          type="text"
          value={formData.username}
          onChange={(e) => handleChange("username", e.target.value)}
          label="Username"
          placeholder="john_doe"
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
        <Input
          id="confirmPassword"
          required={true}
          type={showConfirmPassword ? "text" : "password"}
          value={formData.confirmPassword}
          onChange={(e) => handleChange("confirmPassword", e.target.value)}
          label="Confirm Password"
          placeholder="••••••••"
          icon={
            showConfirmPassword ? (
              <EyeOffIcon className="h-5 w-5" />
            ) : (
              <EyeIcon className="h-5 w-5" />
            )
          }
          onIconClick={() => setShowConfirmPassword(!showConfirmPassword)}
        />

        <div>
          <button
            type="submit"
            className="cursor-pointer hover:bg-eggshell/80 flex justify-center mx-auto py-2 px-10 border rounded-2xl border-gray-300 shadow-sm text-sm transition duration-300"
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
