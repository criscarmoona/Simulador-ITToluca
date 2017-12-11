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
        public ObservableCollection<AgregarCamion> camiones = new ObservableCollection<AgregarCamion>();
        public void Nuevo(int ruta, int capacidad, int tiempoParada, DateTimeOffset horaSalida, int parada)
        {
            AgregarCamion camion = new AgregarCamion(camiones.Count + 1, ruta, parada, capacidad, 0, 0, tiempoParada, horaSalida);
            camiones.Add(camion);
        }
    }
}
