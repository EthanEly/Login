export interface UserFormData {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  confirmPassword: string;
}

export interface UserRegistrationInformation {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

export interface ValidationError {
  field: string;
  message: string;
}
