import {
  GET_USERMANAGEMENT_LIST,
  GET_USERMANAGEMENT_LIST_FAILED,
  GET_USERMANAGEMENT_LIST_SUCCESS,
  RESET_COLOR
} from "../actions/user.management.list.action";

const initialState = {
  pageSize: 0,
  pageIndex: 1,
  sources: [],
  totalPages: 1,
  loading: false,
  failed: false
};

export function userManagementListReducer(state = initialState, action) {
  switch (action.type) {
    case GET_USERMANAGEMENT_LIST:
      return Object.assign({}, state, {
        loading: true,
        failed: false
      });
    case GET_USERMANAGEMENT_LIST_SUCCESS:
      return Object.assign({}, state, {
        sources: action.payload.sources,
        pageIndex: action.payload.pageIndex,
        totalPages: action.payload.totalPages,
        pageSize: action.payload.pageSize,
        loading: false,
        failed: false
      });
    case GET_USERMANAGEMENT_LIST_FAILED:
      return Object.assign({}, state, {
        loading: true,
        failed: true
      });
    case RESET_COLOR:
      return Object.assign({}, state, {
        sources: action.sources
      });
    default:
      return state;
  }
}
