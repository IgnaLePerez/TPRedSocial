namespace TPRedSocial.Models
{
    public class Publicaciones
    {
        public string Imagen { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public string NombreUsuario { get; set; }

        public Publicaciones()
        {
        }

        public Publicaciones(string imagen, string titulo, string descripcion, DateTime fechaPublicacion, string nombreUsuario)
        {
            Imagen = imagen;
            Titulo = titulo;
            Descripcion = descripcion;
            FechaPublicacion = fechaPublicacion;
            NombreUsuario = nombreUsuario;
        }
    }
}
