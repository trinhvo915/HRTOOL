import RequestHelper from "../helpers/request.helper";
import { appConfig } from "../config/app.config";

export default class employeeApi {
    static getEmployeeList(params) {
        return RequestHelper.get(appConfig.apiUrl + "employees", params);
    }

    static addEmployee(employee) {
        return RequestHelper.post(appConfig.apiUrl + "employees", employee);
    }

    static updateEmployee(employee) {
        return RequestHelper.put(
            appConfig.apiUrl + `employees/${employee.id}`,
            employee
        );
    }

    static deleteEmployee(Id) {
        return RequestHelper.delete(appConfig.apiUrl + `employees/${Id}`);
    }
}