using Simulador.Logica;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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
        public MainPage()
        {
            this.InitializeComponent();
            Generador gen = new Generador();           
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            data.Add(DateTime.Now.ToString());
            listView1.ItemsSource = data;
            listView2.ItemsSource = data;
            listView3.ItemsSource = data;

        }
    }
}
