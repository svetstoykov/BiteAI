import { CREATE_GROCERY_LIST, GET_GROCERY_LIST, CHECK_GROCERY_ITEMS } from "../constants/endpoints";
import { GroceryList, ToggleGroceryItemsRequest } from "../models/groceries";
import { ResultWithData } from "../models/result";
import apiClient from "./api-client";

export class GroceryService {
  /**
   * Get existing grocery list for the authenticated user.
   * @returns Result with grocery list data or error message
   */
  public async getGroceryList(): Promise<ResultWithData<GroceryList>> {
    try {
      const response = await apiClient.get<GroceryList>(GET_GROCERY_LIST);

      if (response.success && response.data) {
        return ResultWithData.success(response.data);
      }
      return ResultWithData.failure(response.message || "Failed to retrieve grocery list");
    } catch (err: any) {
      return ResultWithData.failure(
        err.response?.data?.message || "Failed to retrieve grocery list. Please try again."
      );
    }
  }

  /**
   * Generate grocery list for the authenticated user.
   * @returns Result with grocery list data or error message
   */
  public async generateGroceryList(): Promise<ResultWithData<GroceryList>> {
    try {
      const response = await apiClient.post<GroceryList, {}>(CREATE_GROCERY_LIST, {});

      if (response.success && response.data) {
        return ResultWithData.success(response.data);
      }
      return ResultWithData.failure(response.message || "Failed to generate grocery list");
    } catch (err: any) {
      return ResultWithData.failure(
        err.response?.data?.message || "Failed to generate grocery list. Please try again."
      );
    }
  }

  /**
   * Toggle the checked state of a grocery list item.
   * @param groceryListItemId The ID of the grocery list item
   * @returns Result with success or error message
   */
  public async toggleGroceryItemCheck(groceryListItemId: string): Promise<ResultWithData<boolean>> {
    return this.toggleGroceryItemsCheck([groceryListItemId]);
  }

  /**
   * Toggle the checked state of multiple grocery list items.
   * @param itemIds The IDs of the grocery list items
   * @returns Result with success or error message
   */
  public async toggleGroceryItemsCheck(itemIds: string[]): Promise<ResultWithData<boolean>> {
    try {
      const request: ToggleGroceryItemsRequest = { ItemIds: itemIds };
      const response = await apiClient.post<boolean, ToggleGroceryItemsRequest>(CHECK_GROCERY_ITEMS, request);

      if (response.success) {
        return ResultWithData.success(response.data || true);
      }
      return ResultWithData.failure(response.message || "Failed to toggle items check");
    } catch (err: any) {
      return ResultWithData.failure(
        err.response?.data?.message || "Failed to toggle items check. Please try again."
      );
    }
  }
}