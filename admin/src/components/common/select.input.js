import React from "react";
import PropTypes from "prop-types";
import { FormGroup, Label } from "reactstrap";

const SelectInput = ({
  title,
  placeholder,
  name,
  value,
  options,
  onChange,
  inputClass,
  required,
  valueField,
  nameField,
  // defaultValue,
  disabled,
}) => {
  return (
    <FormGroup>
      <Label className="label-input" for={name}><strong>
        {title}</strong>
      </Label>
      <select
        onChange={onChange}
        className={`form-control`}
        type="select"
        name={name}
        required={required}
        value={value || ""}
        // defaultValue={defaultValue || ""}
        disabled={disabled}
      >
        {placeholder && <option>{placeholder}</option>}
        {options.map((o, i) => {
          return (
            <option key={i} value={o.id}>
              {o.name}
            </option>
          );
        })}
      </select>
    </FormGroup>
  );
};

SelectInput.propTypes = {
  title: PropTypes.string,
  options: PropTypes.array.isRequired,
  name: PropTypes.string,
  placeholder: PropTypes.string
};

SelectInput.defaultProps = {
  title: "",
  name: "",
  placeholder: ""
};

export default SelectInput;
