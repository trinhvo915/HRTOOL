import RequestHelper from "../helpers/request.helper";
import { appConfig } from "../config/app.config";
import { uploadFiles } from "../helpers/upload.files.helper";
import { uploadFile } from "../helpers/upload.file.helper";
export default class ApiEmailTemplate {
  // EmailTemplate
  static getEmailTemplateList(params) {
    return RequestHelper.get(appConfig.apiUrl + "emailtemplates", params);
  }

  static updateEmailTemplate(emailTemplate) {
    return RequestHelper.put(appConfig.apiUrl + `emailtemplates/${emailTemplate.id}`, emailTemplate);
  }

  static uploadFile(folder, file) {
    return uploadFile(folder, file);
  }

  static uploadFiles(folder, files) {
    return uploadFiles(folder, files);
  }
}