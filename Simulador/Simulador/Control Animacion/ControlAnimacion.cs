
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Simulador.Control_Animacion
{
    public class ControlAnimacion
    {
       public int Ruta { get; set; }
       public int Parada { get; set; }
       public int Num_Camiones_Espera { get; set; }
       public Grid Grid_Camiones { get; set; }
       public ObservableCollection<Control_Camion>   Camiones { get; set; }
       public Control_Persona Personas { get; set; }
       public ObservableCollection<Control_int_Animacion> Variables_Animaciones { get; set; }

    }
}
