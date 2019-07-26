import RequestHelper from "../helpers/request.helper";
import { appConfig } from "../config/app.config";

export default class CommentApi {
    static getCommentList(id, params){
        return RequestHelper.get(appConfig.apiUrl + `comments/${id}`, params);
    }

    static addComment(params){
        return RequestHelper.post(appConfig.apiUrl + `comments/${params.jobId}`, params.value);
    }

    static updateComment(params){
        return RequestHelper.put(appConfig.apiUrl + `comments/${params.jobId}/${params.commentId}`, params.value)
    }

    static deleteComment(params){
        return RequestHelper.delete(appConfig.apiUrl + `comments/${params.jobId}/${params.commentId}`);
    }
}