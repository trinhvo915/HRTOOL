export const GET_COMMENT_LIST = "[COMMENT_LIST] GET_COMMENT_LIST";
export const GET_COMMENT_LIST_SUCCESS = "[COMMENT_LIST] GET_COMMENT_LIST_SUCCESS";
export const GET_COMMENT_LIST_FAILED = "[COMMENT_LIST] GET_COMMENT_LIST_FAILED";

export const getCommentList = (id, params) => {
    return {
        type: GET_COMMENT_LIST,
        payload: {
            id,
            params
        }
    };
}

export const getCommentListSuccess = payload => {
    return {
        type: GET_COMMENT_LIST_SUCCESS,
        payload
    };
};

export const getCommentListFailed = () => {
    return {
        type: GET_COMMENT_LIST_FAILED
    };
};