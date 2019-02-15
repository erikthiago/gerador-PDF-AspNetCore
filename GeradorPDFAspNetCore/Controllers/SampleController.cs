using DinkToPdf;
using DinkToPdf.Contracts;
using GeradorPDFAspNetCore.Models;
using GeradorPDFAspNetCore.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GeradorPDFAspNetCore.Controllers
{
    [Route("api/sample")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly IViewRenderService _viewRenderService;
        private IConverter _converter;

        public SampleController(IViewRenderService viewRenderService, IConverter converter)
        {
            _viewRenderService = viewRenderService;
            _converter = converter;
        }

        Person person = new Person()
        {
            Id = Guid.NewGuid(),
            Name = "Primeiro",
            LastName = "Ultimo"
        };

        [Route("pdf")]
        public async Task<IActionResult> DinkToPDF()
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
                PaperSize = PaperKind.A4Plus,
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = await _viewRenderService.RenderToStringAsync("DinkToPDF", person),
                        WebSettings = { DefaultEncoding = "utf-8" },
                        HeaderSettings = { FontSize = 9, Right = "Página [page] de [toPage]", Line = true, Spacing = 2.812 }
                    }
                }
            };

            byte[] pdf = _converter.Convert(doc);

            return File(pdf, "application/pdf");
        }
    }
}