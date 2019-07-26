using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.SignalRNotification.Hubs
{
    public class UserInfor
    {
        public string IdConnect { get; set; }

        public Guid IdUser { get; set; }

        public UserInfor()
        {

        }

        public UserInfor(string IdConnect, Guid IdUser)
        {
            this.IdConnect = IdConnect;
            this.IdUser = IdUser;
        }
    }
}
