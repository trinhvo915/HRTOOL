import React, { Component } from "react";
import { connect } from "react-redux";
import { Row, Col, Button, Table, FormGroup, Label, Input, TabPane, TabContent, Nav, NavItem, NavLink } from "reactstrap";
import Form from "react-validation/build/form";
import lodash from "lodash";
import "react-datepicker/dist/react-datepicker.css";
import Datetime from "react-datetime"
import moment from "moment";

import "./job.list.page.css";
import classNames from "classnames";
import Api from "../../../api/api.job";
import CommentApi from "../../../api/api.comment";
import { toastSuccess, toastError } from "../../../helpers/toast.helper";
import { getJobList } from "../../../actions/job.list.action";
import { getUserList } from "../../../actions/user.list.action";
import { getAllCategoryListNoFilter } from "../../../actions/category.list.action";
import { getCommentList } from "../../../actions/comment.list.action";
import Pagination from "../../../components/pagination/Pagination";
import ModalConfirm from "../../../components/modal/modal.confirm";
import ModalInfo from "../../../components/modal/modal.info";
import ValidationInput from "../../../components/common/validation.input";
import { pagination } from "../../../constant/app.constant";
import MultipleSelect from "../../../components/common/multiple.select";
import SelectInput from "../../../components/common/select.input";
import InputField from "../../../components/common/input.field";
import DatetimeSelect from "../../../components/common/datetime.select";
import CommentItem from "../../../components/common/comment.item";
import CommentInput from "../../../components/common/comment.input";
import StepList from "./step.list.page";
import StepApi from "../../../api/api.step";
import CookieHelper from "../../../helpers/cookie.helper";
import { appConfig } from "../../../config/app.config";
import StarRatingComponent from 'react-star-rating-component';

