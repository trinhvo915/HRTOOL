import { call, put, takeLatest } from "redux-saga/effects";
import {
  GET_ROLE_LIST,
  getRoleListSuccess,
  getRoleListFailed
} from "../actions/role.list.action";
import Api from "../api/api";

function* getRoleList(action) {
  try {
    const payload = yield call(Api.getRoleList, action.payload.params);
    yield put(getRoleListSuccess(payload));
  } catch (err) {
    yield put(getRoleListFailed());
  }
}

export function* watchRoleListSagasAsync() {
  yield takeLatest(GET_ROLE_LIST, getRoleList);
}
