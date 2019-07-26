import { GET_CANDIDATE_LIST, GET_CANDIDATE_LIST_SUCCESS, GET_CANDIDATE_LIST_FAILED } from "../actions/candidate.list.action";

const initialState = {
  candidateList: {},
  loading: false,
  failed: false
};

export function candidateListReducer (state = initialState, action) {
  switch (action.type) {
    case GET_CANDIDATE_LIST:
      return { ...state, loading: true, failed: false }
    case GET_CANDIDATE_LIST_SUCCESS:
      return { ...state, candidateList: action.payload, loading: false, failed: false }
    case GET_CANDIDATE_LIST_FAILED:
      return { ...state, loading: false, failed: true }
    default:
      return state
  }
}