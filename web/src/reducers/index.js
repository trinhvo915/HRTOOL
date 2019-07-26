import { combineReducers } from "redux";
import { profileReducer } from "./profile.reducer";
import { jobListReducer } from "./job.list.reducer"
import { questionAnswerListReducer } from "./questionAnswer.reducer";
import { calendarByUserListReducer } from './calendar.list.reducer';
import { userListReducer } from "./user.list.reducer";
import { calendarTypeListReducer } from "./calendar.type.list.reducer";
import { connectHubNotificationCalendarReducer } from './hub.notification.calendar.reducer';
export default combineReducers({
  userList: userListReducer,
  calendarTypeList: calendarTypeListReducer,
  profile: profileReducer,
  jobList: jobListReducer,
  calendarList: calendarByUserListReducer,
  questionAnswerList: questionAnswerListReducer,
  hubConnectionCalendar: connectHubNotificationCalendarReducer
});
