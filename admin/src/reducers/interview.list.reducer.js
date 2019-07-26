import {
    GET_INTERVIEW_LIST,
    GET_INTERVIEW_LIST_SUCCESS,
    GET_INTERVIEW_LIST_FAILED
} from "../actions/interview.list.action";

const initialState = {
    interviewList: {},
    loading: false,
    failed: false
};

export function interviewListReducer(state = initialState, action) {
    switch (action.type) {
        case GET_INTERVIEW_LIST:
            return Object.assign({}, state, {
                loading: true,
                failed: false
            });
        case GET_INTERVIEW_LIST_SUCCESS:
            return Object.assign({}, state, {
                interviewList: action.payload,
                loading: false,
                failed: false
            });
        case GET_INTERVIEW_LIST_FAILED:
            return Object.assign({}, state, {
                loading: false,
                failed: true
            });
        default:
            return state;
    }
}
