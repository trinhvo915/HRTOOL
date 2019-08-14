using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Common.Constants
{
    public static class DepartmentConstants
    {
        public const string SA = "1033D9FB-FAF4-43DB-8D0D-83185400ABCD";
        public static Guid OfficeManager = new Guid(SA);

        public const string SB = "1033D9FB-FAF4-43DB-8D0D-8318540ABCDD";
        public static Guid Recruitment = new Guid(SB);

        public const string SC = "1033D9FB-FAF4-43DB-8D0D-8318540ABCDA";
        public static Guid CB = new Guid(SC);

        public const string SD = "1033D9FB-FAF4-43DB-8D0D-8318540AAAAA";
        public static Guid AdminReceptionist = new Guid(SD);

        public const string SE = "1033D9FB-FAF4-43DB-8D0D-8318540AACAA";
        public static Guid AdminEngagement = new Guid(SE);
    }
}
