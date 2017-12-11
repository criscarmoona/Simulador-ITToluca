using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulador.Logica
{
    public class Camiones
    {
        ObservableCollection<AgregarCamion> camiones = new ObservableCollection<AgregarCamion>();
        public void Nuevo(int ruta, int capacidad)
        {
            AgregarCamion camion = new AgregarCamion(camiones.Count + 1, ruta, 1, capacidad, 0, 0, 5);
        }
    }
}
