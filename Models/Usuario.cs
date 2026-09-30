namespace TPRedSocial.Models
{
    public class Usuario
    {
        /*
        crea una clase "Usuario.cs" en Models en la cual hayan los siguientes atributos privados:

        nombreUsuario (string)
        contraseña (string)
        nombre (string)
        apellido (string)
        tipoUsuario (string)

        Crea el constructor de Usuario

        haz que los atributos de Usuario.cs se puedan acceder desde otros lados con getters y setters
        */
        public int id { get; set; }
        public string nombreUsuario { get; set; }
        public string contraseña { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }

        public Usuario(int id, string nombreUsuario, string contraseña, string nombre, string apellido)
        {
            this.nombreUsuario = nombreUsuario;
            this.contraseña = contraseña;
            this.nombre = nombre;
            this.apellido = apellido;
            this.id = id;
        }
    }
}