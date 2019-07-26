import React, { Component } from "react";
import { connect } from "react-redux";
// import { Row, Col, Button, Table } from "reactstrap";
import Form from "react-validation/build/form";
import lodash from "lodash";
import Api from "../../../api/api.questionanswer";
import "../../../api/api.questionanswer";
import { toastSuccess, toastError } from "../../../helpers/toast.helper";
import { getQuestionAnswerList } from "../../../actions/questionanswer.list.action";
import Pagination from "../../../components/pagination/Pagination";
import ModalInfo from "../../../components/modal/modal.info";
import ValidationInput from "../../../components/common/validation.input";
import { pagination } from "../../../constant/app.constant";

import {
  Alert,
  Row,
  Col,
  Button,
  Table,
  FormGroup,
  Label,
  Input,
  Modal,
  ModalHeader,
  ModalBody,
  ModalFooter
} from "reactstrap";
import SelectInput from "../../../components/common/select.input"
import "./questionAnswer.page.css";

class QuestionAnswerPage extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isShowInfoModal: false,
      isShowAddNewAnswer: false,
      item: {},
      itemId: null,
      params: {
        skip: pagination.initialPage,
        take: pagination.defaultTake
      },
      query: "",
      isDesc: true,
      sortName: "true",
      activeSort: ""
    };
    this.delayedCallback = lodash.debounce(this.search, 1000);
  }

  toggleModalInfo = (item, title) => {
    this.setState(prevState => ({
      isShowInfoModal: !prevState.isShowInfoModal,
      item: item || {},
      formTitle: title
    }));
    this.getQuestionAnswerList();
  };

  toggleAddNewAnswerModal = (item, title) => {
    this.setState(prevState => ({
      isShowAddNewAnswer: !prevState.isShowAddNewAnswer,
      item: item || {},
      formTitle: title
    }));
  };

  showAddNew = () => {
    let title = "Create new question and answer";
    let questionanswer = {
      question: "",
      answer: "",
      type: 1,
      level: 1
    };
    this.toggleModalInfo(questionanswer, title);
  };

  showAddNewAnswer = item => {
    let title = "Add new answer";
    var answer = {
      content: "",
      questionId: item.id
    };
    this.toggleAddNewAnswerModal(answer, title);
  };

  onModelChange = el => {
    let inputName;
    let inputValue;
    let item = Object.assign({}, this.state.item);
    if (el.target != null) {
      inputName = el.target.name;
      inputValue = el.target.value;
      item[inputName] = inputValue;
    }

    this.setState({
      item
    });
  };

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
        this.getQuestionAnswerList(this.state.isDesc, this.state.sortName);
      }
    );
  };

  getQuestionAnswerList = (isDesc = true, sortName = "true") => {
    let params = Object.assign({}, this.state.params, {
      query: this.state.query,
      isDesc: isDesc,
      sortName: sortName
    });
    this.props.getQuestionAnswerList(params);
  };

  onSearchChange = e => {
    e.persist();
    this.delayedCallback(e);
  };

  addNewAnswer = async () => {
    const answer = Object.assign({}, this.state.item);
    const { isDesc, sortName } = this.state;
    try {
      await Api.addNewAnswer(answer);
      this.toggleAddNewAnswerModal();
      this.getQuestionAnswerList(isDesc, sortName);
      toastSuccess(" Answer have been created successfully");
    } catch (err) {
      toastError(err.message);
    }
  };

  async addQuestionAnswerList() {
    const question = Object.assign({}, this.state.item);
    const { isDesc, sortName } = this.state;
    try {
      await Api.addQuestionAnswerList(question);
      this.toggleModalInfo();
      this.getQuestionAnswerList(isDesc, sortName);
      toastSuccess("Question and Answer have been created successfully");
    } catch (err) {
      toastError(err.message);
    }
  }

  saveQuestionAnswer() {
    let { id } = this.state.item;
    if (!id) this.addQuestionAnswerList();
  }

  onSubmit(e) {
    e.preventDefault();
    this.form.validateAll();
    this.saveQuestionAnswer();
  }

  handlePageClick = e => {
    const { params, isDesc, sortName } = this.state;
    params.take = 10;
    params.skip = e.selected + 1;
    this.setState(
      {
        params
      },
      () => this.getQuestionAnswerList(isDesc, sortName)
    );
  };

  toggleSort = sortName => {
    const { isDesc } = this.state;
    this.setState(
      {
        isDesc: !isDesc,
        sortName: sortName,
        activeSort: sortName
      },
      () => this.getQuestionAnswerList(this.state.isDesc, this.state.sortName)
    );
  };

  componentDidMount() {
    this.getQuestionAnswerList();
  }

  render() {
    const { isShowInfoModal, item } = this.state;
    const { questionAnswerList } = this.props.questionAnswerList;
    const { sources, pageIndex, totalPages } = questionAnswerList;
    const hasResults =
      questionAnswerList.sources && questionAnswerList.sources.length > 0;
    return (
      <div className="animated fadeIn">
        {" "}
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
                ref={c => {
                  this.form = c;
                }}
              >
                <ValidationInput
                  name="question"
                  title="Question Content"
                  required={true}
                  value={item.question}
                  onChange={this.onModelChange}
                />
                <ValidationInput
                  name="answer"
                  title="Answer Content"
                  required={true}
                  value={item.answer}
                  onChange={this.onModelChange}
                />

                <Row>
                  <Col xs="6">
                    <SelectInput
                      name="level"
                      title="Level"
                      nameField="nameField"
                      valueField="valueField"
                      onChange={this.onModelChange}
                      value={item.level}
                      options={[
                        {
                          name: "Fresher",
                          id: 1
                        },
                        {
                          name: "Junior",
                          id: 2
                        },
                        {
                          name: "Senior",
                          id: 3
                        }
                      ]}
                    />
                  </Col>

                  <Col xs="6">
                    <SelectInput
                      name="type"
                      title="Type"
                      nameField="nameField"
                      valueField="valueField"
                      onChange={this.onModelChange}
                      value={item.type}
                      options={[
                        {
                          name: "BackEnd",
                          id: 1
                        },
                        {
                          name: "FrondEnd",
                          id: 2
                        },
                        {
                          name: "Design",
                          id: 3
                        }
                      ]}
                    />
                  </Col>
                </Row>

                <div className="text-center">
                  <Button className="btn-info" type="submit">
                    Confirm
                  </Button>
                  {"  "}
                  <Button className="btn-danger" onClick={this.toggleModalInfo}>
                    Cancel
                  </Button>
                </div>
              </Form>
            </div>
          </div>
        </ModalInfo>
        <Modal
          isOpen={this.state.isShowAddNewAnswer}
          toggle={this.toggleAddNewAnswerModal}
        >
          <ModalHeader>Add content answer</ModalHeader>
          <ModalBody>
            <Form
              onSubmit={e => this.onSubmit(e)}
              ref={c => {
                this.form = c;
              }}
            >
              <ValidationInput
                name="content"
                title="Answer Content"
                required={true}
                value={item.content}
                onChange={this.onModelChange}
              />
            </Form>
          </ModalBody>
          <ModalFooter>
            <Button
              className="btn-info"
              type="submit"
              onClick={this.addNewAnswer}
            >
              Confirm
            </Button>
            {"  "}
            <Button
              className="btn-danger"
              onClick={this.toggleAddNewAnswerModal}
            >
              Cancel
            </Button>
          </ModalFooter>
        </Modal>
        <div className="animated fadeIn">
          <Row className="flex-container header-table">
            <Col xs="3">
              <Button
                onClick={this.showAddNew}
                className="btn btn-success btn-sm"
              >
                {" "}
                Create{" "}
              </Button>
            </Col>

            <Col xs="6">
              <input
                onChange={this.onSearchChange}
                className="form-control form-control-sm custom_search"
                placeholder="Searching..."
              />
            </Col>
          </Row>
          <Table
            responsive
            bordered
            className="react-bs-table react-bs-table-bordered data-table"
          >
            <thead>
              <tr className="table-header">
                <th>STT</th>
                <th
                  className="group-sort"
                  onClick={() => this.toggleSort("question")}
                >
                  Question Content
                  <i
                    className={`${
                      this.state.activeSort === "Question"
                        ? "active-sort"
                        : "disactive-sort"
                      } ${
                      !this.state.isDesc
                        ? "fa fa-caret-down fa-lg"
                        : "fa fa-caret-up fa-lg"
                      }`}
                  />
                </th>
                <th>Answer Content </th>
                <th
                  className="group-sort"
                  onClick={() => this.toggleSort("level")}
                >
                  Level
                  <i
                    className={`${
                      this.state.activeSort === "Level"
                        ? "active-sort"
                        : "disactive-sort"
                      } ${
                      !this.state.isDesc
                        ? "fa fa-caret-down fa-lg"
                        : "fa fa-caret-up fa-lg"
                      }`}
                  />
                </th>
                <th
                  className="group-sort"
                  onClick={() => this.toggleSort("type")}
                >
                  Type
                  <i
                    className={`${
                      this.state.activeSort === "Type"
                        ? "active-sort"
                        : "disactive-sort"
                      } ${
                      !this.state.isDesc
                        ? "fa fa-caret-down fa-lg"
                        : "fa fa-caret-up fa-lg"
                      }`}
                  />
                </th>
                <th
                  className="group-sort"
                  onClick={() => this.toggleSort("status")}
                >
                  Status
                  <i
                    className={`${
                      this.state.activeSort === "Status"
                        ? "active-sort"
                        : "disactive-sort"
                      } ${
                      !this.state.isDesc
                        ? "fa fa-caret-down fa-lg"
                        : "fa fa-caret-up fa-lg"
                      }`}
                  />
                </th>
                <th>Action</th>
              </tr>
            </thead>
            <tbody>
              {hasResults &&
                sources.filter(question => question.status == 1).map((itemData, i) => {
                  return (
                    <tr className="table-row" key={i}>
                      <td>
                        {" "}
                        {questionAnswerList.pageIndex *
                          questionAnswerList.pageSize -
                          questionAnswerList.pageSize +
                          ++i}
                      </td>
                      <td>{itemData.content}</td>
                      <td> {itemData.answers.map((answer) => (
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
                      <td>{itemData.levelText}</td>
                      <td>{itemData.typeText}</td>
                      <td>{itemData.statusText}</td>
                      <td>
                        <Button
                          className="btn btn-primary fa fa-plus"
                          onClick={() => this.showAddNewAnswer(itemData)}
                        />
                      </td>
                    </tr>
                  );
                })}
            </tbody>
          </Table>
          {hasResults && totalPages > 1 && (
            <Pagination
              totalPages={totalPages}
              page={pageIndex}
              initialPage={0}
              forcePage={pageIndex - 1}
              pageRangeDisplayed={2}
              onPageChange={this.handlePageClick}
            />
          )}
        </div>
      </div>
    );
  }
}

const mapStateToProps = state => {
  return {
    questionAnswerList: state.questionAnswerList
  };
};

export default connect(
  mapStateToProps,
  { getQuestionAnswerList }
)(QuestionAnswerPage);
