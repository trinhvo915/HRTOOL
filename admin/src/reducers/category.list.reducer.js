import {
  GET_CATEGORY_LIST,
  GET_ALL_CATEGORY_LIST_NO_FILTER,
  GET_CATEGORY_LIST_FAILED,
  GET_CATEGORY_LIST_SUCCESS
} from "../actions/category.list.action";

const initialState = {
  categoryList: {},
  loading: false,
  failed: false
};

export function categoryListReducer(state = initialState, action) {
  switch (action.type) {
    case GET_CATEGORY_LIST || GET_ALL_CATEGORY_LIST_NO_FILTER:
      return Object.assign({}, state, {
        loading: true,
        failed: false
      });
    case GET_CATEGORY_LIST_SUCCESS:
      return Object.assign({}, state, {
        categoryList: action.payload,
        loading: false,
        failed: false
      });
    case GET_CATEGORY_LIST_FAILED:
      return Object.assign({}, state, {
        loading: false,
        failed: true
      });
    default:
      return state;
  }
}
