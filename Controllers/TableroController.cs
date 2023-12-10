using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_iignac.Models;
using tl2_tp10_2023_iignac.ViewModels;
namespace tl2_tp10_2023_iignac.Controllers;

public class TableroController : Controller
{
    private readonly ILogger<TableroController> _logger;
    private ITableroRepository tableroRepo;
    private IUsuarioRepository usuarioRepo;


    public TableroController(ILogger<TableroController> logger)
    {
        _logger = logger;
        tableroRepo = new TableroRepository();
        usuarioRepo = new UsuarioRepository();
    }

    private bool IsLogged(){
        return !string.IsNullOrEmpty(HttpContext.Session.GetString("usuario"));
    }

    private bool IsAdmin(){
        return HttpContext.Session.GetString("rol") == "Administrador";
    }

    [HttpGet]
    public IActionResult ListarTableros(int? idUsuario = null){ 
        if(IsLogged()){
            if( idUsuario == null || (!IsAdmin() && Convert.ToInt32(HttpContext.Session.GetString("id")) == (int)idUsuario) ){
                List<Tablero> listaTableros = tableroRepo.GetAllByIdUsuario(Convert.ToInt32(HttpContext.Session.GetString("id")));
                return View(new TablerosUsuarioViewModel(listaTableros, Convert.ToInt32(HttpContext.Session.GetString("id")), HttpContext.Session.GetString("usuario")));
            }
            if(IsAdmin()){
                Usuario usuario = usuarioRepo.GetUsuario((int)idUsuario);
                if(!string.IsNullOrEmpty(usuario.NombreUsuario)){
                    UsuarioViewModel usuarioVM = new UsuarioViewModel(usuario);
                    List<Tablero> listaTableros = tableroRepo.GetAllByIdUsuario((int)idUsuario);
                    return View(new TablerosUsuarioViewModel(listaTableros, (int)idUsuario, usuarioVM.Nombre));
                }
            }
        }
        return RedirectToRoute(new {controller = "Logueo", action="Index"}); // Puede redireccionar a un 404
    }

    [HttpGet]
    public IActionResult CrearTablero(int? idUsuario){
        if(IsLogged()){
            if(idUsuario == null) return View(new CrearTableroViewModel(Convert.ToInt32(HttpContext.Session.GetString("id"))));
            Usuario usuario = usuarioRepo.GetUsuario((int)idUsuario);
            if(!string.IsNullOrEmpty(usuario.NombreUsuario)){
                if(IsAdmin() || (Convert.ToInt32(HttpContext.Session.GetString("id")) == idUsuario)) return View(new CrearTableroViewModel((int)idUsuario));
            }
        }
        return RedirectToRoute(new {controller ="Logueo", action = "Index"}); //Debe retornar un 404
    }

    [HttpPost]
    public IActionResult CrearTablero(CrearTableroViewModel nuevo){
        Tablero nuevoTablero = new Tablero(nuevo.TableroVM);
        tableroRepo.Create(nuevoTablero);
        return RedirectToAction("ListarTableros", new{idUsuario = nuevoTablero.IdUsuarioPropietario});
        // new{...}: objeto anónimo que se pasa como parámetro a la acción “ListarTableros”
    }

    [HttpGet]
    public IActionResult EditarTablero(int idTablero){  // Falta controlar si idTablero es null
        if(IsLogged()){
            Tablero tablero = tableroRepo.GetByIdTablero(idTablero);
            if(!string.IsNullOrEmpty(tablero.Nombre)){
                if(IsAdmin() || (Convert.ToInt32(HttpContext.Session.GetString("id")) == tablero.IdUsuarioPropietario)){
                    return View(new TableroViewModel(tablero));
                }
            }
        }
        HttpContext.Session.Clear();
        return RedirectToRoute(new {controller = "Logueo", action="Index"}); //Debe retornar un 404
    }

    [HttpPost]
    public IActionResult EditarTablero(TableroViewModel tableroAEditar){
        tableroRepo.Update(new Tablero(tableroAEditar));
        return RedirectToAction("ListarTableros", new{idUsuario = tableroAEditar.IdUsuarioPropietario});
    }

    public IActionResult EliminarTablero(int idTablero){
        int idPropietario = tableroRepo.GetByIdTablero(idTablero).IdUsuarioPropietario; 
        tableroRepo.Remove(idTablero);
        return RedirectToAction("ListarTableros", new{idUsuario = idPropietario});
    }
}