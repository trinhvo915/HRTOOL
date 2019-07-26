import RequestHelper from "../helpers/request.helper";
import { appConfig } from "../config/app.config";
import { uploadFile } from "../helpers/upload.file.helper";
import { generatePDF } from "../helpers/generatePDF.helper"

export default class Api {
  // Category
  static getCategoryList(params) {
    return RequestHelper.get(appConfig.apiUrl + "categories", params);
  }

  static getAllCategoryListNoFilter(params) {
    return RequestHelper.get(
      appConfig.apiUrl + "categories/all-categories",
      params
    );
  }

  static addCategory(category) {
    return RequestHelper.post(appConfig.apiUrl + "categories", category);
  }
  static updateCategory(category) {
    return RequestHelper.put(
      appConfig.apiUrl + `categories/${category.id}`,
      category
    );
  }
  static deleteCategory(categoryId) {
    return RequestHelper.delete(appConfig.apiUrl + `categories/${categoryId}`);
  }
  static getRoleList(params) {
    return RequestHelper.get(appConfig.apiUrl + "roles", params);
  }
  static resetColor(user) {
    return RequestHelper.put(
      appConfig.apiUrl + `users/${user.id}/color/origin`,
      user.colorIds
    );
  }

  //upload file
  static uploadAvatar(folder, file) {
    return uploadFile(folder, file);
  }
  //upload file

  static generatePDF(query) {
    return generatePDF(query);
  }

  // sso
  static login(data) {
    return RequestHelper.post(appConfig.apiUrl + "sso/login", data);
  }

  static getPermissons() {
    return RequestHelper.get(appConfig.apiUrl + "sso/permissions");
  }

  static getProfile() {
    return RequestHelper.get(appConfig.apiUrl + "sso/profile");
  }
}
