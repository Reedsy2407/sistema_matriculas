using MatriculasMODELS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                BaseAddress = new System.Uri("https://localhost:7117/")
            };
        }

        private List<SelectListItem> aCarreras()
        {
            HttpResponseMessage resp = httpClient.GetAsync("Carrera/ListadoCarreras").Result;
            string json = resp.Content.ReadAsStringAsync().Result;
            List<Carrera> list = JsonConvert.DeserializeObject<List<Carrera>>(json)!;
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (Carrera c in list)
            {
                items.Add(new SelectListItem
                {
                    Value = c.id_carrera.ToString(),
                    Text = c.nom_carrera
                });
            }
            return items;
        }

        private List<Alumno> aAlumnos()
        {
            HttpResponseMessage response = httpClient.GetAsync("Alumno/ListadoAlumnos").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<Alumno>>(data)!;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult listadoAlumnos()
        {
            var alumnos = aAlumnos();

            var dict = new Dictionary<int, string>();
            foreach (var a in alumnos)
            {
                var resp = httpClient
                    .GetAsync($"Alumno/HorariosMatriculados/{a.id_usuario}?idPeriodo=1")
                    .Result;

                dict[a.id_usuario] = resp.IsSuccessStatusCode
                    ? resp.Content.ReadAsStringAsync().Result
                    : "<span class=\"text-muted\">Error al cargar horarios</span>";
            }
            ViewBag.Horarios = dict;
            return View(alumnos);
        }

        [HttpGet]
        public IActionResult registrarAlumno()
        {
            ViewBag.Carreras = aCarreras();
            return View(new AlumnoO());
        }

        [HttpPost]
        public IActionResult registrarAlumno(AlumnoO model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Carreras = aCarreras();
                return View(model);
            }

            string json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage resp = httpClient.PostAsync("Alumno/RegistrarAlumno", content).Result;

            ViewBag.Mensaje = resp.IsSuccessStatusCode
                ? "Alumno registrado correctamente"
                : "Error al registrar alumno";

            ViewBag.Carreras = aCarreras();
            return View(new AlumnoO());
        }

        [HttpGet]
        public IActionResult editarAlumno(int id)
        {
            HttpResponseMessage resp = httpClient.GetAsync($"Alumno/BuscarAlumno/{id}").Result;
            if (!resp.IsSuccessStatusCode)
            {
                ViewBag.Mensaje = "No se encontró el alumno con ID " + id;
                ViewBag.Carreras = aCarreras();
                return View(new AlumnoO { id_usuario = id });
            }

            AlumnoO model = JsonConvert.DeserializeObject<AlumnoO>(resp.Content.ReadAsStringAsync().Result)!;

            List<SelectListItem> items = aCarreras();
            foreach (SelectListItem it in items)
            {
                if (model.id_carreras.Contains(int.Parse(it.Value!)))
                {
                    it.Selected = true;
                }
            }
            ViewBag.Carreras = items;
            return View(model);
        }

        [HttpPost]
        public IActionResult editarAlumno(AlumnoO model)
        {
            if (!ModelState.IsValid)
            {
                // volver a pintar y marcar
                List<SelectListItem> items = aCarreras();
                foreach (SelectListItem it in items)
                {
                    if (model.id_carreras.Contains(int.Parse(it.Value!)))
                    {
                        it.Selected = true;
                    }
                }
                ViewBag.Carreras = items;
                return View(model);
            }

            string json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage resp = httpClient.PutAsync("Alumno/ActualizarAlumno", content).Result;

            ViewBag.Mensaje = resp.IsSuccessStatusCode
                ? "Alumno modificado correctamente"
                : "Error al modificar alumno";

            List<SelectListItem> itemsPost = aCarreras();
            foreach (SelectListItem it in itemsPost)
            {
                if (model.id_carreras.Contains(int.Parse(it.Value!)))
                {
                    it.Selected = true;
                }
            }
            ViewBag.Carreras = itemsPost;

            return View(model);
        }


    }
}
