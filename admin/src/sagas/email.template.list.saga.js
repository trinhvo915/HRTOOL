import { put, call, takeLatest } from "redux-saga/effects";
import EmailTemplateApi from "../api/api.email.template.js";
import {
  getEmailTemplateListSuccess,
  getEmailTemplateListFailed,
  GET_EMAILTEMPLATE_LIST
} from "../actions/email.template.list.action";

function* getEmailTemplateList(action) {
  try {
    const payload = yield call(EmailTemplateApi.getEmailTemplateList, action.payload.params);
    yield put(getEmailTemplateListSuccess(payload));
  } catch {
    yield put(getEmailTemplateListFailed());
  }
}

export function* watchEmailTemplateListSagaAsync() {
  yield takeLatest(GET_EMAILTEMPLATE_LIST, getEmailTemplateList);
}
