using System;
using Windows.System.Threading;

namespace Simulador.Logica
{
    public class Generador
    {
        public EventHandler<Persona> NuevaPersona { get; set; }
        public EventHandler<Camion> NuevoCamion { get; set; }
        public EventHandler<int> QuitarCamion { get; set; }
        public EventHandler<DateTime> NuevaHora { get; set; }

        ThreadPoolTimer temporizador;
        private int multiplicadorTiempo;
        public DateTime horaSimulador = new DateTime(2017, 12, 9, 6, 0, 0);

        public Generador()
        {
            IniciarSimulador();

        }
        public void IniciarSimulador()
        {
            multiplicadorTiempo = 1;
            temporizador = ThreadPoolTimer.CreatePeriodicTimer(cambioHora, TimeSpan.FromMilliseconds(CalcularPeriodo()));
        }
        void cambioHora(ThreadPoolTimer timer)
        {
            horaSimulador = horaSimulador.AddMinutes(1);
            NuevaHora?.Invoke(null, horaSimulador);
            Imprimir(horaSimulador.ToString("hh:mm:ss tt") + " multiplicador: " + multiplicadorTiempo);
        }

        public void CambiarVelocidad(double multiplicador)
        {
            multiplicadorTiempo = Convert.ToInt32(multiplicador);
            temporizador.Cancel();
            temporizador = ThreadPoolTimer.CreatePeriodicTimer(cambioHora, TimeSpan.FromMilliseconds(CalcularPeriodo()));
        }
        private double CalcularPeriodo()
        {
            double nuevoPeriodo = 1000 / multiplicadorTiempo;
            Imprimir("Periodo: " + nuevoPeriodo);
            return nuevoPeriodo;
        }
        private void Imprimir(string texto)
        {
            System.Diagnostics.Debug.WriteLine(texto);
        }
    }
}
