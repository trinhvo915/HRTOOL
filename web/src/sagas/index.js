import { all, fork } from "redux-saga/effects";
import { watchProfileSagasAsync } from "./profile.saga";
import { watchJobListSagasAsync } from "./job.list.saga"
import { watchCalendarByUserAsync } from "./calendar.list.saga";
import { watchCalendarTypeListSagasAsync } from "./calendar.type.list.saga";
import { watchUserListSagaAsync } from "./user.list.saga";
import { watchConnectHubNotificationCalendarAsync } from "./hub.notification.calendar.saga";
import { watchQuestionAnswerSagasAsync} from "./questionAnswer.saga";

export default function* sagas() {
  yield all([
    fork(watchUserListSagaAsync),
    fork(watchProfileSagasAsync),
    fork(watchCalendarByUserAsync),
    fork(watchConnectHubNotificationCalendarAsync),
    fork(watchQuestionAnswerSagasAsync),
    fork(watchJobListSagasAsync),
    fork(watchCalendarTypeListSagasAsync)
  ]);
}
