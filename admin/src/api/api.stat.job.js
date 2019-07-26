import RequestHelper from "../helpers/request.helper";
import { appConfig } from "../config/app.config";

export default class StatJobApi {
    static getStatJobList() {
        return RequestHelper.get(appConfig.apiUrl + "dashboard/jobs");
    }

    static getStatJobAnimationList(params) {
        return RequestHelper.get(appConfig.apiUrl + "dashboard/jobs-animation",params);
    }
}