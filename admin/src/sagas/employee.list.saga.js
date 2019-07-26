import { put, call, takeLatest } from "redux-saga/effects";
import employeeApi from "../api/api.employee";
import {
  getEmployeeListSuccess,
  getEmployeeListFaild,
  GET_EMPLOYEE_LIST
} from "../actions/employee.list.action";

function* getEmployeeList(action) {
  try {
    const payload = yield call(employeeApi.getEmployeeList, action.payload.params);
    yield put(getEmployeeListSuccess(payload));
  } catch {
    yield put(getEmployeeListFaild());
  }
}

export function* watchEmployeeListSagaAsync() {
  yield takeLatest(GET_EMPLOYEE_LIST, getEmployeeList);
}
