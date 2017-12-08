using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulador.Control
{
     public class Control_Datos
    {
        private int _indexParosPendientes;
        public int numero { get; set; }

        public int IndexParosPendientes
        {
            get { return _indexParosPendientes; }
            set
            {
                //if (value != _indexParosPendientes)
                //{
                //    _indexParosPendientes = value;
                //    NotifyPropertyChanged
                //    RaisePropertyChanged(() => IndexParosPendientes);
                //}
            }
        }



    }
}
