export const GET_TECHNICALSKILL_LIST = "[TECHNICALSKILL_LIST] GET_TECHNICALSKILL_LIST";
export const GET_TECHNICALSKILL_LIST_SUCCESS = "[TECHNICALSKILL_LIST] GET_TECHNICALSKILL_LIST_SUCCESS";
export const GET_TECHNICALSKILL_LIST_FAILED = "[TECHNICALSKILL_LIST] GET_TECHNICALSKILL_LIST_FAILED";
export const GET_ALL_TECHNICALSKILL_LIST = "[TECHNICALSKILL_LIST] GET_ALL_TECHNICALSKILL_LIST";

export const getTechnicalSkillList = params => {
    return {
        type: GET_TECHNICALSKILL_LIST,
        payload: {
            params
        }
    };
};

export const getAllTechnicalSkillList = () => {
    return {
        type: GET_ALL_TECHNICALSKILL_LIST
    };
};

export const getTechnicalSkillListSuccess = payload => {
    return {
        type: GET_TECHNICALSKILL_LIST_SUCCESS,
        payload
    };
};

export const getTechnicalSkillListFailed = () => {
    return {
        type: GET_TECHNICALSKILL_LIST_FAILED
    };
};