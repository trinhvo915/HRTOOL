import RequestHelper from "../helpers/request.helper";
import { appConfig } from "../config/app.config";

export default class DepartmentApi {

  static getDepartmentList(params) {
    return RequestHelper.get(appConfig.apiUrl + "departments", params);
  }

  static addDepartment(department) {
    return RequestHelper.post(appConfig.apiUrl + "departments", department);
  }
  static updateDepartment(department) {
    return RequestHelper.put(
      appConfig.apiUrl + `departments/${department.id}`,
      department
    );
  }
  static deleteDepartment(departmentId) {
    return RequestHelper.delete(appConfig.apiUrl + `departments/${departmentId}`);
  }
} 