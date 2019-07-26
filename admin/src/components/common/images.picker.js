import React, { Component } from "react";
import { FormGroup, Label } from "reactstrap";
import { FilePicker } from "react-file-picker";

class ImagesPicker extends Component {
  constructor(props) {
    super(props);
    this.state = {};
  }

  render = () => {
    const { title, images, onImagesChange } = this.props;
    return (
      <FormGroup>
        <Label className="label-input">{title}</Label>
        <FilePicker
          extensions={["jpg", "jpeg", "png"]}
          onChange={file => onImagesChange([...images, file])}
          onError={err => {}}
        >
          <div>Thêm ảnh</div>
        </FilePicker>
        {images &&
          images.map((image, index) => {
            const src = image.id ? image.thumb : URL.createObjectURL(image);
            return (
              <img
                alt=""
                key={index.toString()}
                src={src}
                width="100"
                height="100"
              />
            );
          })}
      </FormGroup>
    );
  };
}

export default ImagesPicker;
