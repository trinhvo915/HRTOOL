import React, { Fragment } from "react";
import PropTypes from "prop-types";
import { TabPane, Row, Col, FormGroup, Badge } from "reactstrap";
import { getJobList } from "../.../../../actions/job.list.action";
import { connect } from "react-redux";
import moment from "moment";
import { Line } from 'rc-progress';
import StarRatingComponent from 'react-star-rating-component';
import "./detail.css";

const Detail = ({
    detail
}) => {
    return (
        <TabPane tabId="1">

            <div>
                <Row>
                    <Col>
                        <FormGroup>
                            <div><strong> <i className="fa fa-users" aria-hidden="true"></i> Job name: </strong> {detail.name} </div>
                        </FormGroup>
                    </Col>
                </Row>

                <Row>
                    <Col>
                        <FormGroup>
                            <div>
                                <strong> <i className="fa fa-user " aria-hidden="true"></i> Priority: </strong>
                                <StarRatingComponent
                                    className="star-priority"
                                    name="rate1"
                                    starCount={5}
                                    value={detail.priority}
                                    editing={false}
                                    emptyStarColor="#EEEFFF" />
                            </div>
                        </FormGroup>

                    </Col>
                </Row>

                <Row>
                    <Col>
                        <FormGroup>
                            <div> <strong><i className="fa fa-clock-o" aria-hidden="true"></i> Date Start: </strong> {moment(detail.dateStart).format("HH:mm A DD-MM-YYYY")} </div>
                        </FormGroup>
                    </Col>
                </Row>

                <Row>
                    <Col>
                        <FormGroup>
                            <div> <strong> <i className="fa fa-clock-o" aria-hidden="true"></i> Date End: </strong> {detail.dateEnd ? moment(detail.dateEnd).format("HH:mm A DD-MM-YYYY") : ""} </div>
                        </FormGroup>
                    </Col>
                </Row>

                <Row>
                    <Col>
                        <FormGroup>
                            <div> <strong> <i className="fa fa-sticky-note-o" aria-hidden="true"></i> Description: </strong> {detail ? detail.description : 'Not Comment'} </div>
                        </FormGroup>
                    </Col>
                </Row>

                <Row>
                    <Col>
                        <FormGroup>
                            <div> <strong> <i className="fa fa-sticky-note-o" aria-hidden="true"></i> Reporter: </strong> {detail.reporter ? detail.reporter.name : ''} </div>
                        </FormGroup>
                    </Col>
                </Row>

                <Row>
                    <Col>
                        <FormGroup>
                            <div> <strong> <i className="fa fa-sticky-note-o" aria-hidden="true"></i> Attachment: </strong>
                                {detail && detail.attachments && Array.isArray(detail.attachments) && detail.attachments.map((attachment, key) =>
                                    <Fragment key={key}>
                                        <br />
                                        <a href={attachment.link} target="_blank" rel="noopener noreferrer" download>
                                            <i className="fa fa-download" aria-hidden="true"> {`${attachment.fileName}${attachment.extension}`}  </i>
                                        </a>
                                    </Fragment>
                                )}
                            </div>
                        </FormGroup>
                    </Col>
                </Row>

                <Row>
                    <Col>
                        <FormGroup>
                            <div> <strong> <i className="fa fa-users" aria-hidden="true"></i> Status:</strong> <Badge color="info" pill> {detail.statusText} </Badge> </div>
                        </FormGroup>
                    </Col>
                </Row>

                <Line percent="10"
                    strokeWidth="1"
                    strokeColor="#60c6f6"
                    trailWidth="1"
                    trailColor="#d9d9d9"
                    percent={detail.status === 1 ? 0 : detail.status === 2 ? 50 : 100}
                />

            </div>
        </TabPane>
    );

};

Detail.propTypes = {
    detail: PropTypes.object
};

Detail.defaultProps = {
    detail: {}
};

export default connect(
    state => ({
        jobList: state.jobList
    }),
    {
        getJobList
    }
)(Detail);