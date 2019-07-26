import React, { Component } from "react";
import { connect } from "react-redux";
import { Row, Col, Button, Table } from "reactstrap";
import Form from "react-validation/build/form";
import ModalConfirm from "../../../components/modal/modal.confirm";
import Pagination from "../../../components/pagination/Pagination";
import ModalInfo from "../../../components/modal/modal.info";
import ValidationInput from "../../../components/common/validation.input";
import { toastSuccess, toastError } from "../../../helpers/toast.helper";
import lodash from "lodash";
import { getCategoryList } from "../../../actions/category.list.action";
import Api from "../../../api/api";
import { pagination } from "../../../constant/app.constant";

class CategoryListPage extends Component {
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
        let title = "Create Job Category";
        let category = {
            name: ""
        };
        this.toggleModalInfo(category, title);
    };

    showUpdateModal = item => {
        let title = "Update Job Category";
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
                this.getCategoryList();
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
            () => this.getCategoryList()
        );
    };

    getCategoryList = () => {
        let params = Object.assign({}, this.state.params, {
            query: this.state.query
        });
        this.props.getCategoryList(params);
    };

    addCategory = async () => {
        const { name } = this.state.item;
        const category = { name };
        try {
            await Api.addCategory(category);
            this.toggleModalInfo();
            this.getCategoryList();
            toastSuccess("The job category has been created successfully");
        } catch (err) {
            toastError(err);
        }
    };

    updateCategory = async () => {
        const { id, name } = this.state.item;
        const category = { id, name };
        try {
            await Api.updateCategory(category);
            this.toggleModalInfo();
            this.getCategoryList();
            toastSuccess("The job category has been updated successfully");
        } catch (err) {
            toastError(err);
        }
    };

    deleteCategory = async () => {
        try {
            await Api.deleteCategory(this.state.itemId);
            this.toggleDeleteModal();
            this.getCategoryList();
            toastSuccess("The job category has been deleted successfully");
        } catch (err) {
            toastError(err);
        }
    };

    saveCategory = () => {
        let { id } = this.state.item;
        if (id) {
            this.updateCategory();
        } else {
            this.addCategory();
        }
    };

    onSubmit(e) {
        e.preventDefault();
        this.form.validateAll();
        this.saveCategory();
    }

    componentDidMount() {
        this.getCategoryList();
    }

    render() {
        const { isShowDeleteModal, isShowInfoModal, item } = this.state;
        const { categoryList } = this.props.categoryList;
        const { sources, pageIndex, totalPages } = categoryList;
        const hasResults = categoryList.sources && categoryList.sources.length > 0;
        return (
            <div className="animated fadeIn">
                <ModalConfirm
                    clickOk={this.deleteCategory}
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
                                <th>Name</th>
                                <th className="custom_action">Action</th>
                            </tr>
                        </thead>

                        <tbody>
                            {hasResults &&
                                sources.map((item,i) => {
                                    return (
                                        <tr className="table-row" key={i}>
                                            <td > {categoryList.pageIndex !== 0 ? categoryList.pageIndex * categoryList.pageSize - categoryList.pageSize + ++i : ++i }</td>
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
        categoryList: state.categoryList
    }),
    {
        getCategoryList
    }
)(CategoryListPage);
