import React, { Component } from "react";
import { Route } from "react-router-dom";
import LoginRedirect from "./login.redirect";
import cookie from "react-cookies";

export default class AuthenticationRoute extends Component {
  render() {
    const { path, exact, name, component, key } = this.props;
    const token = cookie.load("token");
    return (
      <Route
        path={path}
        exact={exact}
        name={name}
        component={token ? component : LoginRedirect}
        key={key}
      />
    );
  }
}
