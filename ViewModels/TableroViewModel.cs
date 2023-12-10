using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_iignac.Models;
namespace tl2_tp10_2023_iignac.ViewModels;

public class TableroViewModel
{
    private int id;
    private int idUsuarioPropietario;
    private string nombre;
    private string? descripcion;

    public int Id { get => id; set => id = value; }
    public int IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }
    
    [Required(ErrorMessage = "Este campo es requerido")]
    [Display(Name = "Nombre del tablero")]
    public string Nombre { get => nombre; set => nombre = value; }
    
    [Display(Name = "Descripción del tablero")]
    public string? Descripcion { get => descripcion; set => descripcion = value; }

    public TableroViewModel(Tablero tablero){
        this.id = tablero.Id;
        this.IdUsuarioPropietario = tablero.IdUsuarioPropietario;
        this.nombre = tablero.Nombre;
        this.descripcion = tablero.Descripcion;
    }

    public TableroViewModel(){}
}