class JobListPage extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isShowInfoModal: false,
            isShowCommentModal: false,
            isShowDeleteModal: false,
            isShowDelCommentModal: false,
            isShowDelStepModal: false,
            item: {},
            commentItem: {},
            stepItem: [],
            itemId: null,
            commentItemId: null,
            stepItemId: null,
            userIdLogin: null,
            params: {
                skip: pagination.initialPage,
                take: pagination.defaultTake
            },
            query: "",
            isDesc: true,
            sortName: "Priority",
            activeSort: "Priority",
            activeTab: '1',
            isOpen: false,
            isOpenInputAddStep: false,
            usesIdBefores: [],
            stepValue: ""
        };
        this.handleChangeStart = this.handleChangeStart.bind(this);
        this.handleChangeEnd = this.handleChangeEnd.bind(this);
        this.delayedCallback = lodash.debounce(this.search, 1000);
        this.removeFile = this.removeFile.bind(this);
    }

    toggleModalInfo = (item, title, isOpen = false) => {
        this.setState(prevState => ({
            isShowInfoModal: !prevState.isShowInfoModal,
            item: item || {},
            formTitle: title,
            stepItem: [],
            activeTab: '1',
            isOpen
        }));
    };

    toggleDeleteModal = () => {
        this.setState(prevState => ({
            isShowDeleteModal: !prevState.isShowDeleteModal
        }));
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

    showAddNew = () => {
        const { reporterId } = this.state.item;
        let title = "Create new Job";
        let job = {
            name: "",
            description: "",
            dateStart: new Date(),
            dateEnd: null,
            status: 1,
            priority: 2,
            reporterId
        };
        this.toggleModalInfo(job, title);
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

    fileOnChange = (event) => {
        let item = Object.assign({}, this.state.item);
        var name = event.target.name;
        var value = event.target.files;
        item[name] = value;
        this.setState({
            item
        });
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
            () => { this.getJobList(this.state.isDesc, this.state.sortName); }
        )
    }

    handleChangeStart(date) {
        let item = Object.assign({}, this.state.item);
        item["dateStart"] = date._d;
        this.setState({
            item
        });
    }

    handleChangeEnd(date) {
        let item = Object.assign({}, this.state.item);
        item["dateEnd"] = date._d;
        this.setState({
            item
        });
    }

    getJobList = (isDesc = true, sortName = "Priority") => {
        let params = Object.assign({}, this.state.params, {
            query: this.state.query,
            isDesc: isDesc,
            sortName: sortName
        });
        this.props.getJobList(params);
    }

    getAllCategoryListNoFilter = () => {
        let params = Object.assign({}, {
            query: this.state.query
        });
        this.props.getAllCategoryListNoFilter(params);
    }

    getUserList = () => {
        let params = Object.assign({}, {
            query: this.state.query
        });
        this.props.getUserList(params);
    }

    onSearchChange = e => {
        e.persist();
        this.delayedCallback(e);
    }

    uploadFile = async (attachments) => {
        var result;
        try {
            if (!attachments) {
                result = [];
            }
            else {
                if (attachments.length > 1) {
                    result = await Api.uploadFiles("attachment", attachments);

                } else {
                    result = await Api.uploadFile("attachment", attachments[0]);
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

        }
    }

    async addJob() {
        const job = Object.assign({}, this.state.item);
        const { isDesc, sortName, stepItem } = this.state;
        const { attachments } = job;
        job.steps = stepItem;
        job.attachments = await this.uploadFile(attachments);
        try {
            await Api.addJob(job)
            this.toggleModalInfo();
            this.getJobList(isDesc, sortName);
            toastSuccess("The job has been created successfully");
            // notification
            let nameSender = CookieHelper.getUser().JwtPayload.Name;
            this.props.hubConnectionCalendar.invoke("sendMessage", nameSender, job.userIds, job.description, appConfig.job_add)
                .catch((err) => {
                    console.log(err);
                })
            //emd_notification
        } catch (err) {

        }
    }

    async updateJob() {
        const job = Object.assign({}, this.state.item);
        const { isDesc, sortName } = this.state;
        const { attachments } = job;

        // real-time-notification
        const { usesIdBefores } = this.state;
        // end-real-time-notification

        if (job.attachments && job.attachments.length > 0 && !job.attachments[0].link) {
            job.attachments = await this.uploadFile(attachments);
        }
        try {
            await Api.updateJob(job);
            this.toggleModalInfo();
            this.getJobList(isDesc, sortName);
            toastSuccess("The job has been updated successfully");
            // real-time-notification
            let nameSender = CookieHelper.getUser().JwtPayload.Name;
            this.props.hubConnectionCalendar.invoke("sendUpdate", nameSender, job.userIds, usesIdBefores, job.description, appConfig.job_update)
                .catch((err) => {
                    console.log(err);
                })
            // real-time-notification
        } catch (err) {

        }
    }

    saveJob() {
        let { id } = this.state.item;
        if (id) {
            this.updateJob()
        } else {
            if (this.validateStep()) {
                this.addJob();
            }
            else {
                toastError("Step must not be null");
            }
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

    async showUpdateModal(val) {
        // real-time-notification
        let usesIdBefores = val.users.map((e) =>
            e.id
        );

        await this.setState({
            usesIdBefores: usesIdBefores
        });
        // end-real-time-notification

        let title = "Update Job";
        var item = Object.assign({}, val);
        item.reporterId = item.reporter.id;
        item.userIds = item.users.map((val, idx) => {
            return val.id;
        })
        item.categoryIds = item.categories.map((val, idx) => {
            return val.id;
        })
        this.toggleModalInfo(item, title, true)
        console.log(item);
    }

    showConfirmDelete(itemId) {
        this.setState(
            {
                itemId: itemId
            },
            () => this.toggleDeleteModal()
        );
    }

    deleteJob = async () => {
        const { itemId, isDesc, sortName } = this.state;
        try {
            await Api.deleteJob(itemId);
            if (this.props.jobList.jobList.sources.length === 1) {
                this.setState({
                    params: { ...this.state.params, skip: this.props.jobList.jobList.pageIndex - 1 }
                })
            }

            this.getJobList(isDesc, sortName)
            this.toggleDeleteModal();
            toastSuccess("The job has been deleted successfully");
        } catch (err) {

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

    toggleSort = (sortName) => {
        const { isDesc } = this.state;
        this.setState({
            isDesc: !isDesc,
            sortName: sortName,
            activeSort: sortName
        },
            () => this.getJobList(this.state.isDesc, this.state.sortName)
        );
    }

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
        this.getUserList();
        this.getAllCategoryListNoFilter();

        var tokenInfo = this.getCookie("token").split(".")[1];
        try {
            var userToken = JSON.parse(window.atob(tokenInfo));
            var userInfo = userToken.JwtPayload;
            this.setState({
                userIdLogin: userInfo.UserId,
                item: {
                    reporterId: userInfo.UserId
                }
            })
        } catch (err) {

        }
    }

    removeFile() {
        this.setState({
            item: { ...this.state.item, attachments: [] }
        })
    }

    getCommentList = async (jobId, users, load) => {
        let title = "Comment"
        var params = Object.assign({}, {
            isDesc: true
        });
        var commentItem = await CommentApi.getCommentList(jobId, params);
        this.setState({
            commentItem: {
                users,
                jobId,
                ...commentItem
            }
        },
            () => this.toggleModalComment(commentItem, title, load)
        )
    }

    toggleModalComment = (comment, title, load = true) => {
        if (load) {
            this.setState(prevState => ({
                isShowCommentModal: !prevState.isShowCommentModal,
                comment: comment,
                formTitle: title
            }))
        }
    }

    showComment(jobId, users) {
        this.getCommentList(jobId, users)

        // real-time-notification
        this.props.hubConnectionCalendar.on('Receive', (nameSender, idUsers, secalendarDescription, type) => {
            if (type === appConfig.comment_add || type === appConfig.comment_update) {
                this.getCommentList(jobId, users);
            }
        });
        // end-real-time-notification
    }

    async updateItem(content) {
        const { jobId, users } = this.state.commentItem;
        let params = {
            users,
            jobId,
            value: { content }
        }
        let userIds = users.map(item => item.id);
        let id_current = CookieHelper.getUser().JwtPayload.UserId;
        userIds.push(id_current);

        await CommentApi.addComment(params);
        this.getCommentList(jobId, users, false);

        // real-time-notification
        let nameSender = CookieHelper.getUser().JwtPayload.Name;
        this.props.hubConnectionCalendar.invoke("sendMessage", nameSender, userIds, null, appConfig.comment_add)
            .catch((err) => {
                console.log(err);
            })
        // end-real-time-notification
    }

    updateComemnt = async (data, commentId) => {
        try {
            const { jobId, users } = this.state.commentItem;
            var params = {
                users,
                jobId,
                commentId,
                value: { content: data }
            }
            await CommentApi.updateComment(params)
            this.getCommentList(jobId, users, false);
        } catch (error) {

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

    deleteComment = async () => {
        try {
            const { commentItemId } = this.state;
            const { jobId, users } = this.state.commentItem;
            var params = {
                jobId,
                commentId: commentItemId
            }
            await CommentApi.deleteComment(params);
            this.getCommentList(jobId, users, false);
            this.toggleDeleteCommentModal();
        } catch (error) {

        }
    }

    toggleTab = (tab) => {
        this.setState({
            activeTab: tab
        })
    }

    addStep = () => {
        const { stepItem } = this.state;
        stepItem.push({ name: "" });

        this.setState({
            stepItem,
            isOpenInputAddStep: true
        })
    }

    removeStep = (idx) => {
        const { stepItem } = this.state;
        stepItem.splice(idx, 1);

        this.setState({
            stepItem
        })
    }

    onChangeStep = (e) => {
        const value = e.target.value;
        const index = parseInt(e.target.name.split('-')[1]);
        const { stepItem } = this.state;
        stepItem[index] = { name: value }
        this.setState({
            stepItem
        })
    }



    getStepList = async (jobId) => {
        try {
            const item = Object.assign({}, this.state.item);
            const stepList = await StepApi.getStepListByJob(jobId);
            item.steps = stepList.sources;
            this.setState({
                item
            })
        } catch (error) {

        }
    }

    getJobById = async (jobId) => {
        try {
            const job = await Api.getJobById(jobId);
            this.setState(prevState => ({
                item: job.data || {},
                stepItem: [],
                isOpenInputAddStep: false
            }));
        } catch (error) {

        }
    }

    updateStep = async (data) => {
        try {
            await StepApi.updateStep(data);
            this.getJobById(data.jobId);
            toastSuccess("Step of job has been updated successfully");
            return true;
        } catch (error) {

        }
    }

    onKeyDownStep = async (e) => {
        e.persist();
        if (e.keyCode === 13) {
            const { id } = this.state.item;
            const { stepValue } = this.state;
            if (stepValue.trim() !== "") {
                const params = {
                    jobId: id,
                    data: { name: stepValue }
                }
                try {
                    await StepApi.addStep(params);
                    await this.getJobById(id);
                    toastSuccess("Step of job has been created successfully")
                } catch (error) {

                }
            }
        }
        if (e.keyCode === 27) {
            this.setState({
                isOpenInputAddStep: false
            })
        }
    }

    onChangeStepValue = (e) => {
        this.setState({
            stepValue: e.target.value
        })
    }

    onClickAddStep = async () => {
        const { stepValue } = this.state;
        const { id } = this.state.item;
        if (stepValue.trim() !== "") {
            const params = {
                jobId: id,
                data: { name: stepValue }
            }
            try {
                await StepApi.addStep(params);
                await this.getJobById(id);
                toastSuccess("Step of job has been created successfully")
            } catch (error) {

            }
        }
    }

    onClickCancelStep = () => {
        this.setState({
            isOpenInputAddStep: false
        })
    }

    validateStep = () => {
        const { stepItem } = this.state;
        return stepItem.every(val => val.name.trim() !== "");
    }

    showConfirmDeleteStep = (stepId) => {
        this.setState({
            stepItemId: stepId
        },
            () => this.toggleDeleteStepModal()
        )
    }

    deleteStep = async () => {
        try {
            const { stepItemId } = this.state;
            const { id } = this.state.item;
            await StepApi.deleteStep({ id: stepItemId })
            this.getJobById(id);
            this.toggleDeleteStepModal();
            toastSuccess("Step of job has been deleted successfully")
        } catch (error) {

        }
    }

    onStarClick = (value) => {
        this.setState({ item: { ...this.state.item, priority: value } });
    }

    render() {
        const { isShowDelStepModal, isShowDeleteModal, isShowDelCommentModal, isShowInfoModal, isShowCommentModal, item, userIdLogin, isOpen, stepItem, isOpenInputAddStep, stepValue } = this.state;
        const { jobList } = this.props.jobList;
        const { sources, pageIndex, totalPages } = jobList;
        const { userList } = this.props.userList;
        console.log(userList);
        const hasResults = jobList.sources && jobList.sources.length > 0;
        console.log(this.props.categoryList)
        const categoryList = Object.assign([], this.props.categoryList.categoryList);
        const commentList = this.state.commentItem.sources;
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
                    isShowModal={isShowCommentModal}
                    title={this.state.formTitle + `(${commentList ? commentList.length : ""})`}
                    hiddenFooter
                >
                    <TabPane style={{ marginBottom: '10px' }} >

                        <div className="visible-scrollbar" id="scroll">
                            {
                                commentList && commentList.map((val, idx) => <CommentItem
                                    key={idx}
                                    name={val.username}
                                    comment={val.content}
                                    day={moment(val.dateComment).fromNow()}
                                    userId={val.userId}
                                    userIdLogin={userIdLogin}
                                    updateComemnt={(data) => this.updateComemnt(data, val.id)}
                                    deleteComment={() => this.showConfirmDeleteComment(val.id)}
                                />)
                            }
                        </div>
                        <hr />
                        <CommentInput updateItem={(content) => this.updateItem(content)} />
                    </TabPane>
                    <div className="text-center">
                        <Button className="btn-danger" onClick={this.toggleModalComment}> Cancel </Button>
                    </div>
                </ModalInfo>



                <ModalInfo
                    size="lg"
                    title={this.state.formTitle}
                    isShowModal={isShowInfoModal}
                    hiddenFooter >
                    <Nav tabs>
                        <NavItem>
                            <NavLink
                                className={classNames({ active: this.state.activeTab === '1' })}
                                onClick={() => this.toggleTab('1')}
                            >
                                Detail
                                </NavLink>
                        </NavItem>
                        {
                            isOpen ?
                                <NavItem >
                                    <NavLink
                                        className={classNames({ active: this.state.activeTab === '2' })}
                                        onClick={() => this.toggleTab('2')}
                                    >
                                        Progress
                                    </NavLink>
                                </NavItem> : ""
                        }
                    </Nav>
                    <TabContent activeTab={this.state.activeTab}>
                        <TabPane tabId="1">
                            <div className="modal-wrapper">
                                <div className="form-wrapper">
                                    <Form
                                        encType="multipart/form-data"
                                        onSubmit={e => this.onSubmit(e)}
                                        ref={c => {
                                            this.form = c;
                                        }}
                                    >
                                        <ValidationInput
                                            name="name"
                                            title="Job Name"
                                            required={true}
                                            value={item.name}
                                            onChange={this.onModelChange}
                                        />

                                        <MultipleSelect
                                            defaultValue={item.categories ? item.categories : []}
                                            name="categoryIds"
                                            title="Category"
                                            labelField="name"
                                            valueField="id"
                                            options={categoryList}
                                            onChange={this.onChangeCategories}
                                        />

                                        <Row>
                                            <Col xs="6">
                                                <SelectInput
                                                    name="status"
                                                    title="Status"
                                                    nameField="nameField"
                                                    valueField="valueField"
                                                    onChange={this.onModelChange}
                                                    value={item.status}
                                                    options={[
                                                        {
                                                            name: 'Pending',
                                                            id: 1
                                                        },
                                                        {
                                                            name: 'Doing',
                                                            id: 2
                                                        },
                                                        {
                                                            name: 'Done',
                                                            id: 3
                                                        }
                                                    ]}
                                                />
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
                                                    }
                                                    }
                                                    onChange={this.handleChangeStart}

                                                />
                                            </Col>

                                            <Col xs="6">
                                                <DatetimeSelect
                                                    title="Date End"
                                                    name="dateEnd"
                                                    defaultValue={item.dateEnd ? moment(item.dateEnd).format("DD-MM-YYYY HH:mm A") : ""}
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

                                        {/* <SelectInput
                                            name="reporterId"
                                            title="Creater Job"
                                            valueField="id"
                                            nameField="name"
                                            options={userList}
                                            required={true}
                                            onChange={this.onModelChange}
                                            value={item.reporterId}
                                        /> */}

                                        <MultipleSelect
                                            name="userIds"
                                            title="Users receive job"
                                            labelField="name"
                                            valueField="id"
                                            options={userList}
                                            required={true}
                                            onChange={this.onModelChange}
                                            defaultValue={item.users ? item.users : []}
                                        />

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

                                        <InputField
                                            name="description"
                                            title="Description"
                                            type="textarea"
                                            value={item.description}
                                            onChange={this.onModelChange}
                                        />
                                        {
                                            !isOpen ?
                                                <FormGroup id="form-step">
                                                    <Label for="exampleEmail">Steps</Label>

                                                    <Row>
                                                        <Col xs="1"><i onClick={this.addStep} className="fa fa-plus fa-lg mt-2"></i></Col>
                                                        <Col xs="11">
                                                            {
                                                                stepItem.map((val, idx) =>
                                                                    <div className="d-flex justify-content-center mb-2" key={idx}>
                                                                        <Input value={val.name} onChange={(e) => this.onChangeStep(e)} type="text" name={`step-${idx}`} />
                                                                        <i onClick={() => this.removeStep(idx)} className="fa fa-trash fa-lg mt-2"></i>
                                                                    </div>
                                                                )
                                                            }
                                                        </Col>
                                                    </Row>
                                                </FormGroup> : ""
                                        }

                                        <div className="text-center">
                                            <Button className=" btn-primary" type="submit"> Confirm </Button>{" "}
                                            <Button className="btn-danger" onClick={this.toggleModalInfo}> Cancel </Button>
                                        </div>

                                    </Form>
                                </div>
                            </div>

                        </TabPane>
                        <TabPane tabId="2">
                            {
                                item.steps && item.steps.map((val, idx) =>
                                    <StepList
                                        key={idx}
                                        step={val}
                                        updateStep={(data) => this.updateStep(data)}
                                        deleteStep={() => this.showConfirmDeleteStep(val.id)}
                                    />
                                )
                            }

                            {
                                isOpenInputAddStep ?
                                    <div className="wrapper-step d-flex">
                                        <Input autoFocus={true} className="step-input" type="text" value={stepValue} onChange={(e) => this.onChangeStepValue(e)} onKeyDown={(e) => this.onKeyDownStep(e)} />
                                        <i className="enter fa fa-paper-plane fa-lg mt-2 mr-4" onClick={this.onClickAddStep}></i>
                                        <i className="cancel fa fa-trash fa-lg mt-2" onClick={this.onClickCancelStep}></i>
                                    </div> :
                                    <button className="btn btn-info btn-lg btn-block" onClick={this.addStep} ><i className="fa fa-plus fa-lg mt-2"></i></button>
                            }

                            <div className="text-center mt-2">
                                <Button className="btn-danger" onClick={this.toggleModalInfo}> Cancel </Button>
                            </div>
                        </TabPane>
                    </TabContent>

                </ModalInfo>

                <div className="animated fadeIn">
                    <Row className="flex-container header-table">
                        <Col xs="5" sm="5" md="5" lg="5">
                            <Button
                                onClick={this.showAddNew}
                                className="btn btn-success btn-sm"> Create </Button>
                            <Button
                                href={appConfig.apiUrl + "export/jobs"}
                                className="btn btn-primary btn-sm">Export Excel</Button>
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
                                <th className="group-sort" onClick={() => this.toggleSort("Name")} >
                                    <div className="d-flex">
                                        <span> Name</span>
                                        <i className={`${this.state.activeSort === 'Name' ? "active-sort" : "disactive-sort"} ${this.state.activeSort === 'Name' && !this.state.isDesc ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}></i>
                                    </div>
                                </th>
                                <th >Categories</th>

                                <th className="group-sort " onClick={() => this.toggleSort("Status")}>
                                    <div className="d-flex">
                                        <span>Status</span>
                                        <i className={`${this.state.activeSort === 'Status' ? "active-sort" : "disactive-sort"} ${this.state.activeSort === 'Status' && !this.state.isDesc ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}></i>
                                    </div>
                                </th>

                                <th className="group-sort " onClick={() => this.toggleSort("Priority")}>
                                    <div className="d-flex">
                                        <span>Priority</span>
                                        <i className={`${this.state.activeSort === 'Priority' ? "active-sort" : "disactive-sort"} ${this.state.activeSort === 'Priority' && !this.state.isDesc ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}></i>
                                    </div>
                                </th>

                                <th className="group-sort " onClick={() => this.toggleSort("DateStart")}>
                                    <div className="d-flex">
                                        <span>Date Start</span>
                                        <i className={`${this.state.activeSort === 'DateStart' ? "active-sort" : "disactive-sort"} ${this.state.activeSort === 'DateStart' && !this.state.isDesc ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}></i>
                                    </div>
                                </th>

                                <th className="group-sort " onClick={() => this.toggleSort("DateEnd")}>
                                    <div className="d-flex">
                                        <span>Date End</span>
                                        <i className={`${this.state.activeSort === 'DateEnd' ? "active-sort" : "disactive-sort"} ${this.state.activeSort === 'DateEnd' && !this.state.isDesc ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}></i>
                                    </div>
                                </th>

                                <th className="group-sort " onClick={() => this.toggleSort("Description")}>
                                    <div className="d-flex">
                                        <span>Description</span>
                                        <i className={`${this.state.activeSort === 'Description' ? "active-sort" : "disactive-sort"} ${this.state.activeSort === 'Description' && !this.state.isDesc ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}></i>
                                    </div>
                                </th>

                                <th >Creater</th>
                                <th >Users Receive</th>
                                <th>Attachment</th>
                                <th >Action</th>
                            </tr>
                        </thead>
                        <tbody>
                            {hasResults &&
                                sources.map((itemData, i) => {
                                    return (
                                        <tr className="table-row" key={i}>
                                            <td > {jobList.pageIndex !== 0 ? jobList.pageIndex * jobList.pageSize - jobList.pageSize + ++i : ++i}</td>
                                            <td>{itemData.name}</td>
                                            <td>{itemData.categories.map((val, idx) => {
                                                return val.name;
                                            }).join(', ')}</td>
                                            <td>{itemData.statusText}</td>
                                            <td><StarRatingComponent
                                                name="rate1"
                                                starCount={5}
                                                value={itemData.priority}
                                                editing={false}
                                                emptyStarColor="#EEEFFF" />
                                            </td>
                                            <td>{moment(itemData.dateStart).format("DD-MM-YYYY HH:mm A")}</td>
                                            <td>{itemData.dateEnd ? moment(itemData.dateEnd).format("DD-MM-YYYY HH:mm A") : null}</td>
                                            <td className="custom_description_job">{itemData.description}</td>
                                            <td>{itemData.reporter.name}</td>
                                            <td>{itemData.users.map((val, idx) => {
                                                return val.name
                                            }).join(', ')}</td>
                                            <td>
                                                {itemData.attachments &&
                                                    itemData.attachments.map((attachment, idx) =>
                                                        <div key={++idx} style={{ "textAlign": "left" }} >
                                                            {idx}.&nbsp;
                                                            <a href={attachment.link} target="_blank" rel="noopener noreferrer" download>
                                                                {attachment.extension === '.jpg' ||
                                                                    attachment.extension === '.jpeg' ||
                                                                    attachment.extension === '.png' ?
                                                                    <img src={attachment.link} alt="Resume" width="80" /> : attachment.fileName}
                                                            </a><br />
                                                        </div>)}
                                            </td>
                                            <td className="custom_action_column">
                                                <Button
                                                    className="btn btn-primary fa fa-pencil"
                                                    onClick={() => this.showUpdateModal(itemData)} />
                                                <Button
                                                    className="btn btn-danger fa fa-trash"
                                                    onClick={() => this.showConfirmDelete(itemData.id)} />
                                                <Button
                                                    className="btn btn-primary fa fa-comment-o"
                                                    onClick={() => this.showComment(itemData.id)}
                                                />
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
                            page={pageIndex}
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
        categoryList: state.categoryList,
        commentList: state.commentList,
        hubConnectionCalendar: state.hubConnectionCalendar.hubConnectionCalendar
    }
}

export default connect(mapStateToProps, { getJobList, getUserList, getAllCategoryListNoFilter, getCommentList })(JobListPage)