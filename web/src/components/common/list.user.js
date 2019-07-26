import React from "react";
import PropTypes from "prop-types";
import { Row, Col } from "reactstrap";

const ListUser = ({
    username,
    roles
}) => {
    return (
        <Row>
            <Col>
                <div className="avatar float-left">
                    <img className="" src="https://cdn4.iconfinder.com/data/icons/avatars-21/512/avatar-circle-human-male-3-512.png" alt="abc"/>
                </div>
                <div className="content">
                    <div className="username"><h5>{username}</h5></div>
                </div>
                <div className="line">
                    <hr />
                </div>
            </Col>
        </Row>
    );
};

ListUser.propTypes = {
    username: PropTypes.string,
    role: PropTypes.string
};

ListUser.defaultProps = {
    username: "",
    role: ""
};

export default ListUser;