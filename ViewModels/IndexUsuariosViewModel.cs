namespace tl2_tp10_2023_iignac.ViewModels;
using tl2_tp10_2023_iignac.Models;

public class IndexUsuariosViewModel
{
    private List<UsuarioViewModel> listaUsuariosVM;

    public IndexUsuariosViewModel(List<Usuario> listaUsuarios){
        listaUsuariosVM = new List<UsuarioViewModel>();
        foreach (var usuario in listaUsuarios) listaUsuariosVM.Add(new UsuarioViewModel(usuario));
    }

    public List<UsuarioViewModel> ListaUsuariosVM { get => listaUsuariosVM; set => listaUsuariosVM = value; }
}