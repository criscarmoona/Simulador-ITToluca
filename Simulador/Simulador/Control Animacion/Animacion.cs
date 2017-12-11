using Simulador.Logica;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.System.Threading;
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
        Generador gen;
        ThreadPoolTimer Poleo_datos;
        bool Calculando_datos;
        public string[] dato_ocupado = new string[] { "", "","","","","","","","" };




        public Animacion( Uri _uri, Generador _gen)
        {
            url = _uri;
            gen = _gen;
            gen.NuevoCamion += Gen_NuevoCamion;
            gen.QuitarCamion += Gen_QuitarCamion;
            gen.NuevaPersona += Gen_NuevaPersona;
            Poleo_datos = ThreadPoolTimer.CreatePeriodicTimer(Cargar_Personas, TimeSpan.FromMilliseconds(100));
        }

        private void Gen_NuevaPersona(object sender, AgregarPersona e)
        {
            if (e!=null)
            {
                Agregar_Persona(e.Ruta, e.Parada);
            }
        }

        private void Gen_QuitarCamion(object sender, QuitarCamion e)
        {
            Quitar_Camion(e.Ruta, e.Parada);
        }

        private void Gen_NuevoCamion(object sender, AgregarCamion e)
        {
            if (e!=null)
            {
                Agregar_Camion(e.Ruta, e.Parada, e.Capacidad, e.ABordo, e.NumeroCamion, e.TiempoParadaMilisegundos,e.Bajan);
            }

        }

        public async Task<bool>  Animacion_Entrar_Persona( int ruta , int parada, int velocidad)
        {
            TranslateTransform myTranslate = new TranslateTransform();
            ControlAnimacion _Control = Control_Global.Where(p => p.Ruta == ruta && p.Parada == parada).First();
            var _posicion = _Control.Variables_Animaciones.Where(p => p.Tipo == Enum_Destinos.Entrada).First();
            var Control = _Control.Personas;
            Grid Lista = Control.Grid_Personas;
            if (_Control.Camiones.Count>0)
            {
                if (_Control.Camiones[0].Num_Abordo < _Control.Camiones[0].Total_Cap_Camion)
                {
                   
                    if (Lista.Children.Count > 0 && _posicion.Control_numero < Lista.Children.Count)
                    {
                        _Control.Camiones[0].Num_Abordo++;
                        _Control.Personas.Num_Personsa_Espera--;                        
                        Double tope = 0;
                        alto[1]++;

                        var imagen = Lista.Children[_posicion.Control_numero];
                        _posicion.Control_numero++;

                        for (int i = 0; i < 50; i++)
                        {
                            await Task.Delay(velocidad);
                            if (myTranslate.Y > (-70))
                            {
                                myTranslate.Y = myTranslate.Y - Desplazamiento_persona;
                                if (myTranslate.Y > -70)
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
                                          imagen.RenderTransform = myTranslate;
                        }
                         Lista.Children.RemoveAt(0);
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
                                    Debug.WriteLine(myTranslate.Y);
                                    myTranslate.Y = conteo  *10;
                                    imagen.RenderTransform = myTranslate;
                                    if (conteo < Max_imagen&& myTranslate.Y< Max_imagen*10)
                                    {
                                        item.Visibility = Visibility.Visible;
                                    }
                                    else
                                    {
                                        item.Visibility = Visibility.Collapsed;
                                    }
                                }
                                conteo++;

                            }
                        }                        
                        EventoActualizarDatos?.Invoke(this, Control_Global);
                        return true;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }else
            {
                return false;
            }
        }
        public async Task<bool>  Animacion_Salida_Persona( int ruta, int parada, int velocidad)
        {
            TranslateTransform myTranslate = new TranslateTransform();
            ControlAnimacion _Control = Control_Global.Where(p => p.Ruta == ruta && p.Parada == parada).First();
            var Control = _Control.Personas;
            if (_Control.Camiones.Count > 0)
            {
                if (_Control.Camiones[0].Num_Abordo > 0)
                {
                    _Control.Camiones[0].Num_Abordo--;
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

                    for (int i = 0; i < 8; i++)
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

                    
                    EventoActualizarDatos?.Invoke(this, Control_Global);
                    return true;
                }
                else { return false; }
            }
            else
            {
                return false;
            }
        }
        public async void Agregar_Persona( int ruta, int parada) 
        {
            ControlAnimacion _Control = Control_Global.Where(p => p.Ruta == ruta && p.Parada == parada).First();
            var _posicion = _Control.Variables_Animaciones.Where(p => p.Tipo == Enum_Destinos.Entrada).First();          
            var Control = _Control.Personas;
            _Control.Personas.Num_Personsa_Espera = _Control.Personas.Num_Personsa_Espera+1;
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
        public async void Agregar_Camion(int ruta, int parada,int capacidad, int personas, int identifi, float TiempoMilisegundos,int bajan)
        {
            TranslateTransform myTranslate = new TranslateTransform();
            ControlAnimacion _Control = Control_Global.Where(p => p.Ruta == ruta && p.Parada == parada).First();
            _Control.Camiones.Add(new Control_Camion {Total_Cap_Camion= capacidad ,Num_Abordo= personas,Numero_Camion= identifi,TiempoMilisegundos= TiempoMilisegundos,Bajan=bajan });
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
                Lista.Children.RemoveAt(Lista.Children.Count - 1);
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                              () =>
                              {                                  
                                  if (Lista.Children.Count > 0)
                                  {
                                      Lista.Children[0].RenderTransform = myTranslate;
                                  }
                              });
            }
            _Control.Num_Camiones_Espera = Lista.Children.Count;
            EventoActualizarDatos?.Invoke(this, Control_Global);
        }
        public async void Cargar_Personas(ThreadPoolTimer timer)
        {
            try
            {

           
                int conteo = 0;
                foreach (var item in Control_Global)
                {
                    if (item.Camiones.Count > 0)
                    {
                        if (item.Personas.Num_Personsa_Espera > 0 || item.Camiones[0].Num_Abordo >= item.Camiones[0].Bajan)
                        {
                            if (item.Camiones[0].Num_Abordo < item.Camiones[0].Total_Cap_Camion || item.Camiones[0].Num_Abordo >= item.Camiones[0].Bajan)
                            {
                                if (dato_ocupado[conteo]=="")
                                {
                                    dato_ocupado[conteo] = item.Ruta.ToString() + item.Parada.ToString();

                                    int bajar = item.Camiones[0].Bajan;
                                    Double tiempo = item.Camiones[0].TiempoMilisegundos / item.Camiones[0].Total_Cap_Camion;
                                    int tiempo_entero = (int)Math.Truncate(tiempo);
                                    if (item.Camiones[0].Num_Abordo >= item.Camiones[0].Bajan&& item.Camiones[0].Bajan>0)
                                    {                                                                        
                                        for (int i = 0; i < bajar; i++)
                                        {
                                            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                                             () =>
                                             {
                                                 var a = Animacion_Salida_Persona(item.Ruta, item.Parada, 1);
                                             });

                                            await Task.Delay(tiempo_entero*2);
                                        }
                                        item.Camiones[0].Bajan=0;
                                    }
                                    if (item.Camiones[0].Num_Abordo < item.Camiones[0].Total_Cap_Camion)
                                    {

                                        int veces = item.Camiones[0].Total_Cap_Camion - item.Camiones[0].Num_Abordo;
                                        int aux = item.Personas.Num_Personsa_Espera;
                                        if (veces < aux)
                                        {
                                            veces = aux;
                                        }
                                        for (int i = 0; i < veces; i++)
                                        {

                                            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                                             () =>
                                             {
                                                 var a = Animacion_Entrar_Persona(item.Ruta, item.Parada, 1);
                                             });

                                            await Task.Delay(tiempo_entero * 2 + 10);

                                        }
                                    }
                                    dato_ocupado[conteo] = "";
                                }

                            }
                        }

                    }
                     conteo++;
                }
            }
            catch (Exception)
            {
                
            }

        }

    }
}
