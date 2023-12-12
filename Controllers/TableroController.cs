using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_iignac.Models;
using tl2_tp10_2023_iignac.Repository;
using tl2_tp10_2023_iignac.ViewModels;
namespace tl2_tp10_2023_iignac.Controllers;

public class TableroController : Controller
{
    private readonly ILogger<TableroController> _logger;
    private readonly ITableroRepository _tablerosRepo;
    private readonly IUsuarioRepository _usuariosRepo;


    public TableroController(ILogger<TableroController> logger, ITableroRepository tablerosRepo, IUsuarioRepository usuariosRepo)
    {
        _logger = logger;
        _tablerosRepo = tablerosRepo;
        _usuariosRepo = usuariosRepo;
    }

    private bool IsLogged(){
        return !string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"));
    }

    private bool IsAdmin(){
        return HttpContext.Session.GetString("rol") == "Administrador";
    }

    [HttpGet]
    public IActionResult ListarTableros(int? idUsuario){ //el idUsuario null proviene de hacer clic en el link 'Mis tableros'
        try{
            if(!IsLogged()) return RedirectToRoute(new {controller = "Logueo", action="Index"});
            int idSession = Convert.ToInt32(HttpContext.Session.GetString("id"));
            if(idUsuario == null || (!IsAdmin() && idSession == (int)idUsuario)){
                var listaTableros = _tablerosRepo.GetTablerosDeUsuario(idSession);
                return View(new ListarTablerosUsuarioViewModel(idSession, HttpContext.Session.GetString("usuario"), listaTableros));
            } 
            if(IsAdmin()){ // aqui idUsuario no es null y puede pertenecer al admin logueado o a otro usuario (admin u operador) no logueado
                var usuario = _usuariosRepo.GetUsuario((int)idUsuario);
                var usuarioVM = new UsuarioViewModel(usuario);
                var listaTableros = _tablerosRepo.GetTablerosDeUsuario((int)idUsuario);
                return View(new ListarTablerosUsuarioViewModel((int)idUsuario, usuarioVM.Nombre, listaTableros));
            }
            //aqui el usuario logueado es operador y envió por url el id de otro usuario
            //El usuario operador solo puede ver sus propios tableros (esto se debe modificar luego)
            return RedirectToRoute(new {controller = "Logueo", action="Index"});
        }catch(Exception ex){
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error"); // return BadRequest()
        }
    }

    [HttpGet]
    public IActionResult CrearTablero(int? idUsuario){
        try{
            if(!IsLogged()) return RedirectToRoute(new{controller="Logueo", action="Index"});
            if(idUsuario == null) return RedirectToAction("ListarTableros");
            var usuario = _usuariosRepo.GetUsuario((int)idUsuario);

            //un admin puede crear tableros para cualquier usuario
            //un operador solo puede crear sus propios tableros
            if(IsAdmin() || (Convert.ToInt32(HttpContext.Session.GetString("id")) == idUsuario)){ 
                return View(new CrearTableroViewModel((int)idUsuario));
            }
            return RedirectToRoute(new{controller="Logueo", action="Index"});
        }catch(Exception ex){
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error"); // return BadRequest()
        }
    }

    [HttpPost]
    public IActionResult CrearTablero(CrearTableroViewModel nuevo){
        try{
            if(!IsLogged()) return RedirectToRoute(new{controller="Logueo", action="Index"});
            if(!ModelState.IsValid) return RedirectToRoute(new{controller="Logueo", action="Index"});
            var tablero = new Tablero(nuevo.TableroVM);
            _tablerosRepo.CreateTablero(tablero);
            return RedirectToAction("ListarTableros", new{idUsuario = tablero.IdUsuarioPropietario}); // new{...}: objeto anónimo que se pasa como parámetro a la acción “ListarTableros”
        }catch(Exception ex){
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error"); // return BadRequest()
        }
    }

    [HttpGet]
    public IActionResult EditarTablero(int idTablero){  // Falta controlar si idTablero es null
        try{
            if(!IsLogged()) return RedirectToRoute(new{controller="Logueo", action="Index"});
            var tablero = _tablerosRepo.GetTablero(idTablero);

            //el admin puede editar cualquier tablero
            //el operador puede editar solo sus propios tableros
            if(IsAdmin() || (Convert.ToInt32(HttpContext.Session.GetString("id")) == tablero.IdUsuarioPropietario)){
                return View(new TableroViewModel(tablero));
            }
            return RedirectToRoute(new {controller = "Logueo", action="Index"});
        }catch(Exception ex){
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error"); // return BadRequest()
        }
    }

    [HttpPost]
    public IActionResult EditarTablero(TableroViewModel tableroVM){
        try{
            if(IsLogged()) return RedirectToRoute(new {controller = "Logueo", action="Index"});
            if(!ModelState.IsValid) return RedirectToRoute(new{controller="Logueo", action="Index"});
            var tableroActualizado = new Tablero(tableroVM);
            _tablerosRepo.UpdateTablero(tableroActualizado);
            return RedirectToAction("ListarTableros", new{idUsuario = tableroVM.IdUsuarioPropietario});
        }catch(Exception ex){
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error"); // return BadRequest()
        } 
    }

    public IActionResult EliminarTablero(int idTablero){
        try{
            if(!IsLogged()) return RedirectToRoute(new{controller="Logueo", action="Index"});
            int idPropietario = _tablerosRepo.GetTablero(idTablero).IdUsuarioPropietario; 
            _tablerosRepo.RemoveTablero(idTablero);
            return RedirectToAction("ListarTableros", new{idUsuario = idPropietario});
        }catch(Exception ex){
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error"); // return BadRequest()
        }
    }

    public IActionResult Error(){
        return View(new ErrorViewModel());
    }
}