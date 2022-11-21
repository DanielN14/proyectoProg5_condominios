namespace proyectoDB2_condominios.Models
{
    public class Usuario
    {
        public int idUsuario { get; set; }
        public string? nombre { get; set; }
        public string? primerApellido { get; set; }
        public string? segundoApellido { get; set; }
        public string? cedula { get; set; }
        public string? foto { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public int idRolUsuario { get; set; }
        public string? nombreRol { get; set; }
        public int? idPersona { get; set; }
        public int? idProyectoHabitacional { get; set; }
        public string? nombreCondominio { get; set; }
    }
}
