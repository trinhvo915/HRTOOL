import React from 'react'
import { Link } from 'react-router-dom'
import { Button } from 'reactstrap'

const BottomBar = ({
  disabled,
  leftTitle,
  rightTitle,
  leftEndpoint,
  rightEndpoint,
  handleLeftClick,
  handleRightClick
}) => {
  return (
    <>
      <div className="spacer" />

      <div
        id="bottom-bar"
        className={`bottom-bar ${leftTitle && 'content-center'}`}
      >
        {leftTitle && (
          <div className="bottom-button-container" onClick={handleLeftClick}>
            <Link
              to={leftEndpoint}
              className={`bottom-button ${leftTitle && 'bottom-button-center'}`}
            >
              {leftTitle}
            </Link>
          </div>
        )}

        <div
          className={`bottom-button-container ${disabled && 'btn-disable'}`}
          onClick={handleRightClick}
        >
          {rightEndpoint ? (
            <Link
              to={rightEndpoint}
              className={`bottom-button ${leftTitle && 'bottom-button-center'}`}
            >
              {rightTitle}
            </Link>
          ) : (
            <div className="bottom-button">{rightTitle}</div>
          )}
        </div>
      </div>
    </>
  )
}

export default BottomBar
