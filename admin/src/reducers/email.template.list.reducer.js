import {
    GET_EMAILTEMPLATE_LIST,
    GET_EMAILTEMPLATE_LIST_FAILED,
    GET_EMAILTEMPLATE_LIST_SUCCESS
} from '../actions/email.template.list.action';

const initialState = {
    emailTemplateList: {},
    loading: false,
    failed: false
};

export function emailTemplateReducer(state = initialState, action) {
    switch (action.type) {
        case GET_EMAILTEMPLATE_LIST:
            return Object.assign({}, state, {
                loading: true,
                failed: false
            });
        case GET_EMAILTEMPLATE_LIST_SUCCESS:
            return Object.assign({}, state, {
                emailTemplateList: action.payload,
                loading: false,
                failed: false
            });
        case GET_EMAILTEMPLATE_LIST_FAILED:
            return Object.assign({}, state, {
                loading: false,
                failed: true
            });
        default:
            return state;
    }
}