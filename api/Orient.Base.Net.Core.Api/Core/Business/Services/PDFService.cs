using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Orient.Base.Net.Core.Api.Core.Business.Models.Questions;
using Orient.Base.Net.Core.Api.Core.Common.Constants;
using Orient.Base.Net.Core.Api.Core.Common.Helpers;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;
using RazorLight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Services
{
    public interface IPDFService
    {
        Task<ResponseModel> CreatePDFAsync(IEnumerable data, string templatePath, string fileName);
    }

    public class PDFService : IPDFService
    {
        private readonly IRazorLightEngine _razorEngine;
        private readonly IConverter _pdfConverter;

        private readonly IHostingEnvironment _hostingEnvironment;

        public PDFService(IRazorLightEngine razorEngine, IConverter pdfConverter, IHostingEnvironment env)
        {
            _razorEngine = razorEngine;
            _pdfConverter = pdfConverter;
            _hostingEnvironment = env;
        }

        public async Task<ResponseModel> CreatePDFAsync(IEnumerable data, string templatePath, string fileName)
        {
            var pdfTemplatePath = Path.Combine(_hostingEnvironment.ContentRootPath, templatePath);
            string template = await _razorEngine.CompileRenderAsync(pdfTemplatePath, data);

            var filePath = FileHelper.MapPath("/Files/PDFDocument");

            // Create if not exist
            Directory.CreateDirectory(filePath);

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings() { Top = 10, Bottom = 10, Left = 10, Right = 10 },
                DocumentTitle = fileName,
                Out = Path.Combine(_hostingEnvironment.ContentRootPath, $"wwwroot\\Files\\PDFDocument\\{fileName}")
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = template,
                WebSettings = { DefaultEncoding = "utf-8" },
                FooterSettings = { FontName = "Arial", FontSize = 12, Line = true, Right = "Page [page] of [toPage]" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            byte[] file = _pdfConverter.Convert(pdf);

            if (file == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotImplemented,
                    Message = MessageConstants.UNEXPECTED_ERROR
                };
            }

            return new ResponseModel()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = MessageConstants.CREATED_SUCCESSFULLY,
                Data = fileName
            };
        }

    }
}

