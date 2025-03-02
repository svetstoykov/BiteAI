import { LOGIN_USER, REGISTER_USER } from "../constants/endpoints";
import { LoginDto, LoginResponse, RegisterDto } from "../models/authentication";
import { Result } from "../models/result";
import apiClient from "./api-client";

export class AuthenticationService {
  /**
   * Register a new user
   * @param registerData User registration information
   * @returns Authentication result with token or error
   */
  public async register(registerData: RegisterDto): Promise<Result> {
    try {
      const response = await apiClient.post<string, RegisterDto>(
        REGISTER_USER,
        registerData
      );

      if (response.success) {
        return Result.success("Registration successful.");
      }

      return Result.failure(response.message!);
    } catch (err: any) {
      return Result.failure(
        err.response?.data?.message || "Registration failed. Please try again."
      );
    }
  }

  /**
   * Login an existing user
   * @param loginData User login credentials
   * @returns Authentication result with token or error
   */
  public async login(loginData: LoginDto): Promise<Result> {
    try {
      const response = await apiClient.post<LoginResponse, LoginDto>(
        LOGIN_USER,
        loginData
      );

      if (response.success) {
        this.saveAuthData(response.data!);
        return Result.success("Login successful.");
      }

      return Result.failure(response.message!);
    } catch (err: any) {
      return Result.failure(
        err.response?.data?.message || "Login failed. Please check your credentials."
      );
    }
  }

  /**
   * Check if user is currently authenticated by verifying token exists
   * and isn't expired
   * @returns True if user is authenticated
   */
  public isAuthenticated(): boolean {
    const token = localStorage.getItem("auth_token");
    const expiresAt = localStorage.getItem("auth_expires_at");

    if (!token || !expiresAt) {
      return false;
    }

    const expirationDate = new Date(expiresAt);
    return expirationDate > new Date();
  }

  /**
   * Store authentication data in local storage
   * @param authData Authentication response data
   */
  public saveAuthData(authData: LoginResponse): void {
    localStorage.setItem("auth_token", authData.token);
    localStorage.setItem("auth_user_id", authData.userId);
    localStorage.setItem("auth_email", authData.email);
    localStorage.setItem("auth_username", authData.userName);
    localStorage.setItem("auth_first_name", authData.firstName);
    localStorage.setItem("auth_last_name", authData.lastName);
    localStorage.setItem("auth_expires_at", authData.tokenExpiration.toString());
  }

  /**
   * Clear authentication data from local storage
   */
  public logout(): void {
    localStorage.removeItem("auth_token");
    localStorage.removeItem("auth_user_id");
    localStorage.removeItem("auth_email");
    localStorage.removeItem("auth_username");
    localStorage.removeItem("auth_first_name");
    localStorage.removeItem("auth_last_name");
    localStorage.removeItem("auth_expires_at");
  }

  /**
   * Get the current user's authentication token
   * @returns The auth token if available, null otherwise
   */
  public getToken(): string | null {
    return localStorage.getItem("auth_token");
  }

  /**
   * Get the current user's basic info
   * @returns Object with user data if authenticated, null otherwise
   */
  public getCurrentUser(): {
    userId: string;
    email: string;
    username: string;
    firstName: string;
    lastName: string;
  } | null {
    if (!this.isAuthenticated()) {
      return null;
    }

    return {
      userId: localStorage.getItem("auth_user_id") || "",
      email: localStorage.getItem("auth_email") || "",
      username: localStorage.getItem("auth_username") || "",
      firstName: localStorage.getItem("auth_first_name") || "",
      lastName: localStorage.getItem("auth_last_name") || "",
    };
  }
}
