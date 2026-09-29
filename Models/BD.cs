using Dapper;
using Microsoft.Data.SqlClient;


namespace TPRedSocial.Models
{
    public class BD
    {
        private static string _connectionString = @"Server=localhost; DataBase=tpRedSocial;Integrated Security=True;TrustServerCertificate=True;";

        public string BuscarSesion(string nombreUsuario, string contraseña)
        {
            string id = "-1";
            string query = "SELECT id FROM Usuarios WHERE NombreUsuario = @NombreUsuario AND Contraseña = @Contraseña";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                if(connection.QueryFirstOrDefault<string>(query, new { NombreUsuario = nombreUsuario, Contraseña = contraseña }) != null)
                {
                    id = connection.QueryFirstOrDefault<string>(query, new { NombreUsuario = nombreUsuario, Contraseña = contraseña }).ToString();
                }
            }
            return id;
        }

        public Usuario MostrarUsuario(int id){
            Usuario user = null;
            string query = "SELECT * FROM Usuarios WHERE Id = @Id";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                user = connection.QueryFirstOrDefault<Usuario>(query, new { Id = id });
            }
            return user;
        }

        public void CrearUsuario(Usuario user){
            string query = "INSERT INTO Usuarios (NombreUsuario, Contraseña, Nombre, Apellido) VALUES (@NombreUsuario, @Contraseña, @Nombre, @Apellido)";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, new { NombreUsuario = user.nombreUsuario, Contraseña = user.contraseña, Nombre = user.nombre, Apellido = user.apellido });
            }
        }

        public bool ValidarNombreUsuario(string nombreUsuario){
            string query = "SELECT COUNT(NombreUsuario) FROM Usuarios WHERE NombreUsuario = @NombreUsuario";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                int count = connection.QueryFirstOrDefault<int>(query, new { NombreUsuario = nombreUsuario });
                if (count > 0)
                {
                    return false;
                }  
            }
            return true;
        }

        public List<Publicaciones> MostrarPublicaciones(){
            List<Publicaciones> publicaciones = new List<Publicaciones>();
            string query = "SELECT * FROM Publicaciones";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                publicaciones = connection.Query<Publicaciones>(query).ToList();
            }
            return publicaciones;
        }

        public void CrearPublicacion(Publicaciones publicacion){
            string query = "INSERT INTO Publicaciones (Imagen, Titulo, Descripcion, FechaPublicacion, IdUsuario) VALUES (@Imagen, @Titulo, @Descripcion, @FechaPublicacion, (SELECT Id FROM Usuarios WHERE NombreUsuario = @NombreUsuario))";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, new { Imagen = publicacion.Imagen, Titulo = publicacion.Titulo, Descripcion = publicacion.Descripcion, FechaPublicacion = publicacion.FechaPublicacion, NombreUsuario = publicacion.NombreUsuario });
            }
        }
    }
}
