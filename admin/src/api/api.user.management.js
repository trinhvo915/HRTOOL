import RequestHelper from "../helpers/request.helper";
import { appConfig } from "../config/app.config";

export default class ApiUserManagement {
  //ManagementUser
  static getUserManagementList(params) {
    return RequestHelper.get(appConfig.apiUrl + "users", params);
  }

  static updateUserManagementList(user) {
    return RequestHelper.put(appConfig.apiUrl + `users/${user.id}`, user);
  }

  //Role
  static getRoleList(params) {
    return RequestHelper.get(appConfig.apiUrl + "roles", params);
  }
}
