import RequestHelper from "../helpers/request.helper";
import { appConfig } from "../config/app.config";

export default class Api {
  // sso
  static login(data) {
    return RequestHelper.post(appConfig.apiUrl + "sso/login", data);
  }

  static getPermissons() {
    return RequestHelper.get(appConfig.apiUrl + "sso/permissions");
  }

  static getProfile() {
    return RequestHelper.get(appConfig.apiUrl + "sso/profile");
  }
}
