import React from "react";
import numeral from "numeral";

const CurrencyText = ({ value, format }) => {
  return <>{numeral(value).format(format)} đ</>;
};

CurrencyText.defaultProps = {
  format: "0,0"
};

export default CurrencyText;
