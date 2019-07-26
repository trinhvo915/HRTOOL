import { appConfig } from "../config/app.config";
import RequestHelper from "../helpers/request.helper";

export default class ApiCalendar {
   // calendar 
  static getCalendarList(params) {
    return RequestHelper.get(appConfig.apiUrl + "calendars", params);
  }

  static postCalendar(calendar){
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
   // end calendar 
  
   // get all user
  static getUserList() {
    return RequestHelper.get(appConfig.apiUrl + "users/all-users");
  }

  // get all canlendar type
  static getCalendarTypeList() {
    return RequestHelper.get(appConfig.apiUrl + "calendartypes");
  }
}
