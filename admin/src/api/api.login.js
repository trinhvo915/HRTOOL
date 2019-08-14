import RequestHelper from "../helpers/request.helper";
import { appConfig } from "../config/app.config";

export default class ApiLogin {
  static getProfileByToken(token) {
    return RequestHelper.getByToken(token, appConfig.apiUrl + "sso/profile");
  }

  static getRoleAdmin() {
    return RequestHelper.get(appConfig.apiUrl + "roles/admin")
  }
}
