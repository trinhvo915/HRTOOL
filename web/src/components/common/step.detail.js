import React, { Component } from 'react';
import classNames from 'classnames'
import "./step.detail.css";
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from "reactstrap";

class Step extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isOpen: false,
            update: false,
            value: "",
            isShowStatusModal: false,
        }
        this.wrapperRef = React.createRef();
    }

    onClick = () => {
        const { isOpen } = this.state;
        this.setState({
            isOpen: !isOpen
        })
    }

    updateClick = () => {
        this.setState({
            update: !this.state.update,
            value: this.props.step.name
        })
    }

    showUpdateStatusModal = (step) => {
        this.toggleStatusupdateModal(step);
    };

    toggleStatusupdateModal = (step) => {
        this.setState(prevState => ({
            isShowStatusModal: !prevState.isShowStatusModal,
            step: step || {},
        }));
    }

    onModelChange = el => {
        let status = el.target.name;
        let inputValue = el.target.value;
        let step = { ...this.state.step }
        step[status] = inputValue;
        this.setState({ step });
    };

    updateStatus = async () => {
        await this.props.updateStep(this.state.step)
        this.toggleStatusupdateModal()
    }


    onKeyDown = async (e) => {
        e.persist();
        if (e.keyCode === 13) {
            if (this.state.value.trim() !== "") {
                await this.props.updateStep(this.state.step)
                this.setState({
                    isOpen: false,
                    update: false,
                })
            }
        }
        if (e.keyCode === 27) {
            this.setState({
                update: false
            })
        }
    }

    onChange = (e) => {
        let { value } = this.state;
        value = e.target.value;
        this.setState({
            value,
            step: {
                ...this.state.step,
                name: value
            }
        })
    }

    componentDidMount() {
        document.addEventListener('click', this.handleClick);
        this.setState({
            value: this.props.step.name,
            isOpen: false,
            update: false,
            step: this.props.step
        })
    }

    componentWillUnmount() {
        document.removeEventListener('click', this.handleClick)
    }

    handleClick = (event) => {
        const { target } = event;
        if (this.wrapperRef.current && !this.wrapperRef.current.contains(target) && this.wrapperRef.current.className === "option") {
            this.setState(preState => ({
                isOpen: !preState.isOpen
            }))
        }
    }

    render() {
        const { step, deleteStep } = this.props;
        const { isOpen, value, update } = this.state;
        return (
            <div className="detail-step">
                <Modal isOpen={this.state.isShowStatusModal} toggle={this.toggleStatusupdateModal} >
                    <ModalHeader>
                        Update Status
                    </ModalHeader >
                    <ModalBody>
                        <select className="form-control" defaultValue={step.status} name='status' onChange={this.onModelChange}>
                            <option > Please Choose Your Status... </option>
                            <option value="1"> Pending </option>
                            <option value="2"> Doing </option>
                            <option value="3"> Done </option>
                        </select>
                    </ModalBody>
                    <ModalFooter>
                        <Button className=" btn-primary" onClick={this.updateStatus} > Confirm </Button>{" "}
                        <Button className="btn-danger" onClick={this.toggleStatusupdateModal}> Cancel </Button>
                    </ModalFooter>
                </Modal>
                {update ?
                    <input className="comment" type="text" value={value} onKeyDown={(e) => this.onKeyDown(e)} onChange={(e) => this.onChange(e)} autoFocus /> :
                    <p className="step">{step.name}</p>
                }

                <span ref={this.wrapperRef} id="icon-option" className={classNames("option", { "isOpen": !isOpen })} onClick={this.onClick}>
                    <i className="icons cui-options"></i>
                    <div className={classNames("update-delete", { "isOpenSelect": !isOpen })}>
                        <div className="update" onClick={this.updateClick} data-toggle="tooltip" title="Update" >
                            <i className="fa fa-pencil-square-o padding-icon " aria-hidden="true"></i>
                            {/* <span>Update</span> */}
                        </div>
                        <div className="delete" onClick={deleteStep} data-toggle="tooltip" title="Delete"  >
                            <i className="fa fa-times padding-icon " aria-hidden="true"></i>
                            {/* <span>Delete</span> */}
                        </div>
                    </div>
                </span>

                <Button
                    className="step_status_button"
                    color={step.status === 2 ? "secondary" : step.status === 3 ? "success" : "warning"}
                    onClick={() => this.showUpdateStatusModal(step)}>
                    {step.status === 2 ? "Doing" : step.status === 3 ? "Done" : "Pending"}
                </Button>
            </div>
        );
    }
}

export default Step;