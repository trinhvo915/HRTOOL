import React, { Component, Fragment } from 'react'
import { connect } from 'react-redux';
import { getEmailTemplateList } from '../../../actions/email.template.list.action';
import { pagination } from "../../../constant/app.constant";
import { Row, Col, Button, Table, Label, FormGroup, Input } from "reactstrap";
import Pagination from "../../../components/pagination/Pagination";
import lodash from 'lodash';
import CKEditor from 'ckeditor4-react';
import ValidationInput from "../../../components/common/validation.input";
import Form from 'react-validation/build/form';
import { toastSuccess, toastError } from "../../../helpers/toast.helper";
import ModalInfo from "../../../components/modal/modal.info";
import ApiEmailTemplate from '../../../api/api.email.template';
import { ReactMultiEmail, isEmail } from 'react-multi-email';
import 'react-multi-email/style.css';
class EmailTemplateListPage extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isShowInfoModal: false,
            formTitle: "",
            item: {},
            params: {
                skip: pagination.initialPage,
                take: pagination.defaultTake
            },
            sortName: "",
            isDesc: false,
            query: ""
        }
        this.delayedCallback = lodash.debounce(this.search, 1000);
    }

    getEmailTemplateList = () => {
        let params = Object.assign({}, this.state.params, {
            query: this.state.query,
            sortName: this.state.sortName,
            isDesc: this.state.isDesc
        });
        this.setState({
            item: {}
        });
        this.props.getEmailTemplateList(params);
    }

    search = e => {
        this.setState(
            {
                params: {
                    ...this.state.params,
                    skip: 1
                },
                query: e.target.value
            },
            () => {
                this.getEmailTemplateList();
            }
        );
    };

    onSearchChange = e => {
        e.persist();
        this.delayedCallback(e);
    };

    onModelChange = (e) => {
        let inputName = e.target.name;
        let inputValue = {};
        if (inputName === "templateAttachments") {
            inputValue = e.target.files
        }
        else {
            inputValue = e.target.value;
        }
        let item = { ...this.state.item };
        item[inputName] = inputValue;
        this.setState({
            item
        });
    }

    onEditerChange = e => {
        let item = { ...this.state.item };
        item.body = e.editor.getData();
        this.setState({
            item
        });
    }

    onEmailChange = (e, type) => {
        let item = { ...this.state.item };
        item[type] = e.join(" ");
        this.setState({
            item
        });
    }

    uploadFile = async (attachments) => {
        var result;
        try {
            if (!attachments) {
                result = [];
            }
            else {
                if (attachments.length > 1) {
                    result = await ApiEmailTemplate.uploadFiles("templateAttachment", attachments);
                } else {
                    result = await ApiEmailTemplate.uploadFile("templateAttachment", attachments[0]);
                }
                var length = attachments.length;
                var temp = [];
                for (let i = 0; i < length; i++) {
                    var fullName = attachments[i].name;
                    var dotIndex = fullName.lastIndexOf(".");
                    var extension = fullName.substring(dotIndex, fullName.length);
                    var fileName = fullName.substring(0, dotIndex);
                    temp.push({
                        link: length > 1 ? result[i] : result,
                        fileName,
                        extension
                    })
                }
                return temp;
            }
        }
        catch (err) {

        }
    }

    removeFile = () => {
        this.setState({
            item: { ...this.state.item, templateAttachments: [] }
        })
    }

    onSubmit(e) {
        e.preventDefault();
        this.form.validateAll();
        this.updateEmailTemplate();
    }

    updateEmailTemplate = async () => {
        const emailTemplate = Object.assign({}, this.state.item);
        const { templateAttachments } = this.state.item;
        if (emailTemplate.templateAttachments && emailTemplate.templateAttachments.length > 0 && !emailTemplate.templateAttachments[0].link) {
            emailTemplate.templateAttachments = await this.uploadFile(templateAttachments);
        }
        try {
            await ApiEmailTemplate.updateEmailTemplate(emailTemplate);
            this.toggleModalInfo();
            this.getEmailTemplateList();
            toastSuccess("The email template has been updated successfully");
        } catch (err) {
            toastError(err);
        }
    }

    toggleModalInfo = (item, title) => {
        if (this.state.isShowInfoModal === true) {
            item = {};
        }
        this.setState(prevState => ({
            isShowInfoModal: !prevState.isShowInfoModal,
            item: item || {},
            formTitle: title
        }));
    };

    showUpdateModal = item => {
        let title = "Update Email Template";
        this.toggleModalInfo(item, title);
    };

    sort = (sortName) => {
        let isCheck = (sortName === this.state.sortName) || (this.state.sortName === "");
        this.setState(
            {
                params: {
                    ...this.state.params,
                    skip: 1
                },
                sortName,
                isDesc: isCheck ? !this.state.isDesc : false
            },
            () => this.getEmailTemplateList()
        );
    };

    handlePageClick = e => {
        this.setState(
            {
                params: {
                    ...this.state.params,
                    skip: e.selected + 1
                }
            },
            () => this.getEmailTemplateList()
        );
    };

    componentDidMount = () => {
        this.getEmailTemplateList();
    }

    render() {
        const { item, isShowInfoModal } = this.state;
        const { emailTemplateList } = this.props.emailTemplateList;
        const { sources } = emailTemplateList;
        const hasResults = emailTemplateList.sources && emailTemplateList.sources.length > 0;
        const { pageIndex, totalPages } = emailTemplateList;
        return (
            <div>
                <ModalInfo
                    size="lg"
                    title={this.state.formTitle}
                    isShowModal={isShowInfoModal}
                    hiddenFooter>

                    <div className="modal-wrapper">
                        <div className="form-wrapper">
                            <Form onSubmit={e => this.onSubmit(e)} ref={c => { this.form = c; }}>
                                <Row>
                                    <Col>
                                        <ValidationInput
                                            name="name"
                                            title="Name"
                                            type="text"
                                            required={true}
                                            value={item.name}
                                            onChange={this.onModelChange} />
                                    </Col>
                                    <Col>
                                        <ValidationInput
                                            name="type"
                                            title="Type"
                                            type="text"
                                            required={true}
                                            value={item.type === 1 ? "Job" : item.type === 2 ? "Interview" : item.type === 3 ? "Calendar" : null}
                                            disabled={true} />
                                    </Col>
                                </Row>
                                <Row>
                                    <Col>
                                        <ValidationInput
                                            name="from"
                                            title="From"
                                            type="email"
                                            required={true}
                                            value={item.from}
                                            onChange={this.onModelChange} />
                                    </Col>
                                    <Col>
                                        <ValidationInput
                                            name="fromName"
                                            title="From Name"
                                            type="text"
                                            required={true}
                                            value={item.fromName}
                                            onChange={this.onModelChange} />
                                    </Col>
                                </Row>
                                <Row>
                                    <Col>
                                        <ValidationInput
                                            name="subject"
                                            title="Subject"
                                            type="text"
                                            required={true}
                                            value={item.subject}
                                            onChange={this.onModelChange} />
                                    </Col>
                                </Row>
                                <Row>
                                    <Col>
                                        <FormGroup>
                                            <Label className="label-input"><strong> Cc</strong></Label>
                                            <ReactMultiEmail
                                                emails={item.cc ? item.cc.split(" ") : []}
                                                onChange={(e) => this.onEmailChange(e, "cc")}
                                                validateEmail={email => {
                                                    return isEmail(email); // return boolean
                                                }}
                                                getLabel={(email, index, removeEmail) => {
                                                    return (
                                                        <div data-tag key={index}>
                                                            {email}
                                                            <span data-tag-handle onClick={() => removeEmail(index)}>
                                                                ×
                                                            </span>
                                                        </div>
                                                    );
                                                }}
                                            />
                                        </FormGroup>
                                    </Col>
                                </Row>
                                <Row>
                                    <Col>
                                        <FormGroup>
                                            <Label className="label-input"><strong> Bcc</strong></Label>
                                            <ReactMultiEmail
                                                emails={item.bcc ? item.bcc.split(" ") : []}
                                                onChange={(e) => this.onEmailChange(e, "bcc")}
                                                validateEmail={email => {
                                                    return isEmail(email); // return boolean
                                                }}
                                                getLabel={(email, index, removeEmail) => {
                                                    return (
                                                        <div data-tag key={index}>
                                                            {email}
                                                            <span data-tag-handle onClick={() => removeEmail(index)}>
                                                                ×
                                                            </span>
                                                        </div>
                                                    );
                                                }}
                                            />
                                        </FormGroup>
                                    </Col>
                                </Row>
                                <Row>
                                    <Col>
                                        <FormGroup>
                                            <Label for="examplePicture"><strong>Attachment: </strong></Label>
                                            {item.templateAttachments && item.templateAttachments.length > 0 && item.templateAttachments[0].link ?
                                                <Row>
                                                    <Col lg='10'>
                                                        {item.templateAttachments.map((val, idx) =>
                                                            <Input
                                                                key={idx}
                                                                name="templateAttachments"
                                                                title="Attachment"
                                                                defaultValue={val.fileName}
                                                                disabled={true}
                                                                type="text"
                                                            />
                                                        )}
                                                    </Col>

                                                    <Col lg='2'>
                                                        <i className="fa fa-times-circle fa-2x" onClick={this.removeFile} aria-hidden="true"></i>
                                                    </Col>
                                                </Row>

                                                : <Input
                                                    name="templateAttachments"
                                                    title="Attachment"
                                                    type="file"
                                                    multiple
                                                    onChange={this.onModelChange} />
                                            }
                                        </FormGroup>
                                    </Col>
                                </Row>
                                <FormGroup>
                                    <Label><strong>Content </strong><span className="text-danger">*</span></Label>
                                    <CKEditor
                                        data={item.body}
                                        onChange={this.onEditerChange}
                                        config={{
                                            "extraPlugins": 'divarea'
                                        }}
                                    />
                                </FormGroup>

                                <div className="text-center ">
                                    <Button className=" btn-primary" type="submit" > Confirm </Button>{" "}
                                    <Button className="btn-danger" onClick={this.toggleModalInfo}> Cancel </Button>
                                </div>

                            </Form>
                        </div>
                    </div>
                </ModalInfo>

                <div className="animated fadeIn">
                    <Row className="flex-container header-table">
                        <Col xs="5" sm="5" md="5" lg="5">
                        </Col>
                        <Col xs="5" sm="5" md="5" lg="5"  >
                            <input onChange={this.onSearchChange} className="form-control form-control-sm custom_search"
                                placeholder="Searching..." />
                        </Col>
                    </Row>

                    <Table responsive bordered className="react-bs-table react-bs-table-bordered data-table" style={{ "wordBreak": "break-word" }}>
                        <thead>
                            <tr className="table-header">
                                <th>STT</th>
                                <th><div className="d-flex"> Name <i onClick={() => this.sort("Name")} className={`${this.state.isDesc && (this.state.sortName === "Name" || this.state.sortName === "") ? "fa fa-caret-up fa-lg" : "fa fa-caret-down fa-lg"} ${(this.state.sortName === "Name" || this.state.sortName === "") ? "active-sort" : "disactive-sort"} `} ></i></div></th>
                                <th>Type</th>
                                <th>Subject</th>
                                <th>From</th>
                                <th><div className="d-flex">FromName <i onClick={() => this.sort("FromName")} className={`${this.state.isDesc && (this.state.sortName === "FromName") ? "fa fa-caret-up fa-lg" : "fa fa-caret-down fa-lg"} ${(this.state.sortName === "FromName") ? "active-sort" : "disactive-sort"} `} ></i></div></th>
                                <th>CC</th>
                                <th>BCC</th>
                                <th>Attachments</th>
                                <th className="custom_action">Action</th>
                            </tr>
                        </thead>
                        <tbody >
                            {hasResults && sources.map((item, i) => {
                                return (
                                    <tr className="table-row" key={i}>
                                        <td> {emailTemplateList.pageIndex !== 0 ? emailTemplateList.pageIndex * emailTemplateList.pageSize - emailTemplateList.pageSize + ++i : ++i}</td>
                                        <td> {item.name} </td>
                                        <td style={{ "whiteSpace": "nowrap" }}> {item.type === 1 ? "Job" : item.type === 2 ? "Interview" : item.type === 3 ? "Calendar" : null} </td>
                                        <td> {item.subject} </td>
                                        <td> {item.from} </td>
                                        <td> {item.fromName} </td>
                                        <td style={{ "textAlign": "left" }}> {item.cc.split(" ").map((item, i) => item).join(", ")} </td>
                                        <td style={{ "textAlign": "left" }}> {item.bcc.split(" ").map((item, i) => item).join(", ")} </td>
                                        <td style={{ "textAlign": "left" }}> {item.templateAttachments.map((item, i) => {
                                            return (
                                                <Fragment key={i}>
                                                    {i + 1}. <a href={item.link} target="_blank" rel="noopener noreferrer" download>{`${item.fileName}${item.extension}`}</a><br />
                                                </Fragment>
                                            )
                                        })} </td>
                                        <td>
                                            <div>
                                                <Button className="btn btn-primary fa fa-pencil"
                                                    onClick={() => this.showUpdateModal(item)} />
                                            </div>
                                        </td>
                                    </tr>
                                );
                            })}
                        </tbody>
                    </Table>

                    {hasResults && totalPages > 1 &&
                        <Pagination
                            totalPages={totalPages}
                            page={pageIndex}
                            initialPage={0}
                            forcePage={pageIndex - 1}
                            pageRangeDisplayed={2}
                            onPageChange={this.handlePageClick} />}
                </div>

            </div>
        )
    }
}

const mapStateToProps = (state) => ({
    emailTemplateList: state.emailTemplateList
});

const mapDispathToProps = {
    getEmailTemplateList
}
export default connect(mapStateToProps, mapDispathToProps)(EmailTemplateListPage);