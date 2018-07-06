using GeradorPDFAspNetCore.Models;
using jsreport.AspNetCore;
using jsreport.Types;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using System;

namespace GeradorPDFAspNetCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RotativaPDF()
        {
            Person person = new Person()
            {
                Id = Guid.NewGuid(),
                Name = "Primeiro",
                LastName = "Ultimo"
            };

            var pdf = new ViewAsPdf(person);

            return pdf;
        }

        [MiddlewareFilter(typeof(JsReportPipeline))]
        public IActionResult JSReportPDF()
        {
            Person person = new Person()
            {
                Id = Guid.NewGuid(),
                Name = "Primeiro",
                LastName = "Ultimo"
            };

            HttpContext.JsReportFeature().Recipe(Recipe.PhantomPdf);

            return View(person);
        }
    }
}