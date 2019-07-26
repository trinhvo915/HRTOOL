using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;
using Orient.Base.Net.Core.Api.Core.Business.Filters;
using Orient.Base.Net.Core.Api.Core.Common.Helpers;
using System.Collections.Generic;

namespace Orient.Base.Net.Core.Api.Controllers
{
    [Route("api/media")]
    [EnableCors("CorsPolicy")]
    public class MediaController : BaseController
    {
        [HttpPost]
        [CustomAuthorize]
        public async Task<IActionResult> UploadFile(string folder, string fileName, IFormFile file)
        {
            var t1 = Task.Run(() => FileHelper.SaveFile(folder, fileName, file));

            await Task.WhenAll(t1);

            var extension = Path.GetExtension(file.FileName);

            return Ok(Url.Action("GetFile", "Media", new { folder = folder, fileName = string.Format("{0}{1}", fileName, extension) }, Request.Scheme));
        }

        [HttpPost("upload-files")]
        [CustomAuthorize]
        public async Task<IActionResult> UploadFiles(string folder, string fileName, List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return NotFound();
            }

            var result = new List<string>();
            var index = 1;
            foreach (var file in files)
            {
                var fileSavingName = $"{fileName}_{index++}";

                var t1 = Task.Run(() => FileHelper.SaveFile(folder, fileSavingName, file));

                await Task.WhenAll(t1);

                var extension = Path.GetExtension(file.FileName);

                result.Add(Url.Action("GetFile", "Media", new { folder = folder, fileName = string.Format("{0}{1}", fileSavingName, extension) }, Request.Scheme));
            }

            return Ok(result);
        }

        [HttpGet("{folder}/{fileName}")]
		public async Task<IActionResult> GetFile(string folder, string fileName)
		{
			var t1 = Task.Run(() => FileHelper.GetFile(folder, fileName));

            await Task.WhenAll(t1);

            var fileStream = System.IO.File.OpenRead(t1.Result);

            string contentType = GetMimeType(fileName);

            return base.File(fileStream, contentType);
        }

        private string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }
    }
}