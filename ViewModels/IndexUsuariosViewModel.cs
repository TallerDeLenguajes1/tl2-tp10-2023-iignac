namespace tl2_tp10_2023_iignac.ViewModels;
using tl2_tp10_2023_iignac.Models;

public class IndexUsuariosViewModel
{
    private List<UsuarioViewModel> listaUsuariosVM;

    public IndexUsuariosViewModel(List<Usuario> usuarios){
        this.listaUsuariosVM = new List<UsuarioViewModel>();
        foreach (var usuario in usuarios){
            var usuarioVM = new UsuarioViewModel(usuario);
            listaUsuariosVM.Add(usuarioVM);
        }
    }

    public List<UsuarioViewModel> ListaUsuariosVM { get => listaUsuariosVM; set => listaUsuariosVM = value; }
}