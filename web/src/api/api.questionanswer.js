import RequestHelper from "../helpers/request.helper";
import { appConfig } from "../config/app.config";
export default class QuestionAnswerApi {
  static getQuestionAnswerList(params) {
    return RequestHelper.get(appConfig.apiUrl + "questions", params);
  }
  static addQuestionAnswerList(question) {
    return RequestHelper.post(appConfig.apiUrl + "questions", question);
  }
  static addNewAnswer(answer) {
    return RequestHelper.post(appConfig.apiUrl + "answers", answer);
  }
}
