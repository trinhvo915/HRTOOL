import React, { Component } from "react";
import { Route } from "react-router-dom";
import LoginRedirect from "./login.redirect";
import cookie from "react-cookies";
import ApiLogin from "../../api/api.login";
import { toastError } from "../../helpers/toast.helper";

export default class AuthenticationRoute extends Component {
  isMount = false;
  constructor(props) {
    super(props);
    this.state = {
      isAdmin: false,
      isLoading: true,
    }
  }

  async componentDidMount() {
    let token = cookie.load("token");
    this.isMount = true;
    this.isMount && this.isAdminFunc(token);
  }

  componentWillUnmount() {
    this.isMount = false;
  }

  isAdminFunc = async (token) => {
    if (token) {
      var user = await ApiLogin.getProfileByToken(token);
      var roleAdmin = await ApiLogin.getRoleAdmin();
      user.roles.forEach(roleUser => {
        roleAdmin.forEach(roleAdmin => {
          if (roleUser.id === roleAdmin.id) {
            this.isMount && this.setState({
              isAdmin: true,
            })
          }
        });
      });
    }
    this.setState({
      isLoading: false,
    })
  }

  render() {
    const { path, exact, name, component, key } = this.props;
    const { isAdmin, isLoading } = this.state;
    let token = cookie.load("token");
    if (!isLoading && !isAdmin && token && token.length > 0) {
      cookie.remove("token");
      toastError("Your account don't enough permission!!!");
    }
    return (
      <>
        {
          !isLoading && <Route
            path={path}
            exact={exact}
            name={name}
            component={this.state.isAdmin ? component : LoginRedirect}
            key={key}
          />
        }
      </>
    );
  }
}
