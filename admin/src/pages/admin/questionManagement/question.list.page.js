import React, { Component } from "react";
import { connect } from "react-redux";
import Form from "react-validation/build/form";
import ModalConfirm from "../../../components/modal/modal.confirm";
import Pagination from "../../../components/pagination/Pagination";
import ModalInfo from "../../../components/modal/modal.info";
import ValidationInput from "../../../components/common/validation.input";
import SelectInput from "../../../components/common/select.input";
import questionApi from "../../../api/api.question";
import Api from "../../../api/api";
import { toastSuccess, toastError } from "../../../helpers/toast.helper";
import { getQuestionList } from "../../../actions/question.list.action";
import lodash from "lodash";
import { Alert, Row, Col, Button, Table, FormGroup, Modal, ModalHeader, ModalBody } from "reactstrap";
import { pagination } from "../../../constant/app.constant";

class QuestionListPage extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isShowDeleteModal: false,
            isShowInfoModal: false,
            isShowStatusModal: false,
            isShowGeneratePDFModal: false,
            item: {},
            itemId: null,
            params: {
                skip: pagination.initialPage,
                take: pagination.defaultTake
            },
            query: "",
            isDesc: false,
            sortName: "status",
            activeSort: "status",
            alertVisible: false
        };
        this.delayedCallback = lodash.debounce(this.search, 1000);
        this.onDismissAlert = this.onDismissAlert.bind(this);
    }

    onModelChange = el => {
        let inputName = el.target.name;
        let inputValue = el.target.value.trim();
        let item = Object.assign({}, this.state.item);
        item[inputName] = inputValue;
        this.setState({ item });
    };

    onDismissAlert() {
        this.setState({ alertVisible: false });
    }

    toggleModalInfo = (item, title) => {
        this.setState(prevState => ({
            isShowInfoModal: !prevState.isShowInfoModal,
            item: item || {},
            formTitle: title
        }));
    };

    toggleStatusUpdateModal = (item) => {
        this.setState(prevState => ({
            isShowStatusModal: !prevState.isShowStatusModal,
            item: item || {},
        }));
    }

    toggleGeneratePDFModal = (item) => {
        this.setState(prevState => ({
            isShowGeneratePDFModal: !prevState.isShowGeneratePDFModal,
            item: item || {},
        }));
    }

    toggleDeleteModal = () => {
        this.setState(prevState => ({
            isShowDeleteModal: !prevState.isShowDeleteModal
        }));
    };

    showAddNew = () => {
        let title = "Create Question";
        let question = {
            question: "",
            answer: "",
            level: 1,
            type: 1
        };
        this.toggleModalInfo(question, title);
    };

    showUpdateModal = item => {
        var answerList = item.answers.map(elm => (
            {
                id: elm.id,
                name: elm.content
            }
        ))
        let question = {
            id: item.id,
            question: item.content,
            answerList: answerList,
            level: item.level,
            type: item.type,
            answerId: answerList[0] ? answerList[0].id : ""
        };
        let title = "Update Question";
        this.toggleModalInfo(question, title);
    };

    showConfirmDelete = itemId => {
        this.setState(
            {
                itemId: itemId
            },
            () => this.toggleDeleteModal()
        );
    };

    showUpdateStatusModal = (item) => {
        this.toggleStatusUpdateModal(item);
    };

    showGeneratePDF = () => {
        let item = {
            level: 1,
            types: 1,
            quantity: 10
        };
        this.toggleGeneratePDFModal(item);
    }

    getQuestionList = (isDesc = false, sortName = "status") => {
        let params = Object.assign({}, this.state.params, {
            query: this.state.query,
            isDesc: isDesc,
            sortName: sortName
        });
        this.props.getQuestionList(params);
    };

    addQuestion = async () => {
        try {
            await questionApi.addQuestion(this.state.item);
            this.getQuestionList();
            this.toggleModalInfo();
            toastSuccess("The question has been created successfully");
        } catch (err) {
            return null;
        }
    };

    updateQuestion = async () => {
        const question = Object.assign({}, this.state.item);
        try {
            await questionApi.updateQuestion(question);
            this.toggleModalInfo();
            this.getQuestionList();
            toastSuccess("The question has been updated successfully");
        } catch (err) {
            toastError(err.message);
        }
    };

    deleteQuestion = async () => {
        try {
            await questionApi.deleteQuestion(this.state.itemId);
            if (this.props.questionList.questionList.sources.length === 1) {
                this.setState({
                    params: { ...this.state.params, skip: this.state.params.skip - 1 }
                })
            }
            this.toggleDeleteModal();
            this.getQuestionList();
            toastSuccess("The question has been deleted successfully");
        } catch (err) {
            toastError(err);
        }
    };

    onUpdateStatus(e) {
        e.preventDefault();
        this.form.validateAll();
        this.updateStatus();
    }

    updateStatus = async () => {
        const { status, id, answers } = this.state.item;
        const question = { status, id };
        const approved = 3;

        if (status === approved && answers.length > 1) {
            let containApproveAnswer = false;

            answers.forEach(answer => {
                if (answer.status === approved) {
                    containApproveAnswer = true;
                }
            })

            if (!containApproveAnswer) {
                toastError("Please select right answer before appove");
                return
            }
        }

        try {
            await questionApi.updateStatus(question);
            this.toggleStatusUpdateModal();
            this.getQuestionList();
            toastSuccess("The status has been updated successfully");
        } catch (err) {
            toastError(err);
        }

    }

    onGeneratePDF(e) {
        e.preventDefault();
        this.form.validateAll();
        this.generatePDF();
    }

    generatePDF = async () => {
        const { level, types, quantity } = this.state.item;
        const query = { level, types, quantity };
        console.log(query)
        this.onDismissAlert()
        try {
            var data = await Api.generatePDF(query);
            this.setState({
                pdfLink: data,
                alertVisible: true
            })
            this.toggleGeneratePDFModal();
        } catch (err) {
            toastError(err);
        }
    }


    saveQuestion = () => {
        let { id } = this.state.item;
        if (id) {
            this.updateQuestion();
        } else {
            this.addQuestion();
        }
    };

    onSubmit(e) {
        e.preventDefault();
        this.form.validateAll();
        this.saveQuestion();
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
                this.getQuestionList();
            }
        );
    };

    onSearchChange = e => {
        e.persist();
        this.delayedCallback(e);
    };

    handleSort = (sortName) => {
        const { isDesc } = this.state;
        this.setState({
            isDesc: !isDesc,
            sortName: sortName,
            activeSort: sortName
        },
            () => this.getQuestionList(this.state.isDesc, this.state.sortName)
        );
    }

    componentDidMount() {
        this.getQuestionList();
    }

    render() {
        const { isShowDeleteModal, isShowInfoModal, item } = this.state;
        const { questionList } = this.props.questionList;
        const { sources, pageIndex, totalPages } = questionList;
        const hasResults = questionList.sources && questionList.sources.length > 0;
        return (
            <div className="animated fadeIn">
                <ModalConfirm
                    clickOk={this.deleteQuestion}
                    isShowModal={isShowDeleteModal}
                    toggleModal={this.toggleDeleteModal}
                />

                <Alert color="primary" isOpen={this.state.alertVisible} toggle={this.onDismissAlert} fade={false}>
                    PDF File Generate Success, You Can Check <a href={this.state.pdfLink} className="alert-link">Here</a> 
                </Alert>

                <Modal isOpen={this.state.isShowStatusModal} toggle={this.toggleStatusUpdateModal} >
                    <ModalHeader>
                        Update Status
                    </ModalHeader >
                    <ModalBody>
                        <Form
                            onSubmit={e => this.onUpdateStatus(e)}
                            ref={c => { this.form = c; }}
                        >
                            <FormGroup>
                                <SelectInput
                                    name="status"
                                    title="Status"
                                    nameField="nameField"
                                    valueField="valueField"
                                    value={item.status}
                                    onChange={this.onModelChange}
                                    options={[
                                        { id: 1, name: "Pending" },
                                        { id: 2, name: "Rejected" },
                                        { id: 3, name: "Approved" }
                                    ]}
                                    required={true} />
                                <div className="text-center">
                                    <Button className=" btn-primary" type="submit">
                                        Confirm </Button>{" "}
                                    <Button className="btn-danger" onClick={this.toggleStatusUpdateModal}>
                                        Cancel </Button>
                                </div>
                            </FormGroup>
                        </Form>
                    </ModalBody>
                </Modal>

                <Modal isOpen={this.state.isShowGeneratePDFModal} toggle={this.toggleGeneratePDFModal} >
                    <ModalHeader>
                        Generate PDF
                    </ModalHeader >
                    <ModalBody>
                        <Form
                            onSubmit={e => this.onGeneratePDF(e)}
                            ref={c => { this.form = c; }}
                        >
                            <FormGroup>
                                <SelectInput
                                    name="level"
                                    title="Level"
                                    nameField="nameField"
                                    valueField="valueField"
                                    value={item.level}
                                    onChange={this.onModelChange}
                                    options={[
                                        { id: 1, name: "Fresher" },
                                        { id: 2, name: "Junior" },
                                        { id: 3, name: "Senior" }
                                    ]}
                                    required={true} />
                                <SelectInput
                                    name="types"
                                    title="Type"
                                    nameField="nameField"
                                    valueField="valueField"
                                    value={item.types}
                                    onChange={this.onModelChange}
                                    options={[
                                        { id: 1, name: "BackEnd" },
                                        { id: 2, name: "FrontEnd" },
                                        { id: 3, name: "Design" },
                                        { id: 4, name: "Testing"}
                                    ]}
                                    required={true} />
                                <SelectInput
                                    name="quantity"
                                    title="Quantity"
                                    nameField="nameField"
                                    valueField="valueField"
                                    value={item.quantity}
                                    onChange={this.onModelChange}
                                    options={[
                                        { id: 10, name: "10" },
                                        { id: 20, name: "20" },
                                        { id: 30, name: "30" },
                                        { id: 40, name: "40" }
                                    ]}
                                    required={true} />
                                <div className="text-center">
                                    <Button className=" btn-primary" type="submit">
                                        Confirm </Button>{" "}
                                    <Button className="btn-danger" onClick={this.toggleGeneratePDFModal}>
                                        Cancel </Button>
                                </div>
                            </FormGroup>
                        </Form>
                    </ModalBody>
                </Modal>

                <ModalInfo
                    size="lg"
                    title={this.state.formTitle}
                    isShowModal={isShowInfoModal}
                    hiddenFooter
                >
                    <div className="modal-wrapper">
                        <div className="form-wrapper">
                            <Form
                                onSubmit={e => this.onSubmit(e)}
                                ref={c => { this.form = c; }}>
                                <FormGroup>
                                    <ValidationInput
                                        name="question"
                                        title="Question"
                                        type="text"
                                        onChange={this.onModelChange}
                                        value={item.question}
                                        required={true} />
                                    {this.state.item.id ? (
                                        <SelectInput
                                            name="answerId"
                                            title="Best Answer"
                                            nameField="nameField"
                                            valueField="valueField"
                                            onChange={this.onModelChange}
                                            value={item.answerId}
                                            options={item.answerList}
                                        />
                                    ) : (
                                            <ValidationInput
                                                name="answer"
                                                title="Answer"
                                                type="text"
                                                onChange={this.onModelChange}
                                                value={item.answer}
                                                required={true} />
                                        )}
                                </FormGroup>

                                <Row>
                                    <Col xs="6" sm="6" md="6" lg="6">
                                        <FormGroup>
                                            <SelectInput
                                                name="level"
                                                title="Level"
                                                nameField="nameField"
                                                valueField="valueField"
                                                onChange={this.onModelChange}
                                                value={item.level}
                                                options={[
                                                    { id: 1, name: "Fresher" },
                                                    { id: 2, name: "Junior" },
                                                    { id: 3, name: "Senior" },
                                                ]} />
                                        </FormGroup>
                                    </Col>

                                    <Col xs="6" sm="6" md="6" lg="6">
                                        <FormGroup>
                                            <SelectInput
                                                name="type"
                                                title="Type"
                                                nameField="nameField"
                                                valueField="valueField"
                                                value={item.type}
                                                onChange={this.onModelChange}
                                                options={[
                                                    { id: 1, name: "BackEnd" },
                                                    { id: 2, name: "FrontEnd" },
                                                    { id: 3, name: "Design" },
                                                    { id: 4, name: "Testing"}
                                                ]}
                                                required={true} />
                                        </FormGroup>
                                    </Col>
                                </Row>

                                <div className="text-center">
                                    <Button className=" btn-primary" type="submit">
                                        Confirm </Button>{" "}
                                    <Button className="btn-danger" onClick={this.toggleModalInfo}>
                                        Cancel </Button>
                                </div>
                            </Form>
                        </div>
                    </div>
                </ModalInfo>

                <div className="animated fadeIn">
                    <Row className="flex-container header-table">
                        <Col xs="5" sm="5" md="5" lg="5">
                            <Button
                                onClick={this.showAddNew}
                                className="btn btn-success btn-sm" > Create </Button>
                            <Button
                                onClick={this.showGeneratePDF}
                                className="btn btn-warning btn-sm" > GeneratePDF </Button>
                        </Col>

                        <Col xs="5" sm="5" md="5" lg="5">
                            <input
                                onChange={this.onSearchChange}
                                className="form-control form-control-sm custom_search"
                                placeholder="Searching..." />
                        </Col>
                    </Row>
                    <Table responsive bordered className="react-bs-table react-bs-table-bordered data-table">
                        <thead>
                            <tr className="table-header">
                                <th>STT</th>
                                <th className="sort-column" onClick={() => this.handleSort("question")}>Question
                                    <i className={`${this.state.activeSort === "question" ? "active-sort" : "disactive-sort"} 
                                        ${!this.state.isDesc ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}>
                                    </i>
                                </th>
                                <th >Answer</th>
                                <th className="sort-column" onClick={() => this.handleSort("level")}>Level
                                    <i className={`${this.state.activeSort === "level" ? "active-sort" : "disactive-sort"} 
                                        ${!this.state.isDesc ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}>
                                    </i>
                                </th>
                                <th className="sort-column" onClick={() => this.handleSort("type")}>Type
                                    <i className={`${this.state.activeSort === "type" ? "active-sort" : "disactive-sort"} 
                                        ${!this.state.isDesc ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}>
                                    </i>
                                </th>
                                <th className="custom_action">Action</th>
                                <th className="sort-column" onClick={() => this.handleSort("status")}>Status
                                    <i className={`${this.state.activeSort === "status" ? "active-sort" : "disactive-sort"} 
                                        ${!this.state.isDesc ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}>
                                    </i>
                                </th>
                            </tr>
                        </thead>

                        <tbody>
                            {hasResults &&
                                sources.map((item, i) => {
                                    return (
                                        <tr className="table-row" key={i}>
                                            <td> {questionList.pageIndex * questionList.pageSize - questionList.pageSize + ++i}</td>
                                            <td>{item.content}</td>
                                            <td> {item.answers.map((answer) => (
                                                <p>
                                                    {

                                                        answer.status === 2 ?
                                                            <i className="fa fa-times" style={{ color: "red" }} aria-hidden="true"></i> :

                                                            answer.status === 3 ?

                                                                <i className="fa fa-check" style={{ color: "green" }} aria-hidden="true"></i> :

                                                                <i className="fa fa-spinner" style={{ color: "orange" }} aria-hidden="true"></i>
                                                    }
                                                    &nbsp; {answer.content}
                                                </p>
                                            ))}
                                            </td>
                                            <td>{item.levelText}</td>
                                            <td>{item.typeText}</td>
                                            <td>
                                                <Button
                                                    className="btn btn-primary fa fa-pencil"
                                                    onClick={() => this.showUpdateModal(item)} />

                                                <Button
                                                    className="btn btn-danger fa fa-trash"
                                                    onClick={() => this.showConfirmDelete(item.id)} />
                                            </td>

                                            <td onClick={() => this.showUpdateStatusModal(item)}>{

                                                item.status === 2 ?
                                                    <Button className="status_button" color="secondary"> Reject </Button> :

                                                    item.status === 3 ?

                                                        <Button className="status_button" color="success"> Approve </Button> :

                                                        <Button className="status_button" color="info"> Pending </Button>}</td>
                                        </tr>
                                    );

                                })}
                        </tbody>
                    </Table>

                    {hasResults && totalPages > 1 &&
                        <Pagination
                            initialPage={0}
                            totalPages={totalPages}
                            forcePage={pageIndex - 1}
                            pageRangeDisplayed={2}
                            onPageChange={this.handlePageClick}
                        />
                    }

                </div>
            </div>
        );
    }
}


export default connect(
    state => ({
        questionList: state.questionList
    }),
    {
        getQuestionList
    }
)(QuestionListPage);
