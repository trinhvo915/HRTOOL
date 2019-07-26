import {
  GET_TECHNICALSKILL_LIST,
  GET_TECHNICALSKILL_LIST_SUCCESS,
  GET_TECHNICALSKILL_LIST_FAILED
} from "../actions/technical.skill.list.action";

const initialState = {
  technicalSkillList: [],
  loading: false,
  failed: false
};

export function technicalSkillListReducer(state = initialState, action) {
  switch (action.type) {
    case GET_TECHNICALSKILL_LIST:
      return Object.assign({}, state, {
        loading: true,
        failed: false
      });
    case GET_TECHNICALSKILL_LIST_SUCCESS:
      return Object.assign({}, state, {
        technicalSkillList: action.payload,
        loading: false,
        failed: false
      });
    case GET_TECHNICALSKILL_LIST_FAILED:
      return Object.assign({}, state, {
        loading: false,
        failed: true
      });
    default:
      return state;
  }
}