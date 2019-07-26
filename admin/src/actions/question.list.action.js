export const GET_QUESTION_LIST = "[QUESTION_LIST] GET_QUESTION_LIST";
export const GET_QUESTION_LIST_SUCCESS = "[QUESTION_LIST] GET_QUESTION_LIST_SUCCESS";
export const GET_QUESTION_LIST_FAILED = "[QUESTION_LIST] GET_QUESTION_LIST_FAILED";

export const getQuestionList = params => {
    return {
        type: GET_QUESTION_LIST,
        payload: {
            params
        }
    };
};

export const getQuestionListSuccess = payload => {
    return {
        type: GET_QUESTION_LIST_SUCCESS,
        payload
    };
};

export const getQuestionListFailed = () => {
    return {
        type: GET_QUESTION_LIST_FAILED
    };
};