import { Row, Col, Button } from 'reactstrap';
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
            isShowDeleteModal: false,
            isShowInfoModal: false,
            item: {},
        };
    }

    showChangeAvatar = (item) => {
        this.toggleModalUpload(item, "Change Avatar")
    }

    toggleModalUpload = (item, title) => {
        this.setState(prevState => ({
            isShowUploadModal: !prevState.isShowUploadModal,
            item: item || {},
            formTitle: title
        }));
    }

    showUpdateModal = item => {
        let title = "Update Candidate";
        this.toggleModalInfo(item, title, false);
    };

    componentDidMount() {
        this.props.getProfile();
    }

    render() {
        const UserData = this.props.profile.profile;
        const { isShowDeleteModal, isShowInfoModal, isShowUploadModal, formTitle } = this.state;
        console.log(UserData);
        return (
            <div>
                {/* {this.renderViewProfile()} */}
                <Row>
                    <Col lg="2">
                        <Button
                            onClick={this.showAddNew}
                            className="btn btn-success btn-sm"> Update
                        </Button>
                    </Col>

                    <Col lg="3" className="profile_img">
                        <img onClick={() => this.showChangeAvatar(UserData)} className="custom_avatar"  src={UserData.avatar !== null ? "http://" + UserData.avatar : require("../../../assets/img/brand/ava2.jpg")} alt="avatar_picture"/>
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

                    <Col lg="2" />
                </Row>
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
