namespace proyectoDB2_condominios.Models
{
    public class Visitante
    {
        public int idVisitante { get; set; }
        public string? nombre  { get; set; }
        public string? primerApellido { get; set; }
        public string? segundoApellido { get; set; }
        public string? cedula { get; set; }
        public char favorito { get; set; }
        public int idPersona { get; set; }

    }
}
