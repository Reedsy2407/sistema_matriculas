using Microsoft.AspNetCore.Mvc;

namespace Matriculas.Controllers
{
    public class ClaseController : Controller
    {

        Uri direccion = new Uri("https://localhost:7117/Clase/");
        private readonly HttpClient httpClient;

        public ClaseController()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = direccion;
        }
        public IActionResult seleccionCarrera()
        {|
            return View();
        }
    }
}
