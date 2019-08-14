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
      name: "Calendar",
      url: "/calendars",
      icon: "cui-calendar"
    },

    {
      name: "Task",
      icon: "cui-task",
      children: [
        {
          name: "Job Category",
          url: "/categories",
          icon: "cui-list",
        },
        {
          name: "Job",
          url: "/Jobs",
          icon: "cui-magnifying-glass"
        }
      ]
    },

    {
      name: "Interview",
      icon: "cui-star",
      children: [
        {
          name: "TechnicalSkill",
          url: "/technicalSkills",
          icon: "cui-calendar"
        },
        {
          name: "Candidate",
          url: "/candidates",
          icon: "fa fa-user"
        },
        {
          name: "InterviewQA",
          url: "/interviewQAs",
          icon: "fa fa-file-pdf-o"
        },
        {
          name: "Interview Arrangement",
          url: "/interviews",
          icon: "fa fa-address-card"
        },
      ]
    },

    {
      name: "Setting",
      icon: "cui-settings ",
      children: [
        {
          name: "User Management",
          url: "/user-management",
          icon: "fa fa-users "
        },
        {
          name: "Department",
          url: "/department",
          icon: "cui-globe icons"
        },
        {
          name: "Email Template",
          url: "/emailtemplates",
          icon: "cui-envelope-letter"
        },
        {
          name: "Report",
          url: "/report",
          icon: "cui-file"
        },
      ]
    }

  ]
};
