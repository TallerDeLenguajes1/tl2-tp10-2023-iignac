namespace tl2_tp10_2023_iignac.Models;

public interface ITareaRepository
{
    public void Create(Tarea tarea);
    public void Update(Tarea tarea);
    public void Remove(int idTarea);
    public List<Tarea> GetAllByIdTablero(int idTablero);
    public Tarea GetByIdTarea(int idTarea);
    // public bool AsignarUsuario(int idUsuario, int idTarea);
    // public bool UpdateNombre(int id, string nuevoNombre);
    // public bool UpdateEstado(int idTarea, EstadoTarea nuevoEstado);
    // public List<Tarea> GetAllByIdUsuario(int idUsuario);
    // public int GetCantTareasSegunEstado(EstadoTarea estado);
}