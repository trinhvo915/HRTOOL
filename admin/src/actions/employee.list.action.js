export const GET_EMPLOYEE_LIST = "[EMPLOYEE_LIST] GET_EMPLOYEE_LIST";
export const GET_EMPLOYEE_LIST_SUCCESS =
  "[EMPLOYEE_LIST] GET_EMPLOYEE_LIST_SUCCESS";
export const GET_EMPLOYEE_LIST_FAILED =
  "[EMPLOYEE_LIST] GET_EMPLOYEE_LIST_FAILED";
export const getEmployeeList = params => {
  return {
    type: GET_EMPLOYEE_LIST,
    payload: {
      params
    }
  };
};

export const getEmployeeListSuccess = payload => {
  return {
    type: GET_EMPLOYEE_LIST_SUCCESS,
    payload
  };
};

export const getEmployeeListFaild = () => {
  return {
    type: GET_EMPLOYEE_LIST_FAILED
  };
};
