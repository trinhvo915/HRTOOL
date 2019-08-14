import React, { Component } from "react";
import { connect } from "react-redux";
import MultipleSelect from "../../../components/common/multiple.select";
import {
  getUserManagementList,
  resetColor
} from "../../../actions/user.management.list.action";
import SelectInput from "../../../components/common/select.input"
import { getDepartmentList } from "../../../actions/department.list.action";
import { getRoleList } from "../../../actions/role.list.action";
import "react-color-picker/index.css";
import { SketchPicker } from "react-color";
import { pagination } from "../../../constant/app.constant";
import { Row, Col, Button, Label, Table, FormGroup } from "reactstrap";
import Pagination from "../../../components/pagination/Pagination";
import lodash from "lodash";
import Form from "react-validation/build/form";
import { toastSuccess, toastError } from "../../../helpers/toast.helper";
import ModalInfo from "../../../components/modal/modal.info";
import ApiUserManagement from "../../../api/api.user.management";
class UserManagementPage extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isShowInfoModal: false,
      formTitle: "",
      item: {},
      itemId: null,
      params: {
        skip: pagination.initialPage,
        take: pagination.defaultTake
      },
      isDesc: true,
      sortName: "Role",
      query: "",
      activeSort: "Role"
    };
    this.delayedCallback = lodash.debounce(this.search, 1000);
  }

  getUserManagementList = (isDesc = true, sortName = "Role") => {
    let params = Object.assign(
      {},
      {
        query: this.state.query,
        isDesc: isDesc,
        sortName: sortName
      }
    );

    this.props.getUserManagementList(params);
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
        this.getUserManagementList(this.state.isDesc, this.state.sortName);
      }
    );
  };

  getRoleList = () => {
    let params = Object.assign(
      {},
      {
        query: this.state.query
      }
    );
    this.props.getRoleList(params);
  };

  onSearchChange = e => {
    e.persist();
    this.delayedCallback(e);
  };

  onModelChange = el => {
    let inputName;
    let inputValue;
    let item = Object.assign({}, this.state.item);
    if (el.target != null) {
      inputName = el.target.name;
      inputValue = el.target.value;
      item[inputName] = inputValue;
    } else {
      inputValue = el;
      item["roleIds"] = inputValue.map((val, idx) => {
        return val.id;
      });
    }
    this.setState({
      item
    });
  };

  onModelChangeDepartment = el => {
    let inputName = el.target.name;
    let inputValue = el.target.value;
    let item = Object.assign({}, this.state.item);
    item[inputName] = inputValue;
    this.setState({ item }, () => console.log(this.state.item));
  };

  changeComplete = color => {
    let item = Object.assign({}, this.state.item);
    item.color = color.hex;
    this.setState({ item, color: color.hex });
  };

  resetColor = item => {
    const { sources } = this.props.userManagementList;
    const sourcesCp = [...sources];
    const userIndex = sources.findIndex(x => x.id === item.id);
    sourcesCp[userIndex].color = "#000000";

    this.props.resetColor(sourcesCp);
    this.resetColorAPI(item);
  };

  resetColorAPI = async item => {
    const { id, color, roles } = Object.assign({}, item);
    const user = {
      color,
      id,
      roleIds: roles.map(role => role.id)
    };
    const { isDesc, sortName } = this.state;

    try {
      await ApiUserManagement.updateUserManagementList(user);
      this.getUserManagementList(isDesc, sortName);
    } catch (err) {
      toastError(err.message);
    }
  };

  onSubmit(e) {
    e.preventDefault();
    this.form.validateAll();
    this.saveUserManagementList();
  }

  updateUserManagementList = async () => {
    const { id, color, roleIds, departmentId, department } = Object.assign({}, this.state.item);
    let IdDepartment = departmentId ? departmentId : (department ? department.id : null);
    const user = { color, id, roleIds, "departmentId": IdDepartment };
    const { isDesc, sortName } = this.state;
    try {
      await ApiUserManagement.updateUserManagementList(user);
      this.toggleModalInfo();
      this.getUserManagementList(isDesc, sortName);
      toastSuccess("User has been updated successfully");
    } catch (err) {
      toastError(err.message);
    }
  };

  saveUserManagementList() {
    let { id } = this.state.item;
    if (id) {
      this.updateUserManagementList();
    }
  }

  toggleModalInfo = (item, title) => {
    this.setState(prevState => ({
      isShowInfoModal: !prevState.isShowInfoModal,
      item: item || {},
      formTitle: title
    }));
  };

  showUpdateModal = val => {
    let title = "Update Role And Color For User";
    var item = Object.assign({}, val);
    item.roleIds = item.roles.map((val, idx) => {
      return val.id;
    });
    this.toggleModalInfo(item, title);
  };

  toggleSort = sortName => {
    const { isDesc } = this.state;
    this.setState(
      {
        isDesc: !isDesc,
        sortName: sortName,
        activeSort: sortName
      },
      () => this.getUserManagementList(this.state.isDesc, this.state.sortName)
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
      () => this.getUserManagementList()
    );
  };

  componentDidMount = () => {
    this.getUserManagementList();
    this.getRoleList();
    this.props.getDepartmentList();
  };

  render() {
    const titlecolor = "Color";
    const { item, isShowInfoModal } = this.state;
    const {
      sources,
      pageIndex,
      totalPages,
      pageSize
    } = this.props.userManagementList;
    const hasResults = sources && sources.length > 0;
    const roleList = Object.assign([], this.props.roleList.roleList.sources);
    const { departmentList } = this.props.departmentList;
    return (
      <div>
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
                <Row>
                  <Col xs="3">
                    {/* Candidate */}
                    <FormGroup>
                      <SelectInput
                        options={departmentList.sources || []}
                        title="Candidate: "
                        name="departmentId"
                        required={true}
                        placeholder="Choose Department"
                        value={item.departmentId ? item.departmentId : (item.department ? item.department.id : "")}
                        onChange={this.onModelChangeDepartment} >
                      </SelectInput>
                    </FormGroup>
                  </Col>
                  <Col xs="3">
                    <MultipleSelect
                      name="roleIds"
                      title="Role"
                      labelField="name"
                      valueField="id"
                      options={roleList}
                      required={true}
                      onChange={this.onModelChange}
                      defaultValue={item.roles ? item.roles : []}
                    />
                  </Col>
                  <Col xs="6">
                    <div>
                      {" "}
                      <label>
                        <strong>{titlecolor}</strong>
                      </label>
                    </div>
                    <SketchPicker
                      style={{ width: 300 }}
                      color={this.state.item.color}
                      onChangeComplete={this.changeComplete}
                    />
                  </Col>
                </Row>

                <div className="text-center " style={{ marginTop: 20 }}>
                  <Button className=" btn-primary" type="submit">
                    {" "}
                    Confirm{" "}
                  </Button>{" "}
                  <Button className="btn-danger" onClick={this.toggleModalInfo}>
                    {" "}
                    Cancel{" "}
                  </Button>
                </div>
              </Form>
            </div>
          </div>
        </ModalInfo>

        <div className="animated fadeIn">
          <Row className="flex-container header-table">
            <Col xs="5" sm="5" md="5" lg="5" />
            <Col xs="5" sm="5" md="5" lg="5">
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
                  onClick={() => this.toggleSort("Name")}
                >
                  Name{" "}
                  <i
                    className={`${
                      this.state.activeSort === "Name"
                        ? "active-sort"
                        : "disactive-sort"
                      } ${
                      !this.state.isDesc
                        ? "fa fa-caret-down fa-lg"
                        : "fa fa-caret-up fa-lg"
                      }`}
                  />
                </th>
                <th>Email</th>
                <th
                  className="group-sort"
                  onClick={() => this.toggleSort("Role")}
                >
                  Role
                  <i
                    className={`${
                      this.state.activeSort === "Role"
                        ? "active-sort"
                        : "disactive-sort"
                      } ${
                      !this.state.isDesc
                        ? "fa fa-caret-down fa-lg"
                        : "fa fa-caret-up fa-lg"
                      }`}
                  />
                </th>
                <th>Department</th>
                <th>Color</th>
                <th className="custom_action">Action</th>
              </tr>
            </thead>
            <tbody>
              {hasResults &&
                sources.map((itemData, i) => {
                  return (
                    <tr className="table-row" key={i}>
                      <td>
                        {" "}
                        {pageIndex !== 0
                          ? pageIndex * pageSize - pageSize + ++i
                          : ++i}
                      </td>
                      <td> {itemData.name} </td>
                      <td> {itemData.email} </td>
                      <td>
                        {itemData.roles
                          .map((val, idx) => {
                            return val.name;
                          })
                          .join(", ")}
                      </td>
                      <td>{itemData.department != null ? itemData.department.name : ""}</td>
                      <td>
                        <div>
                          <i
                            className="fa fa-square"
                            style={{
                              defaultValue: "grey",
                              color: itemData.color,
                              fontSize: 40
                            }}
                          />
                          {}
                        </div>
                      </td>
                      <td>
                        {console.log(itemData)}
                        <div className="custom_action_column">
                          <Button
                            className="btn btn-primary fa fa-pencil"
                            onClick={() => this.showUpdateModal(itemData)}
                            style={{ fontSize: 20 }}
                          />
                          <Button
                            className="btn-success"
                            onClick={() => this.resetColor(itemData)}
                          >
                            Reset Color
                          </Button>
                        </div>
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
export default connect(
  state => ({
    roleList: state.roleList,
    userManagementList: state.userManagementList,
    departmentList: state.departmentList
  }),
  {
    getUserManagementList,
    getRoleList,
    resetColor,
    getDepartmentList
  }
)(UserManagementPage);
