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
        public EventHandler<string> NuevoEventoTenango { get; set; }
        public EventHandler<string> NuevoEventoToluca { get; set; }
        public EventHandler<string> NuevoEventoTerminal { get; set; }

        ThreadPoolTimer temporizador;
        private int multiplicadorTiempo;
        public DateTime horaSimulador = new DateTime(2017, 12, 9, 6, 0, 0);
        public DateTime horaFinSimulador = new DateTime(2017, 12, 9, 9, 0, 0);

        public Camiones camiones = new Camiones();

        private Random random = new Random();

        private DateTimeOffset tiempoSalidaCamionesTenango;
        private DateTimeOffset tiempoSalidaCamionesToluca;
        private DateTimeOffset tiempoSalidaCamionesTerminal;

        ThreadPoolTimer Personas;

        private int rangoMinimoSalidaCamionesTenango = 300;
        private int rangoMaximoSalidaCamionesTenango = 480;

        private int rangoMinimoSalidaCamionesToluca = 300;
        private int rangoMaximoSalidaCamionesToluca = 480;

        private int rangoMinimoSalidaCamionesTerminal = 300;
        private int rangoMaximoSalidaCamionesTerminal = 480;

        private int rangoMinimoTiempoTransporteTenango = 300;
        private int rangoMaximoTiempoTransporteTenango = 480;

        private int rangoMinimoTiempoTransporteToluca = 300;
        private int rangoMaximoTiempoTransporteToluca = 480;

        private int rangoMinimoTiempoTransporteTerminal = 300;
        private int rangoMaximoTiempoTransporteTerminal = 480;

        private int rangoMinimoTiempoEsperaTenango = 300;
        private int rangoMaximoTiempoEsperaTenango = 480;
        
        private int rangoMinimoTiempoEsperaToluca = 300;
        private int rangoMaximoTiempoEsperaToluca = 480;

        private int rangoMinimoTiempoEsperaTerminal = 300;
        private int rangoMaximoTiempoEsperaTerminal = 480;

        public Generador()
        {
            IniciarSimulador();

        }
        private void CrearCamion(int ruta, int parada, int tiempoParadaMs, DateTimeOffset horaSalida)
        {
            //Imprimir("Hora actual: " + horaSimulador);
            var ca = camiones.Nuevo(ruta, 40, tiempoParadaMs, horaSalida, parada);
            Imprimir("LLega camion " + ca.NumeroCamion + " ruta " + ca.Ruta + " con salida: " + horaSalida.ToString("hh:mm:ss tt"), ruta);
            if (ruta == 1)
            {
                NuevoCamion?.Invoke(null, ca);
            }
            else if (ruta == 2)
            {
                NuevoCamion?.Invoke(null, ca);
            }
            else if (ruta == 3)
            {
                NuevoCamion?.Invoke(null, ca);
            }
        }
        private void TransportarCamion(AgregarCamion camion, DateTimeOffset horaSalida)
        {
            //Imprimir("Hora actual: " + horaSimulador);
            camion = camiones.Transportar(camion, horaSalida);
            Imprimir("Sale camion " + camion.NumeroCamion + " ruta " + camion.Ruta + " de parada: " + camion.Parada + ". Llega a su siguiente parada a las: " + horaSalida.ToString("HH:mm:ss"), camion.Ruta);
            QuitarCamionV(camion);
        }
        private void LlegoCamionSiguienteParada(AgregarCamion camion, DateTimeOffset horaSalida)
        {
            //Imprimir("Hora actual: " + horaSimulador);
            camion = camiones.Llego(camion, horaSalida);
            Imprimir("LLega camion " + camion.NumeroCamion + " ruta " + camion.Ruta + " a la parada parada: " + camion.Parada + " vuelve a salir a las " + horaSalida.ToString("HH:mm:ss"), camion.Ruta);
            if (camion.Ruta == 1)
            {
                NuevoCamion?.Invoke(null, camion);
            }
            else if (camion.Ruta == 2)
            {
                NuevoCamion?.Invoke(null, camion);
            }
            else if (camion.Ruta == 3)
            {
                NuevoCamion?.Invoke(null, camion);
            }
        }

        int ObtenerVariable(int rangoMinimo, int rangoMaximo)
        {
            return random.Next(rangoMinimo, rangoMaximo);
        }

        public void IniciarSimulador()
        {
            multiplicadorTiempo = 1;
            tiempoSalidaCamionesTenango = horaSimulador.AddSeconds(ObtenerVariable(rangoMinimoSalidaCamionesTenango, rangoMaximoSalidaCamionesTenango));
            tiempoSalidaCamionesToluca = horaSimulador.AddSeconds(ObtenerVariable(rangoMinimoSalidaCamionesToluca, rangoMaximoSalidaCamionesToluca));
            tiempoSalidaCamionesTerminal = horaSimulador.AddSeconds(ObtenerVariable(rangoMinimoSalidaCamionesTerminal, rangoMaximoSalidaCamionesTerminal));
            Imprimir("El siguiente camion Tenango llega a las : " + tiempoSalidaCamionesTenango.ToString("HH:mm:ss"), 1);
            Imprimir("El siguiente camion Toluca llega a las : " + tiempoSalidaCamionesToluca.ToString("HH:mm:ss"), 2);
            Imprimir("El siguiente camion Terminal llega a las : " + tiempoSalidaCamionesTerminal.ToString("HH:mm:ss"), 3);
            temporizador = ThreadPoolTimer.CreatePeriodicTimer(CambioHora, TimeSpan.FromMilliseconds(CalcularPeriodo()));
            //Personas = ThreadPoolTimer.CreatePeriodicTimer(PersonasAleatorio, TimeSpan.FromMilliseconds(2000));
        }
        //void PersonasAleatorio(ThreadPoolTimer timer)
        //{
        //    int parada_ = random.Next(1, 4);
        //    int ruta_ = random.Next(1, 4);
        //    int num_Personas = random.Next(1, 10);

        //    for (int i = 0; i < num_Personas; i++)
        //    {
        //        NuevaPersona?.Invoke(null, new AgregarPersona { Parada = parada_, Ruta = ruta_ });
        //    }
        //}
        bool eliminando = false;

        void ChecarSalidas()
        {
            if (horaSimulador >= tiempoSalidaCamionesTenango)
            {
                int tiempoParadaMs = ObtenerVariable(rangoMinimoTiempoEsperaTenango, rangoMaximoTiempoEsperaTenango);
                DateTimeOffset tiempoSalida = horaSimulador.AddSeconds(tiempoParadaMs);

                CrearCamion(1, 1, tiempoParadaMs, tiempoSalida);

                int tiempoSiguienteCamion = ObtenerVariable(rangoMinimoSalidaCamionesTenango, rangoMaximoSalidaCamionesTenango);
                DateTimeOffset tiempoSiguienteSalida = horaSimulador.AddSeconds(tiempoSiguienteCamion);
                tiempoSalidaCamionesTenango = tiempoSiguienteSalida;
                Imprimir("El siguiente camion Tenango llega a las : " + tiempoSalidaCamionesTenango.ToString("hh:mm:ss tt"), 1);
            }
            if (horaSimulador >= tiempoSalidaCamionesToluca)
            {
                int tiempoParadaMs = ObtenerVariable(rangoMinimoTiempoEsperaToluca, rangoMaximoTiempoEsperaToluca);
                DateTimeOffset tiempoSalida = horaSimulador.AddSeconds(tiempoParadaMs);

                CrearCamion(2, 1, tiempoParadaMs, tiempoSalida);

                int tiempoSiguienteCamion = ObtenerVariable(rangoMinimoSalidaCamionesToluca, rangoMaximoSalidaCamionesToluca);
                DateTimeOffset tiempoSiguienteSalida = horaSimulador.AddSeconds(tiempoSiguienteCamion);
                tiempoSalidaCamionesToluca = tiempoSiguienteSalida;
                Imprimir("El siguiente camion Toluca llega a las : " + tiempoSalidaCamionesToluca.ToString("hh:mm:ss tt"), 2);
            }
            if (horaSimulador >= tiempoSalidaCamionesTerminal)
            {
                int tiempoParadaMs = ObtenerVariable(rangoMinimoTiempoEsperaTerminal, rangoMaximoTiempoEsperaTerminal);
                DateTimeOffset tiempoSalida = horaSimulador.AddSeconds(tiempoParadaMs);

                CrearCamion(3, 1, tiempoParadaMs, tiempoSalida);

                int tiempoSiguienteCamion = ObtenerVariable(rangoMinimoSalidaCamionesTerminal, rangoMaximoSalidaCamionesTerminal);
                DateTimeOffset tiempoSiguienteSalida = horaSimulador.AddSeconds(tiempoSiguienteCamion);
                tiempoSalidaCamionesTerminal = tiempoSiguienteSalida;
                Imprimir("El siguiente camion Terminal llega a las : " + tiempoSalidaCamionesTerminal.ToString("hh:mm:ss tt"), 3);
            }
        }
        void ChecarEsperas()
        {
            if (camiones.camionesTenango.Any(p => p.Ruta == 1 && p.Moviendose == false && p.Parada < 3 && p.HoraSalida <= horaSimulador && !eliminando))
            {
                eliminando = true;
                var camionesSalen = camiones.camionesTenango.Where(p => p.Ruta == 1 && p.Moviendose == false && p.Parada < 3 && p.HoraSalida <= horaSimulador).ToList();
                foreach (AgregarCamion camionSale in camionesSalen)
                {
                    int tiempoParadaMs = ObtenerVariable(rangoMinimoTiempoEsperaTenango, rangoMaximoTiempoTransporteTenango);
                    DateTimeOffset tiempoSalida = horaSimulador.AddSeconds(tiempoParadaMs);

                    TransportarCamion(camionSale, tiempoSalida);
                }
                eliminando = false;
            }
            if (camiones.camionesToluca.Any(p => p.Ruta == 2 && p.Moviendose == false && p.Parada < 3 && p.HoraSalida <= horaSimulador && !eliminando))
            {
                eliminando = true;
                var camionesSalen = camiones.camionesToluca.Where(p => p.Ruta == 2 && p.Moviendose == false && p.Parada < 3 && p.HoraSalida <= horaSimulador).ToList();
                foreach (AgregarCamion camionSale in camionesSalen)
                {
                    int tiempoParadaMs = ObtenerVariable(rangoMinimoTiempoEsperaToluca, rangoMaximoTiempoTransporteToluca);
                    DateTimeOffset tiempoSalida = horaSimulador.AddSeconds(tiempoParadaMs);

                    TransportarCamion(camionSale, tiempoSalida);
                }
                eliminando = false;
            }
            if (camiones.camionesTerminal.Any(p => p.Ruta == 3 && p.Moviendose == false && p.Parada < 3 && p.HoraSalida <= horaSimulador && !eliminando))
            {
                eliminando = true;
                var camionesSalen = camiones.camionesTerminal.Where(p => p.Ruta == 3 && p.Moviendose == false && p.Parada < 3 && p.HoraSalida <= horaSimulador).ToList();
                foreach (AgregarCamion camionSale in camionesSalen)
                {
                    int tiempoParadaMs = ObtenerVariable(rangoMinimoTiempoEsperaTerminal, rangoMaximoTiempoTransporteTerminal);
                    DateTimeOffset tiempoSalida = horaSimulador.AddSeconds(tiempoParadaMs);

                    TransportarCamion(camionSale, tiempoSalida);
                }
                eliminando = false;
            }
        }
        void ChecarLlegadas()
        {
            if (camiones.camionesTenango.Any(p => p.Ruta == 1 && p.Moviendose == true && p.Parada < 3 && p.HoraSalida <= horaSimulador))
            {
                var camionesSalen = camiones.camionesTenango.Where(p => p.Ruta == 1 && p.Moviendose == true && p.Parada < 3 && p.HoraSalida <= horaSimulador).ToList();
                foreach (AgregarCamion camionSale in camionesSalen)
                {
                    int miliSegundosSalida = ObtenerVariable(rangoMinimoTiempoTransporteTenango, rangoMaximoTiempoTransporteTenango);
                    LlegoCamionSiguienteParada(camionSale, horaSimulador.AddSeconds(miliSegundosSalida));
                }

            }
            if (camiones.camionesToluca.Any(p => p.Ruta == 2 && p.Moviendose == true && p.Parada < 3 && p.HoraSalida <= horaSimulador))
            {
                var camionesSalen = camiones.camionesToluca.Where(p => p.Ruta == 2 && p.Moviendose == true && p.Parada < 3 && p.HoraSalida <= horaSimulador).ToList();
                foreach (AgregarCamion camionSale in camionesSalen)
                {
                    int miliSegundosSalida = ObtenerVariable(rangoMinimoTiempoTransporteToluca, rangoMaximoTiempoTransporteToluca);
                    LlegoCamionSiguienteParada(camionSale, horaSimulador.AddSeconds(miliSegundosSalida));
                }

            }
            if (camiones.camionesTerminal.Any(p => p.Ruta == 3 && p.Moviendose == true && p.Parada < 3 && p.HoraSalida <= horaSimulador))
            {
                var camionesSalen = camiones.camionesTerminal.Where(p => p.Ruta == 3 && p.Moviendose == true && p.Parada < 3 && p.HoraSalida <= horaSimulador).ToList();
                foreach (AgregarCamion camionSale in camionesSalen)
                {
                    int miliSegundosSalida = ObtenerVariable(rangoMinimoTiempoTransporteTerminal, rangoMaximoTiempoTransporteTerminal);
                    LlegoCamionSiguienteParada(camionSale, horaSimulador.AddSeconds(miliSegundosSalida));
                }

            }
        }
        void CambioHora(ThreadPoolTimer timer)
        {
            if (horaSimulador < horaFinSimulador)
            {
                horaSimulador = horaSimulador.AddSeconds(6);
                NuevaHora?.Invoke(null, horaSimulador);
                ChecarSalidas();
                ChecarEsperas();
                ChecarLlegadas();
            }
        }
        public void QuitarCamionV(AgregarCamion camion)
        {
            //Imprimir("Quitar Camion r: " + camion.Ruta + " p: " + camion.Parada);
            QuitarCamion?.Invoke(null, new QuitarCamion(camion.Ruta, camion.Parada));
        }

        public void CambiarVelocidad(double multiplicador)
        {
            multiplicadorTiempo = Convert.ToInt32(multiplicador);
            temporizador.Cancel();
            temporizador = ThreadPoolTimer.CreatePeriodicTimer(CambioHora, TimeSpan.FromMilliseconds(CalcularPeriodo()));
        }
        private double CalcularPeriodo()
        {
            double nuevoPeriodo = 100 / multiplicadorTiempo;
            //Imprimir("Periodo: " + nuevoPeriodo);
            return nuevoPeriodo;
        }
        private void Imprimir(string texto, int ruta)
        {
            System.Diagnostics.Debug.WriteLine(texto);
            switch (ruta)
            {
                case 1:
                    NuevoEventoTenango?.Invoke(null, texto);
                    break;
                case 2:
                    NuevoEventoToluca?.Invoke(null, texto);
                    break;
                case 3:
                    NuevoEventoTerminal?.Invoke(null, texto);
                    break;
            }
        }
    }
}
