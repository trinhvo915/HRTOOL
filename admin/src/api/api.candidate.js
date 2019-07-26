import RequestHelper from "../helpers/request.helper";
import { appConfig } from "../config/app.config";
import { uploadFile } from "../helpers/upload.file.helper";

export default class Api {
  // candidate
  static getCandidateList(params) {
    return RequestHelper.get(appConfig.apiUrl + "candidates", params);
  }

  static addCandidate(candidate) {
    return RequestHelper.post(appConfig.apiUrl + "candidates", candidate);
  }

  static updateCandidate(candidate) {
    return RequestHelper.put(appConfig.apiUrl + `candidates/${candidate.id}`, candidate);
  }
  
  static deleteCandidate(candidateId) {
    return RequestHelper.delete(appConfig.apiUrl + `candidates/${candidateId}`);
  }

  //upload file
  static uploadAvatar (folder, file) {
    return uploadFile (folder, file);
  }
}