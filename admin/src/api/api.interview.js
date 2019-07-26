import RequestHelper from "../helpers/request.helper";
import { appConfig } from "../config/app.config";
import { uploadFile } from "../helpers/upload.file.helper";
import { uploadFiles } from "../helpers/upload.files.helper";

export default class ApiInterview {
    static getInterviewList(params) {
        return RequestHelper.get(appConfig.apiUrl + "interviews", params);
    }

    static addInterview(interview) {
        return RequestHelper.post(appConfig.apiUrl + "interviews", interview);
    }

    static updateInterview(interview) {
        return RequestHelper.put(
            appConfig.apiUrl + `interviews/${interview.id}`, interview
        );
    }

    static deleteInterview(interviewId) {
        return RequestHelper.delete(appConfig.apiUrl + `interviews/${interviewId}`);
    }

    static updateStatus(interview) {
        return RequestHelper.put(appConfig.apiUrl + `interviews/${interview.id}/status`, interview);
    }
    
    static uploadFile (folder, file) {
        return uploadFile (folder, file);
    }

    static uploadFiles (folder, files) {
        return uploadFiles (folder, files);
    }
}
