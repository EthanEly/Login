"use client";
import { useState } from "react";
import { UserFormData, UserRegistrationInformation } from "./models";
import TextInput from "../../components/textInput";
import { ValidationError } from "../../common/models";
import { useRouter } from "next/navigation";

enum TextInputField {
  FirstName = "first-name",
  LastName = "last-name",
  Email = "email-address",
  Password = "password",
  ConfirmPassword = "confirm-password",
}

export default function Register() {
  const router = useRouter();
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");

  const [errors, setErrors] = useState<ValidationError[]>([]);

  const handleSubmit = async () => {
    const validationErrors = validateFormData({
      firstName,
      lastName,
      email,
      password,
      confirmPassword,
    });
    setErrors(validationErrors);

    if (validationErrors.length === 0) {
      const successfulRegistration = await registerUser({ firstName, lastName, email, password });

      if (successfulRegistration) {
        router.push("/login");
      }
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-zinc-50 font-sans dark:bg-black">
      <main className="flex min-h-screen w-full max-w-3xl flex-col items-center justify-between py-32 px-16 bg-white dark:bg-black sm:items-start">
        <div className="flex flex-col items-center gap-6 text-center sm:items-start sm:text-left">
          <h1 className="max-w-xs text-3xl font-semibold leading-10 tracking-tight text-black dark:text-zinc-50">
            Hello!
          </h1>
          <div className="max-w-md text-2xl text-zinc-600 dark:text-zinc-400">
            <p className="max-w-md py-2 leading-8">First time? Let&apos;s get you registered!</p>
            <TextInput
              field={TextInputField.FirstName}
              name="First name"
              type="text"
              value={firstName}
              onChange={setFirstName}
              errors={errors}
            />
            <TextInput
              field={TextInputField.LastName}
              name="Last name"
              type="text"
              value={lastName}
              onChange={setLastName}
              errors={errors}
            />
            <TextInput
              field={TextInputField.Email}
              name="Email address"
              type="text"
              value={email}
              onChange={setEmail}
              errors={errors}
            />
            <TextInput
              field={TextInputField.Password}
              name="Password"
              type="password"
              value={password}
              onChange={setPassword}
              errors={errors}
            />
            <TextInput
              field={TextInputField.ConfirmPassword}
              name="Confirm password"
              type="password"
              value={confirmPassword}
              onChange={setConfirmPassword}
              errors={errors}
            />
          </div>
          <div className="flex flex-col text-base font-medium sm:flex-row">
            <button
              onClick={handleSubmit}
              className="flex h-12 w-full items-center justify-center gap-2 rounded-full bg-foreground px-5 text-background transition-colors hover:bg-[#383838] dark:hover:bg-[#ccc] md:w-[158px]"
            >
              Submit
            </button>
          </div>
        </div>
      </main>
    </div>
  );
}

export function validateFormData(userData: UserFormData): ValidationError[] {
  const { firstName, lastName, email, password, confirmPassword } = userData;

  const errors: ValidationError[] = [];

  if (firstName.trim() === "") {
    errors.push({ field: TextInputField.FirstName, message: "First name is required" });
  }

  if (lastName.trim() === "") {
    errors.push({ field: TextInputField.LastName, message: "Last name is required" });
  }

  if (email.trim() === "") {
    errors.push({ field: TextInputField.Email, message: "Email address is required" });
  } else if (!/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/.test(email)) {
    errors.push({ field: TextInputField.Email, message: "Email address is invalid" });
  }

  if (password.trim() === "") {
    errors.push({ field: TextInputField.Password, message: "Password is required" });
  }

  if (confirmPassword.trim() === "") {
    errors.push({ field: TextInputField.ConfirmPassword, message: "Confirm password is required" });
  }

  if (password !== confirmPassword) {
    errors.push({ field: TextInputField.ConfirmPassword, message: "Passwords do not match" });
  }

  return errors;
}

export async function registerUser(userData: UserRegistrationInformation) {
  try {
    const response = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/register`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(userData),
    });

    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }

    return true;
  } catch (error) {
    throw error;
  }
}
