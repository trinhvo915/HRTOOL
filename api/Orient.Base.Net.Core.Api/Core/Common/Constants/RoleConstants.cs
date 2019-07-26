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
        /// BoD
        /// </summary>
        public const string BOD = "506F8FC8-816A-4505-82ED-E271447938FE";
        public static Guid BODId = new Guid(BOD);

        /// <summary>
        /// HR Manager
        /// </summary>
        public const string HRM = "5D8C397B-5F6B-42D6-87F6-1D73870F8798";
        public static Guid HRMId = new Guid(HRM);

        /// <summary>
        /// HR
        /// </summary>
        public const string HR = "F67AFD57-D2B5-41FF-8FDB-A7F08B57EA58";
        public static Guid HRId = new Guid(HR);

        /// <summary>
        /// Dev
        /// </summary>
        public const string Dev = "1EA026DB-41BF-4497-8498-94BEE0316C7B";
        public static Guid DevId = new Guid(Dev);
    }
}
