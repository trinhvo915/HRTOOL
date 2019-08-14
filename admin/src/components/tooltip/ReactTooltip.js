import React from "react";
import ToolTip from "react-portal-tooltip";

class ReactPortalTooltip extends React.Component {
  state = {
    isTooltipActive: false
  };

  showTooltip() {
    this.setState({ isTooltipActive: true });
  }

  hideTooltip() {
    this.setState({ isTooltipActive: false });
  }

  render() {
    return (
      <div
        id={this.props.id}
        onMouseEnter={this.showTooltip.bind(this)}
        onMouseLeave={this.hideTooltip.bind(this)}
      >
        <p>{this.props.name}</p>
        <img src={this.props.avatarUrl} />
        <ToolTip
          active={this.state.isTooltipActive}
          position="right"
          arrow="center"
          parent={`#${this.props.id}`}
        >
          <div>
            <p>{this.props.taskDescription}</p>
            <img src="image.png" />
          </div>
        </ToolTip>
      </div>
    );
  }
}

export default ReactPortalTooltip;
