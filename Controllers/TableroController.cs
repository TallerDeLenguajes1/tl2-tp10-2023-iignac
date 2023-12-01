using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_iignac.Models;

namespace tl2_tp10_2023_iignac.Controllers;

public class TableroController : Controller
{
    private readonly ILogger<TableroController> _logger;
    private ITableroRepository tableroRepo;

    public TableroController(ILogger<TableroController> logger)
    {
        _logger = logger;
        tableroRepo = new TableroRepository();
    }

    [HttpGet]
    public IActionResult ListarTablerosUsuario(int idUsuario){ 
        ViewBag.IdUsuario = idUsuario;
        return View(tableroRepo.GetAllByIdUsuario(idUsuario));
    }

    [HttpGet]
    public IActionResult CrearTablero(int idUsuario){
        ViewBag.IdUsuario = idUsuario;
        return View(new Tablero());
    }

    [HttpPost]
    public IActionResult CrearTablero(Tablero nuevoTablero){
        tableroRepo.Create(nuevoTablero);
        return RedirectToAction("ListarTablerosUsuario", new{idUsuario = nuevoTablero.IdUsuarioPropietario});
        // new{...}: objeto anónimo que se pasa como parámetro a la acción “ListarTablerosUsuario”
    }

    [HttpGet]
    public IActionResult EditarTablero(int idTablero){   
        return View(tableroRepo.GetByIdTablero(idTablero));
    }

    [HttpPost]
    public IActionResult EditarTablero(Tablero tableroEditar){
        tableroRepo.Update(tableroEditar);
        return RedirectToAction("ListarTablerosUsuario", new{idUsuario = tableroEditar.IdUsuarioPropietario});
    }

    public IActionResult EliminarTablero(int idTablero){
        int id = tableroRepo.GetByIdTablero(idTablero).IdUsuarioPropietario;
        tableroRepo.Remove(idTablero);
        return RedirectToAction("ListarTablerosUsuario", new{idUsuario = id});
    }
}