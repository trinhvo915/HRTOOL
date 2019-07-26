export const GET_CANDIDATE_LIST = "[CANDIDATE_LIST] GET_CANDIDATE_LIST";
export const GET_CANDIDATE_LIST_SUCCESS =
  "[CANDIDATE_LIST] GET_CANDIDATE_LIST_SUCCESS";
export const GET_CANDIDATE_LIST_FAILED =
  "[CANDIDATE_LIST] GET_CANDIDATE_LIST_FAILED";

export const getCandidateList = params => {
  return {
    type: GET_CANDIDATE_LIST,
    payload: {
      params
    }
  };
};

export const getCandidateListSuccess = payload => {
  return {
    type: GET_CANDIDATE_LIST_SUCCESS,
    payload
  };
};

export const getCandidateListFailed = () => {
  return {
    type: GET_CANDIDATE_LIST_FAILED
  };
};
