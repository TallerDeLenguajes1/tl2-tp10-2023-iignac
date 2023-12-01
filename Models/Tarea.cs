namespace tl2_tp10_2023_iignac.Models;

public enum EstadoTarea{ Ideas, ToDo, Doing, Review, Done}

public class Tarea
{
    public int Id { get; set; } //PK - autoincremental
    public int IdTablero { get; set; } //FK
    public string Nombre { get; set; }
    public EstadoTarea Estado { get; set; }
    public string? Descripcion { get; set; }
    public string? Color { get; set; }
    public int? IdUsuarioAsignado { get; set; }
    // una tarea puede o no tener un usuario asignado, por eso IdUsuarioAsignado puede ser null
}