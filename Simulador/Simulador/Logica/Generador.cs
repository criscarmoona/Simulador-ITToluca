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
        public EventHandler<int> QuitarCamion { get; set; }
        public EventHandler<string> NuevaHora { get; set; }

        ThreadPoolTimer dispatcherTimer;

        private int TiempoParadaCamionMilisegundos = 5;

        public Generador()
        {
            IniciarTiempo();
        }
        public void IniciarTiempo()
        {
           
        }
        void dispatcherTimer_Tick(ThreadPoolTimer timer)
        {
        }
        private void Imprimir(string texto)
        {
            System.Diagnostics.Debug.WriteLine(texto);
        }
    }
}
