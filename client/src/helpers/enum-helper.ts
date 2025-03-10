import { SelectOption } from "../components/common/Select";

export function enumToArray<T extends Record<string, string | number>>(
  enumObj: T,
  descriptions?: Record<number, string>
): Array<[number, string]> {
  return Object.entries(enumObj)
    .filter(([key, value]) => !isNaN(Number(value)))
    .map(([key, value]) => [Number(value), descriptions?.[Number(value)] || key]);
}

export function enumToSelectOptions<T extends Record<string, string | number>>(
  enumObj: T,
  descriptions?: Record<number, string>
): SelectOption[] {
  return enumToArray(enumObj, descriptions).map(([value, label]) => ({
    value: value,
    label,
  }));
}
