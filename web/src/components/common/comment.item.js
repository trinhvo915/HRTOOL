import React, { Component } from "react";
import PropTypes from "prop-types";
import { Row, Col, Input } from "reactstrap";
import classNames from "classnames";


import "./comment.item.css"

class CommentItem extends Component {
    constructor(props){
        super(props);
        this.state = {
            isOpen: false,
            update: false,
            value: ""
        }
        this.wrapperRef = React.createRef();
    }

    onClick = () => {
        this.setState(preState => ({
            isOpen: !preState.isOpen
        }))
    }

    onChange = (e) => {
        this.setState({
            value: e.target.value
        })
    }

    updateClick = () => {
        this.setState({
            update: !this.state.update,
            value: this.props.comment
        })
    }

    onKeyDown = async (e) => {
        e.persist();
        if(e.keyCode === 13){
            if(this.state.value.trim() !== ""){
                await this.props.updateComment(this.state.value)
                this.setState({
                    isOpen: false,
                    update: false,
                })
            }
        }
        if(e.keyCode === 27){
            this.setState({
                update: false
            })
        }
    }
    

    componentDidMount(){
        document.addEventListener('click', this.handleClick);
        this.setState({
            value: this.props.comment,
            isOpen: false,
            update: false,
        })
    }

    componentWillUnmount() {
        document.removeEventListener('click', this.handleClick)
    }

    handleClick = (event) => {
        const { target } = event;
        if( this.wrapperRef.current && !this.wrapperRef.current.contains(target) && this.wrapperRef.current.className === "option"){
            this.setState(preState => ({
                isOpen: !preState.isOpen
            }))
        }
    }

    render(){
        const { comment, name, day, userId, userIdLogin, deleteComment } = this.props;
        const { isOpen, update, value } = this.state;
        return (
            <div>
                <Row >
                    <Col sm='2'>
                        <div className="ant-comment-inner" style={{"justifyContent": "center"}} >
                            <div className="ant-comment-avatar" >
                                <img alt="" src="https://cdn4.iconfinder.com/data/icons/avatars-21/512/avatar-circle-human-male-3-512.png" />
                            </div>
                        </div>
                    </Col>
                    <Col sm='10' className="content">
                        <div className="ant-comment-content">
                            <div className="ant-comment-content-author"></div>
                            <span className="ant-comment-content-author-name">{name}</span>
                            <span className="ant-comment-content-author-time">
                                <span>{day}</span>
                            </span>
                        </div>
                        <div className="ant-comment-content-detail">
                            <div className="wrap-text">
                                {update ? 
                                    <Input className="comment" type="text" value={value} onKeyDown={(e) => this.onKeyDown(e)}  onChange={(e) => this.onChange(e)} autoFocus/> : 
                                    comment
                                }
                                {
                                    userId === userIdLogin ? 
                                
                                    <span ref={this.wrapperRef} id="icon-option"  className={classNames("option", {"isOpen": !isOpen})} onClick={this.onClick}>
                                        <i className="icons cui-options"></i>
                                        <div className={classNames("update-delete", {"isOpen": !isOpen})}>
                                            <div className="update" onClick={this.updateClick} data-toggle="tooltip"  title="Update" >
                                                <i className="fa fa-pencil-square-o padding-icon " aria-hidden="true"></i>
                                                {/* <span>Update</span> */}
                                            </div>
                                            <div className="delete" onClick={deleteComment} data-toggle="tooltip" title="Delete"  >
                                                <i className="fa fa-times padding-icon " aria-hidden="true"></i>
                                                {/* <span>Delete</span> */}
                                            </div>
                                        </div>
                                    </span> : ""
                                }
                            </div>
                        </div>
                    </Col>

                </Row>
                <div className="line">
                    <hr />
                </div>
            </div>
        );
    };
}
CommentItem.propTypes = {
    name: PropTypes.string,
    comment: PropTypes.string
};

CommentItem.defaultProps = {
    name: "",
    comment: ""
};

export default CommentItem;