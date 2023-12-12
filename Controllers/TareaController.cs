using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_iignac.Models;
using tl2_tp10_2023_iignac.Repository;
using tl2_tp10_2023_iignac.ViewModels;
namespace tl2_tp10_2023_iignac.Controllers;

public class TareaController : Controller 
{
    private readonly ILogger<TareaController> _logger;
    private ITareaRepository _tareasRepo;
    private ITableroRepository _tablerosRepo;
    private IUsuarioRepository _usuariosRepo;

    public TareaController(ILogger<TareaController> logger, ITareaRepository tareasRepo, ITableroRepository tablerosRepo, IUsuarioRepository usuariosRepo)
    {
        _logger = logger;
        _tareasRepo = tareasRepo;
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
    public IActionResult ListarTareas(int idTablero){
        try{
            if(!IsLogged()) return RedirectToRoute(new {controller = "Logueo", action="Index"});
            var tablero = _tablerosRepo.GetTablero(idTablero);

            // un admin puede ver todos los tableros y todas las tareas
            // un operador puede ver solo sus tableros y las tareas de estos
            if(!IsAdmin() && Convert.ToInt32(HttpContext.Session.GetString("id")) != tablero.IdUsuarioPropietario){
                return RedirectToRoute(new {controller = "Logueo", action="Index"});
            }
            var listaTareas = _tareasRepo.GetTareasDeTablero(idTablero);
            var listaUsuarios = _usuariosRepo.GetAllUsuarios();
            return View(new ListarTareasTableroViewModel(listaTareas, listaUsuarios, tablero.Nombre));
        }catch(Exception ex){
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error"); // return BadRequest()
        }
    }

    [HttpGet]
    public IActionResult CrearTarea(int idTablero){
        try{
            if(!IsLogged()) return RedirectToRoute(new {controller = "Logueo", action="Index"});
            Tablero tablero = _tablerosRepo.GetTablero(idTablero);

            //un admin puede crear tareas en todos los tableros
            //un operador solo puede crear tareas en sus propios tableros
            if(!IsAdmin() && Convert.ToInt32(HttpContext.Session.GetString("id")) != tablero.IdUsuarioPropietario){
                return RedirectToRoute(new {controller = "Logueo", action="Index"});
            }
            List<Usuario> listaUsuarios = _usuariosRepo.GetAllUsuarios();
            List<UsuarioViewModel> listaUsuariosVM = new List<UsuarioViewModel>();
            foreach (Usuario usuario in listaUsuarios) listaUsuariosVM.Add(new UsuarioViewModel(usuario));
            return View(new CrearTareaViewModel(listaUsuariosVM, idTablero));
        }catch(Exception ex){
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    public IActionResult CrearTarea(CrearTareaViewModel nueva){
        try{
            if(!IsLogged()) return RedirectToRoute(new {controller = "Logueo", action="Index"});
            if(!ModelState.IsValid) return RedirectToAction("CrearTarea", new {idTablero = nueva.IdTablero});
            Tarea nuevaTarea = new Tarea(nueva.TareaVM);
            _tareasRepo.CreateTarea(nuevaTarea);
            return RedirectToAction("ListarTareas", new{idTablero = nuevaTarea.IdTablero});
        }catch(Exception ex){
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult EditarTarea(int idTarea){
        try{
            if(!IsLogged()) return RedirectToRoute(new {controller = "Logueo", action="Index"});
            Tarea tarea = _tareasRepo.GetTarea(idTarea);
            
            //un admin puede editar todas las tareas
            //un operador solo puede editar las tareas de sus propios tableros
            if(!IsAdmin() && Convert.ToInt32(HttpContext.Session.GetString("id")) != _tablerosRepo.GetTablero(tarea.IdTablero).IdUsuarioPropietario){
                return RedirectToRoute(new {controller = "Logueo", action="Index"});
            }
            return View(new EditarTareaViewModel(tarea, _usuariosRepo.GetAllUsuarios()));
        }catch(Exception ex){
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    public IActionResult EditarTarea(EditarTareaViewModel tareaAEditar){
        try{
            if(!IsLogged()) return RedirectToRoute(new {controller = "Logueo", action="Index"});
            if(!ModelState.IsValid) return RedirectToAction("EditarTarea", new {idTarea = tareaAEditar.TareaVM.Id});
            Tarea tareaEditada = new Tarea(tareaAEditar.TareaVM);
            _tareasRepo.UpdateTarea(tareaEditada);
            return RedirectToAction("ListarTareas", new{idTablero = tareaEditada.IdTablero});
        }catch(Exception ex){
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
        }
    }

    public IActionResult EliminarTarea(int idTarea){
        try{
            if(!IsLogged()) RedirectToRoute(new {controller = "Logueo", action="Index"});
            int id = _tareasRepo.GetTarea(idTarea).IdTablero;
            _tareasRepo.RemoveTarea(idTarea);
            return RedirectToAction("ListarTareas", new{idTablero = id});
        }catch(Exception ex){
            _logger.LogError(ex.ToString());
            return RedirectToAction("Error");
        }
    }

    public IActionResult Error(){
        return View(new ErrorViewModel());
    }
}