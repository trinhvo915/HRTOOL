import React from "react";
import { FormGroup, Label } from "reactstrap";

const InfoRow = ({ title, value }) => {
  return (
    <FormGroup>
      <Label>
        {title}: {value}
      </Label>
    </FormGroup>
  );
};

export default InfoRow;
