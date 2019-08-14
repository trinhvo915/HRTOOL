import { call, put, takeLatest } from 'redux-saga/effects';
import { GET_DEPARTMENT_LIST, getDepartmentListFailed, getDepartmentListSuccess } from '../actions/department.list.action';
import departmentApi from '../api/api.department';

function* getDepartmentList (action) {
  try {
    const payload = yield call (departmentApi.getDepartmentList, action.payload.params);
    yield put (getDepartmentListSuccess(payload));
  } catch (error) {
    yield put (getDepartmentListFailed());
  }
}

export function* watchDepartmentSagaAsync () {
  yield takeLatest (GET_DEPARTMENT_LIST, getDepartmentList)
}