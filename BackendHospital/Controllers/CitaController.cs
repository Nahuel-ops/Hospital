using BackendHospital.Interfaces;
using ContratoHospital;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendHospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CitaController : ControllerBase
    {
        private readonly ICita _citaService;

        public CitaController(ICita citaService)
        {
            _citaService = citaService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_citaService.ObtenerTodos());
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var cita = _citaService.ObtenerPorId(id);
            if (cita == null)
            {
                return NotFound("El Id no se encuentra en cita");
            }
            return Ok(cita);
        }

        [HttpPost]
        public IActionResult Post(CitaDTOs dto)
        {
            _citaService.Crear(dto);
            return Ok("Cita registrada correctamente");
        }
    }
}
