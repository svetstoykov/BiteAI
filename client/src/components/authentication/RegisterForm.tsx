import { use, useState } from "react";
import Input from "../common/Input";

interface RegisterFormProps {
  onSubmit?: (
    firstName: string,
    lastName: string,
    email: string,
    username: string
  ) => void;
}

export default function RegisterForm({ onSubmit }: RegisterFormProps = {}) {
  const [firstName, setFirstName] = useState<string>("");
  const [lastName, setLastName] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const [username, setUsername] = useState<string>("");
  const [errors, setErrors] = useState<{
    firstName?: string;
    lastName?: string;
    email?: string;
    username?: string;
  }>({});

  const clearError = (field: string) => {
    setErrors((prev) => ({ ...prev, [field]: undefined }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    const newErrors: {
      firstName?: string;
      lastName?: string;
      email?: string;
      username?: string;
    } = {};

    // Validate first name
    if (!firstName) {
      newErrors.firstName = "First name is required";
    }

    // Validate last name
    if (!lastName) {
      newErrors.lastName = "Last name is required";
    }

    // Validate email
    if (!email) {
      newErrors.email = "Email is required";
    } else if (!/\S+@\S+\.\S+/.test(email)) {
      newErrors.email = "Please enter a valid email";
    }

    // Validate username
    if (!username) {
      newErrors.username = "Username is required";
    } else if (username.length < 3) {
      newErrors.username = "Username must be at least 3 characters";
    }

    if (Object.keys(newErrors).length > 0) {
      setErrors(newErrors);
      return;
    }

    // Clear errors
    setErrors({});

    // Call the onSubmit prop if provided
    if (onSubmit) {
      onSubmit(firstName, lastName, email, username);
    }
  };

  return (
    <div className="min-w-md mx-auto p-6 rounded-lg bg-white shadow-md">
      <h2 className="text-3xl font-thin text-center text-gray-800 mb-6">
        Create Account
      </h2>

      <form onSubmit={handleSubmit} className="space-y-6">
        {/* First Name Field */}
        <Input
          id="firstName"
          type="text"
          value={firstName}
          onChange={(e) => {
            setFirstName(e.target.value);
            clearError("firstName");
          }}
          label="First Name"
          placeholder="John"
          error={errors.firstName}
        />

        {/* Last Name Field */}
        <Input
          id="lastName"
          type="text"
          value={lastName}
          onChange={(e) => {
            setLastName(e.target.value);
            clearError("lastName");
          }}
          label="Last Name"
          placeholder="Doe"
          error={errors.lastName}
        />

        {/* Email Field */}
        <Input
          id="email"
          type="email"
          value={email}
          onChange={(e) => {
            setEmail(e.target.value)
            clearError("email")
          }}
          label="Email"
          placeholder="you@domain.com"
          error={errors.email}
        />

        {/* Username Field */}
        <Input
          id="username"
          type="text"
          value={username}
          onChange={(e) => {
            setUsername(e.target.value);
            clearError("username");
          }}
          label="Username"
          placeholder="johndoe"
          error={errors.username}
        />

        {/* Submit Button */}
        <div>
          <button
            type="submit"
            className="hover:bg-gray-100 flex justify-center mx-auto py-2 px-10 border rounded-2xl border-gray-300 shadow-sm text-sm transition duration-300"
          >
            Register
          </button>
        </div>

        {/* Login Link */}
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
