import React, { Component } from 'react';
import { pagination } from "../../../constant/app.constant";
import { getDepartmentList } from "../../../actions/department.list.action";
import { connect } from "react-redux";
import { Row, Col, Button, Table } from "reactstrap";
import Form from "react-validation/build/form";
import ModalConfirm from "../../../components/modal/modal.confirm";
import Pagination from "../../../components/pagination/Pagination";
import ModalInfo from "../../../components/modal/modal.info";
import ValidationInput from "../../../components/common/validation.input";
import { toastSuccess, toastError } from "../../../helpers/toast.helper";
import lodash from "lodash";
import DepartmentApi from "../../../api/api.department";
class Department extends Component {

    constructor(props) {
        super(props);
        this.state = {
            isShowDeleteModal: false,
            isShowInfoModal: false,
            item: {},
            itemId: null,
            params: {
                skip: pagination.initialPage,
                take: pagination.defaultTake
            },
            query: ""
        };
        this.delayedCallback = lodash.debounce(this.search, 1000);
    }

    toggleDeleteModal = () => {
        this.setState(prevState => ({
            isShowDeleteModal: !prevState.isShowDeleteModal
        }));
    };

    toggleModalInfo = (item, title) => {
        this.setState(prevState => ({
            isShowInfoModal: !prevState.isShowInfoModal,
            item: item || {},
            formTitle: title
        }));
    };

    showConfirmDelete = itemId => {
        this.setState(
            {
                itemId: itemId
            },
            () => this.toggleDeleteModal()
        );
    };

    showAddNew = () => {
        let title = "Create Department";
        let category = {
            name: ""
        };
        this.toggleModalInfo(category, title);
    };

    showUpdateModal = item => {
        let title = "Update Department";
        this.toggleModalInfo(item, title);
    };

    onModelChange = el => {
        let inputName = el.target.name;
        let inputValue = el.target.value;
        let item = Object.assign({}, this.state.item);
        item[inputName] = inputValue;
        this.setState({ item });
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
                this.getDepartmentList();
            }
        );
    };

    onSearchChange = e => {
        e.persist();
        this.delayedCallback(e);
    };

    handlePageClick = e => {
        this.setState(
            {
                params: {
                    ...this.state.params,
                    skip: e.selected + 1
                }
            },
            () => this.getDepartmentList()
        );
    };

    getDepartmentList = () => {
        let params = Object.assign({}, this.state.params, {
            query: this.state.query
        });
        this.props.getDepartmentList(params);
    };

    addDepartment = async () => {
        const { name } = this.state.item;
        const department = { name };
        try {
            await DepartmentApi.addDepartment(department);
            this.toggleModalInfo();
            this.getDepartmentList();
            toastSuccess("The job category has been created successfully");
        } catch (err) {
            toastError(err);
        }
    };

    updateDepartment = async () => {
        const { id, name } = this.state.item;
        const department = { id, name };
        try {
            await DepartmentApi.updateDepartment(department);
            this.toggleModalInfo();
            this.getDepartmentList();
            toastSuccess("The job category has been updated successfully");
        } catch (err) {
            toastError(err);
        }
    };

    deleteDepartment = async () => {
        const { itemId, isDesc, sortName } = this.state;
        try {
            await DepartmentApi.deleteDepartment(itemId);
            if (this.props.departmentList.departmentList.sources.length === 1) {
                this.setState({
                    params: { ...this.state.params, skip: this.props.departmentList.departmentList.pageIndex - 1 }
                })
            }
            this.getDepartmentList(isDesc, sortName);
            this.toggleDeleteModal();
            toastSuccess("The job category has been deleted successfully");
        } catch (err) {
            toastError(err);
        }
    };

    saveDepartment = () => {
        let { id } = this.state.item;
        if (id) {
            this.updateDepartment();
        } else {
            this.addDepartment();
        }
    };

    onSubmit(e) {
        e.preventDefault();
        this.form.validateAll();
        this.saveDepartment();
    }

    componentDidMount() {
        this.getDepartmentList();
    }

    render() {
        const { isShowDeleteModal, isShowInfoModal, item } = this.state;
        const { departmentList } = this.props.departmentList;
        const { sources, pageIndex, totalPages } = departmentList;
        const hasResults = departmentList.sources && departmentList.sources.length > 0;
        return (
            <div className="animated fadeIn">
                <ModalConfirm
                    clickOk={this.deleteDepartment}
                    isShowModal={isShowDeleteModal}
                    toggleModal={this.toggleDeleteModal}
                />

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
                                    name="name"
                                    title="Name"
                                    type="text"
                                    required={true}
                                    value={item.name}
                                    onChange={this.onModelChange}
                                />
                                <div className="text-center">
                                    <Button className=" btn-primary" type="submit">
                                        Confirm
                                    </Button>{" "}
                                    <Button className="btn-danger" onClick={this.toggleModalInfo}>
                                        Cancel
                                    </Button>
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
                                <th>Department</th>
                                <th className="custom_action">Action</th>
                            </tr>
                        </thead>

                        <tbody>
                            {hasResults &&
                                sources.map((item, i) => {
                                    return (
                                        <tr className="table-row" key={i}>
                                            <td > {departmentList.pageIndex !== 0 ? departmentList.pageIndex * departmentList.pageSize - departmentList.pageSize + ++i : ++i}</td>
                                            <td>{item.name}</td>
                                            <td>
                                                <Button
                                                    className="btn btn-primary fa fa-pencil"
                                                    onClick={() => this.showUpdateModal(item)} />

                                                <Button
                                                    className="btn btn-danger fa fa-trash"
                                                    onClick={() => this.showConfirmDelete(item.id)} />
                                            </td>
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
        departmentList : state.departmentList
    }),
    {
        getDepartmentList
    }
)(Department);