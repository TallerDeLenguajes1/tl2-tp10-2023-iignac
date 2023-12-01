using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_iignac.Models;

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

    public IActionResult Index(){
        return View(usuarioRepo.GetAll());
    }

    [HttpGet]
    public IActionResult CrearUsuario(){   
        return View(new Usuario());
    }

    [HttpPost]
    public IActionResult CrearUsuario(Usuario nuevoUsuario){
        usuarioRepo.Create(nuevoUsuario);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditarUsuario(int idUsuario){   
        return View(usuarioRepo.GetByIdUsuario(idUsuario));
    }

    [HttpPost]
    public IActionResult EditarUsuario(Usuario usuarioEditar){
        usuarioRepo.Update(usuarioEditar);
        return RedirectToAction("Index");
    }

    public IActionResult EliminarUsuario(int idUsuario){
        usuarioRepo.Remove(idUsuario);
        return RedirectToAction("Index");
    }
}