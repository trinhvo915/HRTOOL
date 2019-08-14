import { all, fork } from "redux-saga/effects";

import { watchCategoryListSagasAsync } from "./category.list.saga";
import { watchProfileSagasAsync } from "./profile.saga";
import { watchCandidateSagaAsync } from "./candidate.list.saga";
import { watchJobListSagasAsync } from "./job.list.saga";
import { watchUserListSagaAsync } from "./user.list.saga";
import { watchCalendarListSagasAsync } from "./calendar.list.saga";
import { watchEmployeeListSagaAsync } from "./employee.list.saga";
import { watchInterviewListSagasAsync } from "./interview.list.saga";
import { watchCommentListSagasAsync } from "./comment.list.saga";
import { watchCalendarTypeListSagasAsync } from "./calendar.type.list.saga";
import { watchQuestionListSagasAsync } from "./question.list.saga";
import { watchEmailTemplateListSagaAsync } from "./email.template.list.saga";
import { watchTechnicalSkillListSagaAsync } from "./technical.skill.list.saga";
import { watchConnectHubNotificationCalendarAsync } from "./hub.notification.calendar.saga";
import { watchUserManagementListSagasAsync } from "./user.management.list.saga";
import { watchRoleListSagasAsync } from "./role.list.saga";
import { watchDepartmentSagaAsync } from "./department.list.saga";

export default function* sagas() {
  yield all([
    fork(watchUserListSagaAsync),
    fork(watchCategoryListSagasAsync),
    fork(watchCandidateSagaAsync),
    fork(watchProfileSagasAsync),
    fork(watchJobListSagasAsync),
    fork(watchCalendarListSagasAsync),
    fork(watchEmployeeListSagaAsync),
    fork(watchInterviewListSagasAsync),
    fork(watchQuestionListSagasAsync),
    fork(watchCommentListSagasAsync),
    fork(watchCalendarTypeListSagasAsync),
    fork(watchEmailTemplateListSagaAsync),
    fork(watchTechnicalSkillListSagaAsync),
    fork(watchConnectHubNotificationCalendarAsync),
    fork(watchUserManagementListSagasAsync),
    fork(watchRoleListSagasAsync),
    fork(watchDepartmentSagaAsync),
  ]);
}
