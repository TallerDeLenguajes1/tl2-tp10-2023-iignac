using tl2_tp10_2023_iignac.Models;
namespace tl2_tp10_2023_iignac.Repository;

public interface IUsuarioRepository
{
    public void CreateUsuario(Usuario usuario);
    public void UpdateUsuario(Usuario usuario);
    public void RemoveUsuario(int idUsuario);
    public List<Usuario> GetAllUsuarios();
    public Usuario GetUsuario(int idUsuario);
    public Usuario GetUsuario(string nombre, string contrasenia);
}