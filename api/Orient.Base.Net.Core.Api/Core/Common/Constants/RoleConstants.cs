using System;

namespace Orient.Base.Net.Core.Api.Core.Common.Constants
{
    /// <summary>
    /// 
    /// </summary>
    public static class RoleConstants
    {
        /// <summary>
        /// Super Admin
        /// </summary>
        public const string SA = "075c1072-92a2-4f99-ac11-bf985b23da6e";
        public static Guid SuperAdminId = new Guid(SA);

        /// <summary>
        /// Admin
        /// </summary>
        public const string AD = "F67AFD57-D2B5-41FF-8FDB-A7F08B57EA58";
        public static Guid AdminId = new Guid(AD);

        /// <summary>
        /// NormalUser
        /// </summary>
        public const string NM = "1EA026DB-41BF-4497-8498-94BEE0316C7B";
        public static Guid NormalUserId = new Guid(NM);


    }
}
