using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_iignac.Models;

namespace tl2_tp10_2023_iignac.Controllers;

public class TareaController : Controller //NO hereda de ControllerBase
{
    private readonly ILogger<TareaController> _logger;
    private ITareaRepository tareaRepo;

    public TareaController(ILogger<TareaController> logger)
    {
        _logger = logger;
        tareaRepo = new TareaRepository();
    }

    public IActionResult ListarTareasTablero(int idTablero){
        return View(tareaRepo.GetAllByIdTablero(idTablero));
    }

    [HttpGet]
    public IActionResult CrearTarea(int idTablero){  
        ViewBag.IdTablero = idTablero;
        return View();
    }

    [HttpPost]
    public IActionResult CrearTarea(Tarea nuevaTarea){
        tareaRepo.Create(nuevaTarea);
        return RedirectToAction("ListarTareasTablero", new{idTablero = nuevaTarea.IdTablero});
    }

    [HttpGet]
    public IActionResult EditarTarea(int idTarea){   
        return View(tareaRepo.GetByIdTarea(idTarea));
    }

    [HttpPost]
    public IActionResult EditarTarea(Tarea tareaEditar){
        tareaRepo.Update(tareaEditar);
        return RedirectToAction("ListarTareasTablero", new{idTablero = tareaEditar.IdTablero});
    }

    public IActionResult EliminarTarea(int idTarea){
        int id = tareaRepo.GetByIdTarea(idTarea).IdTablero;
        tareaRepo.Remove(idTarea);
        return RedirectToAction("ListarTareasTablero", new{idTablero = id});
    }
}