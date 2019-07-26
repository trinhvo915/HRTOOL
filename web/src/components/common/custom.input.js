import React from 'react'
import PropTypes from 'prop-types'
import {
  DropdownItem,
  InputGroup,
  InputGroupButtonDropdown,
  DropdownToggle,
  DropdownMenu
} from 'reactstrap'

const CustomInput = ({
  disabled,
  title,
  type,
  name,
  placeholder,
  value,
  onChange,
  onFocus,
  onBlur,

  // props for dropdown
  isOpen,
  toggle,
  dataDropdown,
  dropdownItemSelected
}) => {
  return (
    <>
      <div className="form-group">
        <label htmlFor={name} className="label-input">
          {title}
        </label>
        <input
          name={name}
          id={name}
          placeholder={placeholder}
          type={type}
          onChange={onChange}
          className="custom-form-coltrol form-control"
          value={value}
          disabled={disabled}
          onFocus={onFocus}
          onBlur={onBlur}
        />
      </div>

      {isOpen && dataDropdown && dataDropdown.length > 0 && (
        <InputGroup>
          <InputGroupButtonDropdown
            addonType="append"
            isOpen={isOpen}
            toggle={toggle}
          >
            <DropdownToggle />

            <DropdownMenu>
              {isOpen &&
                dataDropdown &&
                dataDropdown.length > 0 &&
                dataDropdown.map(dropdownItem => {
                  return (
                    <DropdownItem
                      key={dropdownItem.id}
                      onClick={() => dropdownItemSelected(dropdownItem)}
                    >
                      {dropdownItem.name}
                    </DropdownItem>
                  )
                })}
            </DropdownMenu>
          </InputGroupButtonDropdown>
        </InputGroup>
      )}
    </>
  )
}

export default CustomInput
