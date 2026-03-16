"use client";
import { isUserDetails, UserDetails } from "./models";

export default async function Register() {
  const userId = 1;

  const userDetails = await getUserDetails(userId);
  const firstName = userDetails.firstName;
  const lastName = userDetails.lastName;
  const email = userDetails.email;

  return (
    <div className="flex min-h-screen items-center justify-center bg-zinc-50 font-sans dark:bg-black">
      <main className="flex min-h-screen w-full max-w-3xl flex-col items-center justify-between py-32 px-16 bg-white dark:bg-black sm:items-start">
        <div className="flex flex-col items-center gap-6 text-center sm:items-start sm:text-left">
          <h1 className="max-w-xs text-3xl font-semibold leading-10 tracking-tight text-black dark:text-zinc-50">
            Welcome, {firstName} {lastName}!
          </h1>
          <div className="max-w-md text-2xl text-zinc-600 dark:text-zinc-400">
            <p className="max-w-md py-2 leading-8">Glad to have you back.</p>
          </div>
        </div>
      </main>
    </div>
  );
}

async function getUserDetails(userId: number): Promise<UserDetails> {
  try {
    const response = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/details/${userId}`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    });

    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }

    const result = await response.json();

    if (!isUserDetails(result)) {
      throw new Error("Invalid user details format");
    }

    return result;
  } catch (error) {
    throw error;
  }
}
