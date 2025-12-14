using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerFinanc.Web.Models;
using System.Diagnostics;

namespace PerFinanc.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Sobre()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contato()
        {
            return View(new ContatoViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contato(ContatoViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
           
            TempData["ContatoOk"] = "Mensagem enviada! Valeu — já já a gente te responde 😄";
            return RedirectToAction(nameof(Contato));
        }
    }
}
