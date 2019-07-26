export const GET_INTERVIEW_LIST = "[INTERVIEW_LIST] GET_INTERVIEW_LIST";
export const GET_INTERVIEW_LIST_SUCCESS =
  "[INTERVIEW_LIST] GET_INTERVIEW_LIST_SUCCESS";
export const GET_INTERVIEW_LIST_FAILED =
  "[INTERVIEW_LIST] GET_INTERVIEW_LIST_FAILED";

export const getInterviewList = params => {
  return {
    type: GET_INTERVIEW_LIST,
    payload: {
      params
    }
  };
};

export const getInterviewListSuccess = payload => {
  return {
    type: GET_INTERVIEW_LIST_SUCCESS,
    payload
  };
};

export const getInterviewListFailed = () => {
  return {
    type: GET_INTERVIEW_LIST_FAILED
  };
};
