using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_iignac.Models;
using tl2_tp10_2023_iignac.ViewModels;

namespace tl2_tp10_2023_iignac.Controllers;

public class TareaController : Controller //NO hereda de ControllerBase
{
    private readonly ILogger<TareaController> _logger;
    private ITareaRepository tareasRepo;
    private ITableroRepository tableroRepo;
    private IUsuarioRepository usuarioRepo;

    public TareaController(ILogger<TareaController> logger)
    {
        _logger = logger;
        tareasRepo = new TareaRepository();
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
    public IActionResult ListarTareas(int idTablero){
        if(IsLogged()){
            Tablero tablero = tableroRepo.GetByIdTablero(idTablero);
            if(!string.IsNullOrEmpty(tablero.Nombre)){
                if(!IsAdmin() && Convert.ToInt32(HttpContext.Session.GetString("id")) != tablero.IdUsuarioPropietario){
                    return RedirectToRoute(new {controller = "Logueo", action="Index"}); //Debe retornar un 404
                }
                List<Tarea> listaTareas = tareasRepo.GetAllByIdTablero(idTablero);
                List<Usuario> listaUsuarios = usuarioRepo.GetAll();
                return View(new TareasTableroViewModel(listaTareas, listaUsuarios, tablero.Nombre));
            }
            return RedirectToRoute(new {controller = "Logueo", action="Index"}); //Debe retornar un 404
        }
        return RedirectToRoute(new {controller = "Logueo", action="Index"});
    }

    [HttpGet]
    public IActionResult CrearTarea(int idTablero){  
        if(IsLogged()){
            Tablero tablero = tableroRepo.GetByIdTablero(idTablero);
            if(!string.IsNullOrEmpty(tablero.Nombre)){
                if(!IsAdmin() && Convert.ToInt32(HttpContext.Session.GetString("id")) != tablero.IdUsuarioPropietario){
                    return RedirectToRoute(new {controller = "Logueo", action="Index"}); //Debe retornar un 404
                }
                List<Usuario> listaUsuarios = usuarioRepo.GetAll();
                List<UsuarioViewModel> listaUsuariosVM = new List<UsuarioViewModel>();
                foreach (Usuario usuario in listaUsuarios) listaUsuariosVM.Add(new UsuarioViewModel(usuario));
                return View(new CrearTareaViewModel(listaUsuariosVM, idTablero));
            }
            return RedirectToRoute(new {controller = "Logueo", action="Index"}); //Debe retornar un 404
        }
        return RedirectToRoute(new {controller = "Logueo", action="Index"});
    }

    [HttpPost]
    public IActionResult CrearTarea(CrearTareaViewModel nueva){
        Tarea nuevaTarea = new Tarea(nueva.TareaVM);
        tareasRepo.Create(nuevaTarea);
        return RedirectToAction("ListarTareas", new{idTablero = nuevaTarea.IdTablero});
    }

    [HttpGet]
    public IActionResult EditarTarea(int idTarea){
        if(IsLogged()){
            Tarea tarea = tareasRepo.GetByIdTarea(idTarea);
            if(!string.IsNullOrEmpty(tarea.Nombre)){
                if(!IsAdmin() && Convert.ToInt32(HttpContext.Session.GetString("id")) != tableroRepo.GetByIdTablero(tarea.IdTablero).IdUsuarioPropietario){
                    return RedirectToRoute(new {controller = "Logueo", action="Index"}); //Debe retornar un 404
                }
                return View(new EditarTareaViewModel(tarea, usuarioRepo.GetAll()));
            }
            return RedirectToRoute(new {controller = "Logueo", action="Index"}); //Debe retornar un 404
        }
        return RedirectToRoute(new {controller = "Logueo", action="Index"});
    }

    [HttpPost]
    public IActionResult EditarTarea(EditarTareaViewModel tareaAEditar){
        Tarea tareaEditada = new Tarea(tareaAEditar.TareaVM);
        tareasRepo.Update(tareaEditada);
        return RedirectToAction("ListarTareas", new{idTablero = tareaEditada.IdTablero});
    }

    public IActionResult EliminarTarea(int idTarea){
        int id = tareasRepo.GetByIdTarea(idTarea).IdTablero;
        tareasRepo.Remove(idTarea);
        return RedirectToAction("ListarTareas", new{idTablero = id});
    }
}