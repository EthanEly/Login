import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import { AuthProvider, useAuth } from "../../src/apiClients/authContext";

describe("AuthContext", () => {
  const TestComponent = () => {
    const { user, login, logout } = useAuth();

    const validToken = "header.eyJuYW1laWQiOiIxMjMiLCJlbWFpbCI6InRlc3RAdGVzdC5jb20ifQ.signature";

    return (
      <div>
        <div data-testid="user-id">{user?.id}</div>
        <div data-testid="user-email">{user?.email}</div>
        <button onClick={() => login(validToken)}>Login Valid</button>
        <button onClick={() => login("invalid-token-string")}>Login Invalid</button>
        <button onClick={logout}>Logout</button>
      </div>
    );
  };

  beforeEach(() => {
    localStorage.clear();
    jest.spyOn(console, "error").mockImplementation(() => {});
  });

  afterEach(() => {
    jest.restoreAllMocks();
  });

  it("should not contain user data initially", () => {
    render(
      <AuthProvider>
        <TestComponent />
      </AuthProvider>,
    );

    expect(screen.getByTestId("user-id").textContent).toBe("");
    expect(screen.getByTestId("user-email").textContent).toBe("");
  });

  it("should decodes token, updates state, and set localStorage on successful login", () => {
    render(
      <AuthProvider>
        <TestComponent />
      </AuthProvider>,
    );

    fireEvent.click(screen.getByText("Login Valid"));

    expect(screen.getByTestId("user-id").textContent).toBe("123");
    expect(screen.getByTestId("user-email").textContent).toBe("test@test.com");
    expect(localStorage.getItem("token")).toContain("eyJuYW1laWQiOiIxMjMi");
  });

  it("should clear state and remove token from localStorage on logout", () => {
    render(
      <AuthProvider>
        <TestComponent />
      </AuthProvider>,
    );

    fireEvent.click(screen.getByText("Login Valid"));
    expect(localStorage.getItem("token")).not.toBeNull();

    fireEvent.click(screen.getByText("Logout"));

    expect(screen.getByTestId("user-id").textContent).toBe("");
    expect(localStorage.getItem("token")).toBeNull();
  });

  it("should handle invalid token without crashing", () => {
    render(
      <AuthProvider>
        <TestComponent />
      </AuthProvider>,
    );

    fireEvent.click(screen.getByText("Login Invalid"));

    expect(screen.getByTestId("user-id").textContent).toBe("");
    expect(console.error).toHaveBeenCalledWith("Failed to decode token", expect.any(Error));
  });

  it("throws an error if useAuth is used in a component without an AuthProvider", () => {
    expect(() => render(<TestComponent />)).toThrow("useAuth must be used within an AuthProvider");
  });
});
