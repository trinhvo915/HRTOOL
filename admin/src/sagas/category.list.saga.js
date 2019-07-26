import { call, put ,takeEvery, all } from "redux-saga/effects";
import {
  GET_CATEGORY_LIST,
  GET_ALL_CATEGORY_LIST_NO_FILTER,
  getCategoryListSuccess,
  getCategoryListFailed
} from "../actions/category.list.action";
import Api from "../api/api";

function* getCategoryList(action) {
  try {
    const payload = yield call(Api.getCategoryList, action.payload.params);
    yield put(getCategoryListSuccess(payload));
  } catch (error) {
    yield put(getCategoryListFailed());
  }
}

function* getAllCategoryListNoFilter(action) {
  try {
    const payload = yield call(Api.getAllCategoryListNoFilter, action.payload.params);
    yield put(getCategoryListSuccess(payload));
  } catch (error) {
    yield put(getCategoryListFailed());
  }
}

export function* watchCategoryListSagasAsync() {
  yield all([
    takeEvery(GET_CATEGORY_LIST, getCategoryList),
    takeEvery(GET_ALL_CATEGORY_LIST_NO_FILTER, getAllCategoryListNoFilter),
  ]);
}
