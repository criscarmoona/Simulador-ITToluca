using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.System.Threading;
using Windows.UI.Xaml;

namespace Simulador.Logica
{
    public class Generador
    {
        public EventHandler<Persona> NuevaPersona { get; set; }
        public EventHandler<Camion> NuevoCamion { get; set; }
        public EventHandler<string> NuevaHora { get; set; }

        ThreadPoolTimer dispatcherTimer;
        DateTimeOffset startTime;
        DateTimeOffset lastTime;
        DateTimeOffset stopTime;
        int timesTicked = 1;
        int timesToTick = 10;

        private int TiempoParadaCamionMinutos = 5;

        public Generador()
        {
            IniciarTiempo();
        }
        public void IniciarTiempo()
        {
            dispatcherTimer = ThreadPoolTimer.CreatePeriodicTimer(dispatcherTimer_Tick, TimeSpan.FromSeconds(1));
            //IsEnabled defaults to false
            startTime = DateTimeOffset.Now;
            lastTime = startTime;
            Imprimir("Iniciando()\n");
            //IsEnabled should now be true after calling start
        }
        void dispatcherTimer_Tick(ThreadPoolTimer timer)
        {
            DateTimeOffset time = DateTimeOffset.Now;
            TimeSpan span = time - lastTime;
            lastTime = time;
            //Time since last tick should be very very close to Interval
            Imprimir(timesTicked + "\t Tiempo desde ultimo tick: " + span.ToString() + "\n");
            timesTicked++;
            if (timesTicked > timesToTick)
            {
                stopTime = time;
                Imprimir("Deteniendo\n");
                dispatcherTimer.Cancel();
                //IsEnabled should now be false after calling stop
                span = stopTime - startTime;
                Imprimir("Tiempo total inicio fin: " + span.ToString() + "\n");
            }
        }
        private void Imprimir(string texto)
        {
            System.Diagnostics.Debug.WriteLine(texto);
        }
    }
}
