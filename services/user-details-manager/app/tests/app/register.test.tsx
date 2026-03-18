import "@testing-library/jest-dom";
import { render, screen } from "@testing-library/react";
import Register, { registerUser, validateFormData } from "../../src/app/register/page";
import * as TextInputMock from "../../src/components/textInput";
import { UserFormData, UserRegistrationInformation } from "@/src/app/register/models";

jest.mock("next/navigation", () => ({
  useRouter: () => ({
    push: jest.fn(),
  }),
}));

describe("Register Page", () => {
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
    render(<Register />);

    const heading = screen.getByRole("heading", {
      name: /Hello!/,
    });

    expect(heading).toBeVisible();
  });

  it("should call the text input component with the correct props", () => {
    const textInputSpy = jest.spyOn(TextInputMock, "default");

    render(<Register />);

    expect(textInputSpy).toHaveBeenCalledWith(
      {
        errors: [],
        field: "first-name",
        name: "First name",
        onChange: expect.any(Function),
        type: "text",
        value: "",
      },
      undefined,
    );
    expect(textInputSpy).toHaveBeenCalledWith(
      {
        errors: [],
        field: "last-name",
        name: "Last name",
        onChange: expect.any(Function),
        type: "text",
        value: "",
      },
      undefined,
    );
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
    expect(textInputSpy).toHaveBeenCalledWith(
      {
        errors: [],
        field: "confirm-password",
        name: "Confirm password",
        onChange: expect.any(Function),
        type: "password",
        value: "",
      },
      undefined,
    );
  });

  it("should render the submit button", () => {
    render(<Register />);

    const submitButton = screen.getByRole("button", {
      name: /Submit/,
    });

    expect(submitButton).toBeVisible();
  });

  describe("validateFormData", () => {
    const userData: UserFormData = {
      firstName: "TEST_FIRST_NAME",
      lastName: "TEST_LAST_NAME",
      email: "TEST_EMAIL@EXAMPLE.COM",
      password: "TEST_PASSWORD",
      confirmPassword: "TEST_PASSWORD",
    };

    it("should not return an error if form is valid", () => {
      const errors = validateFormData(userData);

      expect(errors).toEqual([]);
    });

    const testArrangement = [
      {
        userFormDataKey: "firstName",
        fieldKey: "first-name",
        expectedErrorMessage: "First name is required",
      },
      {
        userFormDataKey: "lastName",
        fieldKey: "last-name",
        expectedErrorMessage: "Last name is required",
      },
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
      {
        userFormDataKey: "confirmPassword",
        fieldKey: "confirm-password",
        expectedErrorMessage: "Confirm password is required",
      },
    ];

    it.each(testArrangement)(
      "should return a validation error if field is not populated",
      (testData) => {
        const { userFormDataKey, fieldKey, expectedErrorMessage } = testData;
        const userDataWithEmptyField: UserFormData = { ...userData, [userFormDataKey]: "" };

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
      const userDataWithInvalidEmail = { ...userData, email: "invalid-email" };

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

    it("should return a validation error if password and confirm password do not match", () => {
      const userDataWithInvalidEmail = {
        ...userData,
        password: "password",
        confirmPassword: "not-matching-password",
      };

      const errors = validateFormData(userDataWithInvalidEmail);

      expect(errors).toEqual(
        expect.arrayContaining([
          {
            field: "confirm-password",
            message: "Passwords do not match",
          },
        ]),
      );
    });
  });

  describe("registerUser", () => {
    const userData: UserRegistrationInformation = {
      firstName: "TEST_FIRST_NAME",
      lastName: "TEST_LAST_NAME",
      email: "TEST_EMAIL@EXAMPLE.COM",
      password: "TEST_PASSWORD",
    };

    it("should POST to register endpoint with user data", async () => {
      (global as any).fetch = jest.fn().mockResolvedValueOnce({
        ok: true,
        json: async () => ({ success: true }),
      });

      await registerUser(userData);

      expect((global as any).fetch).toHaveBeenCalledWith("http://myTestWebsite:8080/register", {
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

      await expect(registerUser(userData)).rejects.toThrow("HTTP error! Status: 500");
    });

    it("throws an error when fetch fails", async () => {
      (global as any).fetch = jest.fn().mockImplementationOnce(() => {
        throw new Error("Network error");
      });

      await expect(registerUser(userData)).rejects.toThrow("Network error");
    });
  });
});
