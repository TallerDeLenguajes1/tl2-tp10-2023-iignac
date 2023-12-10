using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_iignac.Models;
using tl2_tp10_2023_iignac.ViewModels;

namespace tl2_tp10_2023_iignac.Controllers;

public class UsuarioController : Controller //NO hereda de ControllerBase
{
    private readonly ILogger<UsuarioController> _logger;
    private IUsuarioRepository usuarioRepo;

    public UsuarioController(ILogger<UsuarioController> logger)
    {
        _logger = logger;
        usuarioRepo = new UsuarioRepository();
    }

    private bool IsLogged(){
        return !string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"));
    }

    private bool IsAdmin(){
        return HttpContext.Session.GetString("rol") == "Administrador"; // == Rol.Administrador.ToString()
    }

    [HttpGet]
    public IActionResult Index(){
        if(IsLogged() && IsAdmin()){
            return View(new IndexUsuariosViewModel(usuarioRepo.GetAll()));
        }
        return RedirectToRoute(new {controller = "Logueo", action="Index"});
    }

    [HttpGet]
    public IActionResult CrearUsuario(){ 
        if(IsLogged() && IsAdmin()) return View(new CrearUsuarioViewModel());  
        return RedirectToRoute(new {controller = "Logueo", action="Index"});
    }

    [HttpPost]
    public IActionResult CrearUsuario(CrearUsuarioViewModel usuario){
        usuarioRepo.Create(new Usuario(usuario.UsuarioNuevo));
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditarUsuario(int idUsuario){
        if(IsLogged() && IsAdmin()){
            Usuario usuario = usuarioRepo.GetUsuario(idUsuario);
            if(!string.IsNullOrEmpty(usuario.NombreUsuario)){
                return View(new UsuarioViewModel(usuario));
            }
        }
        return RedirectToRoute(new {controller = "Logueo", action="Index"});
    }

    [HttpPost]
    public IActionResult EditarUsuario(UsuarioViewModel usuario){
        Usuario usuarioAEditar = new Usuario(usuario);
        usuarioRepo.Update(usuarioAEditar);
        return RedirectToAction("Index");
    }

    public IActionResult EliminarUsuario(int idUsuario){
        usuarioRepo.Remove(idUsuario);
        return RedirectToAction("Index");
    }
}