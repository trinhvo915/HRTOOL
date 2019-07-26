export default {
  items: [
    {
      name: "Dashboard",
      url: "/dashboard",
      icon: "icon-speedometer",
      badge: {
        variant: "info",
        text: "NEW"
      }
    },
    {
      name: "Job List",
      url: "/jobs",
      // permissions: ["user in job"],
      icon: "fa fa-list"
    },
    {
      name: "Calendar ",
      url: "/calendars",
      icon: "fa fa-calendar"
    },
    {
      name: "InterviewQA",
      url: "/interviewQAs",
      icon: "fa fa-file-pdf-o"
    }
  ]
};
