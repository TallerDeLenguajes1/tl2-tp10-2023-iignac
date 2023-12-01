namespace tl2_tp10_2023_iignac.Models;

public interface ITableroRepository
{
    public void Create(Tablero tablero);
    public void Update(Tablero tablero);
    public void Remove(int idTablero);
    public Tablero GetByIdTablero(int idTablero);
    public List<Tablero> GetAllByIdUsuario(int idUsuario);
}