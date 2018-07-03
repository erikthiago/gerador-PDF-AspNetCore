using GeradorPDFAspNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Rotativa.NetCore;
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
                Name = "Érik",
                LastName = "Thiago"
            };

            return new ViewAsPdf(person);
        }
    }
}