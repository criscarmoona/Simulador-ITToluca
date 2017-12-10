using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Simulador.Control_Animacion
{
    public class Control_Persona
    {
        public int Num_Personsa_Espera { get; set; }
        public Grid Grid_Personas { get; set; }
        public Grid Grid_Personas_SALIDA { get; set; }
        public int Num_animaciones { get; set; }
        public Visibility Datos_visibles { get; set; } = Visibility.Collapsed;
    }
}
