namespace tl2_tp10_2023_iignac.ViewModels;
using tl2_tp10_2023_iignac.Models;

public class ListarTablerosUsuarioViewModel
{
    private int idPropietario;
    private string nombrePropietario;
    private List<TableroViewModel> listaTablerosVM;

    public ListarTablerosUsuarioViewModel(int idPropietario, string nombrePropietario, List<Tablero> listaTableros){
        this.idPropietario = idPropietario;
        this.nombrePropietario = nombrePropietario;
        listaTablerosVM = new List<TableroViewModel>();
        foreach (var tablero in listaTableros) listaTablerosVM.Add(new TableroViewModel(tablero));
    }

    public int IdPropietario { get => idPropietario; set => idPropietario = value; }
    public string NombrePropietario { get => nombrePropietario; set => nombrePropietario = value; }
    public List<TableroViewModel> ListaTablerosVM { get => listaTablerosVM; set => listaTablerosVM = value; }
}