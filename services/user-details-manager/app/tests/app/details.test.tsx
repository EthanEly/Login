import "@testing-library/jest-dom";
import { render, screen } from "@testing-library/react";
import DetailsPage, { getUserDetails } from "../../src/app/details/page";
import { UserDetails } from "@/src/app/details/models";

jest.mock("../../src/apiClients/AuthContext", () => {
  return {
    useAuth: () => ({
      userEmail: "TEST_EMAIL@EXAMPLE.COM",
    }),
  };
});
jest.mock("next/navigation", () => ({
  useRouter: () => ({
    push: jest.fn(),
  }),
}));

describe("Details Page", () => {
  const originalEnv = process.env;
  const userData: UserDetails = {
    firstName: "TEST_FIRST_NAME",
    lastName: "TEST_LAST_NAME",
    email: "TEST_EMAIL@EXAMPLE.COM",
  };

  beforeEach(() => {
    process.env = {
      ...originalEnv,
      NEXT_PUBLIC_USER_API_URL: "http://myTestWebsite:8080",
    };
    (global as any).fetch = jest.fn().mockResolvedValue({
      ok: true,
      json: async () => userData,
    });
  });

  afterEach(() => {
    process.env = originalEnv;
    jest.clearAllMocks();
  });

  it("should render the page heading", async () => {
    render(<DetailsPage />);

    const heading = await screen.findByRole("heading", {
      name: /Welcome, TEST_FIRST_NAME TEST_LAST_NAME!/,
    });

    expect(heading).toBeVisible();
  });

  describe("getUserDetails", () => {
    const userEmail = "TEST_EMAIL@EXAMPLE.COM";

    it("should get user data using provided email", async () => {
      (global as any).fetch = jest.fn().mockResolvedValueOnce({
        ok: true,
        json: async () => userData,
      });

      const userDetails = await getUserDetails(userEmail);

      expect((global as any).fetch).toHaveBeenCalledWith(
        `http://myTestWebsite:8080/details/${userEmail}`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
          },
        },
      );
      expect(userDetails).toEqual(userData);
    });

    it("throws an error when response is not ok", async () => {
      (global as any).fetch = jest.fn().mockResolvedValueOnce({
        ok: false,
        status: 500,
      });

      await expect(getUserDetails(userEmail)).rejects.toThrow("HTTP error! Status: 500");
    });

    it("throws an error when fetch fails", async () => {
      (global as any).fetch = jest.fn().mockImplementationOnce(() => {
        throw new Error("Network error");
      });

      await expect(getUserDetails(userEmail)).rejects.toThrow("Network error");
    });
  });
});
