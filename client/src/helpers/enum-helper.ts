import { SelectOption } from "../components/common/Select";

export function enumToArray<T extends Record<string, string | number>>(
  enumObj: T,
  descriptions?: Record<number, string>
): Array<[number, string]> {
  return Object.entries(enumObj)
    .filter(([_key, value]) => !isNaN(Number(value)))
    .map(([_key, value]) => [Number(value), descriptions?.[Number(value)] || _key]);
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
