namespace tl2_tp10_2023_iignac.Models;

public class Tablero
{
    public int Id { get; set; } //PK - autoincremental
    public int IdUsuarioPropietario { get; set; } //FK
    public string Nombre { get; set; }
    public string? Descripcion { get; set; }
}