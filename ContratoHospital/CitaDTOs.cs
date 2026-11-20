namespace ContratoHospital
{
    public class CitaDTOs
    {
        public DateOnly Fecha { get; set; }
        public TimeOnly Hora { get; set; }
        public string Comentario { get; set; } = string.Empty;
        public Guid IdPaciente { get; set; }
        public Guid IdMedico { get; set; }
    }
}
