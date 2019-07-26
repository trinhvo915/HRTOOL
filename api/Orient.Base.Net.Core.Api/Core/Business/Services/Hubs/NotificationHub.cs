using Microsoft.AspNetCore.SignalR;
using Orient.Base.Net.Core.Api.Core.Business.Models.Calendars;
using Orient.Base.Net.Core.Api.Core.Common.Helpers;
using Orient.Base.Net.Core.Api.Core.SignalRNotification.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Services.Hubs
{
    public class NotificationHub : Hub
    {
        private static HashSet<UserInfor> userInfors = new HashSet<UserInfor>();

        public Guid GetCurrentUserId()
        {
            var userId = new Guid();

            var accessToken = Context.GetHttpContext().Request.Query["access_token"];
            if (!string.IsNullOrEmpty(accessToken))
            {
                var jwtPayload = JwtHelper.ValidateToken(accessToken.ToString());
                if (jwtPayload != null)
                {
                    userId = jwtPayload.UserId;
                }
            }
            return userId;
        }

        public override Task OnConnectedAsync()
        {
            var userInfor = new UserInfor(Context.ConnectionId, GetCurrentUserId());
            userInfors.Add(userInfor);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userDisConnectId = Context.ConnectionId;
            userInfors.Remove(userInfors.FirstOrDefault(x => x.IdConnect == userDisConnectId));
            return base.OnDisconnectedAsync(exception);
        }

        public async Task sendMessage(string nameSender, Guid[] IdUsers, string secalendarDescription, string type)
        {
            await Clients.Clients(userInfors
                .Where(x => IdUsers.Contains(x.IdUser)).Select(x => x.IdConnect).ToArray())
                .SendAsync("Receive", nameSender, IdUsers, secalendarDescription, type);
        }

        public async Task sendUpdate(string nameSender, Guid[] IdUsers, Guid[] IdUserBefores, string secalendarDescription, string type)
        {
            Guid [] IdUserAdd = IdUsers.Where(x => !IdUserBefores.Contains(x)).ToArray();
            Guid [] IdUserDelete = IdUserBefores.Where(x => !IdUsers.Contains(x)).ToArray();
            Guid [] IdUserAll = IdUserBefores.Concat(IdUserAdd).ToArray();

            await Clients.Clients(userInfors
                .Where(x => IdUserAll.Contains(x.IdUser)).Select(x => x.IdConnect).ToArray())
                .SendAsync("ReceiveUpdate", nameSender, IdUserAdd, IdUserDelete, secalendarDescription, type);
        }
    }
}
