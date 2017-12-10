using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Simulador.Control_Animacion
{
    public class Control_Camion
    {
        public int Numero_Camion { get; set; }       
        public int Total_Cap_Camion { get; set; }
        public int Num_Abordo { get; set; }
        public Visibility Datos_visibles { get; set; } = Visibility.Collapsed;
    }
}
