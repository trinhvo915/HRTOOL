import React, { Component, Fragment } from "react";
import { Route, Switch } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.min.css";
import Loadable from "react-loadable";
import "./App.scss";
import "font-awesome/css/font-awesome.min.css";
import AuthenticationRoute from "./components/common/authentication.route";

const loading = () => (
  <div className="animated fadeIn pt-3 text-center">Đang tải...</div>
);

// Containers
const DefaultLayout = Loadable({
  loader: () => import("./pages/user/User"),
  loading
});

// Pages
const Login = Loadable({
  loader: () => import("./pages/login/login.page"),
  loading
});

const Register = Loadable({
  loader: () => import("./pages/Register/Register"),
  loading
});

const Page404 = Loadable({
  loader: () => import("./pages/Page404/Page404"),
  loading
});

const Page500 = Loadable({
  loader: () => import("./pages/Page500/Page500"),
  loading
});

class App extends Component {
  render() {
    return (
      <Fragment>
        <ToastContainer />
        <Switch>
          <Route exact path="/login" name="Login Page" component={Login} />
          <Route
            exact
            path="/register"
            name="Register Page"
            component={Register}
          />
          <Route exact path="/404" name="Page 404" component={Page404} />
          <Route exact path="/500" name="Page 500" component={Page500} />
          <AuthenticationRoute path="/" name="admin" component={DefaultLayout} />
        </Switch>
      </Fragment>
    );
  }
}

export default App;
