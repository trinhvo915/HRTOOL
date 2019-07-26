import { Row, Col } from 'reactstrap';
import 'react-toastify/dist/ReactToastify.css';
import React, { Component } from 'react';
import 'react-toastify/dist/ReactToastify.css';
import "./user.profile.css";
import moment from 'moment';
import { connect } from 'react-redux';
import { getProfile } from "../../../actions/profile.action";

class UserProfile extends Component {
    constructor(props) {
        super(props);
        this.state = {
            name: '',
            email: '',
            mobile: '',
            gender: '',
            dateOfBirth: '',
            data: [],
            date: new Date(),
            id: '',
            avataURL: '',
        };
    }

    renderViewProfile = () => {

        const UserData = this.props.profile.profile

        console.log(UserData)
        return (
            <Row>
                <Col lg="2"/>

                <Col lg="3" className="profile_img">
                    <img src={UserData.avatar !== null ? "http://" + UserData.avatar : require("../../../assets/img/brand/ava2.jpg")} alt="avatar_picture" />
                </Col>

                <Col lg="5" className="group_info ">
                    <Row>
                        <div className="profile-head">
                            <h3>
                                <strong>{UserData.name}</strong>
                            </h3>
                        </div>
                    </Row>
                    <div className="line_header">
                        <hr />
                    </div>

                    <Row>
                        <Col lg="2">
                            <label> <strong>Email</strong></label>
                        </Col>
                        <Col lg="10">
                            <p>: {UserData.email ? UserData.email : ""}</p>
                        </Col>
                    </Row>
                    <div className="line">
                        <hr />
                    </div>

                    <Row>
                        <Col lg="2">
                            <label> <strong>Phone</strong></label>
                        </Col>
                        <Col lg="10">
                            <p>: {UserData.mobile ? UserData.mobile : ""}</p>
                        </Col>
                    </Row>
                    <div className="line">
                        <hr />
                    </div>

                    <Row>
                        <Col lg="2">
                            <label> <strong>Gender</strong></label>
                        </Col>
                        <Col lg="10">
                            <p>: {UserData.gender === 1 ? "Male" : UserData.gender === 2 ? "Female" : "None"}</p>
                        </Col>
                    </Row>
                    <div className="line">
                        <hr />
                    </div>

                    <Row>
                        <Col lg="2">
                            <label> <strong>Birthday</strong></label>
                        </Col>
                        <Col lg="10">
                            <p>: {UserData.dateOfBirth ? moment(UserData.dateOfBirth).format('DD /MM /YYYY') : "none"}</p>
                        </Col>
                    </Row>
                    <div className="line">
                        <hr />
                    </div>

                    <Row>
                        <Col lg="2">
                            <label> <strong>Address</strong></label>
                        </Col>
                        <Col lg="10">
                            <p>: {UserData.address ? UserData.address : "none"}</p>
                        </Col>
                    </Row>
                </Col>

                <Col lg="2"/>
            </Row>
        )
    }

    render() {
        return (
            <div>
                {this.renderViewProfile()}
            </div>
        );
    }
}

export default connect(
    state => ({
        profile: state.profile
    }),
    {
        getProfile
    }
)(UserProfile);
