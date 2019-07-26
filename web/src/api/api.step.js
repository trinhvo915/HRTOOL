import RequestHelper from "../helpers/request.helper";
import { appConfig } from "../config/app.config";

export default class StepApi {
    static getStepList(id, params){
        return RequestHelper.get(appConfig.apiUrl + `stepInJobs/${id}`, params);
    }

    static addStep(params){
        return RequestHelper.post(appConfig.apiUrl + `stepInJobs/${params.jobId}/step`, params.value);
    }

    static addSteps(params){
        return RequestHelper.post(appConfig.apiUrl + `stepInJobs/${params.jobId}/steps`, params.value);
    }

    static updateStep(params){
        return RequestHelper.put(appConfig.apiUrl + `stepInJobs/${params.stepId}`, params.value)
    }

    static deleteStep(params){
        return RequestHelper.delete(appConfig.apiUrl + `stepInJobs/${params.stepId}`);
    }
}