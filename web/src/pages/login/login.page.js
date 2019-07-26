import React, { Component } from "react";
import cookie from "react-cookies";
import {
  Button,
  Card,
  CardBody,
  Col,
  Container,
  Input,
  InputGroup,
  InputGroupAddon,
  InputGroupText,
  Row
} from "reactstrap";
import Loading from "../../components/common/loading.indicator";
import Api from "../../api/api";

class Login extends Component {
  constructor(props) {
    super(props);
    this.state = {
      loading: false,
      user: {
        email: "",
        password: ""
      },
      error: ""
    };
  }

  onLogin = async (e) => {
    e.preventDefault();
    let { user } = this.state;
    if (!user.email || !user.password)
      return this.setState({
        error: "Please enter the Username and Password!"
      });
    try {
      await this.setState({ loading: true, error: "" });
      const data = await Api.login(user);
      const token = data;
      cookie.save("token", token);
      this.setState({ loading: false });
      this.props.history.push("/");
    } catch (err) {
      //const message = err.response.data.message;
      this.setState({ loading: false, error: "" });
    }
  };

  onChange = e => {
    let value = e.target.value;
    let attr = e.target.name;

    let user = Object.assign({}, this.state.user);
    user[attr] = value;
    this.setState({ user, error: "" });
  };

  render() {
    const { loading, error } = this.state;
    return (
      <>
        {loading && <Loading />}
        <div
          className={`app flex-row align-items-center ${loading &&
            "wrapper-indicator"}`}
        >
          <Container>
            <Row className="justify-content-center">
              <Col md="4">
                <Card className="p-4 text-center">
                  <CardBody>
                    <form onSubmit={this.onLogin}>
                      <p className="text-muted">Login</p>
                      <InputGroup className="mb-3">
                        <InputGroupAddon addonType="prepend">
                          <InputGroupText>
                            <i className="icon-user" />
                          </InputGroupText>
                        </InputGroupAddon>
                        <Input
                          name="email"
                          type="text"
                          placeholder="Username"
                          autoComplete="email"
                          onChange={this.onChange}
                        />
                      </InputGroup>
                      <InputGroup className="mb-4">
                        <InputGroupAddon addonType="prepend">
                          <InputGroupText>
                            <i className="icon-lock" />
                          </InputGroupText>
                        </InputGroupAddon>
                        <Input
                          name="password"
                          type="password"
                          placeholder="Password"
                          autoComplete="password"
                          onChange={this.onChange}
                        />
                      </InputGroup>
                      {error && <p style={{ color: "red" }}>{error}</p>}
                      <Row
                        style={{
                          justifyContent: "center"
                        }}
                      >
                        <Col xs="6">
                          <Button
                            //  onClick={this.onLogin}
                            color="primary"
                            className="px-6"
                            type="submit"
                          >
                            Login
                        </Button>
                        </Col>
                      </Row>
                    </form>
                  </CardBody>
                </Card>
              </Col>
            </Row>
          </Container>
        </div>
      </>
    );
  }
}

export default Login;
