import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse } from "axios";
import { ApiResponse } from "../models/api";

// Ensure you have defined VITE_API_BASE_URL in your .env file.
const baseURL = import.meta.env.VITE_API_BASE_URL;
if (!baseURL) {
  throw new Error("API base URL is not defined in .env file");
}

/**
 * Axios instance configured with the base API URL.
 */
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
      console.error("API Error:", error.response.data);
    } else if (error.request) {
      // The request was made but no response was received
      console.error("Network Error:", error.request);
    } else {
      // Something else happened while setting up the request
      console.error("Error:", error.message);
    }
    return Promise.reject(error);
  }
);

/**
 * Sends a GET request to the specified endpoint.
 *
 * @template ResponseData - The expected shape of the response data.
 * @param url - The API endpoint to fetch data from.
 * @param config - Optional Axios request configuration.
 * @returns A promise resolving to the API response.
 */
export const get = async <ResponseData>(
  url: string,
  config?: AxiosRequestConfig
): Promise<ApiResponse<ResponseData>> => {
  const response: AxiosResponse<ApiResponse<ResponseData>> = await axiosInstance.get(url, config);
  return response.data;
};

/**
 * Sends a POST request with a payload to the specified endpoint.
 *
 * @template ResponseData - The expected shape of the response data.
 * @template RequestPayload - The type of the request payload being sent.
 * @param url - The API endpoint.
 * @param payload - The data to send in the request body.
 * @param config - Optional Axios request configuration.
 * @returns A promise resolving to the API response.
 */
export const post = async <ResponseData, RequestPayload>(
  url: string,
  payload: RequestPayload,
  config?: AxiosRequestConfig
): Promise<ApiResponse<ResponseData>> => {
  const response: AxiosResponse<ApiResponse<ResponseData>> = await axiosInstance.post(
    url,
    payload,
    config
  );
  return response.data;
};

/**
 * Sends a PUT request with a payload to update a resource.
 *
 * @template ResponseData - The expected shape of the response data.
 * @template RequestPayload - The type of the request payload being sent.
 * @param url - The API endpoint.
 * @param payload - The data to update.
 * @param config - Optional Axios request configuration.
 * @returns A promise resolving to the API response.
 */
export const put = async <ResponseData, RequestPayload>(
  url: string,
  payload: RequestPayload,
  config?: AxiosRequestConfig
): Promise<ApiResponse<ResponseData>> => {
  const response: AxiosResponse<ApiResponse<ResponseData>> = await axiosInstance.put(
    url,
    payload,
    config
  );
  return response.data;
};

/**
 * Sends a DELETE request to remove a resource.
 *
 * @template ResponseData - The expected shape of the response data.
 * @param url - The API endpoint.
 * @param config - Optional Axios request configuration.
 * @returns A promise resolving to the API response.
 */
export const remove = async <ResponseData>(
  url: string,
  config?: AxiosRequestConfig
): Promise<ApiResponse<ResponseData>> => {
  const response: AxiosResponse<ApiResponse<ResponseData>> = await axiosInstance.delete(url, config);
  return response.data;
};

/**
 * Exported HTTP client containing all request methods.
 */
const apiClient = {
  get,
  post,
  put,
  delete: remove,
};

export default apiClient;
