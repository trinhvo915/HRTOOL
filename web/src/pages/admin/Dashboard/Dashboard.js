import React, { Component } from 'react';
import { Row } from 'reactstrap';

class Dashboard extends Component {
  constructor(props) {
    super(props);
    this.state = {};
  }

  render() {
    return (
      <div className="animated fadeIn">
        <Row>
          <span>Dashboad load here</span>
        </Row>
      </div>
    );
  }
}

export default Dashboard;
