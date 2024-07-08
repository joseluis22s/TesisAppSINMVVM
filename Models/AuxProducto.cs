
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TesisAppSINMVVM.Models
{
    public class AuxProducto : INotifyPropertyChanged
    {
        private string _producto;
        private string _medida;
        private int _cantidad;
        private double _precio;
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
        public string MEDIDA
        {
            get { return _medida; }
            set
            {
                _medida = value;
                OnPropertyChanged(nameof(MEDIDA));
            }
        }
        public int CANTIDAD
        {
            get { return _cantidad; }
            set
            {
                _cantidad = value;
                OnPropertyChanged(nameof(CANTIDAD));
            }
        }
        public double PRECIO
        {
            get { return _precio; }
            set
            {
                _precio = value;
                OnPropertyChanged(nameof(PRECIO));
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
