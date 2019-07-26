export const GET_USERMANAGEMENT_LIST =
  "[USERMANAGEMENT_LIST] GET_USERMANAGEMENT_LIST";
export const GET_USERMANAGEMENT_LIST_SUCCESS =
  "[USERMANAGEMENT_LIST] GET_USERMANAGEMENT_LIST_SUCCESS";
export const GET_USERMANAGEMENT_LIST_FAILED =
  "[USERMANAGEMENT_LIST] GET_USERMANAGEMENT_LIST_FAILED";

export const RESET_COLOR = "[USERMANAGEMENT_LIST] RESET_COLOR";

export const getUserManagementList = params => {
  return {
    type: GET_USERMANAGEMENT_LIST,
    payload: {
      params
    }
  };
};

export const getUserManagementListSuccess = payload => {
  return {
    type: GET_USERMANAGEMENT_LIST_SUCCESS,
    payload
  };
};

export const getUserManagementListFailed = () => {
  return {
    type: GET_USERMANAGEMENT_LIST_FAILED
  };
};

export const resetColor = sources => {
  return {
    type: RESET_COLOR,
    sources
  };
};
