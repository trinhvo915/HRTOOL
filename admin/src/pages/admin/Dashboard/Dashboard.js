import React, { Component } from 'react';
import {
    Col,
    Row,
    FormGroup, Label
} from 'reactstrap';
import moment from "moment";
import ApiStatJob from "../../../api/api.stat.job";
import ApiStatInterview from "../../../api/api.stat.interview";
import SmallStats from "../../../components/common/small.stats";
import Datetime from "react-datetime";
import CanvasJSReact from '../../../assets/canvasjs.react';

import './Dashboard.css'

var CanvasJSChart = CanvasJSReact.CanvasJSChart;

class Dashboard extends Component {
    constructor(props) {
        super(props);
        this.state = {
            statJob: [],
            statInterview: [],
            params: {
                dateStart: null,
                dateEnd: null
            },
            optionsJob: {
                exportEnabled: true,
                animationEnabled: true,
                theme: "light2",
                title: {
                    text: "JOBS STAT"
                },
                data: [{
                    type: "pie",
                    colorSet: [
                        "#2F4F4F",
                        "#008080",
                        "#2E8B57"
                    ],
                    startAngle: 75,
                    toolTipContent: "<b>{label}</b>: {y}%",
                    showInLegend: "true",
                    legendText: "{label}",
                    indexLabelFontSize: 16,
                    indexLabel: "{label} : {y}%",
                    dataPoints: []
                }]
            },
            optionsInteview: {
                exportEnabled: true,
                animationEnabled: true,
                theme: "light2",
                title: {
                    text: "INTERVIEW STAT"
                },
                data: [{
                    type: "pie",
                    startAngle: 75,
                    toolTipContent: "<b>{label}</b>: {y}%",
                    showInLegend: "true",
                    legendText: "{label}",
                    indexLabelFontSize: 16,
                    indexLabel: "{label} : {y}%",
                    dataPoints: []
                }]
            }
        };
    }

    onDateStartChangeJob = async (el) => {
        let inputValue = new Date(el._d);
        inputValue.setDate(inputValue.getDate());

        let item = Object.assign({}, this.state.params);
        item.dateStart = inputValue.toUTCString();
        await this.setState({ params: item });

        let { params } = this.state;
        let statJobAnimation = await ApiStatJob.getStatJobAnimationList(params);
        let itemJob = this.state.optionsJob.data[0];
        itemJob = { ...itemJob, dataPoints: statJobAnimation };
        let dataJob = [];
        dataJob.push(itemJob);
        await this.setState({
            optionsJob: { ...this.state.optionsJob, data: dataJob },
        })
    };

    onDateEndChangeJob = async (el) => {
        let inputValue = new Date(el._d);
        inputValue.setDate(inputValue.getDate() + 1);

        let item = Object.assign({}, this.state.params);
        item.dateEnd = inputValue.toUTCString();
        await this.setState({ params: item });
        let { params } = this.state;
        let statJobAnimation = await ApiStatJob.getStatJobAnimationList(params);
        let itemJob = this.state.optionsJob.data[0];
        itemJob = { ...itemJob, dataPoints: statJobAnimation };
        let dataJob = [];
        dataJob.push(itemJob);
        await this.setState({
            optionsJob: { ...this.state.optionsJob, data: dataJob },
        })
    };

    onDateStartChangeInterview = async (el) => {
        let inputValue = new Date(el._d);
        inputValue.setDate(inputValue.getDate());

        let item = Object.assign({}, this.state.params);
        item.dateStart = inputValue.toUTCString();
        await this.setState({ params: item });

        let { params } = this.state;
        let statInterview = await ApiStatInterview.getStatInterviewAnimation(params);
        let itemInterview = this.state.optionsInteview.data[0];
        itemInterview = { ...itemInterview, dataPoints: statInterview };
        let dataInterview = [];
        dataInterview.push(itemInterview);
        await this.setState({
            optionsInteview: { ...this.state.optionsInteview, data: dataInterview }
        })
    };

    onDateEndChangeInterview = async (el) => {
        let inputValue = new Date(el._d);
        inputValue.setDate(inputValue.getDate() + 1);

        let item = Object.assign({}, this.state.params);
        item.dateEnd = inputValue.toUTCString();
        await this.setState({ params: item });
        let { params } = this.state;
        let statInterview = await ApiStatInterview.getStatInterviewAnimation(params);
        let itemInterview = this.state.optionsInteview.data[0];
        itemInterview = { ...itemInterview, dataPoints: statInterview };
        let dataInterview = [];
        dataInterview.push(itemInterview);
        await this.setState({
            optionsInteview: { ...this.state.optionsInteview, data: dataInterview }
        })
    };

