import React, { Component } from "react";
import { Redirect } from "react-router-dom";

export default class LoginRedirect extends Component {
  render() {
    return (
      <Redirect
        to={{
          pathname: "/login"
        }}
      />
    );
  }
}
