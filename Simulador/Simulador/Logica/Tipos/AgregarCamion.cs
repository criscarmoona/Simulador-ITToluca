using System;
using Windows.System.Threading;

namespace Simulador.Logica
{
    public class AgregarCamion
    {
        ThreadPoolTimer temporizador;

        public int NumeroCamion { get; set; }
        public int Ruta { get; set; }
        public int Parada { get; set; }
        public int Capacidad { get; set; }
        public int ABordo { get; set; }
        public int Bajan { get; set; }
        public float TiempoParada { get; set; }

        public EventHandler<AgregarCamion> NuevoCamion { get; set; }
        public EventHandler<QuitarCamion> QuitarCamion { get; set; }

        public AgregarCamion(int numeroCamion, int ruta, int parada, int capacidad, int aBordo, int bajan, int tiempoParada)
        {
            NumeroCamion = numeroCamion;
            Ruta = ruta;
            Parada = parada;
            Capacidad = capacidad;
            ABordo = aBordo;
            Bajan = bajan;
            TiempoParada = tiempoParada;
            //temporizador = ThreadPoolTimer.CreatePeriodicTimer(CambioHora, TimeSpan.FromMilliseconds(5000));
        }
    }
}
