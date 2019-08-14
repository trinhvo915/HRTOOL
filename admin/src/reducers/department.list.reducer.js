import {
    GET_DEPARTMENT_LIST,
    GET_DEPARTMENT_LIST_SUCCESS,
    GET_DEPARTMENT_LIST_FAILED
} from "../actions/department.list.action";

const initialState = {
  departmentList: [],
  loading: false,
  failed: false
};

export function departmentListReducer(state = initialState, action) {
  switch (action.type) {
    case GET_DEPARTMENT_LIST:
      return Object.assign({}, state, {
        loading: true,
        failed: false
      });
    case GET_DEPARTMENT_LIST_SUCCESS:
      return Object.assign({}, state, {
        departmentList: action.payload,
        loading: false,
        failed: false
      });
    case GET_DEPARTMENT_LIST_FAILED:
      return Object.assign({}, state, {
        loading: false,
        failed: true
      });
    default:
      return state;
  }
}