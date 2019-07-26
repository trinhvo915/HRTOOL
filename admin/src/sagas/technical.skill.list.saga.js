import { call, put, all, takeEvery } from "redux-saga/effects";
import {
    GET_TECHNICALSKILL_LIST,
    getTechnicalSkillListSuccess,
    getTechnicalSkillListFailed,
    GET_ALL_TECHNICALSKILL_LIST
} from "../actions/technical.skill.list.action";
import TechnicalSkillApi from "../api/api.technical.skill";

function* getTechnicalSkill(action) {
    try {
        const payload = yield call(TechnicalSkillApi.getTechnicalSkill, action.payload.params);
        console.log(payload);
        yield put(getTechnicalSkillListSuccess(payload));
    } catch(err) {
        yield put(getTechnicalSkillListFailed());
    }
}

function* getAllTechnicalSkill(action) {
  try {
      const payload = yield call(TechnicalSkillApi.getAllTechnicalSkill);
      yield put(getTechnicalSkillListSuccess(payload));
  } catch(err) {
      yield put(getTechnicalSkillListFailed());
  }
}

export function* watchTechnicalSkillListSagaAsync() {
  yield all([
    takeEvery(GET_TECHNICALSKILL_LIST, getTechnicalSkill),
    takeEvery(GET_ALL_TECHNICALSKILL_LIST, getAllTechnicalSkill)
  ]);
}
