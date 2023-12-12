using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_iignac.Models;
using tl2_tp10_2023_iignac.Repository;
using tl2_tp10_2023_iignac.ViewModels;
namespace tl2_tp10_2023_iignac.Controllers;

public class LogueoController : Controller
{
    private readonly ILogger<LogueoController> _logger;
    private readonly IUsuarioRepository _usuariosRepo;

    public LogueoController(ILogger<LogueoController> logger, IUsuarioRepository usuariosRepo)
    {
        _logger = logger;
        _usuariosRepo = usuariosRepo; //inyecto el repositorio
    }

    private bool IsAdmin(){
        return HttpContext.Session.GetString("rol") == "Administrador"; // == Rol.Administrador.ToString()
    }

    [HttpGet]
    public IActionResult index(){ 
        return View(); //return View(new LogueoViewModel());
    }
    
    [HttpPost]
    public IActionResult Login(LogueoViewModel logueoUsuario){
        try{
            if(!ModelState.IsValid) return RedirectToAction("Index"); //ModelState.IsValid  -> revisa si los atributos de validación del viewModel son válidos
            var usuarioLogueado = _usuariosRepo.GetUsuario(logueoUsuario.NombreUsuario, logueoUsuario.ContraseniaUsuario);
            LoguearUsuario(usuarioLogueado);
            _logger.LogInformation($"El usuario {usuarioLogueado.NombreUsuario} ingreso correctamente");
            if(IsAdmin()) RedirectToRoute(new {controller = "Usuario", action = "Index"});
            return RedirectToRoute(new {controller = "Tablero", action = "ListarTableros", idUsuario = usuarioLogueado.Id});
        }catch(Exception ex){
            _logger.LogWarning($"Error: {ex} Intento de acceso invalido - Usuario: {logueoUsuario.NombreUsuario} - Clave ingresada: {logueoUsuario.ContraseniaUsuario}");
            return RedirectToAction("Index"); 
        }
    }
    
    private void LoguearUsuario(Usuario usuario){
        HttpContext.Session.SetString("id", usuario.Id.ToString());
        HttpContext.Session.SetString("usuario", usuario.NombreUsuario);
        HttpContext.Session.SetString("rol", usuario.RolUsuario.ToString());
    }
}