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
        public  ObservableCollection<ControlAnimacion> Control_Global = new ObservableCollection<ControlAnimacion>();

        private int separacion = 13;
        private int tamaño_imagen = 35;
        private int Max_imagen = 6;

        private Double centro_maximo = 200;

        private Uri url;

        public Animacion( Uri _uri)
        {
            url = _uri;
        }

        public async void Animacion_Entrar_Persona(Grid Lista, ControlAnimacion Control, int velocidad)
        {
            TranslateTransform myTranslate = new TranslateTransform();
            Double tope = 0;
            var local_control = Control_Global.Where(p => p.Ruta == Control.Ruta && p.Parada == Control.Parada).First();

            if (Lista.Children.Count > 0 && local_control.Control_numero < Lista.Children.Count)
            {

                var imagen = Lista.Children[local_control.Control_numero];
                local_control.Control_numero++;

                for (int i = 0; i < 50; i++)
                {
                    await Task.Delay(velocidad);
                    if (myTranslate.Y > (-70))
                    {
                        myTranslate.Y = myTranslate.Y - 10;
                    }
                    else
                    {
                        myTranslate.Y = -70;
                        tope = myTranslate.Y;
                        myTranslate.X = myTranslate.X + 10;
                        if (myTranslate.X == centro_maximo)
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
                local_control.Control_numero--;
                local_control.Posicion = local_control.Posicion - separacion;
                if (Lista.Children.Count > 0)
                {
                    foreach (var item in Lista.Children)
                    {
                        myTranslate = (TranslateTransform)item.RenderTransform;
                        if (myTranslate.Y > tope)
                        {
                            Debug.WriteLine(myTranslate.Y);
                            myTranslate.Y = myTranslate.Y - separacion;
                            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                              () =>
                              {
                                  imagen.RenderTransform = myTranslate;
                              });
                        }

                    }
                }

            }
        }
        public async void Animacion_Salida_Persona(Grid Lista, ControlAnimacion Control, int velocidad)
        {


            TranslateTransform myTranslate = new TranslateTransform();

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
                myTranslate.X = myTranslate.X - 10;
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




        }
        public async void Agregar_Persona(Grid Lista, ControlAnimacion Control)
        {
            if (Lista.Children.Count < Max_imagen)
            {
                Image img = new Image();
                var local_control = Control_Global.Where(p => p.Ruta == Control.Ruta && p.Parada == Control.Parada).First();
                img.VerticalAlignment = VerticalAlignment.Top;
                BitmapImage bitmapImage = new BitmapImage();
                img.Width = bitmapImage.DecodePixelWidth = tamaño_imagen;
                TranslateTransform myTranslate = new TranslateTransform();
                myTranslate.Y = myTranslate.Y + local_control.Posicion;
                img.RenderTransform = myTranslate;
                bitmapImage.UriSource = new Uri(url, "Assets/Persona3.png");
                img.Source = bitmapImage;
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                               () =>
                               {
                                   Lista.Children.Add(img);
                               });
                local_control.Posicion = local_control.Posicion + separacion;
            }
        }
        public  async void Quitar_Persona(Grid Lista, ControlAnimacion Control)
        {
            if (Lista.Children.Count > 0)
            {
                var local_control = Control_Global.Where(p => p.Ruta == Control.Ruta && p.Parada == Control.Parada).First();
                local_control.Posicion = local_control.Posicion - separacion;
                int aux = Lista.Children.Count - 1;
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                              () =>
                              {
                                  Lista.Children.RemoveAt(aux);
                              });
            }
            else
            {
                Control_Global[0].Posicion = 0;
            }
        }
    }
}
