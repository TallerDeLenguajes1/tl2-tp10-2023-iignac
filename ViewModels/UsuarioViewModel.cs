using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_iignac.Models;
namespace tl2_tp10_2023_iignac.ViewModels;

public class UsuarioViewModel
{
    private int id;
    private string nombre;
    private string contrasenia;
    private Rol rol;

    public int Id { get => id; set => id = value; }

    [Required(ErrorMessage = "Este campo es requerido")]
    [Display(Name = "Nombre de usuario")]
    public string Nombre { get => nombre; set => nombre = value; }

    [Required(ErrorMessage = "Este campo es requerido")]
    [Display(Name = "Contraseña")]
    public string Contrasenia { get => contrasenia; set => contrasenia = value; }

    [Display(Name = "Rol del usuario")]
    public Rol Rol { get => rol; set => rol = value; }

    public UsuarioViewModel(Usuario usuario){
        this.id = usuario.Id;
        this.nombre = usuario.NombreUsuario;
        this.rol = usuario.RolUsuario;
    }

    public UsuarioViewModel(){}
}