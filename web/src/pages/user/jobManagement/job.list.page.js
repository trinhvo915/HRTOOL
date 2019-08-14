import React, { Component } from "react";
import { connect } from "react-redux";
import { TabContent, TabPane, Nav, NavItem, NavLink, Button, Row, Col, Table, Input, FormGroup, Label } from "reactstrap";
import Form from "react-validation/build/form";
import Pagination from "../../../components/pagination/Pagination";
import ModalInfo from "../../../components/modal/modal.info";
import lodash from "lodash";
import { getJobList } from "../../../actions/job.list.action";
import { getUserList } from "../../../actions/user.list.action";
import { pagination } from "../../../constant/app.constant";
import Detail from "../../../components/common/detail";
import CommentItem from "../../../components/common/comment.item";
import CommentInput from "../../../components/common/comment.input"
import ListUser from "../../../components/common/list.user";
import CommentApi from "../../../api/api.comment";
import StepApi from "../../../api/api.step";
import { toastSuccess, toastError } from "../../../helpers/toast.helper";
import moment from 'moment';
import classnames from 'classnames';
import "./job.list.page.css";
import JobApi from './../../../api/api.job';
import ModalConfirm from "../../../components/modal/modal.confirm";

import MultipleSelect from "../../../components/common/multiple.select";
import SelectInput from "../../../components/common/select.input";
import DatetimeSelect from "../../../components/common/datetime.select";
import ValidationInput from "../../../components/common/validation.input";
import Datetime from "react-datetime";

import RequestHelper from "../../../helpers/request.helper";
import { appConfig } from "../../../config/app.config";
import Step from "../../../components/common/step.detail";

import CookieHelper from "../../../helpers/cookie.helper";
import StarRatingComponent from 'react-star-rating-component';

