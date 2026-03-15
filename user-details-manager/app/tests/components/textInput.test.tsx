import "@testing-library/jest-dom";
import { render, screen } from "@testing-library/react";
import TextInput from "../../src/components/textInput";

describe("TextInput component", () => {
  const defaultProps = {
    field: "test-field",
    name: "Test Field",
    value: "test value",
    onChange: jest.fn(),
    errors: [],
  };

  it("should render field name as label for input", () => {
    const props = { ...defaultProps, name: "My New Field" };

    render(<TextInput {...props} />);

    const name = screen.getByLabelText("My New Field");
    const textBox = screen.getByRole("textbox", { name: "My New Field" });

    expect(name).toBeVisible();
    expect(textBox).toBeVisible();
  });

  it("should render value provided", () => {
    const props = { ...defaultProps, value: "A test value" };

    render(<TextInput {...props} />);

    const textBox = screen.getByLabelText(defaultProps.name);

    expect(textBox.getAttribute("value")).toBe("A test value");
  });

  it("should render placeholder text provided", () => {
    const props = { ...defaultProps, placeHolder: "A test placeholder" };

    render(<TextInput {...props} />);

    const textBox = screen.getByLabelText(defaultProps.name);

    expect(textBox.getAttribute("placeholder")).toBe("A test placeholder");
  });

  it("should set provided type on input", () => {
    const props = { ...defaultProps, type: "password" };

    render(<TextInput {...props} />);

    const textBox = screen.getByLabelText(defaultProps.name);

    expect(textBox.getAttribute("type")).toBe("password");
  });

  it("should set type on input to text if no type provided", () => {
    render(<TextInput {...defaultProps} />);

    const textBox = screen.getByLabelText(defaultProps.name);

    expect(textBox.getAttribute("type")).toBe("text");
  });

  it("should render an error message, if error for field exists", () => {
    const props = {
      ...defaultProps,
      errors: [{ field: "test-field", message: "Test error message" }],
    };

    render(<TextInput {...props} />);

    const errorMessage = screen.getByText("Test error message");

    expect(errorMessage).toBeVisible();
  });

  it("should not render an error message, if no error for field exists", () => {
    const props = {
      ...defaultProps,
      errors: [{ field: "another-field-name", message: "Test error message" }],
    };

    render(<TextInput {...props} />);

    const errorMessage = screen.queryByText("Test error message");

    expect(errorMessage).toBeNull();
  });
});
