using MatriculasMODELS;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Matriculas.Controllers
{
    public class AlumnoController : Controller
    {

        private readonly HttpClient httpClient;

        public AlumnoController()
        {
            httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("https://localhost:7117/Alumno/")
            };
        }
        private List<Alumno> aAlumnos()
        {
            var response = httpClient.GetAsync("ListadoAlumnos").Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<Alumno>>(data)!;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult listadoAlumnos()
        {
            return View(aAlumnos());
        }

        [HttpGet]
        public IActionResult registrarAlumno()
        {
            return View(new AlumnoO());
        }

        [HttpPost]
        public IActionResult registrarAlumno(AlumnoO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var resp = httpClient.PostAsync("RegistrarAlumno", content).Result;

            ViewBag.Mensaje = resp.IsSuccessStatusCode
                ? "Alumno registrado correctamente"
                : "Error al registrar alumno";

            return View(new AlumnoO());
        }

        [HttpGet]
        public IActionResult editarAlumno(int id)
        {
            var resp = httpClient.GetAsync($"BuscarAlumno/{id}").Result;
            if (!resp.IsSuccessStatusCode)
            {
                ViewBag.Mensaje = "No se encontró el alumno con ID " + id;
                return View(new AlumnoO { id_usuario = id });
            }

            var data = resp.Content.ReadAsStringAsync().Result;
            var alum = JsonConvert.DeserializeObject<AlumnoO>(data)!;
            return View(alum);
        }

        [HttpPost]
        public IActionResult editarAlumno(AlumnoO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var resp = httpClient.PutAsync("ActualizarAlumno", content).Result;

            ViewBag.Mensaje = resp.IsSuccessStatusCode
                ? "Alumno modificado correctamente"
                : "Error al modificar alumno";

            return View(model);
        }


    }
}
