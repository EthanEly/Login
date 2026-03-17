import { isNotNull, isNotUndefined, isObject, isStringAndDefined } from "../../common/helpers";

export interface UserDetails {
  firstName: string;
  lastName: string;
  email: string;
}

export const isUserDetails = (value: unknown): value is UserDetails => {
  if (!isObject(value)) {
    return false;
  }

  const obj = value as Record<string, unknown>;

  return (
    isNotUndefined(obj["firstName"]) &&
    isNotNull(obj["firstName"]) &&
    isStringAndDefined(obj["firstName"]) &&
    isNotUndefined(obj["lastName"]) &&
    isNotNull(obj["lastName"]) &&
    isStringAndDefined(obj["lastName"]) &&
    isNotUndefined(obj["email"]) &&
    isNotNull(obj["email"]) &&
    isStringAndDefined(obj["email"])
  );
};
