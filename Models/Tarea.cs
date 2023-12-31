using tl2_tp10_2023_iignac.ViewModels;

namespace tl2_tp10_2023_iignac.Models;

public enum EstadoTarea{ Ideas, ToDo, Doing, Review, Done}

public class Tarea
{
    private int id; //PK - autoincremental
    private int idTablero; //FK
    private int? idUsuarioAsignado; //una tarea puede o no tener un usuario asignado, por eso IdUsuarioAsignado puede ser null
    private string nombre;
    private string? descripcion;
    private string? color;
    private EstadoTarea estado;

    public int Id { get => id; set => id = value; }
    public int IdTablero { get => idTablero; set => idTablero = value; }
    public int? IdUsuarioAsignado { get => idUsuarioAsignado; set => idUsuarioAsignado = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string? Descripcion { get => descripcion; set => descripcion = value; }
    public string? Color { get => color; set => color = value; }
    public EstadoTarea Estado { get => estado; set => estado = value; }

    public Tarea(TareaViewModel tarea){
        this.id = tarea.Id;
        this.idTablero = tarea.IdTablero;
        this.idUsuarioAsignado = tarea.IdUsuarioAsignado;
        this.nombre = tarea.Nombre;
        this.descripcion = tarea.Descripcion;
        this.color = tarea.Color;
        this.estado = tarea.Estado;
    }

    public Tarea(){}
}