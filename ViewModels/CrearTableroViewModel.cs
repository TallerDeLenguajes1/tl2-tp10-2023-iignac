namespace tl2_tp10_2023_iignac.ViewModels;

public class CrearTableroViewModel
{
    private int idUsuarioPropietario;
    private TableroViewModel tableroVM;

    public int IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }
    public TableroViewModel TableroVM { get => tableroVM; set => tableroVM = value; }

    public CrearTableroViewModel(int idUsuario){
        idUsuarioPropietario = idUsuario;
    }

    public CrearTableroViewModel(){}
}