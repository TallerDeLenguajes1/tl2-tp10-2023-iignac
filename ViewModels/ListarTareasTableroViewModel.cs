namespace tl2_tp10_2023_iignac.ViewModels;
using tl2_tp10_2023_iignac.Models;

public class ListarTareasTableroViewModel
{
    private string nombreTablero;
    private List<TareaViewModel> listaTareasVM;
    private List<UsuarioViewModel> listaUsuariosVM;

    public string NombreTablero { get => nombreTablero; set => nombreTablero = value; }
    public List<TareaViewModel> ListaTareasVM { get => listaTareasVM; set => listaTareasVM = value; }
    public List<UsuarioViewModel> ListaUsuariosVM { get => listaUsuariosVM; set => listaUsuariosVM = value; }

    public ListarTareasTableroViewModel(List<Tarea> listaTareas, List<Usuario> listaUsuarios, string nombreTablero){
        this.nombreTablero = nombreTablero;
        listaTareasVM = new List<TareaViewModel>();
        listaUsuariosVM = new List<UsuarioViewModel>();
        foreach (Tarea tarea in listaTareas) listaTareasVM.Add(new TareaViewModel(tarea));
        foreach (Usuario usuario in listaUsuarios) listaUsuariosVM.Add(new UsuarioViewModel(usuario));
    }
}