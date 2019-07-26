import { put, call, takeLatest } from "redux-saga/effects";
import ApiUserManagement from "../api/api.user.management.js";
import {
  getUserManagementListSuccess,
  getUserManagementListFailed,
  GET_USERMANAGEMENT_LIST
} from "../actions/user.management.list.action";

function* getUserManagementList(action) {
  try {
    const payload = yield call(
      ApiUserManagement.getUserManagementList,
      action.payload.params
    );
    yield put(getUserManagementListSuccess(payload));
  } catch {
    yield put(getUserManagementListFailed());
  }
}

export function* watchUserManagementListSagasAsync() {
  yield takeLatest(GET_USERMANAGEMENT_LIST, getUserManagementList);
}
