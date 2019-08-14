using System;

namespace Orient.Base.Net.Core.Api.Core.Common.Constants
{
    /// <summary>
    /// 
    /// </summary>
    public static class UserConstants
    {
        /// <summary>
        /// Super Admin
        /// </summary>
        public const string SA = "1033D9FB-FAF4-43DB-8D0D-83185400DFE2";
        public static Guid SuperAdminUserId = new Guid(SA);

        /// <summary>
        /// User
        /// </summary>
        public const string SB = "1033D9FB-FAF4-43DB-8D0D-83185400DFE3";
        public static Guid SuperUserId = new Guid(SB);

        public static string Color = "#c2dff5";
    }
}
