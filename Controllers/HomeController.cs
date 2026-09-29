using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TPRedSocial.Models;

namespace TPRedSocial.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("id") == null){
            return RedirectToAction("VistaIniciarSesion");
        }
        BD bd = new BD();
        ViewBag.user = bd.MostrarUsuario(int.Parse(HttpContext.Session.GetString("id")));
        return View();   
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
            Usuario user = new Usuario(nombreUsuario, contraseña, nombre, apellido, 0);
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

    public IActionResult PostCrearPublicacion(string imagen, string titulo, string descripcion){
        if (HttpContext.Session.GetString("id") == null){
            return RedirectToAction("VistaIniciarSesion");
        }
        BD bd = new BD();
        ViewBag.user = bd.MostrarUsuario(int.Parse(HttpContext.Session.GetString("id")));
        Publicaciones publicacion = new Publicaciones(imagen, titulo, descripcion, DateTime.Now, ViewBag.user.nombreUsuario);
        bd.CrearPublicacion(publicacion);
        return View("Index");
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
}
