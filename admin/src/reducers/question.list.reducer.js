import { GET_QUESTION_LIST, GET_QUESTION_LIST_FAILED, GET_QUESTION_LIST_SUCCESS } from "../actions/question.list.action";

const initialState = {
    questionList: [],
    loading: false,
    failed: false
};

export function questionListReducer(state = initialState, action) {
    switch (action.type) {
        case GET_QUESTION_LIST: 
            return Object.assign({}, state, {
                loading: true,
                failed: false
            });
        case GET_QUESTION_LIST_SUCCESS:
            return Object.assign({}, state, {
                questionList: action.payload,
                loading: false,
                failed: false
            });
        case GET_QUESTION_LIST_FAILED:
            return Object.assign({}, state, {
                loading: true,
                failed: true
            });
        default:
            return state;
    }
}