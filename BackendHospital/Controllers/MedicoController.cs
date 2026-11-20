using BackendHospital.Interfaces;
using ContratoHospital;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendHospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MedicoController : ControllerBase
    {
        private readonly IMedico _medicoService;

        public MedicoController(IMedico medicoService)
        {
            _medicoService = medicoService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_medicoService.ObtenerTodos());
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var medico = _medicoService.ObtenerPorId(id);
            if (medico == null)
            {
                return NotFound("El Id no se encuentra en médico");
            }
            return Ok(medico);
        }

        [HttpPost]
        public IActionResult Post(MedicoDTOs dto)
        {
            _medicoService.Crear(dto);
            return Ok("Médico registrado correctamente");
        }
    }
}
