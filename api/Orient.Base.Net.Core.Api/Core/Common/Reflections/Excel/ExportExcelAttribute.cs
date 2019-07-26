using System;

namespace Orient.Base.Net.Core.Api.Core.Common.Reflections.Excel
{
    public class ExportExcelAttribute : Attribute
    {
        public string DisplayName { get; set; }

        public int Priority { get;  set; }
    }
}
