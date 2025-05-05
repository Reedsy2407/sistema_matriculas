
using MatriculasMODELS;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Matriculas.Controllers
{
    public class AulaController : Controller
    {
        Uri direccion = new Uri("https://localhost:7117/Aula/");
        private readonly HttpClient httpClient;

        public AulaController()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = direccion;
        }

        public List<Aula> aAula()
        {
            List<Aula> lista = new List<Aula>();
            HttpResponseMessage response = httpClient
                .GetAsync(httpClient.BaseAddress + "ListadoAulas")
                .Result;
            var data = response.Content.ReadAsStringAsync().Result;
            lista = JsonConvert.DeserializeObject<List<Aula>>(data);
            return lista;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult listadoAulas()
        {
            return View(aAula());
        }

        [HttpGet]
        public IActionResult registrarAula()
        {
            return View(new Aula());
        }

        [HttpPost]
        public async Task<IActionResult> registrarAula(Aula objA)
        {
            if (!ModelState.IsValid)
                return View(objA);

            var json = JsonConvert.SerializeObject(objA);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("RegistrarAulas", content);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Mensaje = "Aula registrada correctamente";
                return View(new Aula());
            }
            else
            {
                ViewBag.Mensaje = "Error al registrar aula";
                return View(objA);
            }
        }

        // GET: Aula/editarAula/{cod}
        [HttpGet]
        public async Task<IActionResult> editarAula(int cod)
        {
            var response = await httpClient.GetAsync($"BuscarAulas/{cod}");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Mensaje = "No se encontró el aula con código " + cod;
                return View(new Aula { id_aula = cod });
            }

            var data = await response.Content.ReadAsStringAsync();
            var objA = JsonConvert.DeserializeObject<Aula>(data)
                      ?? new Aula { id_aula = cod };
            return View(objA);
        }

        // POST: Aula/editarAula
        [HttpPost]
        public async Task<IActionResult> editarAula(Aula objA)
        {
            if (!ModelState.IsValid)
                return View(objA);

            var json = JsonConvert.SerializeObject(objA);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient
                .PutAsync("ActualizarAulas", content);

            ViewBag.Mensaje = response.IsSuccessStatusCode
                ? "Aula modificada correctamente"
                : "Error al modificar aula";

            return View(objA);
        }
    }
}
