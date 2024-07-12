
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TesisAppSINMVVM.Models
{
    public class AuxProducto : INotifyPropertyChanged
    {
        private string _producto;
        private string _medida;
        private string _cantidad;
        //private string _precio;
        private string _precioE;
        private string _precioD;
        private string _total;
        private string _totalV;
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
        public string CANTIDAD
        {
            get { return _cantidad; }
            set
            {
                _cantidad = value;
                OnPropertyChanged(nameof(CANTIDAD));
            }
        }
        public string PRECIOE
        {
            get { return _precioE; }
            set
            {
                _precioE = value;
                OnPropertyChanged(nameof(PRECIOE));
            }
        }
        public string PRECIOD
        {
            get { return _precioD; }
            set
            {
                _precioD = value;
                OnPropertyChanged(nameof(PRECIOD));
            }
        }
        //public string PRECIO
        //{
        //    get { return _precio; }
        //    set
        //    {
        //        _precio = value;
        //        OnPropertyChanged(nameof(PRECIO));
        //    }
        //}
        public string TOTAL
        {
            get { return _total; }
            set
            {
                _total = value;
                OnPropertyChanged(nameof(TOTAL));
            }
        }
        public string TOTALVENTA
        {
            get { return _totalV; }
            set
            {
                _totalV = value;
                OnPropertyChanged(nameof(TOTALVENTA));
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
