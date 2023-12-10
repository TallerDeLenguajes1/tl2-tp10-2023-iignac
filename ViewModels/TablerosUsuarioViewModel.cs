namespace tl2_tp10_2023_iignac.ViewModels;
using tl2_tp10_2023_iignac.Models;

public class TablerosUsuarioViewModel
{
    private int idPropietario;
    private string nombrePropietario;
    private List<TableroViewModel> listaTablerosUsuario;

    public TablerosUsuarioViewModel(List<Tablero> listaTableros, int idPropietario, string nombrePropietario){
        this.idPropietario = idPropietario;
        this.nombrePropietario = nombrePropietario;
        this.listaTablerosUsuario = new List<TableroViewModel>();
        foreach (var tablero in listaTableros) listaTablerosUsuario.Add(new TableroViewModel(tablero));
    }

    public int IdPropietario { get => idPropietario; set => idPropietario = value; }
    public string NombrePropietario { get => nombrePropietario; set => nombrePropietario = value; }
    public List<TableroViewModel> ListaTablerosUsuario { get => listaTablerosUsuario; set => listaTablerosUsuario = value; }
}