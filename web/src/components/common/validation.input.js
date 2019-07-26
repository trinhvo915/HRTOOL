import React from "react";
import PropTypes from "prop-types";
import { FormGroup, Label } from "reactstrap";
import Input from "react-validation/build/input";
import { isEmpty } from "validator";
const isRequired = value => {
  if (isEmpty(value)) {
    return <small className="form-text text-danger">This field is required!</small>;
  }
};

const ValidationInput = ({
  disabled,
  inputClass,
  title,
  type,
  name,
  placeholder,
  value,
  onChange,
  required,
  validations
}) => {
  if (!validations) validations = [];
  if (required) validations.unshift(isRequired);
  return (
    <FormGroup>
      <Label className="label-input" for={name}>
        {title} {required && <span className="text-danger">*</span>}
      </Label>
      <Input
        disabled={disabled}
        id={name}
        name={name}
        value={"" + value}
        onChange={onChange}
        type={type}
        placeholder={placeholder}
        className={`form-control`}
        validations={validations}
      />
    </FormGroup>
  );
};

ValidationInput.propTypes = {
  title: PropTypes.string,
  type: PropTypes.string,
  name: PropTypes.string,
  placeholder: PropTypes.string
};

ValidationInput.defaultProps = {
  title: "",
  type: "text",
  name: "",
  placeholder: ""
};

export default ValidationInput;
