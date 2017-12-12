using System;
using Windows.System.Threading;

namespace Simulador.Logica
{
    public class AgregarCamion
    {

        public int NumeroCamion { get; set; }
        public int Ruta { get; set; }
        public int Parada { get; set; }
        public int Capacidad { get; set; }
        public int ABordo { get; set; }
        public int Bajan { get; set; }
        public int TiempoParadaMs { get; set; }
        public bool Moviendose { get; set; }
        public DateTimeOffset HoraSalida { get; set; }


        public AgregarCamion(int numeroCamion, int ruta, int parada, int capacidad, int aBordo, int bajan,int tiempoParadaMs, DateTimeOffset horaSalida)
        {
            NumeroCamion = numeroCamion;
            Ruta = ruta;
            Parada = parada;
            Capacidad = capacidad;
            ABordo = aBordo;
            Bajan = bajan;
            TiempoParadaMs = tiempoParadaMs;
            HoraSalida = horaSalida;
        }
    }
}
