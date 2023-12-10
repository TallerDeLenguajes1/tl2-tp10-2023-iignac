using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_iignac.Models;
using tl2_tp10_2023_iignac.ViewModels;

namespace tl2_tp10_2023_iignac.Controllers;

public class LogueoController : Controller
{
    private readonly ILogger<LogueoController> _logger;
    private IUsuarioRepository usuarioRepo;

    public LogueoController(ILogger<LogueoController> logger)
    {
        _logger = logger;
        usuarioRepo = new UsuarioRepository();
    }

    private bool IsAdmin(){
        return HttpContext.Session.GetString("rol") == "Administrador"; // == Rol.Administrador.ToString()
    }

    [HttpGet]
    public IActionResult index(){ 
        return View(); //return View(new LogueoViewModel());
    }

    [HttpPost]
    public IActionResult ProcesoLogueo(LogueoViewModel logueoUsuario){
        var usuarioLogueado = usuarioRepo.GetUsuario(logueoUsuario.NombreUsuario, logueoUsuario.ContraseniaUsuario); //usuarioLogueado puede ser tipo 'Usuario' o null
        if(!string.IsNullOrEmpty(usuarioLogueado.NombreUsuario)){
            LoguearUsuario(usuarioLogueado);
            if(IsAdmin()){
                return RedirectToRoute(new {controller = "Usuario", action = "Index"});
            }
            return RedirectToRoute(new {controller = "Tablero", action = "ListarTableros"});
        }
        return RedirectToAction("Index"); // o mostrar una pantalla de error (404)
    }

    private void LoguearUsuario(Usuario usuario){
        HttpContext.Session.SetString("id", usuario.Id.ToString());
        HttpContext.Session.SetString("usuario", usuario.NombreUsuario);
        HttpContext.Session.SetString("rol", usuario.RolUsuario.ToString());
    }
}