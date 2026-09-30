using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TPRedSocial.Models;

namespace TPRedSocial.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWebHostEnvironment _env;

    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("id") == null){
            return RedirectToAction("VistaIniciarSesion");
        }
        BD bd = new BD();
        ViewBag.user = bd.MostrarUsuario(int.Parse(HttpContext.Session.GetString("id")));
        List<Publicaciones> publicaciones = bd.MostrarPublicaciones();
        return View(publicaciones);   
    }

    public IActionResult VistaIniciarSesion(){
        ViewBag.msj = "";
        return View("IniciarSesion");
    }


    [HttpPost]
    public IActionResult IniciarSesion(string nombreUsuario, string contraseña)
    {
        BD bd = new BD();
        HttpContext.Session.SetString("id", bd.BuscarSesion(nombreUsuario, contraseña));
        if (HttpContext.Session.GetString("id") == "-1"){
            ViewBag.msj = "No existe ese usuario :)";
            return View();
        }
        else{
            return RedirectToAction("Index");
        }
    }

    public IActionResult CerrarSesion(){
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    public IActionResult Registrarse(){
        ViewBag.msj = null;
        return View();
    }


    [HttpPost]
    public IActionResult RegistrarDatos(string nombreUsuario, string contraseña, string nombre, string apellido){
        BD bd = new BD();
        if (bd.ValidarNombreUsuario(nombreUsuario)){
            Usuario user = new Usuario(0, nombreUsuario, contraseña, nombre, apellido);
            bd.CrearUsuario(user);
            HttpContext.Session.SetString("id", bd.BuscarSesion(nombreUsuario, contraseña));
            return RedirectToAction("Index");
        }
        ViewBag.msj = "El nombre de usuario ya existe, por favor elija otro";
        return View("Registrarse");
    }

    public IActionResult CrearPublicacion(){
        if (HttpContext.Session.GetString("id") == null){
            return RedirectToAction("VistaIniciarSesion");
        }
        BD bd = new BD();
        ViewBag.user = bd.MostrarUsuario(int.Parse(HttpContext.Session.GetString("id")));
        return View();
    }

    [HttpPost]
    public IActionResult PostCrearPublicacion(IFormFile imagen, string titulo, string descripcion){
        if (HttpContext.Session.GetString("id") == null){
            return RedirectToAction("VistaIniciarSesion");
        }
        BD bd = new BD();
        ViewBag.user = bd.MostrarUsuario(int.Parse(HttpContext.Session.GetString("id")));

        string rutaCarpeta = Path.Combine(_env.WebRootPath, "imagenes");
        if (!Directory.Exists(rutaCarpeta))
            Directory.CreateDirectory(rutaCarpeta);

        string rutaCompleta = Path.Combine(rutaCarpeta, imagen.FileName);
        using (var stream = new FileStream(rutaCompleta, FileMode.Create))
        {
            imagen.CopyTo(stream);
        }
        Publicaciones publicacion = new Publicaciones(imagen.FileName, titulo, descripcion, DateTime.Now, ViewBag.user.nombreUsuario);
        bd.CrearPublicacion(publicacion);
        return View("Index");
    }

    [HttpPost]
    public IActionResult TogglearLike([FromBody] LikeRequest request)
    {
        if (HttpContext.Session.GetString("id") == null)
            return Unauthorized();

        if (request == null || request.IdPublicacion <= 0)
            return BadRequest();

        int idUsuario = int.Parse(HttpContext.Session.GetString("id"));
        BD bd = new BD();

        if (bd.UsuarioYaLikeó(request.IdPublicacion, idUsuario))
        {
            bd.EliminarLike(request.IdPublicacion, idUsuario);
        }
        else
        {
            bd.AgregarLike(request.IdPublicacion, idUsuario);
        }

        int cantidadLikes = bd.ObtenerCantidadLikes(request.IdPublicacion);
        bool usuarioYaLikeó = bd.UsuarioYaLikeó(request.IdPublicacion, idUsuario);

        return Json(new { cantidadLikes = cantidadLikes, usuarioYaLikeó = usuarioYaLikeó });
    }

    [HttpPost]
    public IActionResult AgregarComentario([FromBody] ComentarioRequest request)
    {
        if (HttpContext.Session.GetString("id") == null)
            return Unauthorized();

        if (request == null || request.IdPublicacion <= 0 || string.IsNullOrWhiteSpace(request.Texto))
            return BadRequest();

        int idUsuario = int.Parse(HttpContext.Session.GetString("id"));
        BD bd = new BD();

        Comentarios comentario = new Comentarios(request.IdPublicacion, idUsuario, request.Texto.Trim(), DateTime.Now);
        bd.CrearComentario(comentario);

        List<Comentarios> comentarios = bd.ObtenerComentarios(request.IdPublicacion);
        return Json(comentarios);
    }

    [HttpGet]
    public IActionResult ObtenerComentarios(int idPublicacion)
    {
        BD bd = new BD();
        List<Comentarios> comentarios = bd.ObtenerComentarios(idPublicacion);
        return Json(comentarios);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public class LikeRequest
    {
        public int IdPublicacion { get; set; }
    }

    public class ComentarioRequest
    {
        public int IdPublicacion { get; set; }
        public string Texto { get; set; }
    }
}
