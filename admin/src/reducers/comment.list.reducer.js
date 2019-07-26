import { GET_COMMENT_LIST, GET_COMMENT_LIST_SUCCESS, GET_COMMENT_LIST_FAILED } from "../actions/comment.list.action";

const initialState = {
    commentList: [],
    loading: false,
    failed: false
};

export function commentListReducer(state = initialState, action){
    switch(action.type) {
        case GET_COMMENT_LIST: {
            return Object.assign(state, {
                loading: true,
                failed: false
            })
        }
        case GET_COMMENT_LIST_SUCCESS: {
            return Object.assign(state, {
                commentList: action.payload,
                loading: false,
                failed: false
            })
        }
        case GET_COMMENT_LIST_FAILED: {
            return Object.assign(state, {
                loading: true,
                failed: true
            })
        }
        default:
            return state;
    }
}