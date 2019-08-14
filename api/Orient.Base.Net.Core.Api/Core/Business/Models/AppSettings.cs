using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Models
{
    public class AppSettings
    {
        public string ProfileImageFolder { get; set; }

        public string SendGridKey { get; set; }

        public string EmailFrom { get; set; }

        public string FromName { get; set; }

        public string Secret { get; set; }

        public string AccountSid { get; set; }

        public string AuthToken { get; set; }

        public string UrlServer { get; set; }

        public string UrlUser { get; set; }

        public string Admin { get; set; }

        public string Password { get; set; }

        public string Domain { get; set; }

        public string SearchFilter { get; set; }
    }
}
