namespace tl2_tp10_2023_iignac.ViewModels;
using tl2_tp10_2023_iignac.Models;

public class EditarTareaViewModel
{
    private TareaViewModel tareaVM;
    private List<UsuarioViewModel> listaUsuariosVM;

    public TareaViewModel TareaVM { get => tareaVM; set => tareaVM = value; }
    public List<UsuarioViewModel> ListaUsuariosVM { get => listaUsuariosVM; set => listaUsuariosVM = value; }

    public EditarTareaViewModel(Tarea tarea, List<Usuario> listaUsuarios){
        this.tareaVM = new TareaViewModel(tarea);
        this.listaUsuariosVM = new List<UsuarioViewModel>();
        foreach (Usuario usuario in listaUsuarios) this.listaUsuariosVM.Add(new UsuarioViewModel(usuario));
    }

    public EditarTareaViewModel(){}
}