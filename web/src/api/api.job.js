import RequestHelper from "../helpers/request.helper";
import { appConfig } from "../config/app.config";
import { uploadFile } from "../helpers/upload.file.helper";
import { uploadFiles } from "../helpers/upload.files.helper";

export default class JobApi {
  static getJobList(params){
    return RequestHelper.get(appConfig.apiUrl + "users/jobs", params)
  }

  static getJobById(id){
    return RequestHelper.get(appConfig.apiUrl + `jobs/${id}`)
  }
  
  static addCommentJob(data){
    return RequestHelper.post(appConfig.apiUrl + `comments/${data.id}`, data.comment);
  }

  static addJob(job) {
    return RequestHelper.post(appConfig.apiUrl + "jobs", job);
  }

  static updateJob(job) {
    return RequestHelper.put(appConfig.apiUrl + `jobs/${job.id}`,job
    );
  }

  static deleteJob(jobId) {
    return RequestHelper.delete(appConfig.apiUrl + `jobs/${jobId}`);
  }

  static uploadFile(folder, file) {
    return uploadFile(folder, file);
  }

  static uploadFiles(folder, files) {
    return uploadFiles(folder, files);
  }

}