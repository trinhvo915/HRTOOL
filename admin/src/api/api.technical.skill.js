import RequestHelper from "../helpers/request.helper";
import { appConfig } from "../config/app.config";

export default class TechnicalSkillApi {
    static getTechnicalSkill(params) {
        return RequestHelper.get(appConfig.apiUrl + "technical-skills", params);
    }

    static addTechnicalSkill(technicalSkill) {
      return RequestHelper.post(appConfig.apiUrl + "technical-skills", technicalSkill);
    }
  
    static updateTechnicalSkill(technicalSkill) {
      return RequestHelper.put(appConfig.apiUrl + `technical-skills/${technicalSkill.id}`, technicalSkill);
    }
    
    static deleteTechnicalSkill(technicalSkillId) {
      return RequestHelper.delete(appConfig.apiUrl + `technical-skills/${technicalSkillId}`);
    }

    static getAllTechnicalSkill() {
      return RequestHelper.get(appConfig.apiUrl + "technical-skills/all");
  }
}