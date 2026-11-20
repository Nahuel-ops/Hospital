using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ContratoHospital;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontendHospital.Pages.Citas
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public CitaDTOs NuevaCita { get; set; } = new()
        {
            Fecha = DateOnly.FromDateTime(DateTime.Now),
            Hora = new TimeOnly(9, 0)
        };

        public List<CitaItem> ListaCitas { get; set; } = new();
        public List<PersonaOpcion> PacientesDisponibles { get; set; } = new();
        public List<MedicoOpcion> MedicosDisponibles { get; set; } = new();

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

            var autorizado = await CargarDatos(client, pagina);
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
                await CargarDatos(client, 1);
                return Page();
            }

            var respuesta = await client.PostAsJsonAsync("api/Cita", NuevaCita);
            if (respuesta.StatusCode == HttpStatusCode.Unauthorized)
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Auth/Login");
            }

            Mensaje = respuesta.IsSuccessStatusCode
                ? "Cita médica registrada correctamente."
                : await respuesta.Content.ReadAsStringAsync();

            return RedirectToPage();
        }

        private async Task<bool> CargarDatos(HttpClient client, int pagina)
        {
            var respCitas = await client.GetAsync("api/Cita");
            if (respCitas.StatusCode == HttpStatusCode.Unauthorized) return false;
            respCitas.EnsureSuccessStatusCode();

            var citas = await respCitas.Content.ReadFromJsonAsync<List<CitaItem>>();
            if (citas != null)
            {
                TotalRegistros = citas.Count;
                TotalPaginas = Math.Max(1, (int)Math.Ceiling(TotalRegistros / (double)RegistrosPorPagina));
                PaginaActual = Math.Clamp(pagina, 1, TotalPaginas);

                ListaCitas = citas
                    .Skip((PaginaActual - 1) * RegistrosPorPagina)
                    .Take(RegistrosPorPagina)
                    .ToList();
            }

            PacientesDisponibles = await client.GetFromJsonAsync<List<PersonaOpcion>>("api/Paciente") ?? new();
            MedicosDisponibles = await client.GetFromJsonAsync<List<MedicoOpcion>>("api/Medico") ?? new();

            return true;
        }

        public class CitaItem
        {
            public Guid Id { get; set; }
            public DateOnly Fecha { get; set; }
            public TimeOnly Hora { get; set; }
            public string Comentario { get; set; } = string.Empty;
            public PersonaOpcion? Paciente { get; set; }
            public MedicoOpcion? Medico { get; set; }
        }

        public class PersonaOpcion
        {
            public Guid Id { get; set; }
            public string Nombre { get; set; } = string.Empty;
            public string Apellido { get; set; } = string.Empty;
        }

        public class MedicoOpcion
        {
            public Guid Id { get; set; }
            public string Nombre { get; set; } = string.Empty;
            public string Apellido { get; set; } = string.Empty;
            public string Especialidad { get; set; } = string.Empty;
        }
    }
}
