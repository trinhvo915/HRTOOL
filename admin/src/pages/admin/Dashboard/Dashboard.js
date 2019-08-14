import React, { Component } from "react";
import "./Dashboard.css";
import OrgChart from "react-orgchart";
import "react-orgchart/index.css";
import userApi from "../../../api/api.user";
import Tooltip from "../../../components/tooltip/ReactTooltip";

const MyNodeComponent = ({ node }) => {
  return (
    <div className="initechNode">
      <Tooltip
        id={node.name ? node.name.split(" ").join("") : ""}
        taskDescription={node.taskDescription}
        name={node.name}
        avatarUrl={node.avatarUrl}
      />
    </div>
  );
};

class Dashboard extends Component {
  constructor(props) {
    super(props);
    this.state = {
      userTree: []
    };
  }

  showTooltip() {
    this.setState({ isTooltipActive: true });
  }

  hideTooltip() {
    this.setState({ isTooltipActive: false });
  }

  componentDidMount = async () => {
    var tree = await userApi.getUserTree();
    this.setState({
      userTree: tree
    });
  };

  render() {
    return (
      this.state.userTree.length > 0 ?
        <OrgChart tree={this.state.userTree[0]} NodeComponent={MyNodeComponent} /> : ""
    );
  }
}
export default Dashboard;
