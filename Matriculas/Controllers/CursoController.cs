using Matriculas.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Matriculas.Controllers
{
    public class CursoController : Controller
    {
        Uri direccion = new Uri("https://localhost:7117/api");
        private readonly HttpClient httpClient;

        public CursoController()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = direccion;
        }

        public List<Curso> aCurso()
        {
            List<Curso> lista = new List<Curso>();
            HttpResponseMessage response = httpClient.GetAsync(httpClient.BaseAddress + "/Matricula/ListadoCursos").Result;
            var data = response.Content.ReadAsStringAsync().Result;
            lista = JsonConvert.DeserializeObject<List<Curso>>(data);
            return lista;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult listadoCursos()
        {
            return View(aCurso());
        }
    }
}
