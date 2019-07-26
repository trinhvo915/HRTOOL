import React from "react";
import DefaultLayout from "./pages/admin/Admin";

const Dashboard = React.lazy(() => import("./pages/admin/Dashboard/Dashboard"));

const CategoryListPage = React.lazy(() =>
  import("./pages/admin/categoryManagement/category.list.page")
);

const CandidateListPage = React.lazy(() =>
  import("./pages/admin/candidateManagement/candidate.list.page")
);

const JobListPage = React.lazy(() =>
  import("./pages/admin/jobManagement/job.list.page")
);

const CalendarListPage = React.lazy(() =>
  import("./pages/admin/calendarManagement/calendar.list.page")
);

const EmployeeListPage = React.lazy(() =>
  import("./pages/admin/employeeManagement/employee.list.page")
);
const InterviewListPage = React.lazy(() =>
  import("./pages/admin/interviewManagement/interview.list.page")
);

const QuestionListPage = React.lazy(() => import("./pages/admin/questionManagement/question.list.page"));
const EmailTemplateListPage = React.lazy(() => import("./pages/admin/emailTemplateManagement/email.template.list.page"));

const TechnicalSkillListPage = React.lazy(() => import("./pages/admin/technicalSkillManagement/technical.skill.list.page"));
const Profile = React.lazy(() => import("./pages/admin/userProfile/user.profile"));

const UserManagementPage = React.lazy(() =>
  import("./pages/admin/userManagement/user.management.list.page")
);
const routes = [
  {
    path: "/",
    exact: true,
    name: "Admin",
    component: DefaultLayout
  },
  { path: "/dashboard", name: "Dashboard", component: Dashboard },
  { path: "/candidates", name: "Candidate", component: CandidateListPage },

  { path: "/jobs", name: "Job", component: JobListPage },
  { path: "/calendars", name: "Calendar", component: CalendarListPage },
  { path: "/categories", name: "Job Category", component: CategoryListPage },
  { path: "/employees", name: "Employee", component: EmployeeListPage },
  { path: "/jobs", name: "Job", component: JobListPage },
  { path: "/interviews", name: "Interview Page", component: InterviewListPage },
  { path: "/emailtemplates", name: "Email Template", component: EmailTemplateListPage },
  { path: "/calendars", name: "Calendar", component: CalendarListPage },
  { path: "/technicalSkills", name: "TechnicalSkill", component: TechnicalSkillListPage },
  { path: "/interviewQAs", name: "InterviewQA", component: QuestionListPage },
  { path: "/users/:userId", name: "User Profile", component: Profile },
  {
    path: "/user-management",
    name: "User Management",
    component: UserManagementPage
  }
];


export default routes;
