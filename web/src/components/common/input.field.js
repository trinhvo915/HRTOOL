import React from "react";
import PropTypes from "prop-types";
import { FormGroup, Label, Input } from "reactstrap";

const InputField = ({
  disabled,
  title,
  type,
  name,
  placeholder,
  value,
  onChange
}) => {
  return (
    <FormGroup>
      {title && (
        <Label className="label-input" for={name}>
          {title}
        </Label>
      )}
      <Input
        className="custom-form-coltrol"
        type={type}
        name={name}
        id={name}
        placeholder={placeholder}
        value={value || ""}
        onChange={onChange}
        disabled={disabled}
      />
    </FormGroup>
  );
};

InputField.propTypes = {
  title: PropTypes.string,
  type: PropTypes.string,
  name: PropTypes.string,
  placeholder: PropTypes.string
};

InputField.defaultProps = {
  title: "",
  type: "text",
  name: "",
  placeholder: ""
};

export default InputField;
