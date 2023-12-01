namespace tl2_tp10_2023_iignac.Models;

public interface IUsuarioRepository
{
    public void Create(Usuario usuario);
    public void Update(Usuario usuario);
    public void Remove(int idUsuario);
    public List<Usuario> GetAll();
    public Usuario GetByIdUsuario(int idUsuario);
}