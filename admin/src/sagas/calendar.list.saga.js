import { call, put, takeLatest } from "redux-saga/effects";
import { 
    GET_CALENDAR_LIST, 
    getCalendarListSuccess, 
    getCalendarListFailed
} from "../actions/calendar.list.action";
import ApiCalendar from "../api/api.calendar";

function* getCalendarList(action) {
    try {
      const payload = yield call(ApiCalendar.getCalendarList, action.payload.params);
      yield put(getCalendarListSuccess(payload));

    } catch (error) {
      yield put(getCalendarListFailed());
    }
  }

  export function* watchCalendarListSagasAsync() {
    yield takeLatest(GET_CALENDAR_LIST, getCalendarList);
  }