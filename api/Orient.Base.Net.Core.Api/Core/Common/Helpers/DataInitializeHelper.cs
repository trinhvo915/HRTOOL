using Orient.Base.Net.Core.Api.Core.Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Common.Helpers
{
    public static class DataInitializeHelper
    {
        private const string ResourceNamespace = "Orient.Base.Net.Core.Api.Core.Business.Resources";

        /// <summary>
        /// Get resource content
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetResourceContent(string resourceName, DataSetupResourceType type = DataSetupResourceType.None)
        {
            if (type == DataSetupResourceType.None)
            {
                return EmbeddedResourceHelper.GetString(resourceName, ResourceNamespace);
            }
            return EmbeddedResourceHelper.GetString(string.Format("{0}.{1}", type.GetEnumName(), resourceName), ResourceNamespace);
        }

        /// <summary>
        /// Get module resource content
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="moduleResourceNameSpace"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetModuleResourceContent(string resourceName, string moduleResourceNameSpace, DataSetupResourceType type = DataSetupResourceType.None)
        {
            if (type == DataSetupResourceType.None)
            {
                return EmbeddedResourceHelper.GetString(resourceName, ResourceNamespace);
            }
            return EmbeddedResourceHelper.GetString(string.Format("{0}.{1}", type.GetEnumName(), resourceName), moduleResourceNameSpace);
        }
    }

    public enum DataSetupResourceType
    {
        [Description("EmailTemplates")]
        EmailTemplate = 1,

        [Description("")]
        None
    }
}
