namespace proyectoDB2_condominios.Models
{
    public class VisitasDelivery
    {
        public int idVisita { get; set; }
        public DateTime FechaEntrada { get; set; }
        public  string? proveedorDelivery { get; set; }
    }

    public class VisitasTradicionales
    {
        public int idVisita { get; set; }
        public DateTime FechaEntrada { get; set; }
        public  string? nomCompletoVisitante { get; set; }
        public  string? cedula { get; set; }
    }

    public class VisitasDelivery_Guarda
    {
        public int idVisita { get; set; }
        public DateTime FechaEntrada { get; set; }
        public string? numeroVivienda { get; set; }
        public  string? nombreResidente { get; set; }
        public  string? proveedorDelivery { get; set; }
        public string? condominio { get; set; }
    }

    public class VisitasTradicionales_Guarda
    {
        public int idVisita { get; set; }
        public DateTime FechaEntrada { get; set; }
        public string? numeroVivienda { get; set; }
        public  string? nombreResidente { get; set; }
        public  string? nombreVisitante { get; set; }
        public  string? cedulaVisitante { get; set; }
        public  string? placaVehiculo { get; set; }
        public  string? marcaVehiculo { get; set; }
        public string? condominio { get; set; }
    }
}