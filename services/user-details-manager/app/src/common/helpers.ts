export const isNotUndefined = (value: unknown) => {
  return value !== undefined;
};

export const isStringAndDefined = (value: unknown): value is string => {
  return typeof value === "string" && value !== "";
};

export const isNotNull = (value: unknown) => {
  return value !== null;
};

export const isObject = (value: unknown): value is object => {
  return typeof value === "object";
};
