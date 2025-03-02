/**
 * Represents a generic result type that encapsulates success, failure, message, and data.
 */
export class ResultWithData<T> {
  readonly success: boolean;
  readonly message: string;
  readonly data?: T;

  private constructor(success: boolean, message: string, data?: T) {
    this.success = success;
    this.message = message;
    this.data = data;
  }

  /**
   * Creates a successful result.
   * @param data The data associated with the success.
   * @param message Optional success message.
   * @returns A successful Result instance.
   */
  static success<T>(data: T, message: string = "Success"): ResultWithData<T> {
    return new ResultWithData<T>(true, message, data);
  }

  /**
   * Creates a failure result.
   * @param message The error message.
   * @returns A failed Result instance.
   */
  static failure<T>(message: string): ResultWithData<T> {
    return new ResultWithData<T>(false, message);
  }
}

/**
 * Represents a result type without data.
 */
export class Result {
  readonly success: boolean;
  readonly message: string;

  private constructor(success: boolean, message: string) {
    this.success = success;
    this.message = message;
  }

  /**
   * Creates a successful result.
   * @param message Optional success message.
   * @returns A successful ResultNonGeneric instance.
   */
  static success(message: string = "Success"): Result {
    return new Result(true, message);
  }

  /**
   * Creates a failure result.
   * @param message The error message.
   * @returns A failed ResultNonGeneric instance.
   */
  static failure(message: string): Result {
    return new Result(false, message);
  }
}
