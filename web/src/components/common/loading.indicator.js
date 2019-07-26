import React from 'react'
import { Spinner } from 'reactstrap'

const LoadingIndicator = () => {
  return (
    <div className="spinner-container ">
      <Spinner color="primary text-danger" />
    </div>
  )
}

export default LoadingIndicator
