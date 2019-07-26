import { call, put, takeLatest } from 'redux-saga/effects';
import { GET_CANDIDATE_LIST, getCandidateListFailed, getCandidateListSuccess } from '../actions/candidate.list.action';
import Api from '../api/api.candidate';

function* getCandidateList (action) {
  try {
    const payload = yield call (Api.getCandidateList, action.payload.params);
    yield put (getCandidateListSuccess(payload));
  } catch (error) {
    yield put (getCandidateListFailed());
  }
}

export function* watchCandidateSagaAsync () {
  yield takeLatest (GET_CANDIDATE_LIST, getCandidateList)
}