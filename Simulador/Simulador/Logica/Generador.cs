using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.System.Threading;
using Windows.UI.Xaml;

namespace Simulador.Logica
{
    public class Generador
    {


        public event EventHandler<Persona> NuevaPersona;
        public event EventHandler<Camion> NuevoCamion;
        public event EventHandler<int> QuitarCamion;
        public event EventHandler<string> NuevaHora;

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