    async componentDidMount() {
        let statJob = await ApiStatJob.getStatJobList();
        let statInterview = await ApiStatInterview.getStatInterviewList();

        let statJobAnimation = await ApiStatJob.getStatJobAnimationList();
        let statInterviewAnimation = await ApiStatInterview.getStatInterviewAnimation()

        let itemJob = this.state.optionsJob.data[0];
        itemJob = { ...itemJob, dataPoints: statJobAnimation };
        let dataJob = [];
        dataJob.push(itemJob);

        let itemInterview = this.state.optionsInteview.data[0];
        itemInterview = { ...itemInterview, dataPoints: statInterviewAnimation };
        let dataInterview = [];
        dataInterview.push(itemInterview);

        this.setState({
            statJob: statJob,
            statInterview: statInterview,
            optionsJob: { ...this.state.optionsJob, data: dataJob },
            optionsInteview: { ...this.state.optionsInteview, data: dataInterview }
        })
    }

    render() {
        const { statJob, statInterview, optionsJob, optionsInteview } = this.state;
        return (
            <div className="animated fadeIn" style={{ 'marginLeft': '12px' }}>
                <Row className="custom_option">

                    {/* start job */}
                    <Col lg="5" className="custome_dashboard_card" >
                        <Row className="custom_title d-flex justify-content-center">
                            <h2>Stat Job</h2>
                        </Row>
                        <Row className="cards">
                            {
                                statJob.map((stats, idx) => (
                                    <Col xs="12" sm="6" lg="6" key={idx}>
                                        <SmallStats
                                            id={`small-stats-${idx}`}
                                            label={stats.label}
                                            value={stats.y}
                                        />
                                    </Col>
                                ))}
                        </Row>
                    </Col>

                    <Col className="custome_dashboard_chart" lg="7">
                        <Row className="custom_select_date ">
                            <Col >
                                <Row>
                                    <Label for="examplePassword">
                                        <strong>Date Start</strong>
                                    </Label>
                                </Row>
                                <Row>
                                    <FormGroup>
                                        <Datetime
                                            defaultValue={moment().format(
                                                "DD-MM-YYYY"
                                            )}
                                            timeFormat={false}
                                            dateFormat="DD-MM-YYYY"
                                            onChange={this.onDateStartChangeJob}
                                        />
                                    </FormGroup>
                                </Row>
                            </Col>
                            <Col>
                                <Row>
                                    <Label for="examplePassword">
                                        <strong>Date End</strong>
                                    </Label>
                                </Row>
                                <Row>
                                    <FormGroup>
                                        <Datetime
                                            defaultValue={moment().format(
                                                "DD-MM-YYYY"
                                            )}
                                            timeFormat={false}
                                            dateFormat="DD-MM-YYYY"
                                            onChange={this.onDateEndChangeJob}
                                        />
                                    </FormGroup>
                                </Row>
                            </Col>
                        </Row>

                        <Row>
                            <CanvasJSChart options={optionsJob} />
                        </Row>
                    </Col>

                </Row>

                <Row className="custom_option">

                    <Col lg="5" className="custome_dashboard_card2" >
                        <Row className="custom_title d-flex justify-content-center">
                            <h3>Stat Interview</h3>
                        </Row>
                        <Row className="cards">
                            {
                                statInterview.map((stats, idx) => (
                                    <Col xs="12" sm="6" lg="6" key={idx}>
                                        <SmallStats
                                            id={`small-stats-${idx}`}
                                            label={stats.label}
                                            value={stats.y}
                                        />
                                    </Col>
                                ))}
                        </Row>
                    </Col>

                    <Col className="custome_dashboard_chart" lg="7">
                        <Row className="custom_select_date">
                            <Col>
                                <Row>
                                    <Label for="examplePassword">
                                        <strong>Date Start</strong>
                                    </Label>
                                </Row>
                                <Row>
                                    <FormGroup>
                                        <Datetime
                                            defaultValue={moment().format(
                                                "DD-MM-YYYY"
                                            )}
                                            timeFormat={false}
                                            dateFormat="DD-MM-YYYY"
                                            onChange={this.onDateStartChangeInterview}

                                        />
                                    </FormGroup>
                                </Row>
                            </Col>
                            <Col>
                                <Row>
                                    <Label for="examplePassword">
                                        <strong>Date End</strong>
                                    </Label>
                                </Row>
                                <Row>
                                    <FormGroup>
                                        <Datetime
                                            defaultValue={moment().format(
                                                "DD-MM-YYYY"
                                            )}
                                            timeFormat={false}
                                            dateFormat="DD-MM-YYYY"
                                            onChange={this.onDateEndChangeInterview}
                                        />
                                    </FormGroup>
                                </Row>
                            </Col>
                        </Row>
                        <Row>
                            <CanvasJSChart options={optionsInteview} />
                        </Row>
                    </Col>
                </Row>
            </div>
        );
    }
}

export default Dashboard;
