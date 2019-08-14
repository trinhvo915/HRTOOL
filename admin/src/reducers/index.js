import { combineReducers } from "redux";
import { categoryListReducer } from "./category.list.reducer";
import { profileReducer } from "./profile.reducer";
import { candidateListReducer } from "./candidate.list.reducer";
import { jobListReducer } from "./job.list.reducer";
import { userListReducer } from "./user.list.reducer";
import { calendarListReducer } from "./calendar.list.reducer";
import { employeeListReducer } from "./employee.list.reducer";
import { interviewListReducer } from "./interview.list.reducer";
import { commentListReducer } from "./comment.list.reducer";
import { calendarTypeListReducer } from "./calendar.type.list.reducer";
import { emailTemplateReducer } from "./email.template.list.reducer";
import { technicalSkillListReducer } from "./technical.skill.list.reducer";
import { connectHubNotificationCalendarReducer } from "./hub.notification.calendar.reducer";
import { userManagementListReducer } from "./user.management.list.reducer";
import { roleListReducer } from "./role.list.reducer";
import { questionListReducer } from "./question.list.reducer";
import { departmentListReducer } from "./department.list.reducer";

export default combineReducers({
  userList: userListReducer,
  jobList: jobListReducer,
  categoryList: categoryListReducer,
  profile: profileReducer,
  candidateList: candidateListReducer,
  commentList: commentListReducer,
  calendarTypeList: calendarTypeListReducer,
  employeeList: employeeListReducer,
  interviewList: interviewListReducer,
  calendarList: calendarListReducer,
  emailTemplateList: emailTemplateReducer,
  technicalSkillList: technicalSkillListReducer,
  hubConnectionCalendar: connectHubNotificationCalendarReducer,
  questionList: questionListReducer,
  userManagementList: userManagementListReducer,
  roleList: roleListReducer,
  departmentList : departmentListReducer
});
