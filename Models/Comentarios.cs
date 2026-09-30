namespace TPRedSocial.Models
{
    public class Comentarios
    {
        public int Id { get; set; }
        public int IdPublicacion { get; set; }
        public int IdUsuarioComenta { get; set; }
        public string Texto { get; set; }
        public DateTime FechaComentario { get; set; }
        public string NombreUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public Comentarios()
        {
        }

        public Comentarios(int idPublicacion, int idUsuarioComenta, string texto, DateTime fechaComentario)
        {
            IdPublicacion = idPublicacion;
            IdUsuarioComenta = idUsuarioComenta;
            Texto = texto;
            FechaComentario = fechaComentario;
        }
    }
}
