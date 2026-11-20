using System.Text.Json.Serialization;

namespace BackendHospital.Models
{
    public class Medico
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public int Telefono { get; set; }
        public string Especialidad { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Cita>? Citas { get; set; }
    }
}
