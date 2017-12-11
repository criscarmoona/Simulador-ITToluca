using System;
using System.Linq;
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
        private void CrearCamion(int ruta, int parada, int tiempoParadaMs, DateTimeOffset horaSalida)
        {
            camiones.Nuevo(ruta, 40, tiempoParadaMs, horaSalida, parada);
            Imprimir("NuevoCamion r: " + ruta + " p: " + parada);
            //NuevoCamion?.Invoke(null, camiones.camiones.Last());
        }
        public void IniciarSimulador()
        {
            multiplicadorTiempo = 1;
            tiempoSalidaCamionesTenangoMinutos = horaSimulador.AddMinutes(RandomDouble(1, 5));
            Imprimir("Tiempo para siguiente camion : " + tiempoSalidaCamionesTenangoMinutos);
            corriendoSimulador = true;
            temporizador = ThreadPoolTimer.CreatePeriodicTimer(CambioHora, TimeSpan.FromMilliseconds(CalcularPeriodo()));
        }
        void CambioHora(ThreadPoolTimer timer)
        {
            horaSimulador = horaSimulador.AddMinutes(1);
            NuevaHora?.Invoke(null, horaSimulador);
            Imprimir("Cambianido hora: " + horaSimulador);
            if (horaSimulador >= tiempoSalidaCamionesTenangoMinutos)
            {
                int miliSegundosSalida = random.Next(1000, 5000);
                Imprimir("Creando camion con salida: " + horaSimulador.AddMilliseconds(miliSegundosSalida));

                CrearCamion(1, 1, miliSegundosSalida, horaSimulador.AddMilliseconds(miliSegundosSalida));

                tiempoSalidaCamionesTenangoMinutos = horaSimulador.AddMinutes(RandomDouble(1, 5));
                Imprimir("Tiempo para siguiente camion : " + tiempoSalidaCamionesTenangoMinutos);
            }
            if (camiones.camiones.Any(p => p.HoraSalida <= horaSimulador))
            {
                Imprimir("Cambniando ruta camion");
                var camionSale = camiones.camiones.Where(p => p.HoraSalida <= horaSimulador).FirstOrDefault();
                QuitarCamionV(camionSale);
                camiones.camiones.Remove(camionSale);
                camionSale.Parada = camionSale.Parada+1;
                if (camionSale.Parada < 4)
                {
                    int miliSegundosSalida = random.Next(1000, 5000);
                    CrearCamion(camionSale.Ruta, camionSale.Parada, miliSegundosSalida, horaSimulador.AddMilliseconds(miliSegundosSalida));
                }
            }
            Imprimir(horaSimulador.ToString("hh:mm:ss tt") + " multiplicador: " + multiplicadorTiempo);
        }
        public void QuitarCamionV(AgregarCamion camion)
        {
            Imprimir("Quitar Camion r: " + camion.Ruta + " p: " + camion.Parada);
            //QuitarCamion?.Invoke(null, new QuitarCamion(camion.Ruta, camion.Parada));
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
