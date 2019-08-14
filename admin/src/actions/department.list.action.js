export const GET_DEPARTMENT_LIST = "[DEPARTMENT_LIST] GET_DEPARTMENT_LIST";
export const GET_ALL_DEPARTMENT_LIST_NO_FILTER = "[DEPARTMENT_LIST] GET_ALL_DEPARTMENT_LIST_NO_FILTER";
export const GET_DEPARTMENT_LIST_SUCCESS =
  "[DEPARTMENT_LIST] GET_DEPARTMENT_LIST_SUCCESS";
export const GET_DEPARTMENT_LIST_FAILED =
  "[DEPARTMENT_LIST] GET_DEPARTMENT_LIST_FAILED";

export const getDepartmentList = params => {
  return {
    type: GET_DEPARTMENT_LIST,
    payload: {
      params
    }
  };
};

export const getAllDepartmentListNoFilter = params => {
  return {
    type: GET_ALL_DEPARTMENT_LIST_NO_FILTER,
    payload: {
      params
    }
  };
};

export const getDepartmentListSuccess = payload => {
  return {
    type: GET_DEPARTMENT_LIST_SUCCESS,
    payload
  };
};

export const getDepartmentListFailed = () => {
  return {
    type: GET_DEPARTMENT_LIST_FAILED
  };
};