class UserInJobListPage extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isOpen: false,
            isShowDeleteModal: false,
            isShowInfoModal: false,
            isShowAddNewModal: false,
            isShowDelCommentModal: false,
            isShowDelStepModal: false,
            isShowPlusButtom: true,
            userIdLogin: null,
            commentItem: {},
            commentItemId: null,
            stepItem: {},
            stepItemId: null,
            item: {},
            itemId: null,
            params: {
                skip: pagination.initialPage,
                take: pagination.defaultTake
            },
            query: "",
            activeTab: '1',
            sortName: "",
            IsDesc: true,
            IsDescName: true,
            IsDescStatus: true,
            IsDescPriority: true,
            IsDescDateStart: true,
            IsDescDateEnd: true,
            activeSort: '',
            field: '',
            categoryList: [],
            steps: [],
            itemAdded: {},
        };
        this.delayedCallback = lodash.debounce(this.search, 1000);
    }

    toggleModalInfo = (item, title) => {
        this.setState(prevState => ({
            isShowInfoModal: !prevState.isShowInfoModal,
            item: item || {},
            formTitle: title,
        }));
        if (item.id) {
            this.getJobById(item.id);
        }
        this.getJobList();
        this.getAllCategoryList();
    };

    toggleModalAddNew = (item, title) => {
        this.setState(prevState => ({
            isShowAddNewModal: !prevState.isShowAddNewModal,
            item: item || {},
            formTitle: title,
            steps: []
        }));
        //this.getJobList();
        if (item && item.id) {
            this.getJobById(item.id);
        }
        this.props.getUserList();
        this.getAllCategoryList();
    };

    toggleModalComment = (comment, title, load = true) => {
        if (load) {
            this.setState(prevState => ({
                isShowInfoModal: !prevState.isShowInfoModal,
                comment: comment,
                formTitle: title
            }))
        }
    }

    showDetailModal = async (item) => {
        // real-time-notification
        this.props.hubConnectionCalendar.on('Receive', (nameSender, idUsers, secalendarDescription, type) => {
            if (type === appConfig.comment_add || type === appConfig.comment_update) {
                this.getJobById(item.id);
            }
        });
        // end-real-time-notification

        let title = "Job Detail";
        this.toggleModalInfo(item, title);
        this.setState({
            commentItem: {
                ...this.state.commentItem,
                jobId: item.id
            },
        })
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
        else {
            inputValue = el;
            item["userIds"] = inputValue.map((val, idx) => {
                return val.id
            })
        }
        this.setState({
            item
        })
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
                this.getJobList();
            }
        );
    };

    handleChangeStart = (date) => {
        let item = Object.assign({}, this.state.item);
        item["dateStart"] = date._d;
        this.setState({
            item
        });
    }

    handleChangeEnd = (date) => {
        let item = Object.assign({}, this.state.item);
        item["dateEnd"] = date._d;
        this.setState({
            item
        });
    }

    onSearchChange = e => {
        e.persist();
        this.delayedCallback(e);
    };

    uploadFile = async (attachments) => {
        var result;
        try {
            if (!attachments) {
                result = [];
            }
            else {
                if (attachments.length > 1) {
                    result = await JobApi.uploadFiles("attachment", attachments);

                } else {
                    result = await JobApi.uploadFile("attachment", attachments[0]);
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
                        extension: extension,
                        fileName: fileName
                    })
                }
                return temp;
            }
        }
        catch (err) {
            toastError(err.message);
        }
    }

    async addJob() {
        let { name, dateStart, categoryIds, userIds, steps } = this.state.item;
        if (!dateStart || !name || !categoryIds || !userIds || steps ? steps.some(x => x.name === '') : null) {
            toastError("Please input all field");
            return;
        }
        else {
            const job = Object.assign({}, this.state.item);
            const { isDesc, sortName } = this.state;
            const { attachments } = job;
            job.attachments = await this.uploadFile(attachments);
            try {
                await JobApi.addJob(job)
                this.toggleModalAddNew();
                this.getJobList(isDesc, sortName);
                toastSuccess("The job has been created successfully");
            } catch (err) {
                toastError(err.message);
            }
        }
    }

    async updateJob() {
        const job = Object.assign({}, this.state.item);
        job.reporterId = job.reporter.id;
        job.CategoryIds = job.categories.map(val => val.id);
        if (!job.userIds) {
            job.userIds = job.users.map(val => val.id)
        }

        const { attachments } = job;
        if (job.attachments && job.attachments.length > 0 && !job.attachments[0].link) {
            job.attachments = await this.uploadFile(attachments);
        }
        try {
            await JobApi.updateJob(job);
            this.toggleModalAddNew(job);
            this.getJobById(job.id);
            toastSuccess("The job has been updated successfully");
        } catch (err) {
            toastError(err.message);
        }
    }

    saveJob() {
        let { id } = this.state.item;
        if (id) {
            this.updateJob()
        } else {
            this.addJob();
        }
    }

    onChangeCategories = (e) => {
        const item = Object.assign({}, this.state.item);
        item["categoryIds"] = e.map((val, idx) => {
            return val.id;
        });
        this.setState({
            item
        })
    }

    showUpdateModal(val) {
        var item = Object.assign({}, val);
        let title = "Update Job";
        item.reporterId = item.reporter.id;
        item.userIds = item.users.map((val, idx) => {
            return val.id;
        })
        item.categoryIds = item.categories.map((val, idx) => {
            return val.id;
        })
        this.toggleModalAddNew(item, title)
    }

    showConfirmDelete(itemId) {
        this.setState(
            {
                itemId: itemId
            },
            () => this.toggleDeleteModal()
        );
    }

    toggleDeleteModal = () => {
        this.setState(prevState => ({
            isShowDeleteModal: !prevState.isShowDeleteModal
        }));
    }

    deleteJob = async () => {
        const { itemId, isDesc, sortName } = this.state;
        try {
            await JobApi.deleteJob(itemId);
            if (this.props.jobList.jobList.sources.length === 1) {
                this.setState({
                    params: { ...this.state.params, skip: this.props.jobList.jobList.pageIndex - 1 }
                })
            }
            this.getJobList(isDesc, sortName)
            this.toggleDeleteModal();
            toastSuccess("The job has been deleted successfully");
        } catch (err) {
            toastError(err);
        }
    }

    onSubmit(e) {
        e.preventDefault();
        this.form.validateAll();
        this.saveJob()
    }

    handlePageClick = e => {
        const { params, isDesc, sortName } = this.state;
        params.take = 10;
        params.skip = e.selected + 1
        this.setState(
            {
                params
            },
            () => this.getJobList(isDesc, sortName)
        )
    }

    getJobList = () => {
        let field = this.state.field;
        let sortName = this.state.sortName;
        let params = Object.assign({}, this.state.params, {
            query: this.state.query,
            sortName: sortName,
            isDesc: this.state[field]
        });
        this.props.getJobList(params);
    };

    getCookie(cname) {
        var name = cname + "=";
        var decodedCookie = decodeURIComponent(document.cookie);
        var ca = decodedCookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) === ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) === 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }

    componentDidMount() {
        this.getJobList();
        var tokenInfo = this.getCookie("token").split(".")[1];
        try {
            var userToken = JSON.parse(window.atob(tokenInfo));
            var userInfo = userToken.JwtPayload;
            this.setState({
                userIdLogin: userInfo.UserId
            })
        } catch (err) {
            toastError(err.message)
        }

        // real time
        this.props.hubConnectionCalendar.on('Receive', (nameSender, idUsers, secalendarDescription, type) => {
            if (type === appConfig.job_add) {
                this.getJobList();
            }
        });

        this.props.hubConnectionCalendar.on('ReceiveUpdate', (nameSender, idUsers, IdUserDelete, secalendarDescription, type) => {
            if (type === appConfig.job_update) {
                this.getJobList();
            }
        });
        // end real time

    }

    getAllCategoryList() {
        RequestHelper.get(appConfig.apiUrl + "categories/all-categories").then(result => {
            this.setState({
                categoryList: result,
            })
        });;
    }

    showAddNew = () => {
        let title = "Create new Job";
        let job = {
            name: "",
            description: "",
            dateStart: new Date(),
            dateEnd: null,
            priority: 1,
            reporterId: this.state.userIdLogin,
        }
        this.toggleModalAddNew(job, title);
    };

    updateItem = async (content) => {
        const { jobId } = this.state.commentItem;
        let params = {
            jobId,
            value: { content }
        }
        await CommentApi.addComment(params)
        this.getCommentList(jobId, false);

        // real-time-notification
        let { item } = this.state;
        let userIds = [...item.users.map(item => item.id), item.reporter.id];
        let nameSender = CookieHelper.getUser().JwtPayload.Name;
        this.props.hubConnectionCalendar.invoke("sendMessage", nameSender, userIds, null, appConfig.comment_add)
            .catch((err) => {
                console.log(err);
            })
        //end-real-time-notification
    }

    deleteComment = async () => {
        try {
            const { commentItemId } = this.state;
            const { jobId } = this.state.commentItem;
            var params = {
                jobId,
                commentId: commentItemId
            }
            await CommentApi.deleteComment(params);
            this.getCommentList(jobId, false);
            //this.getJobList();
            this.toggleDeleteCommentModal();
        } catch (error) {
            toastError(error);
        }
    }

    deleteStep = async () => {
        try {
            const { stepItemId } = this.state;
            const jobId = this.state.item.id;
            var params = {
                jobId,
                stepId: stepItemId
            }
            await StepApi.deleteStep(params);
            this.getJobById(jobId)
            this.toggleDeleteStepModal();
        } catch (error) {
            console.log(error);
        }
    }

    toggle = (tab) => {
        if (this.state.activeTab !== tab) {
            this.setState({
                activeTab: tab
            });
        }
    }

    toggleSort = async (field, Name) => {
        let CurrentState = this.state[field];
        await this.setState({
            [field]: !CurrentState,
            activeSort: Name,
            sortName: Name,
            field: field,
            params: { ...this.state.params, skip: 1 }
        })
        this.getJobList();
    }

    toggleDeleteCommentModal = () => {
        this.setState(prevState => ({
            isShowDelCommentModal: !prevState.isShowDelCommentModal
        }));
    }

    toggleDeleteStepModal = () => {
        this.setState(prevState => ({
            isShowDelStepModal: !prevState.isShowDelStepModal
        }));
    }

    // updateItem = (content) => {
    //     let { item } = this.state;
    //     let data = {
    //         id: item.id,
    //         comment: { content }
    //     }
    //     JobApi.addCommentJob(data).then(() => {
    //         this.getJobById(item.id);
    //     })

    //     // real-time-notification
    //     let userIds = item.users.map(item => item.id);
    //     let nameSender = CookieHelper.getUser().JwtPayload.Name;
    //     this.props.hubConnectionCalendar.invoke("sendMessage", nameSender, userIds, null, appConfig.comment_add)
    //         .catch((err) => {
    //             console.log(err);
    //         })
    //     //end-real-time-notification
    // }

    fileOnChange = (event) => {
        let item = Object.assign({}, this.state.item);
        var name = event.target.name;
        var value = event.target.files;
        item[name] = value;
        this.setState({
            item
        });
    }

    getJobById = async (jobId) => {
        let data = await JobApi.getJobById(jobId);
        this.setState({ item: data.data });
        this.scrollHeight();
    }

    scrollHeight = () => {
        var el = document.getElementById('scroll');
        el.scrollTop = el.scrollHeight;
    }

    getCommentList = async (jobId, load) => {
        let title = "Comment"
        var params = Object.assign({}, {
            isDesc: false
        });
        var commentItem = await CommentApi.getCommentList(jobId, params);
        var comments = commentItem.sources
        this.setState({
            item: {
                id: jobId,
                comments
            }
        },
            () => {
                this.toggleModalComment(commentItem, title, load)
            }
        )
    }

    updateComment = async (data, commentId) => {
        try {
            const { jobId } = this.state.commentItem;
            var params = {
                jobId,
                commentId,
                value: { content: data }
            }
            await CommentApi.updateComment(params)
            this.getJobById(jobId);
        } catch (error) {
            toastError(error);
        }
    }

    onChangeStep = (e) => {
        let name = e.target.name;
        let value = e.target.value;

        const item = Object.assign({}, this.state.item);
        let { steps } = this.state;
        steps[name.slice(4)] = { "name": value };
        item["steps"] = steps
        this.setState({
            item
        })
    }

    addStep = () => {
        let itemAdded = { "name": '' }
        const { steps } = this.state;
        this.setState({
            steps: steps.concat(itemAdded)
        });
    }

    addOneStep = () => {
        let itemAdded = ""
        const { steps } = this.state;
        this.setState({
            steps: steps.concat(itemAdded),
            isShowPlusButtom: false
        });
    }

    onChangeUpdateStep = (e) => {
        let value = e.target.value;
        this.setState({
            stepsTemp: value
        })
    }

    addNewStep = async (stepsTemp) => {
        try {
            const jobId = this.state.item.id;
            var params = {
                jobId,
                value: {
                    name: stepsTemp,
                }
            }
            await StepApi.addStep(params)
            this.getJobById(jobId);
            this.removeStep(0)
        } catch (error) {
            toastError(error);
        }
    }

    updateStep = async (data, stepId) => {
        try {
            const { jobId } = this.state.commentItem;
            var params = {
                jobId,
                stepId,
                value: data
            }
            await StepApi.updateStep(params)
            this.getJobById(jobId);
        } catch (error) {
            toastError(error);
        }
    }

    removeStep = (index) => {
        let { steps } = this.state;
        // steps.splice(index, 1)
        steps = steps.filter((_, i) => i !== index)
        this.setState({
            steps,
            isShowPlusButtom: true
        });
    }

    onKeyDown = async (e) => {
        e.persist();
        if (e.keyCode === 13) {
            if (this.state.stepsTemp && this.state.stepsTemp.trim() !== "") {
                await this.addNewStep(this.state.stepsTemp)
                this.setState({
                    isOpen: false,
                    update: false,
                })
            }
        }
        if (e.keyCode === 27) {
            this.setState({
                update: false,
            })
        }
    }

    showConfirmDeleteComment(commentId) {
        this.setState(
            {
                commentItemId: commentId
            },
            () => this.toggleDeleteCommentModal()
        );
    }

    showConfirmDeleteStep(stepId) {
        this.setState(
            {
                stepItemId: stepId
            },
            () => this.toggleDeleteStepModal()
        );
    }

    removeFile = () => {
        this.setState({
            item: { ...this.state.item, attachments: [] }
        })
    }

    onStarClick = (value) => {
        this.setState({ item: { ...this.state.item, priority: value } });
    }

    render() {
        const { userIdLogin, isShowDelCommentModal, isShowDeleteModal,
            isShowInfoModal, isShowAddNewModal, item, categoryList, isShowDelStepModal, isShowPlusButtom } = this.state;
        const { jobList } = this.props.jobList;
        const { userList } = this.props.userList;
        const { sources, pageIndex, totalPages } = jobList;
        const hasResults = jobList.sources && jobList.sources.length > 0;
        const hasComments = item.comments && item.comments.length > 0;
        const hasUsers = item.users && item.users.length > 0;
        return (
            <div className="animated fadeIn">
                <ModalConfirm
                    title="Are you sure delete this job !"
                    clickOk={this.deleteJob}
                    isShowModal={isShowDeleteModal}
                    toggleModal={this.toggleDeleteModal} />

                <ModalConfirm
                    title="Are you sure delete this comment !"
                    clickOk={this.deleteComment}
                    isShowModal={isShowDelCommentModal}
                    toggleModal={this.toggleDeleteCommentModal} />

                <ModalConfirm
                    title="Are you sure delete this step !"
                    clickOk={this.deleteStep}
                    isShowModal={isShowDelStepModal}
                    toggleModal={this.toggleDeleteStepModal} />

                <ModalInfo
                    size="lg"
                    title={this.state.formTitle}
                    isShowModal={isShowAddNewModal}
                    hiddenFooter >
                    <div className="modal-wrapper">
                        <div className="form-wrapper">
                            <Form
                                encType="multipart/form-data"
                                onSubmit={e => this.onSubmit(e)}
                                ref={c => { this.form = c; }} >

                                <FormGroup>
                                    <ValidationInput
                                        name="name"
                                        title="Job Name"
                                        required={true}
                                        value={item.name}
                                        onChange={this.onModelChange} />
                                </FormGroup>

                                <FormGroup>
                                    <Row>
                                        <Col xs="6">
                                            <MultipleSelect
                                                defaultValue={item.categories ? item.categories : []}
                                                name="categoryIds"
                                                title="Category"
                                                labelField="name"
                                                valueField="id"
                                                options={categoryList || []}
                                                onChange={this.onChangeCategories} />
                                        </Col>

                                        <Col xs="6">
                                            <FormGroup>
                                                <Label for="exampleEmail"><strong>Priority: </strong></Label>
                                                <br />
                                                <StarRatingComponent
                                                    className="rating-star"
                                                    name="Priority"
                                                    starCount={5}
                                                    value={item.priority}
                                                    emptyStarColor="#EEEFFF"
                                                    onStarClick={this.onStarClick} />
                                            </FormGroup>
                                        </Col>
                                    </Row>
                                </FormGroup>

                                <FormGroup>
                                    <Row>
                                        <Col xs="6">
                                            <DatetimeSelect
                                                title="Date Start"
                                                name="dateStart"
                                                defaultValue={moment(item.dateStart).format("DD-MM-YYYY HH:mm A")}
                                                dateFormat="DD-MM-YYYY"
                                                timeFormat="HH:mm A"
                                                isValidDate={(current) => {
                                                    var yesterday = Datetime.moment().subtract(1, 'day');
                                                    if (item.dateEnd) {
                                                        return current.isBetween(yesterday, item.dateEnd);
                                                    }
                                                    else {
                                                        return current.isAfter(yesterday);
                                                    }
                                                }}
                                                onChange={this.handleChangeStart}

                                            />
                                        </Col>

                                        <Col xs="6">
                                            <DatetimeSelect
                                                title="Date End"
                                                name="dateEnd"
                                                defaultValue={item.dateEnd !== null ? moment(item.dateEnd).format("DD-MM-YYYY HH:mm A") : ""}
                                                dateFormat="DD-MM-YYYY"
                                                timeFormat="HH:mm A"
                                                isValidDate={(current) => {
                                                    var yesterday = moment(item.dateStart).subtract(1, 'day');
                                                    return current.isAfter(yesterday);
                                                }}
                                                onChange={this.handleChangeEnd}

                                            />
                                        </Col>
                                    </Row>
                                </FormGroup>

                                <FormGroup>
                                    <MultipleSelect
                                        name="userIds"
                                        title="Users receive job"
                                        labelField="name"
                                        valueField="id"
                                        options={userList || []}
                                        required={true}
                                        onChange={this.onModelChange}
                                        defaultValue={item.users ? item.users : []}
                                    />
                                </FormGroup>

                                <FormGroup>
                                    <Label for="examplePicture"><strong>Attachment: </strong></Label>
                                    {item.attachments && item.attachments.length > 0 && item.attachments[0].link ?
                                        <Row>
                                            <Col lg='10'>
                                                {item.attachments.map((val, idx) =>
                                                    <Input
                                                        key={idx}
                                                        name="attachments"
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
                                            name="attachments"
                                            title="Attachment"
                                            type="file"
                                            accept=".doc, .docx, .pdf"
                                            multiple
                                            onChange={this.fileOnChange} />
                                    }
                                </FormGroup>

                                <FormGroup>
                                    <Label for="exampleEmail"><strong>Description: </strong></Label>
                                    <Input
                                        name="description"
                                        title="Description"
                                        type="textarea"
                                        value={item && item.description ? item.description : ""}
                                        onChange={this.onModelChange}
                                    />
                                </FormGroup>

                                <FormGroup className={`${item.id ? "hiden_process" : "show_process"}`} >
                                    <Row>
                                        <Col lg="2">
                                            <Label onClick={this.addStep}>
                                                <i className="fa fa-plus fa-lg" /> <strong>Process:</strong>
                                            </Label>
                                        </Col>

                                        <Col lg="9">
                                            {this.state.steps.map((value, index) =>
                                                <Row key={index}>
                                                    <Col lg="10">
                                                        <ValidationInput
                                                            value={value.name || ''}
                                                            name={`step${index}`}
                                                            required={true}
                                                            onChange={(e) => this.onChangeStep(e)}
                                                        />
                                                    </Col>

                                                    <Col lg="1">
                                                        {index >= 0 ?
                                                            <i className="fa fa-trash fa-lg custom_color_icon_delete" aria-hidden="true"
                                                                onClick={() => this.removeStep(index)}
                                                            /> : null}
                                                    </Col>
                                                </Row>
                                            )}
                                        </Col>
                                    </Row>
                                </FormGroup>

                                <div className="text-center">
                                    <Button className=" btn-primary" type="submit"> Confirm </Button>{" "}
                                    <Button className="btn-danger" onClick={() => this.toggleModalAddNew(item)}> Cancel </Button>
                                </div>

                            </Form>
                        </div>
                    </div>
                </ModalInfo>

                <ModalInfo
                    size='lg'
                    title={this.state.formTitle}
                    isShowModal={isShowInfoModal}
                    hiddenFooter>

                    <div className="modal-wrapper">
                        <div className="form-wrapper">
                            <Form
                                onSubmit={e => this.onSubmit(e)}
                                ref={c => { this.formUpdate = c }}>

                                <Nav tabs>
                                    <NavItem>
                                        <NavLink
                                            className={classnames({ active: this.state.activeTab === '1' })}
                                            onClick={() => { this.toggle('1'); }} >
                                            {'  '}  Detail {'  '}
                                            <span className="badge badge-success">New</span>
                                        </NavLink>
                                    </NavItem>

                                    <NavItem>
                                        <NavLink
                                            className={classnames({ active: this.state.activeTab === '2' })}
                                            onClick={() => { this.toggle('2'); }} >
                                            {'  '}  Comments {'  '}
                                            <span className="badge badge-pill badge-danger">{hasComments ? item.comments.length : null}</span>
                                        </NavLink>
                                    </NavItem>

                                    <NavItem>
                                        <NavLink
                                            className={classnames({ active: this.state.activeTab === '3' })}
                                            onClick={() => { this.toggle('3'); }} >
                                            {'  '} Users {'  '}
                                            <span className="badge badge-pill badge-danger">{hasUsers ? item.users.length : null}</span>
                                        </NavLink>
                                    </NavItem>

                                    <NavItem>
                                        <NavLink
                                            className={classnames({ active: this.state.activeTab === '4' })}
                                            onClick={() => { this.toggle('4'); }} >
                                            {'  '} Process {'  '}
                                        </NavLink>
                                    </NavItem>
                                </Nav>

                                <TabContent activeTab={this.state.activeTab}>
                                    <Detail
                                        detail={item} />

                                    <TabPane tabId="2">
                                        <div className="visible-scrollbar" id="scroll">
                                            {hasComments &&
                                                item.comments.map((item, key) => {
                                                    return (
                                                        <CommentItem
                                                            key={key}
                                                            name={item.username}
                                                            comment={item.content}
                                                            day={moment(item.dateComment).fromNow()}
                                                            userId={item.userId}
                                                            userIdLogin={userIdLogin}
                                                            updateComment={(data) => this.updateComment(data, item.id)}
                                                            deleteComment={() => this.showConfirmDeleteComment(item.id)}
                                                        />
                                                    );
                                                })}
                                        </div>
                                        <hr />
                                        <CommentInput updateItem={(content) => this.updateItem(content)} />
                                    </TabPane>

                                    <TabPane tabId="3">
                                        {hasUsers &&
                                            item.users.map((item, key) => {
                                                return (
                                                    <ListUser
                                                        key={key}
                                                        username={item.name}
                                                        roles={item.roles}
                                                    />
                                                );
                                            })}
                                    </TabPane>

                                    <TabPane tabId="4">
                                        {hasUsers &&
                                            item.steps.map((step, key) => {
                                                return (
                                                    <Step
                                                        key={key}
                                                        step={step}
                                                        updateStep={(data) => this.updateStep(data, step.id)}
                                                        deleteStep={() => this.showConfirmDeleteStep(step.id)}
                                                    />
                                                );
                                            })}

                                        <Row>
                                            <Col lg="12">
                                                <div data-toggle="tooltip" title="Add Step" onClick={this.addOneStep} className={`${isShowPlusButtom ? "show_plus_button d-flex justify-content-center add_step" : "hidden_plus_button d-flex justify-content-center add_step"}`} >
                                                    <i className="fa fa-plus padding-icon " aria-hidden="true"></i>
                                                </div>
                                            </Col>
                                            <Col lg="12">
                                                {this.state.steps && this.state.steps.map((value, index) =>
                                                    <Row key={index}>
                                                        <Col lg="11">
                                                            <Input
                                                                value={value.name}
                                                                name={`step${index}`}
                                                                onChange={(e) => this.onChangeUpdateStep(e)}
                                                                style={{ margin: "10px" }}
                                                                onKeyDown={(e) => this.onKeyDown(e)}
                                                                autoFocus={true}
                                                            />
                                                        </Col>

                                                        <Col lg="1">
                                                            {index >= 0 ?
                                                                <i className="fa fa-paper-plane fa-lg custom_color_icon_send" aria-hidden="true"
                                                                    onClick={() => this.addNewStep(this.state.stepsTemp)}
                                                                /> : null}
                                                        </Col>
                                                    </Row>
                                                )}
                                                <Col>

                                                </Col>
                                            </Col>
                                        </Row>
                                    </TabPane>

                                </TabContent>
                                <div className="text-center">

                                    <Button
                                        className="btn btn-primary btn btn-secondary fa fa-chevron-up"
                                        onClick={this.toggleModalInfo} />

                                    <Button
                                        className="btn btn-primary fa fa-pencil"
                                        onClick={() => this.showUpdateModal(item)}
                                    />

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
                                className="btn btn-success btn-sm"> Create </Button>
                        </Col>

                        <Col xs="5" sm="5" md="5" lg="5">
                            <input
                                onChange={this.onSearchChange}
                                className="form-control form-control-sm custom_search"
                                placeholder="Searching..." />
                        </Col>
                    </Row>

                    <Table responsive bordered className="react-bs-table react-bs-table-bordered data-table ">
                        <thead>
                            <tr className="table-header">
                                <th>STT</th>
                                <th onClick={() => this.toggleSort("IsDescName", "name")}>Name <i className={`${this.state.activeSort === "name" ? "active-sort" : "disactive-sort"} ${!this.state.IsDescName ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}></i></th>
                                <th>Categories</th>
                                <th onClick={() => this.toggleSort("IsDescStatus", "status")}>Status <i className={`${this.state.activeSort === "status" ? "active-sort" : "disactive-sort"} ${!this.state.IsDescStatus ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}></i></th>
                                <th onClick={() => this.toggleSort("IsDescPriority", "priority")}>Priority <i className={`${this.state.activeSort === "priority" ? "active-sort" : "disactive-sort"} ${!this.state.IsDescPriority ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}></i></th>
                                <th onClick={() => this.toggleSort("IsDescDateStart", "dateStart")}>Date Start <i className={`${this.state.activeSort === "dateStart" ? "active-sort" : "disactive-sort"} ${!this.state.IsDescDateStart ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}></i></th>
                                <th onClick={() => this.toggleSort("IsDescDateEnd", "dateEnd")}>Date End <i className={`${this.state.activeSort === "dateEnd" ? "active-sort" : "disactive-sort"} ${!this.state.IsDescDateEnd ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}></i></th>
                                <th>Detail</th>
                            </tr>
                        </thead>
                        <tbody>
                            {hasResults &&
                                sources.map((item, i) => {
                                    return (
                                        <tr className="table-row" key={i}>
                                            <td > {jobList.pageIndex !== 0 ? jobList.pageIndex * jobList.pageSize - jobList.pageSize + ++i : ++i}</td>
                                            <td>{item.name}</td>
                                            <td>
                                                {
                                                    item.categories.map((category, key) => {
                                                        return (
                                                            <span key={key}>{category.name}</span>
                                                        );
                                                    })
                                                }
                                            </td>
                                            <td>{item.status === 1 ? 'Pending' : item.status === 2 ? 'Doing' : 'Done'}</td>
                                            <td><StarRatingComponent
                                                name="rate1"
                                                starCount={5}
                                                value={item.priority}
                                                editing={false}
                                                emptyStarColor="#EEEFFF" />
                                            </td>
                                            <td>{moment(item.dateStart).format("LLLL")}</td>
                                            <td>{item.dateEnd != null ? moment(item.dateEnd).format("LLLL") : null}</td>
                                            <td>
                                                <Button
                                                    className="btn btn-primary btn btn-secondary fa fa-info-circle fa-lg"
                                                    onClick={() => this.showDetailModal(item)} />

                                                <Button
                                                    className="btn btn-danger fa fa-trash"
                                                    onClick={() => this.showConfirmDelete(item.id)} />
                                            </td>
                                        </tr>
                                    );
                                })}
                        </tbody>
                    </Table>
                    {
                        hasResults && totalPages > 1 &&
                        <Pagination
                            totalPages={totalPages}
                            initialPage={0}
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

const mapStateToProps = (state) => {
    return {
        jobList: state.jobList,
        userList: state.userList,
        hubConnectionCalendar: state.hubConnectionCalendar.hubConnectionCalendar
    }
}

export default connect(mapStateToProps, { getJobList, getUserList, })(UserInJobListPage)