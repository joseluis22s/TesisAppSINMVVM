
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TesisAppSINMVVM.Models
{
    public class ProductoInventarioModel : INotifyPropertyChanged
    {
        private string _producto;
        private string _cantidad;
        private string _descripcion;
        private bool _esSeleccionado;

        public string PRODUCTO
        {
            get { return _producto; }
            set
            {
                _producto = value;
                OnPropertyChanged(nameof(PRODUCTO));
            }
        }
        public string CANTIDAD
        {
            get { return _cantidad; }
            set
            {
                _cantidad = value;
                OnPropertyChanged(nameof(CANTIDAD));
            }
        }
        public string DESCRIPCION
        {
            get { return _descripcion; }
            set
            {
                _descripcion = value;
                OnPropertyChanged(nameof(DESCRIPCION));
            }
        }
        public bool ESSELECCIONADO
        {
            get { return _esSeleccionado; }
            set
            {
                _esSeleccionado = value;
                OnPropertyChanged(nameof(ESSELECCIONADO));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
