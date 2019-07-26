import {
  GET_EMPLOYEE_LIST,
  GET_EMPLOYEE_LIST_SUCCESS,
  GET_EMPLOYEE_LIST_FAILED
} from "../actions/employee.list.action";

const initState = {
  employeeList: {},
  loading: false,
  failed: false
};

export function employeeListReducer(state = initState, action) {
  switch (action.type) {
    case GET_EMPLOYEE_LIST:
      return Object.assign({}, state, {
        loading: true,
        failed: false
      });
    case GET_EMPLOYEE_LIST_SUCCESS:
      return Object.assign({}, state, {
        employeeList: action.payload,
        loading: false,
        failed: false
      });
    case GET_EMPLOYEE_LIST_FAILED:
      return Object.assign({}, state, {
        loading: false,
        failed: true
      });
    default:
      return state;
  }
}
