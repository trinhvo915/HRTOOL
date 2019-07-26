import React, { Component } from "react";
import { connect } from "react-redux";
import { Row, Col, Button, Table, FormGroup, Label, Input, Modal, ModalHeader, ModalBody, ModalFooter } from "reactstrap";
import Form from "react-validation/build/form";
import ModalConfirm from "../../../components/modal/modal.confirm";
import ModalInfo from "../../../components/modal/modal.info";
import MultipleSelect from "../../../components/common/multiple.select";
import Loading from "../../../components/common/loading.indicator";
import { toastSuccess, toastError } from "../../../helpers/toast.helper";
import lodash, { cloneDeep, isEqual } from "lodash";
import { getInterviewList } from "../../../actions/interview.list.action";
import Api from "../../../api/api";
import ApiInterview from "../../../api/api.interview"
import { pagination } from "../../../constant/app.constant";
import Pagination from "../../../components/pagination/Pagination";
import './interview.list.page';
import moment from 'moment';
import Datetime from 'react-datetime';

// Mock data
import SelectInput from "../../../components/common/select.input"
import RequestHelper from '../../../helpers/request.helper';
import { appConfig } from "../../../config/app.config";

class InterviewListPage extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isLoading: false,
            isShowDeleteModal: false,
            isShowInfoModal: false,
            isShowStatusModal: false,
            item: {},
            itemId: null,
            params: {
                skip: pagination.initialPage,
                take: pagination.defaultTake
            },
            query: "",
            interviewerList: [],
            candidateName: [],
            cloneItem: {},
            isEdit: false,
            isCreate: true,
            isDesc: false,
            sortName: "status",
            activeSort: "status"
        };
        this.delayedCallback = lodash.debounce(this.search, 1000);
    }

    handleTimeChange(date) {
        this.setState({
            startDate: date
        });
    }

    toggleDeleteModal = () => {
        this.setState(prevState => ({
            isShowDeleteModal: !prevState.isShowDeleteModal
        }));
    };

    toggleStatusupdateModal = (item) => {
        this.setState(prevState => ({
            isShowStatusModal: !prevState.isShowStatusModal,
            item: item || {},
        }));
    }

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
        let title = "Create Interview";
        let interview = {
            dateStart: new Date(),
            dateEnd: new Date()
        };
        this.setState(
            {
                isCreate: true
            }
        );
        this.getCandidateList();
        this.toggleModalInfo(interview, title);
    };

    showUpdateModal = async item => {
        let title = "Update Interview";

        let convertItem = Object.assign({}, item);

        convertItem["candidateId"] = convertItem.candidate.id;

        let cloneItem = cloneDeep(convertItem);

        await this.setState({ cloneItem: cloneItem, isEdit: false, isCreate: false, item: convertItem });

        this.getCandidateList();
        this.toggleModalInfo(this.state.item, title);
    };

    showUpdateStatusModal = (item) => {
        this.toggleStatusupdateModal(item);
    };

    handleSelectTime = (event, field) => {
        let item = Object.assign({}, this.state.item);
        item[field] = event._d;
        this.setState({ item, isEdit: !isEqual(this.state.cloneItem, item) });
    };

    handleMultipleChange = (event, field) => {
        const item = Object.assign({}, this.state.item);
        item[field] = event;
        this.setState({
            item,
            isEdit: !isEqual(this.state.cloneItem, item)
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
            () => {
                this.getInterviewList();
            }
        );
    };

    onSearchChange = e => {
        e.persist();
        this.delayedCallback(e);
    };

    handlePageClick = e => {
        const { isDesc, sortName } = this.state;
        this.setState(
            {
                params: {
                    ...this.state.params,
                    skip: e.selected + 1
                }
            },
            () => this.getInterviewList(isDesc, sortName)
        );
    };

    getInterviewList = (isDesc = false, sortName = "status") => {
        let params = Object.assign({}, this.state.params, {
            query: this.state.query,
            isDesc: isDesc,
            sortName: sortName
        });
        this.props.getInterviewList(params);
    };

    handleSort = (sortName) => {
        const { isDesc } = this.state;
        this.setState({
            isDesc: !isDesc,
            sortName: sortName,
            activeSort: sortName
        },
            () => this.getInterviewList(this.state.isDesc, this.state.sortName)
        );
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

    onModelChange = el => {
        let inputName = el.target.name;
        let inputValue = {};
        if (inputName === "attachment") {
            inputValue = el.target.files[0]
        }
        else {
            inputValue = el.target.value;
        }
        let item = { ...this.state.item }
        item[inputName] = inputValue;
        this.setState({ item, isEdit: !isEqual(this.state.cloneItem, item) });
    };

    generateQuestion = async (types) => {
        var result;
        try {
            const query = {
                level: this.state.CandidateList
                    .filter(x => x.id == this.state.item.candidateId)
                    .map(candidate => candidate.level)
                    .shift() ||
                    this.state.item.candidate.level,
                types: types
            }

            if (query.level > 3) {
                return
            }

            result = await Api.generatePDF(query)
            var length = types.length;
            var temp = [];
            for (let i = 0; i < length; i++) {
                var fullName = result[i].split('/').slice(-1)[0];
                var dotIndex = fullName.lastIndexOf(".");
                var extension = fullName.substring(dotIndex, fullName.length);
                var fileName = fullName.substring(0, dotIndex);
                temp.push({
                    link: result[i],
                    extension: extension,
                    fileName: fileName
                })
            }
            return temp;
        } catch (err) {
            toastError(err)
        }
    }

    uploadFile = async (attachments) => {
        var result;
        try {
            if (!attachments) {
                result = [];
            }
            else {
                if (attachments.length > 1) {
                    result = await ApiInterview.uploadFiles("attachment", attachments);
                } else {
                    result = await ApiInterview.uploadFile("attachment", attachments[0]);
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
            toastError(err)
        }
    }

    addInterview = async () => {
        let { dateStart, dateEnd, description, candidateId, interviewers, attachments, types } = this.state.item;
        if (!dateStart || !dateEnd || !interviewers || !candidateId || !attachments) {
            toastError("Please input all field");
            return;
        }
        else {
            const interviewerIds = interviewers.map(item => item.id);
            this.setState({ isLoading: true });
            if (types && types.length > 0) {
                const typesId = types.map(item => item.id);
                types = await this.generateQuestion(typesId);
            }
            try {
                attachments = await this.uploadFile(attachments)
                if (types != null) {
                    attachments = attachments.concat(types)
                }
                const interview = { dateStart, dateEnd, interviewerIds, description, candidateId, attachments }
                await ApiInterview.addInterview(interview)
                this.toggleModalInfo();
                this.getInterviewList();
                this.setState({ isLoading: false });
                toastSuccess("The interview has been created successfully");
            } catch (err) {
                this.setState({ isLoading: false });
                toastError(err);
            }
        }
    };

    updateInterview = async () => {
        let { dateStart, dateEnd, description, candidateId, interviewers, id, attachments, types } = this.state.item;
        console.log(this.state.item);
        const interviewerIds = interviewers.map(item => item.id);

        if (attachments && attachments.length > 0 && !attachments[0].link) {
            attachments = await this.uploadFile(attachments);
        }

        if (types && types.length > 0) {
            const typesId = types.map(item => item.id);
            attachments = attachments.filter(x => x.extension != ".pdf");
            types = await this.generateQuestion(typesId);
        }

        try {
            if (types != null) {
                attachments = attachments.concat(types);
            }
            const interview = { dateStart, dateEnd, interviewerIds, description, candidateId, attachments, id };
            await ApiInterview.updateInterview(interview);
            this.toggleModalInfo();
            this.getInterviewList();
            toastSuccess("The interview has been updated successfully");
        } catch (err) {
            toastError(err);
        }
    };

    deleteInterview = async () => {
        try {
            await ApiInterview.deleteInterview(this.state.itemId);
            if (this.props.interviewList.interviewList.sources.length === 1) {
                this.setState({
                    params: { ...this.state.params, skip: this.state.params.skip - 1 }
                })
            }
            this.getInterviewList();
            this.toggleDeleteModal();
            toastSuccess("The interview has been deleted successfully");
        } catch (err) {
            toastError(err);
        }
    };

    updateStatus = async () => {
        const { status, id } = this.state.item;
        const interview = { status, id };
        try {
            await ApiInterview.updateStatus(interview);
            this.toggleStatusupdateModal();
            this.getInterviewList();
        } catch (err) {
            toastError(err);
        }
    }

    saveInterview = () => {
        let { id } = this.state.item;
        if (id) {
            this.updateInterview();
        } else {
            this.addInterview();
        }
    };

    onSubmit(e) {
        e.preventDefault();
        this.form.validateAll();
        this.saveInterview();
    }

    componentDidMount() {
        this.getInterviewList();
        this.getInterviewerList();
    }

    getInterviewerList() {
        let params = Object.assign({}, this.state.params, {
            query: this.state.query
        });
        RequestHelper.get(appConfig.apiUrl + "users/interviewers", params).then(result => {
            this.setState({
                interviewerList: result,
            })
        });
    }

    getCandidateList() {
        let params = Object.assign({}, this.state.params, {
            query: this.state.query
        });
        RequestHelper.get(appConfig.apiUrl + "candidates/all-candidates", params).then(result => {
            this.setState({
                CandidateList: result,
            })
        });
    }

    removeFile = () => {
        this.setState({
            item: {
                ...this.state.item,
                attachments: [],
            },
            isEdit: true
        })
    }

    render() {
        const { interviewList } = this.props.interviewList;
        const { sources } = interviewList;
        const hasResults = interviewList.sources && interviewList.sources.length > 0;
        const { isShowDeleteModal, isShowInfoModal, item, CandidateList } = this.state;
        const { pageIndex, totalPages } = interviewList;
        const { isLoading } = this.state;

        return (
            <div>
                {isLoading && <Loading />}
                <ModalConfirm
                    clickOk={this.deleteInterview}
                    isShowModal={isShowDeleteModal}
                    toggleModal={this.toggleDeleteModal} />

                <Modal isOpen={this.state.isShowStatusModal} toggle={this.toggleStatusupdateModal} >
                    <ModalHeader>
                        Update Status
                    </ModalHeader >
                    <ModalBody>
                        <select className="form-control" defaultValue={this.state.status} name='status' onChange={this.onModelChange}>
                            <option > Please Choose Your Status... </option>
                            <option value="2"> Waiting </option>
                            <option value="3"> Passed </option>
                            <option value="4"> Failed </option>
                        </select>
                    </ModalBody>
                    <ModalFooter>
                        <Button className=" btn-primary" onClick={this.updateStatus} > Confirm </Button>{" "}
                        <Button className="btn-danger" onClick={this.toggleStatusupdateModal}> Cancel </Button>
                    </ModalFooter>
                </Modal>

                <ModalInfo
                    size="lg"
                    title={this.state.formTitle}
                    isShowModal={isShowInfoModal}
                    hiddenFooter>

                    <div className="modal-wrapper">
                        <div className="form-wrapper">
                            <Form onSubmit={e => this.onSubmit(e)} ref={c => { this.form = c; }}>

                                {/* Candidate */}
                                <FormGroup>
                                    <SelectInput
                                        options={CandidateList || []}
                                        title="Candidate: "
                                        name="candidateId"
                                        required={true}
                                        placeholder={!this.state.isCreate && item.candidate ? item.candidate.name : "Choose candidate"}
                                        disabled={!this.state.isCreate}
                                        value={item ? item.candidateId : ''}
                                        onChange={this.onModelChange} >
                                    </SelectInput>
                                </FormGroup>

                                {/* Start Day */}
                                <Row>
                                    <Col xs="3" sm="3" md="3" lg="3">
                                        <FormGroup>
                                            <Label for="examplePassword"> <strong>Date Start: </strong></Label>
                                        </FormGroup>
                                    </Col>
                                    <Col xs="9" sm="9" md="9" lg="9">
                                        <FormGroup>
                                            <Datetime
                                                onChange={(e) => this.handleSelectTime(e, "dateStart")}
                                                defaultValue={moment(item.dateStart).format('DD/MM/YYYY - HH:mm A')}
                                                input={true}
                                                dateFormat='DD/MM/YYYY -'
                                                timeFormat='HH:mm A'
                                                isValidDate={(current) => {
                                                    if (item.dateEnd) {
                                                        return current.isBetween(item.dateStart, moment(item.dateEnd).add(1, 'day'))
                                                    } else {
                                                        var yesterday = Datetime.moment().subtract(1, 'day');
                                                        return current.isAfter(yesterday);
                                                    }
                                                }}
                                            />
                                        </FormGroup>
                                    </Col>
                                </Row>

                                {/* End Day */}
                                <Row>
                                    <Col xs="3" sm="3" md="3" lg="3">
                                        <FormGroup>
                                            <Label for="examplePassword"><strong>Date End: </strong> </Label>
                                        </FormGroup>
                                    </Col>
                                    <Col xs="9" sm="9" md="9" lg="9">
                                        <FormGroup>
                                            <Datetime
                                                required={true}
                                                onChange={(e) => this.handleSelectTime(e, "dateEnd")}
                                                defaultValue={moment(item.dateEnd).format('DD/MM/YYYY - HH:mm A')}
                                                dateFormat='DD/MM/YYYY -'
                                                timeFormat='HH:mm A'
                                                isValidDate={(current) => {
                                                    return current.isAfter(moment(item.dateStart).subtract(1, 'day'))
                                                }} />
                                        </FormGroup>
                                    </Col>
                                </Row>

                                {/* MultipleSelect */}
                                <Row>
                                    <Col>
                                        <FormGroup>
                                            <MultipleSelect
                                                title="Interviewers: "
                                                type="select"
                                                name="interviewers"
                                                options={this.state.interviewerList || []}
                                                labelField="name"
                                                placeholder="Notthing Select"
                                                valueField="id"
                                                // placeholder=""
                                                value={item ? item.interviewers : ''}
                                                onChange={(e) => { this.handleMultipleChange(e, "interviewers") }} />
                                        </FormGroup>
                                    </Col>

                                    <Col xs="6" sm="6" md="6" lg="6">
                                        <FormGroup>
                                            <MultipleSelect
                                                title="Interview Topic: "
                                                type="select"
                                                name="type"
                                                options={[
                                                    {
                                                        name: 'Backend',
                                                        id: 1
                                                    },
                                                    {
                                                        name: 'FrontEnd',
                                                        id: 2
                                                    },
                                                    {
                                                        name: 'Design',
                                                        id: 3
                                                    },
                                                    {
                                                        name: 'Testing',
                                                        id: 4
                                                    }
                                                ]}
                                                labelField="name"
                                                placeholder="Notthing Select"
                                                valueField="id"
                                                onChange={(e) => { this.handleMultipleChange(e, "types") }} />
                                        </FormGroup>
                                    </Col>
                                </Row>

                                {/* Calendar Description */}
                                <FormGroup>
                                    <Label for="exampleEmail"><strong>Calendar Description: </strong></Label>
                                    <Input
                                        type="textarea"
                                        name="description"
                                        value={item && item.description ? item.description : ""}
                                        placeholder="Input text"
                                        onChange={this.onModelChange} />
                                </FormGroup>

                                {/* Attachment */}
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
                                            accept=".doc, .docx, .pdf, .png, .jpg, .jpeg"
                                            multiple
                                            onChange={this.fileOnChange} />
                                    }

                                </FormGroup>

                                <div className="text-center ">
                                    <Button className=" btn-primary" type="submit" disabled={!this.state.isEdit}> Confirm </Button>{" "}
                                    <Button className="btn-danger" onClick={this.toggleModalInfo}> Cancel </Button>
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
                                className="btn btn-success btn-sm">Create</Button>
                            <Button
                                href={appConfig.apiUrl + "export/interviews"}
                                className="btn btn-primary btn-sm">Export Excel</Button>
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
                                <th> STT </th>

                                <th> Day Start </th>

                                <th> Day End </th>

                                <th >Interviewer</th>

                                <th className="sort-column" onClick={() => this.handleSort("candidate")}>Candidate <i className={`${this.state.activeSort === "candidate" ? "active-sort" : "disactive-sort"} ${!this.state.isDesc ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}></i> </th>

                                <th>Attachment</th>

                                <th>Description</th>

                                <th className="custom_action">Action</th>

                                <th className="sort-column" onClick={() => this.handleSort("status")}> Status <i className={`${this.state.activeSort === "status" ? "active-sort" : "disactive-sort"} ${!this.state.isDesc ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}></i> </th>
                            </tr>
                        </thead>

                        <tbody >
                            {hasResults && sources.map((item, i) => {
                                return (
                                    <tr className="table-row" key={i}>
                                        <td > {interviewList.pageIndex !== 0 ? interviewList.pageIndex * interviewList.pageSize - interviewList.pageSize + ++i : ++i}</td>
                                        <td> {moment(item.dateStart).format('HH:mm - DD/MM/YYYY')} </td>
                                        <td> {moment(item.dateEnd).format('HH:mm - DD/MM/YYYY')} </td>
                                        <td>{item.interviewers.map(e => e.name).join(', ')}</td>
                                        <td>{item.candidate.name}</td>
                                        <td>
                                            {item.attachments &&
                                                item.attachments.map((attachment, idx) =>
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
                                        <td className="custom_description" > {item.description} </td>
                                        <td>
                                            <div>
                                                {/* TO DO */}
                                                {/* <Button
                                                    className="btn btn-primary fa fa-pencil"
                                                    disabled={item.status === 1 ? false : true}
                                                    onClick={() => this.showUpdateModal(item)} /> */}
                                                <Button className="btn btn-danger fa fa-trash" onClick={() => this.showConfirmDelete(item.id)} />
                                            </div>
                                        </td>
                                        <td><Button
                                            className="status_button"
                                            color={item.status === 2 ? "warning" : item.status === 3 ? "success" : item.status === 4 ? "secondary" : "info"}
                                            disabled={item.status === 3 || item.status === 4}
                                            onClick={() => this.showUpdateStatusModal(item)}>
                                            {item.status === 2 ? "Waiting" : item.status === 3 ? "Passed" : item.status === 4 ? "Failed" : "Pending"}
                                        </Button>
                                        </td>
                                    </tr>
                                );
                            })}
                        </tbody>

                    </Table>

                    {/* Pagination */}
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

export default connect(
    state => ({
        interviewList: state.interviewList
    }),
    {
        getInterviewList
    }
)(InterviewListPage);
