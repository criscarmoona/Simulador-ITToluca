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

        private DateTimeOffset tiempoSalidaCamionesTenangoMinutos;
        private DateTimeOffset tiempoSalidaCamionesTolucaMinutos;
        private DateTimeOffset tiempoSalidaCamionesTerminalMinutos;


        public Generador()
        {
            IniciarSimulador();
        }
        private void CrearCamion(int ruta, int parada, int tiempoParadaMs, DateTimeOffset horaSalida)
        {
            camiones.Nuevo(ruta, 40, tiempoParadaMs, horaSalida, parada);
            Imprimir("NuevoCamion r: " + ruta + " p: " + parada);
            if (ruta == 1)
            {
                NuevoCamion?.Invoke(null, camiones.camionesTenango.FirstOrDefault());
            }else if (ruta == 2)
            {
                NuevoCamion?.Invoke(null, camiones.camionesToluca.FirstOrDefault());
            }else if (ruta == 3)
            {
                NuevoCamion?.Invoke(null, camiones.camionesTerminal.FirstOrDefault());
            }
        }

        public void IniciarSimulador()
        {
            multiplicadorTiempo = 1;
            tiempoSalidaCamionesTenangoMinutos = horaSimulador.AddMinutes(RandomDouble(1, 5));
            tiempoSalidaCamionesTolucaMinutos = horaSimulador.AddMinutes(RandomDouble(1, 5));
            tiempoSalidaCamionesTerminalMinutos = horaSimulador.AddMinutes(RandomDouble(1, 5));
            //Imprimir("Tiempo para siguiente camion : " + tiempoSalidaCamionesTenangoMinutos);
            temporizador = ThreadPoolTimer.CreatePeriodicTimer(CambioHora, TimeSpan.FromMilliseconds(CalcularPeriodo()));
        }
        void CambioHora(ThreadPoolTimer timer)
        {
            horaSimulador = horaSimulador.AddMinutes(1);
            NuevaHora?.Invoke(null, horaSimulador);
            //Imprimir("Cambianido hora: " + horaSimulador);
            if (horaSimulador >= tiempoSalidaCamionesTenangoMinutos)
            {
                int miliSegundosSalida = random.Next(1000, 5000);
                //Imprimir("Creando camion Tenango con salida: " + horaSimulador.AddMilliseconds(miliSegundosSalida));

                CrearCamion(1, 1, miliSegundosSalida, horaSimulador.AddMilliseconds(miliSegundosSalida));

                tiempoSalidaCamionesTenangoMinutos = horaSimulador.AddMinutes(RandomDouble(1, 5));
                //Imprimir("Tiempo para siguiente camion Tenango: " + tiempoSalidaCamionesTenangoMinutos);
            }
            if (horaSimulador >= tiempoSalidaCamionesTolucaMinutos)
            {
                int miliSegundosSalida = random.Next(1000, 5000);
                //Imprimir("Creando camion Toluca con salida: " + horaSimulador.AddMilliseconds(miliSegundosSalida));

                CrearCamion(2, 1, miliSegundosSalida, horaSimulador.AddMilliseconds(miliSegundosSalida));

                tiempoSalidaCamionesTolucaMinutos = horaSimulador.AddMinutes(RandomDouble(1, 5));
                //Imprimir("Tiempo para siguiente camion Toluca: " + tiempoSalidaCamionesTolucaMinutos);
            }
            if (horaSimulador >= tiempoSalidaCamionesTerminalMinutos)
            {
                int miliSegundosSalida = random.Next(1000, 5000);
                //Imprimir("Creando camion Terminal con salida : " + horaSimulador.AddMilliseconds(miliSegundosSalida));

                CrearCamion(3, 1, miliSegundosSalida, horaSimulador.AddMilliseconds(miliSegundosSalida));

                tiempoSalidaCamionesTerminalMinutos = horaSimulador.AddMinutes(RandomDouble(1, 5));
                //Imprimir("Tiempo para siguiente camion Terminal : " + tiempoSalidaCamionesTerminalMinutos);
            }
            if (camiones.camionesTenango.Any(p => p.HoraSalida <= horaSimulador))
            {
                //Imprimir("Cambniando parada camion Tenango");
                var camionSale = camiones.camionesTenango.Where(p => p.HoraSalida <= horaSimulador).FirstOrDefault();
                QuitarCamionV(camionSale);
                camiones.camionesTenango.Remove(camionSale);
                camionSale.Parada = camionSale.Parada + 1;
                if (camionSale.Parada < 4)
                {
                    int miliSegundosSalida = random.Next(1000, 5000);
                    //Imprimir("Cambio parada Tenango con salida: " + horaSimulador.AddMilliseconds(miliSegundosSalida));
                    CrearCamion(camionSale.Ruta, camionSale.Parada, miliSegundosSalida, horaSimulador.AddMilliseconds(miliSegundosSalida));
                }
            }
            if (camiones.camionesTerminal.Any(p => p.HoraSalida <= horaSimulador))
            {
                //Imprimir("Cambniando parada  camion terminal");
                var camionSale = camiones.camionesTerminal.Where(p => p.HoraSalida <= horaSimulador).FirstOrDefault();
                QuitarCamionV(camionSale);
                camiones.camionesTerminal.Remove(camionSale);
                camionSale.Parada = camionSale.Parada + 1;
                if (camionSale.Parada < 4)
                {
                    int miliSegundosSalida = random.Next(1000, 5000);
                    //Imprimir("Cambio ruta Terminal con salida: " + horaSimulador.AddMilliseconds(miliSegundosSalida));
                    CrearCamion(camionSale.Ruta, camionSale.Parada, miliSegundosSalida, horaSimulador.AddMilliseconds(miliSegundosSalida));
                }
            }
            if (camiones.camionesToluca.Any(p => p.HoraSalida <= horaSimulador))
            {
                //Imprimir("Cambniando parada camion Toluca");
                var camionSale = camiones.camionesToluca.Where(p => p.HoraSalida <= horaSimulador).FirstOrDefault();
                QuitarCamionV(camionSale);
                camiones.camionesToluca.Remove(camionSale);
                camionSale.Parada = camionSale.Parada + 1;
                if (camionSale.Parada < 4)
                {
                    int miliSegundosSalida = random.Next(1000, 5000);
                    //Imprimir("Cambio parada Toluca con salida: " + horaSimulador.AddMilliseconds(miliSegundosSalida));
                    CrearCamion(camionSale.Ruta, camionSale.Parada, miliSegundosSalida, horaSimulador.AddMilliseconds(miliSegundosSalida));
                }
            }
            //Imprimir(horaSimulador.ToString("hh:mm:ss tt") + " multiplicador: " + multiplicadorTiempo);
        }
        public void QuitarCamionV(AgregarCamion camion)
        {
            Imprimir("Quitar Camion r: " + camion.Ruta + " p: " + camion.Parada);
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
            double nuevoPeriodo = 1000 / multiplicadorTiempo;
            //Imprimir("Periodo: " + nuevoPeriodo);
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
