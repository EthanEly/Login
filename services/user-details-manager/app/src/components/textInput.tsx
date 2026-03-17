import { SetStateAction } from "react";
import { ValidationError } from "../common/models";

export default function TextInput(props: {
  field: string;
  name: string;
  value: string;
  onChange: (value: SetStateAction<string>) => void;
  errors: ValidationError[];
  placeHolder?: string;
  type?: string;
}) {
  const { field, name, value, onChange, errors, placeHolder, type } = props;

  let classStyle = "border border-white bg-gray-700 text-white";

  const fieldErrors = errors.filter((error) => error.field === field);
  const hasError = fieldErrors.length > 0;

  if (hasError) {
    classStyle += " border-red-500";
  }

  return (
    <div>
      <label htmlFor={field} className="block max-w-md text-lg py-2">
        {name}
      </label>
      {fieldErrors.map((fieldError, index) => (
        <p key={index} className="max-w-md text-sm pb-2 text-red-500">
          {fieldError.message}
        </p>
      ))}
      <input
        id={field}
        type={type ?? "text"}
        className={classStyle}
        placeholder={placeHolder}
        value={value}
        onChange={(e) => onChange(e.target.value)}
      />
    </div>
  );
}
