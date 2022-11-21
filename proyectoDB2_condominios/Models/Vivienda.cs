namespace proyectoDB2_condominios.Models
{
    public class Vivienda
    {
        public int idVivienda { get; set; }
        public string? numeroVivienda { get; set; }
        public string? descripcion { get; set; }
        public int numeroHabitaciones { get; set; }
        public int cochera { get; set; }
        public int idProyectoHabitacional { get; set; }
        public int? idPersona { get; set; } 
    }
}
