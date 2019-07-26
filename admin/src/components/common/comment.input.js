import "./comment.input.css"
import { Row, Col, Input } from "reactstrap"
import React, { Component } from "react";

class CommentInput extends Component {
    constructor(props) {
        super(props);
        this.state = {
            content: "",
            rows: 1,
            multiKey: {16: false, 13: false} // shift && enter
        }
    }

    onChange = e => {
        let content = e.target.value;
        this.setState({ content }, 
        //     () => {
        //     var textarea = document.getElementById("inputComment");
            
        //     if(textarea.scrollHeight > 54){
        //         if(this.state.rows < 3){
        //             this.setState(preState => ({
        //                 rows: 3
        //             }))
        //         }
        //     }
        //     else if(textarea.scrollHeight > 33){
        //         if(this.state.rows < 3){
        //             this.setState(preState => ({
        //                 rows: 2
        //             }))
        //         }
        //     }
        // }
        );
    }

    onSubmit = () => {
        const { content } = this.state;
        this.props.updateItem(content);
        this.setState({ content: '' });
    }

    onKeyDown = (e) => {
        e.persist();
        const { multiKey, content } = this.state;
        if(e.keyCode === 13 && !multiKey[16]){
            e.preventDefault();
            if(content.trim() !== ""){
                this.onSubmit();
            }
        }

        if(e.keyCode in multiKey){
            var keys = Object.assign({}, multiKey);
            keys[e.keyCode] = true;

            this.setState({
                multiKey: keys
            },
            // () => {
            //         if(this.state.multiKey[16] && this.state.multiKey[13]){
            //             if(this.state.rows < 3){
            //                 this.setState(preState => ({
            //                     rows: ++preState.rows
            //                 }))
            //             }
            //         }
            //     }
            )
        }
       
        // if(e.keyCode === 8){
        //     var textarea = document.getElementById("inputComment");
        //     console.log(textarea.clientHeight, textarea.scrollHeight)
        //     if(textarea.clientHeight >= textarea.scrollHeight){
        //         if(content.trim() === ""){
                    
        //             if(this.state.rows > 1){
        //                 this.setState(preState => ({
        //                     rows: --preState.rows
        //                 }))
        //             }
        //         }
        //     }
        // }
       
    }

    onKeyUp = (e) => {
        const { multiKey } = this.state;
        e.persist()
        if(e.keyCode in multiKey){
            var keys = Object.assign({}, multiKey);
            keys[e.keyCode] = false;

            this.setState({
                multiKey: keys
            })
        }
    }

    scrollHeight = () => {
        var el = document.getElementById('scroll');
        el.scrollTop = el.clientHeight;
    }
    
    render() {
        return (
            <div className="ant-comment-input">
                <Row>
                    <Col sm='2'>
                        <div className="ant-comment-inner" style={{padding: 0}} >
                            <div className="ant-comment-avatar">
                                <span className="ant-avatar ant-avatar-circle ant-avatar-image">
                                    <img src="https://cdn4.iconfinder.com/data/icons/avatars-21/512/avatar-circle-human-male-3-512.png" alt="Han Solo" />
                                </span>
                            </div>
                        </div>
                    </Col>
                    <Col sm='8'>
                    
                        <span className="ant-form-item-children">
                            <Input id="inputComment" type="textarea" onKeyDown={(e) => this.onKeyDown(e)} onKeyUp={(e) => this.onKeyUp(e)} onChange={this.onChange} value={this.state.content} />
                            {/* <textarea id="inputComment" onKeyDown={(e) => this.onKeyDown(e)} onKeyUp={(e) => this.onKeyUp(e)} rows={rows} onChange={this.onChange} className="ant-input" value={this.state.content}></textarea> */}
                        </span>
                        
                    </Col>
                    <Col sm='2'>
                        <button type="submit" className="btn btn-success" disabled={this.state.content.trim() === '' ? true : false}
                        onClick={this.onSubmit} >Comment</button>
                    </Col>
                </Row>
            </div>
        );
    }
};

export default CommentInput;