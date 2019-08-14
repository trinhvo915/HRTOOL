import RequestHelper from "../helpers/request.helper";
import { appConfig } from "../config/app.config";

export default class UserApi {
  static getAllUser(params) {
    return RequestHelper.get(appConfig.apiUrl + "users/all-users", params);
  }

  static getUserTree(params) {
    return RequestHelper.get(appConfig.apiUrl + "users/hr-tree", params);
  }
}
