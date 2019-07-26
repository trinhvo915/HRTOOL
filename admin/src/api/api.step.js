import RequestHelper from "../helpers/request.helper";
import { appConfig } from "../config/app.config";

export default class StepApi{

    static getStepListByJob(jobId){
        return RequestHelper.get(appConfig.apiUrl + `stepInJobs/${jobId}`);
    }

    static addStep(params){
        return RequestHelper.post(appConfig.apiUrl + `stepInJobs/${params.jobId}/step`, params.data)
    }

    static updateStep(params){
        return RequestHelper.put(appConfig.apiUrl + `stepInJobs/${params.id}`, params);
    }

    static deleteStep(params){
        return RequestHelper.delete(appConfig.apiUrl + `stepInJobs/${params.id}`);
    }
} 