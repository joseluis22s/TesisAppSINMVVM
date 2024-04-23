using TesisAppSINMVVM.Views;

namespace TesisAppSINMVVM
{
    public partial class App : Application
    {
        public App()
        {
            //UserAppTheme = AppTheme.Light;

            InitializeComponent();

            //MainPage = new AppShell();

            MainPage = new NavigationPage(new IniciarSesionPage());
        }
    }
}
