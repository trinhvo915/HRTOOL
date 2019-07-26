import React, { Component } from 'react';
import { Button, Modal, ModalHeader, ModalFooter } from 'reactstrap';
import { Input } from "reactstrap";
import Api from "../../api/api.candidate";
import { toastSuccess, toastError } from "../../helpers/toast.helper";

class ModalUpload extends Component {
  constructor(props) {
    super(props);
    this.state = {
      avatarFile:{}
    };
  }

  onHandleChange = (e) => {
    this.setState({
      avatarFile: e.target.files[0]
    });
  }

  doneUpdate = () => {
    this.setState({
      avatarFile:{}
    })
  }

  clickOk = async () => {
    if (!this.state.avatarFile.name) {
      toastError("Please choose an image to update avatar!!!");
      return ;
    }
    const avatarUrl = await Api.uploadAvatar("avatar", this.state.avatarFile);
    let item = {...this.props.item, avatarUrl: avatarUrl};
    try {
      await Api.updateCandidate(item);
      this.props.toggleModalUpload();
      this.props.getCandidateList();
      this.doneUpdate()
      toastSuccess("The job Candidate has been updated successfully");
    } catch (err) {
      toastError(err);
    }
  }
   
  render() {
    return (
      <div>
        <Modal isOpen={this.props.isShowModal} toggle={this.props.toggleModalUpload}>
          <ModalHeader>
            {this.props.title || "Change Avatar"}
          </ModalHeader>

          <Input type="file" name="avatarFile" onChange={this.onHandleChange} accept="image/x-png,image/gif,image/jpeg"/>

          <ModalFooter>
            <Button color="danger" onClick={this.clickOk}>
              Xác nhận
            </Button>{" "}
            <Button color="secondary" onClick={this.props.toggleModalUpload}>
              Huỷ bỏ
            </Button>
          </ModalFooter>
        </Modal>
      </div>
    );
  }

};

export default ModalUpload;