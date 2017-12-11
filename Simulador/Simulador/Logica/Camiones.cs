using System;
using System.Collections.ObjectModel;

namespace Simulador.Logica
{
    public class Camiones
    {
        int contador = 1;
        public ObservableCollection<AgregarCamion> camionesTenango = new ObservableCollection<AgregarCamion>();
        public ObservableCollection<AgregarCamion> camionesToluca = new ObservableCollection<AgregarCamion>();
        public ObservableCollection<AgregarCamion> camionesTerminal = new ObservableCollection<AgregarCamion>();

        public void Nuevo(int ruta, int capacidad, int tiempoParada, DateTimeOffset horaSalida, int parada)
        {
            AgregarCamion camion = new AgregarCamion(contador, ruta, parada, capacidad, 0, 0, tiempoParada, horaSalida);
            contador++;
            if (ruta == 1)
            {
                camionesTenango.Add(camion);
            }
            else if (ruta == 2)
            {
                camionesToluca.Add(camion);
            }
            else if (ruta == 3)
            {
                camionesTerminal.Add(camion);
            }
        }

    }
}
