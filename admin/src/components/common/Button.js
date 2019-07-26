import React from 'react'
import { Button } from 'reactstrap'

const ButtonComponent = ({ title, handleClick, disabled, className }) => {
  return (
    <Button
      className={`btn-primary ${className}`}
      onClick={handleClick}
      disabled={disabled}
    >
      {title}
    </Button>
  )
}

export default ButtonComponent
