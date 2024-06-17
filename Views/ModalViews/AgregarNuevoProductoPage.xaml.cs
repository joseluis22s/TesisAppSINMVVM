using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Plugin.CloudFirestore;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.Views.ModalViews;

public partial class AgregarNuevoProductoPage : ContentPage
{
	public AgregarNuevoProductoPage()
	{
		InitializeComponent();
	}



    // NAVEGACIÓN
    #region NAVEGACIÓN
    private async Task AgregarNuevoProductoPagePopModalAsync(bool mostrarAlerta)
    {
        await PopModalAsync(mostrarAlerta);
    }
    #endregion


    // EVENTOS
    #region EVENTOS
    private async void Button_GuardarNuevoProducto_Clicked(object sender, EventArgs e)
    {
        string nombreProducto = Entry_Producto.Text;
        string medida = Entry_Medida.Text;
        if (string.IsNullOrEmpty(nombreProducto) || string.IsNullOrEmpty(medida))
        {
            await Toast.Make("Llene todos los campos",ToastDuration.Long).Show();
        }
        else
        {
            var productos = await ObtenerProductosDBAsync();
            nombreProducto = nombreProducto.Trim().ToUpper();
            medida = medida.Trim().ToUpper();
            bool existeProducto = productos.Any(p => p.PRODUCTO == nombreProducto && p.MEDIDA == medida);
            if (!existeProducto)
            {
                Producto producto = new Producto();
                producto.PRODUCTO = nombreProducto;
                producto.MEDIDA = medida;
                await GuardarNuevoProductoDBAsync(producto);
                OcultarTeclado();
                await Toast.Make("Se ha guardado el nuevo producto", ToastDuration.Long).Show();
                await AgregarNuevoProductoPagePopModalAsync(false);
            }
            else
            {
                await Toast.Make("Producto ya existente", ToastDuration.Long).Show();
            }
        }
    }
    private async void Button_Cancelar_Clicked(object sender, EventArgs e)
    {
        await Cancelar();
    }
    protected override bool OnBackButtonPressed()
    {
        Cancelar().GetAwaiter();
        return true;
    }
    #endregion


    // LÓGICA PARA EVENTOS
    #region LÓGICA PARA EVENTOS
    private async Task Cancelar()
    {
        string nombreProducto = Entry_Producto.Text;
        string medida = Entry_Medida.Text;
        if (string.IsNullOrEmpty(nombreProducto) && string.IsNullOrEmpty(medida))
        {
            await AgregarNuevoProductoPagePopModalAsync(false);
        }
        else
        {
            await AgregarNuevoProductoPagePopModalAsync(true);
        }
    }
    #endregion


    // LÓGICA
    #region LÓGICA
    private async Task PopModalAsync(bool mostrarAlerta)
    {
        if (mostrarAlerta)
        {
            bool respuesta = await DisplayAlert("Alerta", "¿Desea regresar? Perderá el progreso realizado", "Confimar", "Cancelar");
            if (respuesta)
            {
                await Navigation.PopAsync();
            }
        }
        else
        {
            await Navigation.PopAsync();
        }
    }
    #endregion


    // BASE DE DATOS
    #region BASE DE DATOS
    private async Task<List<Producto>> ObtenerProductosDBAsync()
    {
        return await Producto_Repository.ObtenerProductosAsync();
    }
    private async Task GuardarNuevoProductoDBAsync(Producto producto)
    {
        await Producto_Repository.GuardarNuevoProductoAsync(producto);
    }
    #endregion


    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
    #region LÓGICA DE COSAS VISUALES DE LA PÁGINA
    private void OcultarTeclado()
    {
        Entry_Producto.Unfocus();
        Entry_Medida.Unfocus(); 
    }
    #endregion
}