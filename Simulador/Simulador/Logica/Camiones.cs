using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Simulador.Logica
{
    public class Camiones
    {
        int contador = 1;
        public ObservableCollection<AgregarCamion> camionesTenango = new ObservableCollection<AgregarCamion>();
        public ObservableCollection<AgregarCamion> camionesToluca = new ObservableCollection<AgregarCamion>();
        public ObservableCollection<AgregarCamion> camionesTerminal = new ObservableCollection<AgregarCamion>();

        public AgregarCamion Nuevo(int ruta, int capacidad,int tiempoParadaMs, DateTimeOffset horaSalida, int parada)
        {
            AgregarCamion camion = new AgregarCamion(contador, ruta, parada,  capacidad, 0, 0, tiempoParadaMs,  horaSalida);
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
            return camion;
        }
        public AgregarCamion Transportar(AgregarCamion camion, DateTimeOffset tiempoParaTransportar)
        {
            switch (camion.Ruta)
            {
                case 1:
                    var camionsito = camionesTenango.Where(bus => bus == camion).First();
                    camionsito.HoraSalida = tiempoParaTransportar;
                    camionsito.Moviendose = true;
                    return camionsito;
                case 2:
                    var camionsito1 = camionesToluca.Where(bus => bus == camion).First();
                    camionsito1.HoraSalida = tiempoParaTransportar;
                    camionsito1.Moviendose = true;
                    return camionsito1;
                case 3:
                    var camionsito2 = camionesTerminal.Where(bus => bus == camion).First();
                    camionsito2.HoraSalida = tiempoParaTransportar;
                    camionsito2.Moviendose = true;
                    return camionsito2;
                default:
                    return null;
            }
        }
        public AgregarCamion Llego(AgregarCamion camion, DateTimeOffset tiempoParaTransportar)
        {
            switch (camion.Ruta)
            {
                case 1:
                    var camionsito = camionesTenango.Where(bus => bus == camion).First();
                    camionsito.Parada = camionsito.Parada + 1;
                    camionsito.HoraSalida = tiempoParaTransportar;
                    camionsito.Moviendose = false;
                    return camionsito;
                case 2:
                    var camionsito1 = camionesToluca.Where(bus => bus == camion).First();
                    camionsito1.Parada = camionsito1.Parada + 1;
                    camionsito1.HoraSalida = tiempoParaTransportar;
                    camionsito1.Moviendose = false;
                    return camionsito1;
                case 3:
                    var camionsito2 = camionesTerminal.Where(bus => bus == camion).First();
                    camionsito2.Parada = camionsito2.Parada + 1;
                    camionsito2.HoraSalida = tiempoParaTransportar;
                    camionsito2.Moviendose = false;
                    return camionsito2;
                default:
                    return null;
            }
        }

    }
}
