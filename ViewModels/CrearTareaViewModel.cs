namespace tl2_tp10_2023_iignac.ViewModels;
using tl2_tp10_2023_iignac.Models;

public class CrearTareaViewModel
{
    private TareaViewModel tareaVM;
    private List<UsuarioViewModel> listaUsuariosVM;
    private int idTablero;

    public TareaViewModel TareaVM { get => tareaVM; set => tareaVM = value; }
    public List<UsuarioViewModel> ListaUsuariosVM { get => listaUsuariosVM; set => listaUsuariosVM = value; }
    public int IdTablero { get => idTablero; set => idTablero = value; }

    public CrearTareaViewModel(List<UsuarioViewModel> listaUsuariosVM, int idTablero){
        this.listaUsuariosVM = listaUsuariosVM;
        this.idTablero = idTablero;
    }

    public CrearTareaViewModel(){}
}