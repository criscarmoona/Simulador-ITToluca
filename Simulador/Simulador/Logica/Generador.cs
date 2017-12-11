using System;
using System.Threading.Tasks;
using Windows.System.Threading;

namespace Simulador.Logica
{
    public class Generador
    {
        public EventHandler<AgregarPersona> NuevaPersona { get; set; }
        public EventHandler<AgregarCamion> NuevoCamion { get; set; }
        public EventHandler<QuitarCamion> QuitarCamion { get; set; }
        public EventHandler<DateTime> NuevaHora { get; set; }

        ThreadPoolTimer temporizador;
        private int multiplicadorTiempo;
        public DateTime horaSimulador = new DateTime(2017, 12, 9, 6, 0, 0);

        public Camiones camiones = new Camiones();

        private Random random = new Random();

        private bool corriendoSimulador;
        private DateTimeOffset tiempoSalidaCamionesTenangoMinutos;

        public Generador()
        {
            IniciarSimulador();
        }   
        private void CrearCamion(int ruta)
        {
            camiones.Nuevo(ruta, 40);
        }
        public void IniciarSimulador()
        {
            multiplicadorTiempo = 1;
            tiempoSalidaCamionesTenangoMinutos = horaSimulador.AddMinutes(RandomDouble(1, 5));
            corriendoSimulador = true;
            temporizador = ThreadPoolTimer.CreatePeriodicTimer(CambioHora, TimeSpan.FromMilliseconds(CalcularPeriodo()));
        }
        void CambioHora(ThreadPoolTimer timer)
        {
            if (horaSimulador > tiempoSalidaCamionesTenangoMinutos)
            {
                CrearCamion(1);
                tiempoSalidaCamionesTenangoMinutos = horaSimulador.AddMinutes(RandomDouble(1, 5));
            }
            horaSimulador = horaSimulador.AddMinutes(1);
            NuevaHora?.Invoke(null, horaSimulador);
            Imprimir(horaSimulador.ToString("hh:mm:ss tt") + " multiplicador: " + multiplicadorTiempo);
        }

        public void CambiarVelocidad(double multiplicador)
        {
            multiplicadorTiempo = Convert.ToInt32(multiplicador);
            temporizador.Cancel();
            temporizador = ThreadPoolTimer.CreatePeriodicTimer(CambioHora, TimeSpan.FromMilliseconds(CalcularPeriodo()));
        }
        private double CalcularPeriodo()
        {
            double nuevoPeriodo = 1000 / multiplicadorTiempo;
            Imprimir("Periodo: " + nuevoPeriodo);
            return nuevoPeriodo;
        }
        public double RandomDouble(double minimo, double maximo)
        {
            Random random = new Random();
            return random.NextDouble() * (maximo - minimo) + minimo;
        }
        private void Imprimir(string texto)
        {
            System.Diagnostics.Debug.WriteLine(texto);
        }
    }
}
