import { call, put, takeLatest } from 'redux-saga/effects';
import { GET_QUESTION_LIST, getQuestionListSuccess, getQuestionListFailed } from "../actions/question.list.action";
import Api from "../api/api.question";

function* getQuestionList(action) {
    try {
        const payload = yield call(Api.getQuestionList, action.payload.params);
        yield put(getQuestionListSuccess(payload));
    } catch (err) {
        yield put(getQuestionListFailed());
    }
}

export function* watchQuestionListSagasAsync() {
    yield takeLatest(GET_QUESTION_LIST, getQuestionList)
}