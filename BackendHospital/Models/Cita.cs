using System.ComponentModel.DataAnnotations.Schema;

namespace BackendHospital.Models
{
    public class Cita
    {
        public Guid Id { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly Hora { get; set; }
        public string Comentario { get; set; } = string.Empty;

        public Guid IdPaciente { get; set; }
        [ForeignKey("IdPaciente")]
        public Paciente? Paciente { get; set; }

        public Guid IdMedico { get; set; }
        [ForeignKey("IdMedico")]
        public Medico? Medico { get; set; }
    }
}
