import "@testing-library/jest-dom";
import { render, screen } from "@testing-library/react";
import Home from "../../src/app/page";

describe("Landing Page", () => {
  it("should render the welcome heading", () => {
    render(<Home />);

    const heading = screen.getByRole("heading", {
      name: /Welcome!/,
    });

    expect(heading).toBeVisible();
  });

  it("should render the register link", () => {
    render(<Home />);

    const registerLink = screen.getByRole("link", {
      name: /Register/,
    });

    expect(registerLink).toBeVisible();
    expect(registerLink.getAttribute("href")).toBe("/register");
  });

  it("should render the login link", () => {
    render(<Home />);

    const loginLink = screen.getByRole("link", {
      name: /Login/,
    });

    expect(loginLink).toBeVisible();
    expect(loginLink.getAttribute("href")).toBe("/login");
  });
});
