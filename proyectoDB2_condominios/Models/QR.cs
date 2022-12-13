namespace proyectoDB2_condominios.Models
{
    public class QR
    {
        public int idQR { get; set; }
        public int idVivienda { get; set; }
        public int codigo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaExpiracion { get; set; }
    }
}
