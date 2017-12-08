using Simulador.Control;
using Simulador.Control_Animacion;
using Simulador.Logica;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace Simulador
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ObservableCollection<string> data = new ObservableCollection<string>();
        Control_Datos Control_datos = new Control_Datos();


        Animacion anima;

        public MainPage()
        {
            this.InitializeComponent();
            anima = new Animacion(this.BaseUri);
            Generador gen = new Generador();
            anima.Control_Global.Add(new ControlAnimacion { Control_numero=0,Posicion=0,Ruta=1, Parada=1, Tipo="Entrada"});
            anima.Control_Global.Add(new ControlAnimacion { Control_numero = 0, Posicion = 0, Ruta = 1, Parada = 1 ,Tipo = "Salida" });
            this.DataContext = this;

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            data.Add(DateTime.Now.ToString());
            anima.Agregar_Persona(cola_entrada_1_2, anima.Control_Global[0]);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //anima.Quitar_Persona(ruta1_2, anima.Control_Global[0]);

            anima.Agregar_Camion(camion_1_2);
            

        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Control_datos.numero++;

            anima.Animacion_Entrar_Persona(cola_entrada_1_2, anima.Control_Global[0],20);
            anima.Animacion_Salida_Persona(cola_salida_1_2, anima.Control_Global[1], 20);
        }

       
    }
}
