import { call, put, takeLatest } from "redux-saga/effects";
import {
    GET_INTERVIEW_LIST,
    getInterviewListSuccess,
    getInterviewListFailed
} from "../actions/interview.list.action";
import ApiInterview from "../api/api.interview";

function* getInterviewList(action) {
    try {
        const payload = yield call(ApiInterview.getInterviewList, action.payload.params);

        yield put(getInterviewListSuccess(payload));
    } catch (error) {
        yield put(getInterviewListFailed());
    }
}

export function* watchInterviewListSagasAsync() {
    yield takeLatest(GET_INTERVIEW_LIST, getInterviewList);
}
