import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse } from "axios";
import { ApiResponse } from "../models/api";

// Ensure you have defined VITE_API_BASE_URL in your .env file.
const baseURL = import.meta.env.VITE_API_BASE_URL;
if(!baseURL) {
  throw new Error("API base url is not defined in .env file");
}

const axiosInstance: AxiosInstance = axios.create({
  baseURL,
});

// Response interceptor to handle errors globally
axiosInstance.interceptors.response.use(
  (response: AxiosResponse) => response,
  (error) => {
    // Log detailed error information
    if (error.response) {
      // The server responded with a status code outside the range of 2xx
      console.error('API Error:', error.response.data);
    } else if (error.request) {
      // The request was made but no response was received
      console.error('Network Error:', error.request);
    } else {
      // Something else happened while setting up the request
      console.error('Error:', error.message);
    }
    // Optionally, you can transform the error to match your ApiResponse interface
    return Promise.reject(error);
  }
);

export const get = async <T>(
  url: string,
  config?: AxiosRequestConfig
): Promise<ApiResponse<T>> => {
  const response: AxiosResponse<ApiResponse<T>> = await axiosInstance.get(url, config);
  return response.data;
};

export const post = async <T, U>(
  url: string,
  payload: U,
  config?: AxiosRequestConfig
): Promise<ApiResponse<T>> => {
  const response: AxiosResponse<ApiResponse<T>> = await axiosInstance.post(
    url,
    payload,
    config
  );
  return response.data;
};

export const put = async <T, U>(
  url: string,
  payload: U,
  config?: AxiosRequestConfig
): Promise<ApiResponse<T>> => {
  const response: AxiosResponse<ApiResponse<T>> = await axiosInstance.put(
    url,
    payload,
    config
  );
  return response.data;
};

// DELETE request (renamed to 'remove' to avoid conflict with reserved word)
export const remove = async <T>(
  url: string,
  config?: AxiosRequestConfig
): Promise<ApiResponse<T>> => {
  const response: AxiosResponse<ApiResponse<T>> = await axiosInstance.delete(url, config);
  return response.data;
};

// Export all the methods as a single httpClient object
const apiClient = {
  get,
  post,
  put,
  delete: remove,
};

export default apiClient;
