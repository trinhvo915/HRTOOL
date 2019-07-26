export const GET_EMAILTEMPLATE_LIST = "[EMAILTEMPLATE_LIST] GET_EMAILTEMPLATE_LIST";
export const GET_EMAILTEMPLATE_LIST_SUCCESS =
  "[EMAILTEMPLATE_LIST] GET_EMAILTEMPLATE_LIST_SUCCESS";
export const GET_EMAILTEMPLATE_LIST_FAILED =
  "[EMAILTEMPLATE_LIST] GET_EMAILTEMPLATE_LIST_FAILED";

  export const getEmailTemplateList = params => {
    return {
      type: GET_EMAILTEMPLATE_LIST,
      payload: {
        params
      }
    };
  };
  
  export const getEmailTemplateListSuccess = payload => {
    return {
      type: GET_EMAILTEMPLATE_LIST_SUCCESS,
      payload
    };
  };
  
  export const getEmailTemplateListFailed = () => {
    return {
      type: GET_EMAILTEMPLATE_LIST_FAILED
    };
  };