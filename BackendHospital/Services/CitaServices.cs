using BackendHospital.Data;
using BackendHospital.Interfaces;
using BackendHospital.Models;
using ContratoHospital;
using Microsoft.EntityFrameworkCore;

namespace BackendHospital.Services
{
    public class CitaServices : ICita
    {
        private readonly BaseDeDatos _context;

        public CitaServices(BaseDeDatos context)
        {
            _context = context;
        }

        public List<Cita> ObtenerTodos()
        {
            return _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .OrderByDescending(c => c.Fecha)
                .ThenBy(c => c.Hora)
                .ToList();
        }

        public Cita? ObtenerPorId(Guid id)
        {
            return _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .FirstOrDefault(c => c.Id == id);
        }

        public void Crear(CitaDTOs dto)
        {
            var cita = new Cita
            {
                Id = Guid.NewGuid(),
                Fecha = dto.Fecha,
                Hora = dto.Hora,
                Comentario = dto.Comentario,
                IdPaciente = dto.IdPaciente,
                IdMedico = dto.IdMedico
            };

            _context.Citas.Add(cita);
            _context.SaveChanges();
        }
    }
}
