import React from 'react'
import { Navbar } from 'reactstrap'

const Header = props => {
  return (
    <Navbar
      expand="md"
      fixed={`top`}
      className={`header ${props.title && 'header-left'}`}
    >
      {props.title ? (
        <div className="header-title">{props.title}</div>
      ) : (
        <>
          {props.brand && <div className="logo">{props.brand}</div>}
          <div className="slogan">Lưu giữ kỉ niệm bên nhau</div>
        </>
      )}
    </Navbar>
  )
}

export default Header
