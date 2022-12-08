using BuilderWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace BuilderWeb.Controllers
{
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

        [HttpPost]
        public ActionResult Index(string valor)
        {
          
            bloque resultado = new bloque();

            resultado.bloqueA = obtenerTemperatura("Bloque A").Result;
            resultado.bloqueB = obtenerTemperatura("Bloque B").Result;
            resultado.bloqueC = obtenerTemperatura("Bloque C").Result;

            return View(resultado);
        }

        async Task<string> obtenerTemperatura(string bloque)
        {
            var c = new HttpClient();
            var r = await c.GetAsync("https://WebApi.azurewebsites.net/Adapter?Bloque=" + bloque);
            var json = await r.Content.ReadAsStringAsync();

            return json;

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}