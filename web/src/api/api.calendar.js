import RequestHelper from "../helpers/request.helper";
import { appConfig } from "../config/app.config";

export default class ApiCalendar {
    static getCalendarByUser(params) {
        return RequestHelper.get(appConfig.apiUrl + "users/calendars", params);
    }

    static getUserList() {
        return RequestHelper.get(appConfig.apiUrl + "users/all-users");
    }

    static getCalendarTypeList() {
        return RequestHelper.get(appConfig.apiUrl + "calendartypes");
    }

    static postCalendar(calendar) {
        return RequestHelper.post(appConfig.apiUrl + "calendars", calendar);
    }

    static updateCalendar(calendar) {
        return RequestHelper.put(
            appConfig.apiUrl + `calendars/${calendar.id}`,
            calendar
        );
    }

    static deleteCalendar(calendarId) {
        return RequestHelper.delete(appConfig.apiUrl + `calendars/${calendarId}`);
    }
}