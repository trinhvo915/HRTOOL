import React from "react";
import moment from "moment";

const TimeText = ({ value, format }) => {
  return <>{moment(value).format(format)}</>;
};

TimeText.defaultProps = {
  format: "DD/MM/YYYY HH:mm"
};

export default TimeText;
