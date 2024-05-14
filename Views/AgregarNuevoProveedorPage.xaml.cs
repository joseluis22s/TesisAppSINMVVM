using CommunityToolkit.Maui.Alerts;
using TesisAppSINMVVM.Database.Respositories;

namespace TesisAppSINMVVM.Views;

public partial class AgregarNuevoProveedorPage : ContentPage
{
    private Tbl_Proveedor_Respository TblProveedor_repo;
    public AgregarNuevoProveedorPage()
	{
		InitializeComponent();
        TblProveedor_repo =  new Tbl_Proveedor_Respository();
	}

    // NAVEGACIÓN
    private async Task AgregarNuevaCuentaPagePopAsync(int opcion)
    {
        if (opcion == 0)
        {
            bool respuesta = await DisplayAlert("Alerta", "¿Desea regresar? Perderá el progreso realizado", "Confimar", "Cancelar");
            if (respuesta)
            {
                await Navigation.PopModalAsync();
            }
        }
        else
        {
            await Navigation.PopModalAsync();
        }
        
    }
    // EVENTOS
    private async void Button_Guardar_Clicked(object sender, EventArgs e)
    {
        //
        //      AQUI SE DEBE VERIFICAR SI SE PUEDE INSTACIAR TODOS ESTA AVRIABELS AQUI
        //      CASO CONTRARIO MANTENER EN LO MIMOS METODO EXISTENTES
        //

        //string nombre = Entry_NombreNuevoProveedor.Text;
        //string apellido = Entry_ApellidoNuevoProveedor.Text;
        //string telefono = Entry_TelefonoNuevoProveedor.Text;


        
        bool camposVacios = await ComprobarCamposVacios();
        if (camposVacios)
        {
            await Toast.Make("Se supone que al menos uno esta vacio").Show();
        }
        else
        {
            bool existeProveedor = await VerificarExistenciaProveedorDBAsync();
            if (existeProveedor)
            {
                await Toast.Make("Contacto ya existente").Show();
            }
            else
            {
                await GuardarNuevoProveedorDBAsync();
                await Toast.Make("Contacto guardado exitosamente").Show();
                await AgregarNuevaCuentaPagePopAsync(1);
            }
        }
        
    }
    private async void Button_Cancelar_Clicked(object sender, EventArgs e)
    {
        await AgregarNuevaCuentaPagePopAsync(0);
    }
    // LOGICA PARA EVENTOS
    private Task<bool> ComprobarCamposVacios()
    {
        string nombre = Entry_NombreNuevoProveedor.Text;
        string apellido = Entry_ApellidoNuevoProveedor.Text;
        string telefono = Entry_TelefonoNuevoProveedor.Text;
        if (string.IsNullOrEmpty(nombre))
        {
            nombre = "";
        }
        if (string.IsNullOrEmpty(apellido))
        {
            apellido = "";
        }
        if (string.IsNullOrEmpty(telefono))
        {
            telefono = "";
        }
        if(nombre == "" || apellido == "" || telefono == "")
        {
            return Task.FromResult(true);
            //await Toast.Make("Se supone que uno esta vacio").Show();
        }
        return Task.FromResult(false);
        //else
        //{
        //    //await Toast.Make("Se supone que todos esta llenos").Show();
        //}
    }

    // BASE DE DATOS
    private async Task<bool> VerificarExistenciaProveedorDBAsync()
    {
        string nombre = Entry_NombreNuevoProveedor.Text;
        string apellido = Entry_ApellidoNuevoProveedor.Text;
        if (string.IsNullOrEmpty(nombre))
        {
            nombre = "";
        }
        if (string.IsNullOrEmpty(apellido))
        {
            apellido = "";
        }
        var hoola = await TblProveedor_repo.VerificarExistenciaProveedorAsync(nombre, apellido);
        return hoola;
    }
    private async Task GuardarNuevoProveedorDBAsync()
    {
        string nombre = Entry_NombreNuevoProveedor.Text;
        string apellido = Entry_ApellidoNuevoProveedor.Text;
        string telefono = Entry_TelefonoNuevoProveedor.Text;
        await TblProveedor_repo.GuardarNuevoProveedorAsync(nombre,apellido,telefono);
    }
    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
}