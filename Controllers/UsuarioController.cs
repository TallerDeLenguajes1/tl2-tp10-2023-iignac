using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_iignac.Models;
using tl2_tp10_2023_iignac.Repository;
using tl2_tp10_2023_iignac.ViewModels;
namespace tl2_tp10_2023_iignac.Controllers;

public class UsuarioController : Controller
{
    private readonly ILogger<UsuarioController> _logger;
    private readonly IUsuarioRepository _usuariosRepo;

    public UsuarioController(ILogger<UsuarioController> logger, IUsuarioRepository usuariosRepo)
    {
        _logger = logger;
        _usuariosRepo = usuariosRepo;
    }

    private bool IsLogged(){
        return !string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"));
    }

    private bool IsAdmin(){
        return HttpContext.Session.GetString("rol") == "Administrador"; // == Rol.Administrador.ToString()
    }

    [HttpGet]
    public IActionResult Index(){
        try{
            if(!IsLogged()) return RedirectToRoute(new {controller = "Logueo", action="Index"});
            if(!IsAdmin()) return RedirectToRoute(new {controller = "Logueo", action="Index"});
            var listaUsuarios = _usuariosRepo.GetAllUsuarios();
            return View(new IndexUsuariosViewModel(listaUsuarios));
        }catch(Exception ex){
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error"); // return BadRequest();
        }  
    }

    [HttpGet]
    public IActionResult CrearUsuario(){ 
        if(!IsLogged()) RedirectToRoute(new {controller = "Logueo", action="Index"});
        if(!IsAdmin()) return RedirectToAction("Error"); // return BadRequest();
        return View(new UsuarioViewModel());
    }

    [HttpPost]
    public IActionResult CrearUsuario(UsuarioViewModel usuarioVM){
        try{
            if(!IsLogged()) return RedirectToRoute(new {controller="Logueo", action="Index"});
            if(!IsAdmin()) return RedirectToRoute(new {controller="Logueo", action="Index"}); //se controla si es admin nuevamente, aunque ya se haya controlo en el get, por si en el transcurso de la gestión se le cambia al usuario el rol
            if(!ModelState.IsValid) return RedirectToAction("CrearUsuario");
            Usuario usuario = new Usuario(usuarioVM);
            _usuariosRepo.CreateUsuario(usuario);
            return RedirectToAction("Index");
        }catch(Exception ex){
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error"); //return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult EditarUsuario(int idUsuario){
        try{
            if(!IsLogged()) RedirectToRoute(new {controller="Logueo", action="Index"});
            if(!IsAdmin()) RedirectToRoute(new {controller="Logueo", action="Index"});
            var usuario = _usuariosRepo.GetUsuario(idUsuario);
            return View(new UsuarioViewModel(usuario));
        }catch(Exception ex){
            _logger.LogError(ex.ToString());
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult EditarUsuario(UsuarioViewModel usuarioVM){
        try{
            if(!IsLogged()) return RedirectToRoute(new {controller="Logueo", action="Index"});
            if(!IsAdmin()) return RedirectToRoute(new {controller="Logueo", action="Index"});
            if(!ModelState.IsValid) return RedirectToAction("EditarUsuario", new {idUsuario = usuarioVM.Id});
            Usuario usuario = new Usuario(usuarioVM);
            _usuariosRepo.UpdateUsuario(usuario);
            return RedirectToAction("Index");
        }catch(Exception ex){
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error"); //return BadRequest();
        }
    }

    public IActionResult EliminarUsuario(int idUsuario){
        try{
            if(!IsLogged()) return RedirectToRoute(new {controller="Logueo", action="Index"});
            if(!IsAdmin()) return RedirectToRoute(new {controller="Logueo", action="Index"});
            _usuariosRepo.RemoveUsuario(idUsuario);
            return RedirectToAction("Index");
        }catch(Exception ex){
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error"); //return BadRequest();
        }
    }

    public IActionResult Error(){
        return View(new ErrorViewModel());
    }
}