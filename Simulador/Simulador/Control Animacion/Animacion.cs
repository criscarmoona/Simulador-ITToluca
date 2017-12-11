using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Simulador.Control_Animacion
{
    public class Animacion
    {
        public event EventHandler<ObservableCollection<ControlAnimacion>> EventoActualizarDatos;
        public static ObservableCollection<ControlAnimacion> Control_Global = new ObservableCollection<ControlAnimacion>();       

        private int separacion = 13;
        private int tamaño_imagen = 35;
        private int Max_imagen = 6;
        private int Desplazamiento_persona = 30;

        private Double centro_maximo = 200;
        private Uri url;
        public int[] alto = new int[] { 1,2};

        public Animacion( Uri _uri)
        {
            url = _uri;
           
        }

        public async void Animacion_Entrar_Persona( int ruta , int parada, int velocidad)
        {
            TranslateTransform myTranslate = new TranslateTransform();
            ControlAnimacion _Control = Control_Global.Where(p => p.Ruta == ruta && p.Parada == parada).First();
            var _posicion = _Control.Variables_Animaciones.Where(p => p.Tipo == Enum_Destinos.Entrada).First();
            var Control = _Control.Personas;
            Grid Lista = Control.Grid_Personas;
            Double tope = 0;
            alto[1]++;

            if (Lista.Children.Count > 0 && _posicion.Control_numero < Lista.Children.Count)
            {

                var imagen = Lista.Children[_posicion.Control_numero];
                _posicion.Control_numero++;

                for (int i = 0; i < 50; i++)
                {
                    await Task.Delay(velocidad);
                    if (myTranslate.Y > (-70))
                    {
                        myTranslate.Y = myTranslate.Y - Desplazamiento_persona;
                        if (myTranslate.Y> -70)
                        {
                            myTranslate.Y = -70;
                        }
                    }
                    else
                    {
                        myTranslate.Y = -70;
                        tope = myTranslate.Y;
                        myTranslate.X = myTranslate.X + Desplazamiento_persona;
                        if (myTranslate.X >= centro_maximo)
                        {
                            i = 50;
                        }
                    }
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                              () =>
                              {
                                  imagen.RenderTransform = myTranslate;
                              });
                }
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                              () =>
                              {
                                  Lista.Children.RemoveAt(0);
                              });
                Debug.WriteLine("total " + myTranslate.Y);
                _posicion.Control_numero--;
                _posicion.Posicion = _posicion.Posicion - separacion;
                if (Lista.Children.Count > 0)
                {
                    int conteo = 0;
                    foreach (var item in Lista.Children)
                    {
                        myTranslate = (TranslateTransform)item.RenderTransform;
                        if (myTranslate.Y > tope)
                        {

                            if (conteo < Max_imagen)
                            {
                                item.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                item.Visibility = Visibility.Collapsed;
                            }
                            Debug.WriteLine(myTranslate.Y);
                            myTranslate.Y = myTranslate.Y - separacion;
                            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                              () =>
                              {
                                  imagen.RenderTransform = myTranslate;
                              });
                        }
                        conteo++;

                    }
                }
                _Control.Personas.Num_Personsa_Espera--;
                _Control.Camiones[0].Num_Abordo++;
                EventoActualizarDatos?.Invoke(this, Control_Global);

            }
        }
        public async void Animacion_Salida_Persona( int ruta, int parada, int velocidad)
        {
            TranslateTransform myTranslate = new TranslateTransform();
            ControlAnimacion _Control = Control_Global.Where(p => p.Ruta == ruta && p.Parada == parada).First();
            var Control = _Control.Personas;
            Grid Lista = Control.Grid_Personas_SALIDA;

            Image img = new Image();
            img.VerticalAlignment = VerticalAlignment.Center;
            BitmapImage bitmapImage = new BitmapImage();
            img.Width = bitmapImage.DecodePixelWidth = tamaño_imagen;
            myTranslate.X = myTranslate.X + centro_maximo;
            img.RenderTransform = myTranslate;
            bitmapImage.UriSource = new Uri(url, "Assets/Persona3.png");
            img.Source = bitmapImage;
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                               () =>
                               {
                                   Lista.Children.Add(img);
                               });

            for (int i = 0; i < 25; i++)
            {
                await Task.Delay(velocidad);
                myTranslate.X = myTranslate.X - Desplazamiento_persona;
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                                    () =>
                                    {
                                        img.RenderTransform = myTranslate;
                                    });

            }
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                                () =>
                                {
                                    Lista.Children.Remove(img);
                                });
            
            _Control.Camiones[0].Num_Abordo--;
            EventoActualizarDatos?.Invoke(this, Control_Global);
        }
        public async void Agregar_Persona( int ruta, int parada) 
        {
            ControlAnimacion _Control = Control_Global.Where(p => p.Ruta == ruta && p.Parada == parada).First();
            var _posicion = _Control.Variables_Animaciones.Where(p => p.Tipo == Enum_Destinos.Entrada).First();
            var Control = _Control.Personas;
            Grid Lista = Control.Grid_Personas;
            Image img = new Image();
            img.VerticalAlignment = VerticalAlignment.Top;
            BitmapImage bitmapImage = new BitmapImage();
            img.Width = bitmapImage.DecodePixelWidth = tamaño_imagen;
            TranslateTransform myTranslate = new TranslateTransform();
            myTranslate.Y = myTranslate.Y + _posicion.Posicion;
            img.RenderTransform = myTranslate;
            switch (ruta)
            {
                case 1:
                    bitmapImage.UriSource = new Uri(url, "Assets/Persona.png");
                    break;
                case 2:
                    bitmapImage.UriSource = new Uri(url, "Assets/Persona2.png");
                    break;
                case 3:
                    bitmapImage.UriSource = new Uri(url, "Assets/Persona3.png");
                    break;
            }
            
            img.Source = bitmapImage;
            if (Lista.Children.Count < Max_imagen)
            {
                img.Visibility = Visibility.Visible;
            }
            else
            {
                img.Visibility = Visibility.Collapsed;
            }              

            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                            () =>
                            {
                                Lista.Children.Add(img);
                            });
            _posicion.Posicion = _posicion.Posicion + separacion;
            _Control.Personas.Num_Personsa_Espera++;
            EventoActualizarDatos?.Invoke(this, Control_Global);
        }
        public  async void Quitar_Persona( int ruta, int parada)
        {

            ControlAnimacion _Control = Control_Global.Where(p => p.Ruta == ruta && p.Parada == parada).First();
            var _posicion = _Control.Variables_Animaciones.Where(p=>p.Tipo== Enum_Destinos.Entrada).First();
            var Control = _Control.Personas;
            Grid Lista = Control.Grid_Personas;
            if (Lista.Children.Count > 0)
            {
                _posicion.Posicion = _posicion.Posicion - separacion;
                int aux = Lista.Children.Count - 1;
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                              () =>
                              {
                                  Lista.Children.RemoveAt(aux);
                              });
                _Control.Personas.Num_Personsa_Espera--;
                EventoActualizarDatos?.Invoke(this, Control_Global);
            }
            else
            {
                _posicion.Posicion = 0;
            }
        }
        public async void Agregar_Camion(int ruta, int parada,int capacidad, int personas, int identifi)
        {
            TranslateTransform myTranslate = new TranslateTransform();
            ControlAnimacion _Control = Control_Global.Where(p => p.Ruta == ruta && p.Parada == parada).First();
            _Control.Camiones.Add(new Control_Camion {Total_Cap_Camion= capacidad ,Num_Abordo= personas,Numero_Camion= identifi});
            Grid Lista = _Control.Grid_Camiones;
            Image img = new Image();
            img.VerticalAlignment = VerticalAlignment.Center;
            BitmapImage bitmapImage = new BitmapImage();
            switch (ruta)
            {
                case 1:
                    bitmapImage.UriSource = new Uri(url, "Assets/camionR.png");
                    break;
                case 2:
                    bitmapImage.UriSource = new Uri(url, "Assets/camionA.png");
                    break;
                case 3:
                    bitmapImage.UriSource = new Uri(url, "Assets/camionV.png");
                    break;
            }            
            img.Source = bitmapImage;
            Viewbox vi = new Viewbox();
            vi.Stretch = Stretch.Uniform;
            if (Lista.Children.Count!=0)
            {
                myTranslate.X = myTranslate.X + 20;
                myTranslate.Y = myTranslate.Y - 20;
            }            
            vi.Child = img;
            vi.RenderTransform = myTranslate;
            if (Lista.Children.Count < 2)
            {
                    vi.Visibility = Visibility.Visible;
            }
            else
            {
                    vi.Visibility = Visibility.Collapsed;
            }

            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                                () =>
                                {
                                    Lista.Children.Insert(0, vi);
                                });
            _Control.Num_Camiones_Espera= Lista.Children.Count;
            EventoActualizarDatos?.Invoke(this, Control_Global) ;
        }
        public async void Quitar_Camion(int ruta, int parada)
        {
            ControlAnimacion _Control = Control_Global.Where(p => p.Ruta == ruta && p.Parada == parada ).First();
            var Control = _Control.Camiones[0];
            TranslateTransform myTranslate = new TranslateTransform();
            Grid Lista = _Control.Grid_Camiones;
            myTranslate.X = 0;
            myTranslate.Y = 0;          

            if (Lista.Children.Count > 0)
            {
                _Control.Camiones.RemoveAt(_Control.Camiones.Count - 1);
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                              () =>
                              {
                                  Lista.Children.RemoveAt(Lista.Children.Count-1);
                                  Lista.Children[0].RenderTransform = myTranslate;
                              });
            }
            _Control.Num_Camiones_Espera = Lista.Children.Count;
            EventoActualizarDatos?.Invoke(this, Control_Global);
        }
    }
}
