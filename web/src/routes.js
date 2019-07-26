import React from "react";
import DefaultLayout from "./pages/user/User";

const Dashboard = React.lazy(() => import("./pages/user/Dashboard/Dashboard"));

const JobListPage = React.lazy(() =>
  import ("./pages/user/jobManagement/job.list.page")
);
const CanlendarList = React.lazy(() =>
  import("./pages/user/calendarManagement/calendar.list")
);
const QuestionAnswer = React.lazy(() =>
  import("./pages/user/questionAnswerManagement/questionAnswer.page")
);
const User = React.lazy(() =>
  import("./pages/user/userProfile/user.profile")
);
const routes = [
  {
    path: "/",
    exact: true,
    name: "Web",
    component: DefaultLayout
  },
  { path: "/dashboard", name: "Dashboard", component: Dashboard },
  { path: "/jobs", name: "List Job", component: JobListPage},
  { path: "/calendars", name: "User Calendar", component: CanlendarList},
  { path: "/interviewQAs", name: "InterviewQA", component: QuestionAnswer },
  { path: "/users/:userId", name: "User Profile", component: User }
];

export default routes;
