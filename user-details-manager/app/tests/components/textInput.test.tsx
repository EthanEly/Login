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

  it("renders field name provided", () => {
    const props = { ...defaultProps, name: "My New Field" };

    render(<TextInput {...props} />);

    const name = screen.getByText("My New Field");

    expect(name).toBeVisible();
  });

  it("renders value provided", () => {
    const props = { ...defaultProps, value: "A test value" };

    render(<TextInput {...props} />);

    const textBox = screen.getByLabelText("My New Field");

    expect(textBox.getAttribute("value")).toBe("A test value");
  });

  it("renders placeholder text provided", () => {
    const props = { ...defaultProps, placeHolder: "A test placeholder" };

    render(<TextInput {...props} />);

    const textBox = screen.getByLabelText(defaultProps.name);

    expect(textBox.getAttribute("placeholder")).toBe("A test placeholder");
  });

  it("sets password type on input", () => {
    const props = { ...defaultProps, type: "password" };

    render(<TextInput {...props} />);

    const textBox = screen.getByLabelText(defaultProps.name);

    expect(textBox.getAttribute("type")).toBe("password");
  });

  it("sets type on input to text if no type provided", () => {
    render(<TextInput {...defaultProps} />);

    const textBox = screen.getByLabelText(defaultProps.name);

    expect(textBox.getAttribute("type")).toBe("text");
  });

  it("renders an error message, if error for field exists", () => {
    const props = {
      ...defaultProps,
      errors: [{ field: "test-field", message: "Test error message" }],
    };

    render(<TextInput {...props} />);

    const errorMessage = screen.getByText("Test error message");

    expect(errorMessage).toBeVisible();
  });

  it("does not render an error message, if no error for field exists", () => {
    const props = {
      ...defaultProps,
      errors: [{ field: "another-field-name", message: "Test error message" }],
    };

    render(<TextInput {...props} />);

    const errorMessage = screen.queryByText("Test error message");

    expect(errorMessage).toBeNull();
  });
});
