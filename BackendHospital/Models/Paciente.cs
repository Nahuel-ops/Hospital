using System.Text.Json.Serialization;

namespace BackendHospital.Models
{
    public class Paciente
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public int Edad { get; set; }
        public string EnfermedadesBase { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Cita>? Citas { get; set; }
    }
}
