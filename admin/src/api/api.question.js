import RequestHelper from "../helpers/request.helper";
import { appConfig } from "../config/app.config";

export default class QuestionApi {
    static getQuestionList(params) {
        return RequestHelper.get(appConfig.apiUrl + "questions", params);
    }

    static addQuestion(question) {
        return RequestHelper.post(appConfig.apiUrl + "questions", question);
    }

    static updateQuestion(question){
        return RequestHelper.put(
            appConfig.apiUrl + `questions/${question.id}`,
            question
        );
    }

    static deleteQuestion(questionId) {
        return RequestHelper.delete(appConfig.apiUrl + `questions/${questionId}`);
    }

    static updateStatus(question) {
        return RequestHelper.put(appConfig.apiUrl + `questions/${question.id}/status`, question);
    }

}