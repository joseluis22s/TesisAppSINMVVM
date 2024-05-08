using System.Collections.ObjectModel;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.Views;

public partial class PaginaPrincipalPage : ContentPage
{
    public ObservableCollection<OpcionPaginaPrincipal> Opciones { get; }


    public PaginaPrincipalPage()
    {
        InitializeComponent();
        Opciones = new ObservableCollection<OpcionPaginaPrincipal>()
        {
            new OpcionPaginaPrincipal { Imagen = "group_user_icon.png", Texto = "COMPRA" },
            new OpcionPaginaPrincipal { Imagen = "bookmark_icon.png", Texto = "CHEQUES" },
            new OpcionPaginaPrincipal { Imagen = "registro_icon.png", Texto = "VENTA" }
        };
        BindingContext = this;

    }

}