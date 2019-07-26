import React from "react";
import { connect } from "react-redux";
import Form from "react-validation/build/form";
import lodash from "lodash";

import { getEmployeeList } from "../../../actions/employee.list.action";
import ModalInfo from "../../../components/modal/modal.info";
import ModalConfirm from "../../../components/modal/modal.confirm";
import "react-datepicker/dist/react-datepicker.css";
import employeeApi from "../../../api/api.employee";

import { Row, Col, Table, Button, FormGroup, } from "reactstrap";
import { pagination } from "../../../constant/app.constant";
import ValidationInput from "../../../components/common/validation.input";
import SelectInput from "../../../components/common/select.input";
import { toastSuccess, toastError } from "../../../helpers/toast.helper";
import Pagination from "../../../components/pagination/Pagination";
import moment from "moment"

class EmployeeListPage extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            isShowInfoModal: false,
            isShowDeleteModal: false,
            item: {},
            itemId: null,
            params: {
                skip: pagination.initialPage,
                take: pagination.defaultTake
            },
            sortName: "",
            isDesc: false,
            query: "",
            valueSelected: []
        };
        this.delayedCallback = lodash.debounce(this.search, 1000);
    }

    sortName = (sortName) => {
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
            () => this.getEmployeeList()
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
            () => this.getEmployeeList()
        );
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
                this.getEmployeeList();
            }
        );
    };

    onSearchChange = e => {
        e.persist();
        this.delayedCallback(e);
    };

    deleteEmployee = async () => {
        try {
            await employeeApi.deleteEmployee(this.state.itemId);
            if (this.props.employeeList.employeeList.sources.length === 1) {
                this.setState({
                    params: { ...this.state.params, skip: this.state.params.skip - 1 }
                })
            }
            this.getEmployeeList();
            this.toggleDeleteModal();

            toastSuccess("The employee has been deleted successfully");
        } catch (err) {
            toastError(err);
        }
    };

    toggleDeleteModal = () => {
        this.setState(prevState => ({
            isShowDeleteModal: !prevState.isShowDeleteModal
        }));
    };

    showConfirmDelete = itemId => {
        this.setState({ itemId }, () => this.toggleDeleteModal());
    };

    showUpdateModal = item => {
        let title = "Update Employee";
        item.birthday = moment(item.birthday).format("YYYY-MM-DD");
        this.toggleModalInfo(item, title);
    };

    onModelChange = el => {
        let inputName = el.target.name;
        let inputValue = el.target.value.trim();
        let item = Object.assign({}, this.state.item);
        item[inputName] = inputValue;
        this.setState({ item });
    };

    updateEmployee = async () => {
        try {
            await employeeApi.updateEmployee(this.state.item);
            this.getEmployeeList();
            this.toggleModalInfo();
            toastSuccess("Update successfully");
        } catch (err) {
        }
    };

    addEmployee = async () => {
        try {
            await employeeApi.addEmployee(this.state.item);
            this.getEmployeeList();
            this.toggleModalInfo();
            toastSuccess("The employee has been created successfully");
        } catch (err) {
            return null;
        }
    };

    saveEmployee = () => {
        let { item } = this.state;
        let { id } = item;
        for (let key in item) {
            if (item[key].trim() === "") {
                return null;
            }
        }
        if (!id) {
            this.addEmployee();
        } else {
            this.updateEmployee();
        }
    };

    onSubmit = e => {
        e.preventDefault();
        this.form.validateAll();
        this.saveEmployee();
    };

    getEmployeeList = () => {
        let params = Object.assign({}, this.state.params, {
            query: this.state.query,
            sortName: this.state.sortName,
            isDesc: this.state.isDesc
        });
        this.props.getEmployeeList(params);
    };

    showAddNew = () => {
        let title = "Add new employee";
        let employee = {
            name: "",
            birthday: "",
            gender: "1",
            levelEmp: "1",
            email: ""
        };
        this.toggleModalInfo(employee, title);
    };

    toggleModalInfo = (item, title) => {
        this.setState(prevState => ({
            isShowInfoModal: !prevState.isShowInfoModal,
            item: item || {},
            formTitle: title
        }));
    };

    componentDidMount() {
        this.getEmployeeList();
    }

    render() {

        const { isShowDeleteModal, item } = this.state;
        const { employeeList } = this.props.employeeList;
        const { sources, pageIndex, totalPages } = employeeList;
        const hasResult = sources && sources.length > 0;
        return (
            <div className="animated fadeIn">
                <ModalConfirm
                    clickOk={this.deleteEmployee}
                    isShowModal={isShowDeleteModal}
                    toggleModal={this.toggleDeleteModal}
                />
                <ModalInfo
                    size="lg"
                    isShowModal={this.state.isShowInfoModal}
                    title={this.state.formTitle}
                    hiddenFooter
                >
                    <Form
                        onSubmit={e => this.onSubmit(e)}
                        ref={c => { this.form = c; }}>
                        <FormGroup>
                            <ValidationInput
                                name="name"
                                title="Name"
                                type="text"
                                onChange={this.onModelChange}
                                value={item.name}
                                required={true} />
                        </FormGroup>

                        <ValidationInput
                            name="birthday"
                            title="Birthday"
                            max={moment().format("YYYY-MM-DD")}
                            type="date"
                            onChange={this.onModelChange}
                            value={item.birthday}
                            required={true} />

                        <Row>
                            <Col xs="6" sm="6" md="6" lg="6">
                                <FormGroup>
                                    <SelectInput
                                        name="levelEmp"
                                        title="Level"
                                        nameField="nameField"
                                        valueField="valueField"
                                        onChange={this.onModelChange}
                                        value={item.levelEmp}
                                        options={[
                                            { id: 1, name: "One" },
                                            { id: 2, name: "Two" },
                                            { id: 3, name: "Three" },
                                            { id: 4, name: "Four" },
                                            { id: 5, name: "Five" }
                                        ]} />
                                </FormGroup>
                            </Col>

                            <Col xs="6" sm="6" md="6" lg="6">
                                <FormGroup>
                                    <SelectInput
                                        name="gender"
                                        title="Gender"
                                        nameField="nameField"
                                        valueField="valueField"
                                        value={item.gender}
                                        onChange={this.onModelChange}
                                        options={[
                                            { id: 1, name: "Male" },
                                            { id: 2, name: "Female" },
                                            { id: 3, name: "None" }
                                        ]}
                                        required={true} />
                                </FormGroup>
                            </Col>
                        </Row>

                        <ValidationInput
                            name="email"
                            title="Email"
                            type="email"
                            onChange={this.onModelChange}
                            value={item.email}
                            required={true}/>
                            
                        <div className="text-center">
                            <Button className=" btn-primary" type="submit">
                                Confirm </Button>{" "}
                            <Button className="btn-danger" onClick={this.toggleModalInfo}>
                                Cancel </Button>
                        </div>
                    </Form>
                </ModalInfo>

                <div className="animated fadeIn">
                    <Row className="flex-container header-table" >
                        <Col xs="5" sm="5" md="5" lg="5">
                            <Button
                                onClick={this.showAddNew}
                                className="btn btn-success btn-sm" > Create </Button>
                        </Col>

                        <Col xs="5" sm="5" md="5" lg="5"  >
                            <input
                                onChange={this.onSearchChange}
                                className="form-control form-control-sm custom_search"
                                placeholder="Searching..."
                            />
                        </Col>
                    </Row>

                    <Table responsive bordered className="react-bs-table react-bs-table-bordered data-table">
                        <thead>
                            <tr className="table-header">
                                <th>
                                    Name{" "}
                                    <i onClick={() => this.sortName("Name")} className={`${this.state.isDesc && (this.state.sortName === "Name" || this.state.sortName === "") ? "fa fa-caret-up fa-lg" : "fa fa-caret-down fa-lg"} ${(this.state.sortName === "Name" || this.state.sortName === "") ? "activeSort" : "disactiveSort"} `} ></i>
                                </th>
                                <th>Birthday <i onClick={() => this.sortName("Birthday")} className={`${this.state.isDesc && this.state.sortName === "Birthday" ? "fa fa-caret-up fa-lg" : "fa fa-caret-down fa-lg "} ${this.state.sortName === "Birthday" ? "activeSort" : "disactiveSort"}`} ></i></th>
                                <th>Level <i onClick={() => this.sortName("levelEmp")} className={`${this.state.isDesc && this.state.sortName === "levelEmp" ? "fa fa-caret-up fa-lg" : "fa fa-caret-down fa-lg"} ${this.state.sortName === "levelEmp" ? "activeSort" : "disactiveSort"} `} ></i> </th>
                                <th>Gender <i onClick={() => this.sortName("Gender")} className={`${this.state.isDesc && this.state.sortName === "Gender" ? "fa fa-caret-up fa-lg" : "fa fa-caret-down fa-lg"} ${this.state.sortName === "Gender" ? "activeSort" : "disactiveSort"} `} ></i></th>
                                <th>
                                    Email{" "}
                                    <i onClick={() => this.sortName("Email")} className={`${this.state.isDesc && this.state.sortName === "Email" ? "fa fa-caret-up fa-lg" : "fa fa-caret-down fa-lg"} ${this.state.sortName === "Email" ? "activeSort" : "disactiveSort"} `}></i>
                                </th>
                                <th className="custom_action">Action</th>
                            </tr>
                        </thead>
                        <tbody>
                            {hasResult
                                ? sources.map(item => (
                                    <tr className="table-row" key={item.id}>
                                        <td>{item.name}</td>
                                        <td>
                                            {moment(item.birthday).format("LL")}
                                        </td>
                                        <td>{item.levelEmpText}</td>
                                        <td>{item.genderText}</td>
                                        <td>{item.email}</td>
                                        <td>
                                            <Button
                                                className="btn btn-primary fa fa-pencil"
                                                onClick={() => this.showUpdateModal(item)} />
                                            <Button
                                                className="btn btn-danger fa fa-trash"
                                                color="danger"
                                                onClick={() => this.showConfirmDelete(item.id)} />
                                        </td>
                                    </tr>
                                ))
                                : null}
                        </tbody>
                    </Table>

                    {hasResult && totalPages > 1 && (
                        <Pagination
                            initialPage={0}
                            totalPages={totalPages}
                            forcePage={pageIndex - 1}
                            pageRangeDisplayed={2}
                            onPageChange={this.handlePageClick}
                        />
                    )}
                </div >
            </div >
        );
    }
}

const mapStateToProps = state => {
    const { employeeList } = state;
    return {
        employeeList
    };
};

export default connect(
    mapStateToProps,
    { getEmployeeList }
)(EmployeeListPage);
