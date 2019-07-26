import React, { Component } from "react";
import { Row, Col, Input } from "reactstrap";
import classNames from "classnames";

import "./job.list.page.css";

class StepList extends Component {
    constructor(props){
        super(props);
        this.state = {
            update: false,
            value: "",
            isOpen: false,
            stepStatus: 1
        }
        this.wrapperRef = React.createRef();
    }

    onClick = () => {
        this.setState({
            isOpen: !this.state.isOpen
        })
    }

    updateClick = () => {
        this.setState({
            update: !this.state.update,
            
        })
    }

    onChange = (e) => {
        this.setState({
            value: e.target.value
        })
    }

    onKeyDown = async (e) => {
        e.persist();
        if(e.keyCode === 13){
            if(this.state.value.trim() !== ""){
                var step = Object.assign({}, this.props.step, {name: this.state.value})
                var success = await this.props.updateStep(step);
                
                if(success){
                    this.props.step.name = step.name;
                    this.setState({
                        isOpen: false,
                        update: false,
                    })
                }
            }
        }
        if(e.keyCode === 27){
            this.setState({
                update: false,
                value: this.props.step.name
            })
        }
    }

    changeStatus = async (e) => {
        this.props.step.status = e.target.value;
        this.setState({
            stepStatus: parseInt(e.target.value)
        })
        await this.props.updateStep(this.props.step);
    }

    componentDidMount(){
        this.setState({
            value: this.props.step.name,
            stepStatus: this.props.step.status
        })
        document.addEventListener('click', this.handleClick);
    }

    componentWillUnmount() {
        document.removeEventListener('click', this.handleClick)
    }

    handleClick = (event) => {
        const { target } = event;
        if(this.wrapperRef.current && !this.wrapperRef.current.contains(target) && this.wrapperRef.current.className === "option"){
            this.setState(preState => ({
                isOpen: !preState.isOpen
            }))
        }
    }

    render() {
        const { update, value, isOpen, stepStatus } = this.state;
        const { step, deleteStep } = this.props;
        const status = stepStatus === 1 ? 'bg-warning' : stepStatus === 2 ? 'bg-secondary' : 'bg-success';
        return (
            <div>
                <Row >
                   
                    <Col sm='9' className="content d-flex align-items-center">
                        
                                {update ? 
                                    <Input required={true}  style={{border: "none"}} className="step-input" type="text" value={value} onKeyDown={(e) => this.onKeyDown(e)}  onChange={(e) => this.onChange(e)} autoFocus/> : 
                                     step.name
                                }
                                {
                                    <span ref={this.wrapperRef} id="icon-option"  className={classNames("option", {"isOpen": !isOpen})} onClick={this.onClick}>
                                        <i className="icons cui-options"></i>
                                        <div className={classNames("update-delete", {"isOpen": !isOpen})}>
                                            <div className="update" onClick={this.updateClick} data-toggle="tooltip"  title="Update" >
                                                <i className="fa fa-pencil-square-o padding-icon " style={{"color" : "#12c912"}} aria-hidden="true"></i>
                                                {/* <span>Update</span> */}
                                            </div>
                                            <div className="delete" onClick={deleteStep} data-toggle="tooltip" title="Delete"  >
                                                <i className="fa fa-times padding-icon " aria-hidden="true"></i>
                                                {/* <span>Delete</span> */}
                                            </div>
                                        </div>
                                    </span> 
                                }
                        
                    </Col>

                    <Col sm='3'>
                        <Input value={stepStatus} onChange={(e) => this.changeStatus(e)} type="select" name="select" className={classNames(status)} id="select">
                            <option className="form-control" value={1}>Pending</option>
                            <option className="form-control" value={2}>Doing</option>
                            <option className="form-control" value={3}>Done</option>
                        </Input>
                    </Col>
                </Row>
                <div className="line">
                    <hr />
                </div>
            </div>
        );
    }
}

export default StepList;