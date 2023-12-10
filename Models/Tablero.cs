using tl2_tp10_2023_iignac.ViewModels;

namespace tl2_tp10_2023_iignac.Models;

public class Tablero
{
    private int id; // PK - autoincremental
    private int idUsuarioPropietario; // FK
    private string nombre;
    private string? descripcion;

    public int Id { get => id; set => id = value; }
    public int IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string? Descripcion { get => descripcion; set => descripcion = value; }

    public Tablero(TableroViewModel tablero){
        this.id = tablero.Id;
        this.idUsuarioPropietario = tablero.IdUsuarioPropietario;
        this.nombre = tablero.Nombre;
        this.descripcion = tablero.Descripcion;
    }

    public Tablero(){}
}