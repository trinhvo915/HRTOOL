import { call, put, takeLatest } from 'redux-saga/effects';
import { GET_COMMENT_LIST, getCommentListSuccess, getCommentListFailed } from "../actions/comment.list.action";
import Api from "../api/api.comment";

function *getCommentList(action){
    try{
        const payload = yield call(Api.getCommentList, action.payload.id, action.payload.params);
        yield put(getCommentListSuccess(payload));
    }catch(err){
        yield put(getCommentListFailed());
    }
}

export function* watchCommentListSagasAsync() {
    yield takeLatest(GET_COMMENT_LIST, getCommentList)
}