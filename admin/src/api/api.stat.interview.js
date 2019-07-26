import RequestHelper from "../helpers/request.helper";
import { appConfig } from "../config/app.config";

export default class StatInterviewApi {
    static getStatInterviewAnimation(params) {
        return RequestHelper.get(appConfig.apiUrl + "dashboard/interviews-animation",params);
    }
    static getStatInterviewList() {
        return RequestHelper.get(appConfig.apiUrl + "dashboard/interviews");
    }
}