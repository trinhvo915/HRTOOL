import React, { Component } from 'react';
import {
    Card,
    CardBody
} from 'reactstrap';
import './small.stats.css';
class SmallStats extends Component {

    render() {
        const { label, value } = this.props;
        return (
            <Card className="text-white bg-info" >
                <CardBody className="pb-0">
                    <div className="bock-name-value">
                        <div className="lable">
                            <h4>
                                {label}
                            </h4>
                        </div>
                        <div className="value">
                            <h1>
                                <strong>
                                    {value}
                                </strong>
                            </h1>
                        </div>
                    </div>
                </CardBody>
                <div className="chart-wrapper mx-3" style={{ height: '25px' }}>
                </div>
            </Card>
        );
    }
}

export default SmallStats;