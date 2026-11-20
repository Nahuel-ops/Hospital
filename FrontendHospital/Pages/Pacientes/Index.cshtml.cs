using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ContratoHospital;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontendHospital.Pages.Pacientes
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public PacienteDTOs NuevoPaciente { get; set; } = new();

        public List<PacienteItem> ListaPacientes { get; set; } = new();

        [TempData]
        public string? Mensaje { get; set; }

        public int PaginaActual { get; set; } = 1;
        public int TotalPaginas { get; set; } = 1;
        public int TotalRegistros { get; set; } = 0;
        private const int RegistrosPorPagina = 10;

        private HttpClient? CrearClienteAutenticado()
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token)) return null;

            var client = _httpClientFactory.CreateClient("Api");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        public async Task<IActionResult> OnGetAsync(int pagina = 1)
        {
            var client = CrearClienteAutenticado();
            if (client == null) return RedirectToPage("/Auth/Login");

            var autorizado = await CargarPacientes(client, pagina);
            if (!autorizado)
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Auth/Login");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = CrearClienteAutenticado();
            if (client == null) return RedirectToPage("/Auth/Login");

            if (!ModelState.IsValid)
            {
                await CargarPacientes(client, 1);
                return Page();
            }

            var respuesta = await client.PostAsJsonAsync("api/Paciente", NuevoPaciente);
            if (respuesta.StatusCode == HttpStatusCode.Unauthorized)
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Auth/Login");
            }

            Mensaje = respuesta.IsSuccessStatusCode
                ? "Paciente registrado correctamente."
                : await respuesta.Content.ReadAsStringAsync();

            return RedirectToPage();
        }

        private async Task<bool> CargarPacientes(HttpClient client, int pagina)
        {
            var response = await client.GetAsync("api/Paciente");
            if (response.StatusCode == HttpStatusCode.Unauthorized) return false;

            response.EnsureSuccessStatusCode();
            var resultado = await response.Content.ReadFromJsonAsync<List<PacienteItem>>();

            if (resultado != null)
            {
                TotalRegistros = resultado.Count;
                TotalPaginas = Math.Max(1, (int)Math.Ceiling(TotalRegistros / (double)RegistrosPorPagina));
                PaginaActual = Math.Clamp(pagina, 1, TotalPaginas);

                ListaPacientes = resultado
                    .Skip((PaginaActual - 1) * RegistrosPorPagina)
                    .Take(RegistrosPorPagina)
                    .ToList();
            }

            return true;
        }

        public class PacienteItem
        {
            public Guid Id { get; set; }
            public string Nombre { get; set; } = string.Empty;
            public string Apellido { get; set; } = string.Empty;
            public int Edad { get; set; }
            public string EnfermedadesBase { get; set; } = string.Empty;
        }
    }
}
