export function enumToArray<T extends Record<string, string | number>>(
  enumObj: T,
  descriptions?: Record<number, string>
): Array<[number, string]> {
  return Object.entries(enumObj)
      .filter(([key, value]) => !isNaN(Number(value)))
      .map(([key, value]) => [Number(value), descriptions?.[Number(value)] || key]); 
}
