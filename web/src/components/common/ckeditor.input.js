import React from "react";
import PropTypes from "prop-types";
import { FormGroup, Label } from "reactstrap";
import CKEditor from "ckeditor4-react";

const CKEditorInput = ({ title, name, data, onChange }) => {
  return (
    <FormGroup>
      <Label className="label-input" for={name}>
        {title}
      </Label>
      <CKEditor data={data} id={name} onChange={onChange} />
    </FormGroup>
  );
};

CKEditorInput.propTypes = {
  title: PropTypes.string,
  name: PropTypes.string
};

CKEditorInput.defaultProps = {
  title: "",
  name: ""
};

export default CKEditorInput;
