/* eslint-disable jsx-a11y/anchor-has-content */
import React, { Component } from "react";
import { connect } from "react-redux";
import { Row, Col, Button, Table, Label, Input, FormGroup } from "reactstrap";
import Form from "react-validation/build/form";

import SelectInput from "../../../components/common/select.input";
import ModalConfirm from "../../../components/modal/modal.confirm";
import ModalUpload from "../../../components/modal/modal.upload";
import Pagination from "../../../components/pagination/Pagination";
import ModalInfo from "../../../components/modal/modal.info";
import { toastSuccess, toastError } from "../../../helpers/toast.helper";
import lodash from "lodash";
import { getCandidateList } from "../../../actions/candidate.list.action";
import { getAllTechnicalSkillList } from "../../../actions/technical.skill.list.action";
import Api from "../../../api/api.candidate";
import { pagination } from "../../../constant/app.constant";
import moment from "moment";
import Datetime from 'react-datetime';
import MultipleSelect from "../../../components/common/multiple.select";
import { appConfig } from "../../../config/app.config";

class CandidateListPage extends Component {
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
            query: "",
            sortName: "name",
            isShowUploadModal: false,
            isDesc: false,
            activeSort: 'name',
            field: '',
            skills: []
        };
        this.delayedCallback = lodash.debounce(this.search, 1000);
    }

    toggleDeleteModal = () => {
        this.setState(prevState => ({
            isShowDeleteModal: !prevState.isShowDeleteModal
        }));
    };

    toggleModalInfo = (item, title, action) => {
        this.setState(prevState => ({
            isShowInfoModal: !prevState.isShowInfoModal,
            item: item || {},
            formTitle: title,
            actionAdd: action
        }));
    };

    toggleModalUpload = (item, title) => {
        this.setState(prevState => ({
            isShowUploadModal: !prevState.isShowUploadModal,
            item: item || {},
            formTitle: title
        }));
    }

    showConfirmDelete = itemId => {
        this.setState(
            {
                itemId: itemId
            },
            () => this.toggleDeleteModal()
        );
    };

    showAddNew = () => {
        let title = "Create Candidate";
        let candidate = {
            gender: 1,
            level: 1,
            dateOfBirth: moment().format("Y-MM-DD")
        };
        this.toggleModalInfo(candidate, title, true);
    };

    showUpdateModal = item => {
        let title = "Update Candidate";
        this.toggleModalInfo(item, title, false);
    };

    onModelChange = (el) => {
        let inputName = el.target.name;
        let inputValue = {};
        if (inputName === "avatarFile") {
            inputValue = el.target.files[0];
        } else {
            inputValue = el.target.value;
        }
        let item = { ...this.state.item }
        item[inputName] = inputValue;
        this.setState({ item });
    };

    handleSelectTime = (event, field) => {
        let item = Object.assign({}, this.state.item);
        item[field] = event._d;
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
                this.getCandidateList();
            }
        );
    };

    onSearchChange = e => {
        e.persist();
        this.delayedCallback(e);
    };

    handlePageClick = e => {
        let sortName = this.state.sortName;
        this.setState(
            {
                params: {
                    ...this.state.params,
                    skip: e.selected + 1,
                    sortName: sortName,
                    isDesc: this.state[sortName]
                }
            },
            () => this.getCandidateList()
        );
    };

    getCandidateList = () => {
        let sortName = this.state.sortName;
        let params = Object.assign({}, this.state.params, {
            query: this.state.query,
            sortName: sortName,
            isDesc: this.state.isDesc
        });
        this.props.getCandidateList(params);
        this.props.getAllTechnicalSkillList();
    };

    addCandidate = async () => {
        const { name, dateOfBirth, email, gender, about, address, mobile, facebook, twitter, linkedIn, level, yearOfExperienced, source, technicalSkillIds } = this.state.item;
        if (!this.state.item.avatarFile) {
            toastError("Please input avatar");
            return;
        }
        const avatarUrl = await Api.uploadAvatar("avatar", this.state.item.avatarFile);
        const candidate = { name, dateOfBirth, avatarUrl, email, gender, about, address, mobile, facebook, twitter, linkedIn, level, yearOfExperienced, source, technicalSkillIds };
        if (!name || !dateOfBirth || !email || !gender || !about || !address || !mobile) {
            toastError("Please input all field");
            return;
        }
        try {
            await Api.addCandidate(candidate);
            this.toggleModalInfo();
            this.getCandidateList();
            toastSuccess("The Candidate has been created successfully");
        } catch (err) {
            toastError(err);
        }
    };

    updateCandidate = async () => {
        let { id, name, avatarUrl, dateOfBirth, email, gender, about, address, mobile, facebook, twitter, linkedIn, level, yearOfExperienced, technicalSkillIds } = this.state.item;
        if (!name || !dateOfBirth || !email || !gender || !about || !address || !mobile) {
            toastError("Please input all field");
            return;
        }
        if (technicalSkillIds == null) {
            let listTechnicalSkillId = [];
            this.state.item.technicalSkills.forEach(item => {
                listTechnicalSkillId.push(item.id);
            });
            technicalSkillIds = listTechnicalSkillId;
        }
        if (this.state.item.avatarFile) {
            avatarUrl = await Api.uploadAvatar("avatar", this.state.item.avatarFile);
        }
        const candidate = { id, name, dateOfBirth, avatarUrl, email, gender, about, address, mobile, facebook, twitter, linkedIn, level, yearOfExperienced, technicalSkillIds };

        try {
            await Api.updateCandidate(candidate);
            this.toggleModalInfo();
            this.getCandidateList();
            toastSuccess("The job Candidate has been updated successfully");
        } catch (err) {
            toastError(err);
        }
    };

    onRadioBtnClick = (gender) => {
        let item = { ...this.state.item, gender: gender };
        this.setState({ item, gender })
    }


    deleteCandidate = async () => {
        try {
            await Api.deleteCandidate(this.state.itemId);
            this.toggleDeleteModal();
            if (this.props.candidateList.candidateList.sources.length === 1) {
                this.setState({
                    params: { ...this.state.params, skip: this.state.params.skip - 1 }
                });
            }
            this.getCandidateList();
            toastSuccess("The job Candidate has been deleted successfully");
        } catch (err) {
            toastError(err);
        }
    };

    saveCandidate = () => {
        let { id } = this.state.item;
        if (id) {
            this.updateCandidate();
        } else {
            this.addCandidate();
        }
    };

    onSubmit(e) {
        e.preventDefault();
        this.form.validateAll();
        this.saveCandidate();
    }

    removeFile = () => {
        this.setState({
            item: {
                ...this.state.item,
                avatarUrl: ''
            },
        })
    }

    componentDidMount() {
        this.getCandidateList();
    }

    showChangeAvatar = (item) => {
        this.toggleModalUpload(item, "Change Avatar")
    }

    toggleSort = async (field, name) => {
        if (this.state.sortName === name) {
            await this.setState({
                activeSort: name,
                sortName: name,
                isDesc: !this.state.isDesc,
                params: { ...this.state.params, skip: 1 }
            });
        }
        else {
            await this.setState({
                activeSort: name,
                sortName: name,
                isDesc: false,
                params: { ...this.state.params, skip: 1 }
            });
        }

        this.getCandidateList()
    }

    handleChangeTechnicalSkill = (event) => {
        let listTechnicalSkillId = [];
        let listTechnicalSkill = []
        event.forEach(item => {
            listTechnicalSkillId.push(item.id);
            listTechnicalSkill.push(item)
        });
        const item = { ...this.state.item, technicalSkillIds: listTechnicalSkillId, technicalSkills: listTechnicalSkill }
        this.setState({ item });
    }

    render() {
        const { isShowDeleteModal, isShowInfoModal, item, isShowUploadModal, formTitle } = this.state;
        const { candidateList } = this.props.candidateList;
        const { sources, pageIndex, totalPages } = candidateList;
        const hasResults = candidateList.sources && candidateList.sources.length > 0;
        return (
            <div className="animated fadeIn">
                <ModalConfirm
                    clickOk={this.deleteCandidate}
                    isShowModal={isShowDeleteModal}
                    toggleModal={this.toggleDeleteModal}
                />

                <ModalUpload
                    title={formTitle}
                    isShowModal={isShowUploadModal}
                    item={item}
                    toggleModalUpload={this.toggleModalUpload}
                    getCandidateList={this.getCandidateList}
                />

                <ModalInfo
                    size="lg"
                    title={this.state.formTitle}
                    isShowModal={isShowInfoModal}
                    toggleModal={this.toggleModalInfo}
                    hiddenFooter
                >
                    <div className="modal-wrapper">
                        <div className="form-wrapper">
                            <Form
                                onSubmit={e => this.onSubmit(e)}
                                ref={c => { this.form = c }}>

                                <Row>
                                    <Col lg="6" md="12">
                                        {/* Name */}
                                        <FormGroup>
                                            <Label for="exampleEmail"><strong>Name: </strong></Label>
                                            <Input
                                                type="text"
                                                name="name"
                                                required={true}
                                                value={item.name || ''}
                                                placeholder="Input your name"
                                                onChange={this.onModelChange}
                                            />
                                        </FormGroup>

                                        {/* dateOfBirth */}
                                        <Row>
                                            <Col xs="4" sm="4" md="4" lg="4">
                                                <FormGroup>
                                                    <Label for="examplePassword"> <strong>Date of Birth: </strong></Label>
                                                </FormGroup>
                                            </Col>
                                            <Col xs="8" sm="8" md="8" lg="8">
                                                <FormGroup>
                                                    <Datetime
                                                        onChange={(e) => this.handleSelectTime(e, "dateOfBirth")}
                                                        defaultValue={moment(item.dateOfBirth).format('DD/MM/YYYY')}
                                                        input={true}
                                                        dateFormat='DD/MM/YYYY'
                                                        closeOnSelect={true}
                                                        timeFormat={false}
                                                        isValidDate={(current) => {
                                                            if (item.dateOfBirth) {
                                                                return current.isBefore(moment())
                                                            }
                                                        }}
                                                    />
                                                </FormGroup>
                                            </Col>
                                        </Row>

                                        {/* Email */}
                                        <FormGroup>
                                            <Label for="exampleEmail"><strong>Email: </strong></Label>
                                            <Input
                                                type="text"
                                                name="email"
                                                value={item.email || ''}
                                                placeholder="Input your email"
                                                onChange={this.onModelChange} />
                                        </FormGroup>

                                        {/* Mobile */}
                                        <FormGroup>
                                            <Label for="exampleMobile"><strong>Mobile: </strong></Label>
                                            <Input
                                                type="text"
                                                name="mobile"
                                                value={item.mobile || ''}
                                                placeholder="Input your phone number"
                                                onChange={this.onModelChange} />
                                        </FormGroup>

                                        {/* Gender */}
                                        <FormGroup >
                                            <SelectInput
                                                name="gender"
                                                title="Gender: "
                                                nameField="nameField"
                                                valueField="valueField"
                                                onChange={this.onModelChange}
                                                value={item.gender}
                                                options={[
                                                    {
                                                        name: 'Male',
                                                        id: 1
                                                    },
                                                    {
                                                        name: 'Female',
                                                        id: 2
                                                    },
                                                    {
                                                        name: 'None',
                                                        id: 3
                                                    }
                                                ]}
                                            />
                                        </FormGroup>

                                        {/* avatar */}
                                        <FormGroup>
                                            <Label ><strong>Avatar: </strong></Label>
                                            {item.avatarUrl ?
                                                <Row>
                                                    <FormGroup>
                                                        <img src={item.avatarUrl} alt="avatar" style={{ width: "180px", marginLeft: "100px" }} ></img>
                                                    </FormGroup>
                                                    <Col lg='10'>
                                                        <Input
                                                            name="avatarFile"
                                                            title="Avatar: "
                                                            placeholder={item.avatarUrl}
                                                            disabled={true}
                                                            type="text"
                                                        />
                                                    </Col>

                                                    <Col lg='2'>
                                                        <i className="fa fa-times-circle fa-2x" onClick={this.removeFile} aria-hidden="true"></i>
                                                    </Col>
                                                </Row>

                                                : <Input
                                                    name="avatarFile"
                                                    title="Avatar:"
                                                    type="file"
                                                    accept="image/x-png,image/gif,image/jpeg"
                                                    onChange={this.onModelChange} />
                                            }
                                        </FormGroup>

                                        {/* Level */}
                                        <FormGroup>
                                            <SelectInput
                                                name="level"
                                                title="Level: "
                                                nameField="nameField"
                                                valueField="valueField"
                                                onChange={this.onModelChange}
                                                value={item.level}
                                                options={[
                                                    {
                                                        name: 'Internship',
                                                        id: 1
                                                    },
                                                    {
                                                        name: 'Fresher',
                                                        id: 2
                                                    },
                                                    {
                                                        name: 'Junior',
                                                        id: 3
                                                    },
                                                    {
                                                        name: 'Senior',
                                                        id: 4
                                                    },
                                                    {
                                                        name: 'PM',
                                                        id: 5
                                                    },
                                                    {
                                                        name: 'Other',
                                                        id: 6
                                                    }
                                                ]} />
                                        </FormGroup>

                                        {/* Year of experienced */}
                                        <FormGroup>
                                            <Label for="exampleMobile"><strong>Year of Experienced: </strong></Label>
                                            <Input
                                                type="number"
                                                name="yearOfExperienced"
                                                defaultValue={item.yearOfExperienced}
                                                placeholder="Input your experienced"
                                                onChange={this.onModelChange} />
                                        </FormGroup>
                                    </Col>

                                    <Col lg="6" md="12">
                                        <FormGroup>
                                            <MultipleSelect
                                                title="Technical Skill"
                                                type="select"
                                                name="technicalSkills"
                                                placeholder="Multiple Choose"
                                                options={this.props.technicalSkills.technicalSkillList || []}
                                                labelField="name"
                                                valueField="id"
                                                value={item.technicalSkills}
                                                onChange={this.handleChangeTechnicalSkill}
                                            />
                                        </FormGroup>
                                        {/* About */}
                                        <FormGroup>
                                            <Label for="exampleEmail"><strong>About: </strong></Label>
                                            <Input
                                                name="about"
                                                required={true}
                                                type="textarea"
                                                value={item.about || ''}
                                                onChange={this.onModelChange} />
                                        </FormGroup>

                                        {/* Address */}
                                        <FormGroup>
                                            <Label for="exampleEmail"><strong>Address: </strong></Label>
                                            <Input
                                                name="address"
                                                required={true}
                                                type="textarea"
                                                value={item.address || ''}
                                                onChange={this.onModelChange} />
                                        </FormGroup>

                                        {/* Facebook */}
                                        <FormGroup>
                                            <Label for="exampleEmail"><strong>Facebook: </strong></Label>
                                            <Input
                                                name="facebook"
                                                type="textarea"
                                                value={item.facebook || ''}
                                                onChange={this.onModelChange} />
                                        </FormGroup>

                                        {/* Twitter */}
                                        <FormGroup>
                                            <Label for="exampleEmail"><strong>Twitter: </strong></Label>
                                            <Input
                                                name="twitter"
                                                type="textarea"
                                                value={item.twitter || ''}
                                                onChange={this.onModelChange} />
                                        </FormGroup>

                                        {/* LinkedIn */}
                                        <FormGroup>
                                            <Label for="exampleEmail"><strong>LinkedIn: </strong></Label>
                                            <Input
                                                name="linkedIn"
                                                type="textarea"
                                                value={item.linkedIn || ''}
                                                onChange={this.onModelChange} />
                                        </FormGroup>

                                        {/* Source */}
                                        <FormGroup>
                                            <Label for="exampleEmail"><strong>Source: </strong></Label>
                                            <Input
                                                name="source"
                                                type="textarea"
                                                value={item.source || ''}
                                                onChange={this.onModelChange} />
                                        </FormGroup>
                                    </Col>
                                </Row>

                                <div className="text-center">
                                    <Button className=" btn-primary" type="submit"> Confirm </Button>{" "}
                                    <Button className="btn-danger" onClick={this.toggleModalInfo}> Cancel </Button>
                                </div>
                            </Form>
                        </div>
                    </div>
                </ModalInfo>

                <div className="animated fadeIn">
                    <Row className="flex-container header-table">
                        <Col xs="5" sm="5" md="5" lg="5"  >
                            <Button
                                onClick={this.showAddNew}
                                className="btn btn-success btn-sm" > Create </Button>
                            <Button
                                href={appConfig.apiUrl + "export/candidates"}
                                className="btn btn-primary btn-sm">Export Excel</Button>
                        </Col>
                        <Col xs="5" sm="5" md="5" lg="5"  >
                            <input
                                onChange={this.onSearchChange}
                                className="form-control form-control-sm custom_search"
                                placeholder="Searching" />
                        </Col>
                    </Row>

                    <Table responsive bordered className="react-bs-table react-bs-table-bordered data-table">
                        <thead>
                            <tr className="table-header">
                                <th>STT</th>
                                <th onClick={() => this.toggleSort("isDescSortName", "name")}> Name <i className={`${this.state.activeSort === 'name' ? "active-sort" : "disactive-sort"} ${!this.state.isDesc ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}></i></th>

                                <th onClick={() => this.toggleSort("isDescSortAge", "age")}> Age <i className={`${this.state.activeSort === 'age' ? "active-sort" : "disactive-sort"} ${!this.state.isDesc ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}></i></th>

                                <th onClick={() => this.toggleSort("isDescSortEmail", "email")}> Email <i className={`${this.state.activeSort === 'email' ? "active-sort" : "disactive-sort"} ${!this.state.isDesc ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}></i></th>

                                <th >Technical Skill</th>

                                <th onClick={() => this.toggleSort("isDescSortLevel", "level")}> Level <i className={`${this.state.activeSort === 'level' ? "active-sort" : "disactive-sort"} ${!this.state.isDesc ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}></i></th>

                                <th onClick={() => this.toggleSort("isDescSortYearOfExperienced", "yearOfExperienced")}> Year Of Experience <i className={`${this.state.activeSort === 'yearOfExperienced' ? "active-sort" : "disactive-sort"} ${!this.state.isDesc ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}></i></th>

                                <th >Avatar</th>

                                <th onClick={() => this.toggleSort("isDescSortMobile", "mobile")}> Phone <i className={`${this.state.activeSort === 'mobile' ? "active-sort" : "disactive-sort"} ${!this.state.isDesc ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}></i></th>

                                <th onClick={() => this.toggleSort("isDescSortGender", "gender")}> Gender <i className={`${this.state.activeSort === 'gender' ? "active-sort" : "disactive-sort"} ${!this.state.isDesc ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}></i></th>

                                <th onClick={() => this.toggleSort("isDescSortAbout", "about")}> About <i className={`${this.state.activeSort === 'about' ? "active-sort" : "disactive-sort"} ${!this.state.isDesc ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}></i></th>

                                <th onClick={() => this.toggleSort("isDescSortAddress", "address")}> Address <i className={`${this.state.activeSort === 'address' ? "active-sort" : "disactive-sort"} ${!this.state.isDesc ? "fa fa-caret-down fa-lg" : "fa fa-caret-up fa-lg"}`}></i></th>

                                <th > Social </th>

                                <th className="custom_action">Action</th>
                            </tr>
                        </thead>
                        <tbody>
                            {hasResults &&
                                sources.map((item, i) => {
                                    return (
                                        <tr className="table-row" key={i} >
                                            <td > {candidateList.pageIndex !== 0 ? candidateList.pageIndex * candidateList.pageSize - candidateList.pageSize + ++i : ++i}</td>
                                            <td>{item.name}</td>
                                            <td>{item.age}</td>
                                            <td>{item.email}</td>
                                            <td>{item.technicalSkills.map((technicalSkill, key) => {
                                                return (
                                                    <p key={key}>{technicalSkill.name}</p>
                                                )
                                            })}</td>
                                            <td>{item.level}</td>
                                            <td>{item.yearOfExperienced}</td>
                                            <td><img onClick={() => this.showChangeAvatar(item)} className="custom_avatar" src={item.avatarUrl} alt="avatar" /></td>
                                            <td>{item.mobile}</td>
                                            <td>{item.gender === 1 ? "Male" : item.gender === 2 ? "Female" : "None"}</td>
                                            <td className="custom_col_about">{item.about}</td>
                                            <td className="custom_col_address">{item.address}</td>
                                            <td> {item.facebook ? <a href={item.facebook.includes("https://") ? item.facebook : `https://${item.facebook}`} target="_blank" rel="noopener noreferrer" ><i className="fa fa-facebook-square fa-2x" aria-hidden="true" /></a> : ''}  {item.twitter ? <a href={item.twitter.includes("https://") ? item.twitter : `https://${item.twitter}`} target="_blank" rel="noopener noreferrer" ><i className="fa fa-twitter-square fa-2x" aria-hidden="true" /></a> : ''}  {item.linkedIn ? <a href={item.linkedIn.includes("https://") ? item.linkedIn : `https://${item.linkedIn}`} target="_blank" rel="noopener noreferrer" ><i className="fa fa-linkedin-square fa-2x" aria-hidden="true" /></a> : ''}</td>
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
            </div >
        );
    }
}


export default connect(
    state => ({
        candidateList: state.candidateList,
        technicalSkills: state.technicalSkillList
    }),
    {
        getCandidateList, getAllTechnicalSkillList
    }
)(CandidateListPage);
