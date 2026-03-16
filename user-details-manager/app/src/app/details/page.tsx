"use client";
import { useEffect, useState } from "react";
import { isUserDetails, UserDetails } from "./models";
import { useAuth } from "../../../src/apiClients/AuthContext";
import { useRouter } from "next/navigation";

export default function DetailsPage() {
  const router = useRouter();
  const { userEmail } = useAuth();
  const [userDetails, setUserDetails] = useState<UserDetails>();
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    if (!userEmail) {
      router.push("/login");
      return;
    }

    const fetchUserDetails = async () => {
      try {
        const details = await getUserDetails(userEmail);

        setUserDetails(details);
      } catch (error) {
        console.error("Error fetching user details:", error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchUserDetails();
  }, [userEmail]);

  if (isLoading) {
    return <div>Loading...</div>;
  }

  const showIfAvaliable = (value: string | undefined) => {
    return value !== undefined ? value : "Unavaliable";
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-zinc-50 font-sans dark:bg-black">
      <main className="flex min-h-screen w-full max-w-3xl flex-col items-center justify-between py-32 px-16 bg-white dark:bg-black sm:items-start">
        <div className="flex flex-col items-center gap-6 text-center sm:items-start sm:text-left">
          <h1 className="max-w-xs text-3xl font-semibold leading-10 tracking-tight text-black dark:text-zinc-50">
            Welcome, {showIfAvaliable(userDetails?.firstName)}{" "}
            {showIfAvaliable(userDetails?.lastName)}!
          </h1>
          <div className="max-w-md text-2xl text-zinc-600 dark:text-zinc-400">
            <p className="max-w-md py-2 leading-8">Glad to have you back.</p>
            <p className="max-w-md py-2 text-xl leading-8">Here are your details:</p>
            <table className="w-full mt-4 text-left border-collapse text-base">
              <tbody>
                <tr className="border-b border-zinc-200 dark:border-zinc-800">
                  <th
                    scope="row"
                    className="py-3 pr-4 font-semibold text-zinc-500 dark:text-zinc-400"
                  >
                    First Name
                  </th>
                  <td className="py-3 text-black dark:text-zinc-50">
                    {showIfAvaliable(userDetails?.firstName)}
                  </td>
                </tr>
                <tr className="border-b border-zinc-200 dark:border-zinc-800">
                  <th
                    scope="row"
                    className="py-3 pr-4 font-semibold text-zinc-500 dark:text-zinc-400"
                  >
                    Last Name
                  </th>
                  <td className="py-3 text-black dark:text-zinc-50">
                    {showIfAvaliable(userDetails?.lastName)}
                  </td>
                </tr>
                <tr>
                  <th
                    scope="row"
                    className="py-3 pr-4 font-semibold text-zinc-500 dark:text-zinc-400"
                  >
                    Email
                  </th>
                  <td className="py-3 text-black dark:text-zinc-50">
                    {showIfAvaliable(userDetails?.email)}
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </main>
    </div>
  );
}

export async function getUserDetails(userEmail: string): Promise<UserDetails> {
  try {
    const response = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/details/${userEmail}`, {
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
