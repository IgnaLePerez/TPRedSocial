namespace TPRedSocial.Models
{
    public class PublicacionesMeGusta
    {
        public int Id { get; set; }
        public int IdPublicacion { get; set; }
        public int IdUsuario { get; set; }

        public PublicacionesMeGusta()
        {
        }

        public PublicacionesMeGusta(int idPublicacion, int idUsuario)
        {
            IdPublicacion = idPublicacion;
            IdUsuario = idUsuario;
        }
    }
}
