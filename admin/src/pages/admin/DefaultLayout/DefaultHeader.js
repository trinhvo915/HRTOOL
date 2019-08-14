import React, { Component } from "react";
import { Link } from 'react-router-dom';
import {
    DropdownItem,
    DropdownMenu,
    DropdownToggle,
    Nav,
    UncontrolledDropdown
} from "reactstrap";
import PropTypes from "prop-types";

import {
    AppNavbarBrand,
    AppSidebarToggler
} from "@coreui/react";
import logo from "../../../assets/img/brand/logo.svg";
import sygnet from "../../../assets/img/brand/sygnet.svg";
import { toastError } from "../../../helpers/toast.helper";

const propTypes = {
    children: PropTypes.node
};

const defaultProps = {};

class DefaultHeader extends Component {
    constructor(props) {
        super(props);
        this.state = {
            userIdLogin: null
        };
    }

    getCookie(cname) {
        var name = cname + "=";
        var decodedCookie = decodeURIComponent(document.cookie);
        var ca = decodedCookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) === ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) === 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }

    componentDidMount() {
        var tokenInfo = this.getCookie("token").split(".")[1];
        try {
            var userToken = JSON.parse(window.atob(tokenInfo));
            var userInfo = userToken.JwtPayload;
            this.setState({
                userIdLogin: userInfo.UserId
            })
        } catch (err) {
            toastError(err.message)
        }
    }

    render() {
        const { userIdLogin } = this.state
        // eslint-disable-next-line
        const { children, ...attributes } = this.props;

        return (
            <React.Fragment>
                <AppSidebarToggler className="d-lg-none" display="md" mobile />
                <AppNavbarBrand
                    full={{ src: logo, width: 89, height: 25, alt: "CoreUI Logo" }}
                    minimized={{ src: sygnet, width: 30, height: 30, alt: "CoreUI Logo" }}
                />
                <AppSidebarToggler className="d-md-down-none" display="lg" />

                <Nav className="ml-auto" navbar>
                    <UncontrolledDropdown direction="down">
                        <DropdownToggle nav>
                            <img
                                src={"../../assets/img/avatars/6.jpg"}
                                className="img-avatar"
                                alt="admin@bootstrapmaster.com"
                            />
                        </DropdownToggle>
                        <DropdownMenu right style={{ right: "auto" }}>
                            <DropdownItem header tag="div" className="text-center">
                                <strong>Account</strong>
                            </DropdownItem>

                            <DropdownItem>
                                <i className="fa fa-user-secret">
                                    <Link to={`/users/${userIdLogin}`} style={{ fontFamily: "'Lobster', cursive ", marginLeft: "20px" }} > View Profile </Link>
                                </i>
                            </DropdownItem>

                            <DropdownItem onClick={e => this.props.onLogout(e)}>
                                <i className="fa fa-lock" /> Logout
                        </DropdownItem>
                        </DropdownMenu>
                    </UncontrolledDropdown>
                </Nav>
                {/* <AppAsideToggler className="d-md-down-none" />
                <AppAsideToggler className="d-lg-none" mobile /> */}
            </React.Fragment>
        );
    }
}

DefaultHeader.propTypes = propTypes;
DefaultHeader.defaultProps = defaultProps;

export default DefaultHeader;
