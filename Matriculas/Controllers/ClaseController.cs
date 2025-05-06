using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MatriculasMODELS;

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

        public async Task<IActionResult> seleccionCarrera()
        {
            var response = await httpClient.GetAsync("Periodo/ObtenerPeriodoActual");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var periodo = JsonConvert.DeserializeObject<Periodo>(content);
                ViewBag.CodigoPeriodoActual = periodo?.codigo_periodo; 
            }
            else
            {
                ViewBag.CodigoPeriodoActual = null;
            }

            return View();
        }
    }
}