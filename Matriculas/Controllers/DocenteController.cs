using Microsoft.AspNetCore.Mvc;
using Matriculas.Models;
using Newtonsoft.Json;
using System.Text;



namespace Matriculas.Controllers
{
    public class DocenteController : Controller
    {

        Uri direccion = new Uri("https://localhost:7117/api");
        private readonly HttpClient httpClient;

        public DocenteController()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = direccion;
        }

        public List<Docente> aDocente()
        {
            List<Docente> lista = new List<Docente>();
            HttpResponseMessage response = httpClient.GetAsync(httpClient.BaseAddress + "/Matricula/ListadoDocentes").Result;
            var data = response.Content.ReadAsStringAsync().Result;
            lista = JsonConvert.DeserializeObject<List<Docente>>(data);
            return lista;
        }

        [HttpGet]
        public IActionResult nuevoDocente()
        {
            return View(new Docente());
        }

        [HttpPost]
        public async Task<IActionResult> nuevoDocente(Docente objD)
        {
            if (!ModelState.IsValid)
            {
                return View(objD);
            }

            var json = JsonConvert.SerializeObject(objD);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/api/Matricula/RegistrarDocentes", content);

            ViewBag.Mensaje = response.IsSuccessStatusCode
                ? "Docente registrado correctamente"
                : "Error al registrar docente";

            return View(objD);
        }

        [HttpGet]
        public async Task<IActionResult> editarDocente(int id)
        {
            var response = await httpClient.GetAsync(
                httpClient.BaseAddress + "/Matricula/BuscarDocentes/" + id
            );

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Mensaje = "No se encontró el docente con ID " + id;
                return View(new Docente());
            }

            var content = await response.Content.ReadAsStringAsync();
            var objD = JsonConvert.DeserializeObject<Docente>(content)
                      ?? new Docente { id_docente = id };

            return View(objD);
        }

        [HttpPost]
        public async Task<IActionResult> editarDocente(Docente objD)
        {
            if (!ModelState.IsValid)
                return View(objD);

            var json = JsonConvert.SerializeObject(objD);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync("/api/Matricula/ActualizarDocentes", content);

            ViewBag.Mensaje = response.IsSuccessStatusCode
                ? "Docente modificado correctamente"
                : "Error al modificar docente";

            return View(objD);
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult listadoDocentes()
        {
            return View(aDocente());
        }
    }
}
