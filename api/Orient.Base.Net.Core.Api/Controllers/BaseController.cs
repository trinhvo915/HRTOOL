using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Orient.Base.Net.Core.Api.Core.Common.Helpers;
using System;

namespace Orient.Base.Net.Core.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid CurrentUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BaseController()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionExecutingContext"></param>
        public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
        {
            var accessToken = actionExecutingContext.HttpContext.Request.Headers["x-access-token"];
            if (!string.IsNullOrEmpty(accessToken))
            {
                var jwtPayload = JwtHelper.ValidateToken(accessToken.ToString());
                if (jwtPayload != null)
                {
                    CurrentUserId = jwtPayload.UserId;
                }
            }

            base.OnActionExecuting(actionExecutingContext);
        }
    }
}
