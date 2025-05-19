using MatriculasMODELS.Login;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Matriculas.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly Uri _baseAddress = new Uri("https://localhost:7117/Login");

        public LoginController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = _baseAddress;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_httpClient.BaseAddress, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseContent);

                    if (loginResponse.es_exito)
                    {
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            Expires = DateTime.Now.AddHours(1)
                        };

                        Response.Cookies.Append("id_usuario", loginResponse.id_usuario.ToString(), cookieOptions);
                        Response.Cookies.Append("id_rol", loginResponse.id_rol.ToString(), cookieOptions);


                        if (loginResponse.ultimo_acceso.HasValue)
                        {
                            string iso = loginResponse.ultimo_acceso.Value.ToString("o");
                            Response.Cookies.Append("ultimo_acceso", iso, cookieOptions);
                        }
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos");
                return View(request);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al conectar con el servicio: {ex.Message}");
                return View(request);
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("id_usuario");
            Response.Cookies.Delete("id_rol");
            return RedirectToAction("Login");
        }
    }
}
