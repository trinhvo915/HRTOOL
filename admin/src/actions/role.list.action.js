export const GET_ROLE_LIST = "[ROLE_LIST] GET_ROLE_LIST";
export const GET_ROLE_LIST_SUCCESS = "[ROLE_LIST] GET_ROLE_LIST_SUCCESS";
export const GET_ROLE_LIST_FAILED = "[ROLE_LIST] GET_ROLE_LIST_FAILED";

export const getRoleList = params => {
  return {
    type: GET_ROLE_LIST,
    payload: {
      params
    }
  };
};

export const getRoleListSuccess = payload => {
  return {
    type: GET_ROLE_LIST_SUCCESS,
    payload
  };
};

export const getRoleListFailed = () => {
  return {
    type: GET_ROLE_LIST_FAILED
  };
};
