import {
    GET_CALENDAR_LIST,
    GET_CALENDAR_LIST_SUCCESS,
    GET_CALENDAR_LIST_FAILED
} from "../actions/calendar.list.action";

const initialState = {
  calendarList: [],
  loading: false,
  failed: false
};

export function calendarListReducer(state = initialState, action) {
  switch (action.type) {
    case GET_CALENDAR_LIST:
      return Object.assign({}, state, {
        loading: true,
        failed: false
      });
    case GET_CALENDAR_LIST_SUCCESS:
      return Object.assign({}, state, {
        calendarList: action.payload,
        loading: false,
        failed: false
      });
    case GET_CALENDAR_LIST_FAILED:
      return Object.assign({}, state, {
        loading: false,
        failed: true
      });
    default:
      return state;
  }
}