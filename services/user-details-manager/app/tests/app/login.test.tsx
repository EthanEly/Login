import "@testing-library/jest-dom";
import { render, screen } from "@testing-library/react";
import Login, { loginUser, validateFormData } from "../../src/app/login/page";
import * as TextInputMock from "../../src/components/textInput";
import { UserLoginFormData, UserLoginInformation } from "../../src/app/login/models";

jest.mock("../../src/apiClients/AuthContext", () => {
  return {
    useAuth: () => ({
      login: jest.fn(),
    }),
  };
});
jest.mock("next/navigation", () => ({
  useRouter: () => ({
    push: jest.fn(),
  }),
}));

describe("Login Page", () => {
  const originalEnv = process.env;

  beforeEach(() => {
    process.env = {
      ...originalEnv,
      NEXT_PUBLIC_AUTH_API_URL: "http://myTestWebsite:8080",
    };
  });

  afterEach(() => {
    process.env = originalEnv;
    jest.clearAllMocks();
  });

  it("should render the page heading", () => {
    render(<Login />);

    const heading = screen.getByRole("heading", {
      name: /Welcome back!/,
    });

    expect(heading).toBeVisible();
  });

  it("should call the text input component with the correct props", () => {
    const textInputSpy = jest.spyOn(TextInputMock, "default");

    render(<Login />);

    expect(textInputSpy).toHaveBeenCalledWith(
      {
        errors: [],
        field: "email-address",
        name: "Email address",
        onChange: expect.any(Function),
        type: "text",
        value: "",
      },
      undefined,
    );

    expect(textInputSpy).toHaveBeenCalledWith(
      {
        errors: [],
        field: "password",
        name: "Password",
        onChange: expect.any(Function),
        type: "password",
        value: "",
      },
      undefined,
    );
  });

  it("should render the login button", () => {
    render(<Login />);

    const loginButton = screen.getByRole("button", {
      name: /Login/,
    });

    expect(loginButton).toBeVisible();
  });

  describe("validateFormData", () => {
    const userData: UserLoginFormData = {
      email: "TEST_EMAIL@EXAMPLE.COM",
      password: "TEST_PASSWORD",
    };

    const testArrangement = [
      {
        userFormDataKey: "email",
        fieldKey: "email-address",
        expectedErrorMessage: "Email address is required",
      },
      {
        userFormDataKey: "password",
        fieldKey: "password",
        expectedErrorMessage: "Password is required",
      },
    ];

    it.each(testArrangement)(
      "should return a validation error if field is not populated",
      (testData) => {
        const { userFormDataKey, fieldKey, expectedErrorMessage } = testData;
        const userDataWithEmptyField: UserLoginFormData = { ...userData, [userFormDataKey]: "" };

        const errors = validateFormData(userDataWithEmptyField);

        expect(errors).toEqual(
          expect.arrayContaining([
            {
              field: fieldKey,
              message: expectedErrorMessage,
            },
          ]),
        );
      },
    );

    it("should return a validation error if email is invalid", () => {
      const userDataWithInvalidEmail: UserLoginFormData = { ...userData, email: "invalid-email" };

      const errors = validateFormData(userDataWithInvalidEmail);

      expect(errors).toEqual(
        expect.arrayContaining([
          {
            field: "email-address",
            message: "Email address is invalid",
          },
        ]),
      );
    });
  });

  describe("loginUser", () => {
    const userData: UserLoginInformation = {
      email: "TEST_EMAIL@EXAMPLE.COM",
      password: "TEST_PASSWORD",
    };

    it("should POST to login endpoint with user data", async () => {
      (global as any).fetch = jest.fn().mockResolvedValueOnce({
        ok: true,
        json: async () => ({ success: true }),
      });

      await loginUser(userData);

      expect((global as any).fetch).toHaveBeenCalledWith("http://myTestWebsite:8080/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(userData),
      });
    });

    it("throws an error when response is not ok", async () => {
      (global as any).fetch = jest.fn().mockResolvedValueOnce({
        ok: false,
        status: 500,
      });

      await expect(loginUser(userData)).rejects.toThrow("HTTP error! Status: 500");
    });

    it("throws an error when fetch fails", async () => {
      (global as any).fetch = jest.fn().mockImplementationOnce(() => {
        throw new Error("Network error");
      });

      await expect(loginUser(userData)).rejects.toThrow("Network error");
    });
  });
});
