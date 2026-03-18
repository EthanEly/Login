"use client";
import { useState } from "react";
import TextInput from "../../components/textInput";
import { UserLoginFormData, UserLoginInformation } from "./models";
import { ValidationError } from "../../common/models";
import { useRouter } from "next/navigation";
import { useAuth } from "../../apiClients/authContext";

enum TextInputField {
  Email = "email-address",
  Password = "password",
}

export default function Login() {
  const { login } = useAuth();
  const router = useRouter();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [errors, setErrors] = useState<ValidationError[]>([]);

  const handleLogin = async () => {
    const validationErrors = validateFormData({
      email,
      password,
    });
    setErrors(validationErrors);

    if (validationErrors.length === 0) {
      const tokenGranted = await loginUser({ email, password });

      if (tokenGranted) {
        login(tokenGranted.token);
        router.push("/details");
      }
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-zinc-50 font-sans dark:bg-black">
      <main className="flex min-h-screen w-full max-w-3xl flex-col items-center justify-between py-32 px-16 bg-white dark:bg-black sm:items-start">
        <div className="flex flex-col items-center gap-6 text-center sm:items-start sm:text-left">
          <h1 className="max-w-xs text-3xl font-semibold leading-10 tracking-tight text-black dark:text-zinc-50">
            Welcome back!
          </h1>
          <div className="max-w-md text-2xl text-zinc-600 dark:text-zinc-400">
            <p className="max-w-md py-2 leading-8">Let&apos;s get you logged in.</p>
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
          </div>
          <div className="flex flex-col text-base font-medium sm:flex-row">
            <button
              onClick={handleLogin}
              className="flex h-12 w-full items-center justify-center gap-2 rounded-full bg-foreground px-5 text-background transition-colors hover:bg-[#383838] dark:hover:bg-[#ccc] md:w-[158px]"
            >
              Login
            </button>
          </div>
        </div>
      </main>
    </div>
  );
}

export function validateFormData(userData: UserLoginFormData): ValidationError[] {
  const { email, password } = userData;

  const errors: ValidationError[] = [];

  if (email.trim() === "") {
    errors.push({ field: TextInputField.Email, message: "Email address is required" });
  } else if (!/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/.test(email)) {
    errors.push({ field: TextInputField.Email, message: "Email address is invalid" });
  }

  if (password.trim() === "") {
    errors.push({ field: TextInputField.Password, message: "Password is required" });
  }

  return errors;
}

export async function loginUser(userData: UserLoginInformation) {
  try {
    const response = await fetch(`${process.env.NEXT_PUBLIC_AUTH_API_URL}/login`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(userData),
    });

    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }

    return response.json();
  } catch (error) {
    throw error;
  }
}
