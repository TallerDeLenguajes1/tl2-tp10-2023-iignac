using tl2_tp10_2023_iignac.Models;
namespace tl2_tp10_2023_iignac.Repository;

public interface ITareaRepository
{
    public void CreateTarea(Tarea tarea);
    public void UpdateTarea(Tarea tarea);
    public void RemoveTarea(int idTarea);
    public List<Tarea> GetTareasDeTablero(int idTablero);
    public Tarea GetTarea(int idTarea);
    
    // public bool AsignarUsuario(int idUsuario, int idTarea);
    // public bool UpdateNombre(int id, string nuevoNombre);
    // public bool UpdateEstado(int idTarea, EstadoTarea nuevoEstado);
    // public List<Tarea> GetAllByIdUsuario(int idUsuario);
    // public int GetCantTareasSegunEstado(EstadoTarea estado);
}