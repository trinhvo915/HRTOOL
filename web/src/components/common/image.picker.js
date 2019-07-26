import React, { Component } from "react";
import { FormGroup, Label } from "reactstrap";
import { FilePicker } from "react-file-picker";

class ImagePicker extends Component {
  constructor(props) {
    super(props);
    this.state = {};
  }

  render = () => {
    const { title, image, onImageChange } = this.props;
    return (
      <FormGroup>
        <Label className="label-input">{title}</Label>
        <FilePicker
          extensions={["jpg", "jpeg", "png"]}
          onChange={file => onImageChange(file)}
          onError={err => { }}
        >
          <div>Thêm ảnh</div>
        </FilePicker>
        {image &&
          <img
            alt=""
            src={image.id ? image.thumb : URL.createObjectURL(image)}
            width="100"
            height="100"
          />
        }
      </FormGroup>
    );
  };
}

export default ImagePicker;
