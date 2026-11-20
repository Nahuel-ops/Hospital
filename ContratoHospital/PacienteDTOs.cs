namespace ContratoHospital
{
    public class PacienteDTOs
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public int Edad { get; set; }
        public string EnfermedadesBase { get; set; } = string.Empty;
    }
}
