using tl2_tp10_2023_iignac.Models;
namespace tl2_tp10_2023_iignac.Repository;

public interface ITableroRepository
{
    public void CreateTablero(Tablero tablero);
    public void UpdateTablero(Tablero tablero);
    public void RemoveTablero(int idTablero);
    public List<Tablero> GetAllTableros();
    public Tablero GetTablero(int idTablero);
    public List<Tablero> GetTablerosDeUsuario(int idUsuario);
}