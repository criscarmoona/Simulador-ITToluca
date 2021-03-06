﻿using Simulador.Control;
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

namespace Simulador
{
    public sealed partial class MainPage : Page
    {
        ObservableCollection<string> data = new ObservableCollection<string>();
        Control_Datos Control_datos = new Control_Datos();
        public Animacion anima;
        public Generador generador;

        public MainPage()
        {
            generador = new Generador();
            this.InitializeComponent();
            anima = new Animacion(this.BaseUri, generador);
            this.DataContext = this;
            Conectar_grid_DAtos();
            this.DataContext = anima;
            anima.EventoActualizarDatos += Anima_EventoActualizarDatos;
            Hora.Text = generador.horaSimulador.ToString("hh:mm tt");
        }

        private async void EventoCambioHora(object sender, DateTime e)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
                 {
                     Hora.Text = e.ToString("hh:mm tt");
                 });
        }
        private async void Anima_EventoActualizarDatos(object sender, ObservableCollection<ControlAnimacion> e)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                              () =>
                              {

                                  var parada1_1 = e.Where(p => p.Ruta == 1 && p.Parada == 1).First();
                                  num_personas_1_1.Text = parada1_1.Personas.Num_Personsa_Espera.ToString();
                                  num_Camiones_1_1.Text = parada1_1.Num_Camiones_Espera.ToString();
                                  if (parada1_1.Camiones.Count > 0)
                                  {
                                      cap_auto_1_1.Text = parada1_1.Camiones[0].Num_Abordo.ToString();
                                      cap_max_auto_1_1.Text = parada1_1.Camiones[0].Total_Cap_Camion.ToString();
                                      num_linea_1_1.Text = parada1_1.Camiones[0].Numero_Camion.ToString();
                                  }
                                  else
                                  {
                                      num_linea_1_1.Text = "";
                                      cap_auto_1_1.Text ="0";
                                      cap_max_auto_1_1.Text = "0";
                                  }

                                  var parada1_2 = e.Where(p => p.Ruta == 1 && p.Parada == 2).First();
                                  num_personas_1_2.Text = parada1_2.Personas.Num_Personsa_Espera.ToString();
                                  num_Camiones_1_2.Text = parada1_2.Num_Camiones_Espera.ToString();
                                  if (parada1_2.Camiones.Count > 0)
                                  {
                                      cap_auto_1_2.Text = parada1_2.Camiones[0].Num_Abordo.ToString();
                                      cap_max_auto_1_2.Text = parada1_2.Camiones[0].Total_Cap_Camion.ToString();
                                      num_linea_1_2.Text = parada1_2.Camiones[0].Numero_Camion.ToString();
                                  }
                                  else
                                  {
                                      num_linea_1_2.Text = "";
                                      cap_auto_1_2.Text = "0";
                                      cap_max_auto_1_2.Text = "0";
                                  }

                                  var parada1_3 = e.Where(p => p.Ruta == 1 && p.Parada == 3).First();
                                  num_personas_1_3.Text = parada1_3.Personas.Num_Personsa_Espera.ToString();
                                  num_Camiones_1_3.Text = parada1_3.Num_Camiones_Espera.ToString();
                                  if (parada1_3.Camiones.Count > 0)
                                  {
                                      cap_auto_1_3.Text = parada1_3.Camiones[0].Num_Abordo.ToString();
                                      cap_max_auto_1_3.Text = parada1_3.Camiones[0].Total_Cap_Camion.ToString();
                                      num_linea_1_3.Text = parada1_3.Camiones[0].Numero_Camion.ToString();
                                  }
                                  else
                                  {
                                      num_linea_1_3.Text = "";
                                      cap_auto_1_3.Text = "0";
                                      cap_max_auto_1_3.Text = "0";
                                  }

                                  var parada2_1 = e.Where(p => p.Ruta == 2 && p.Parada == 1).First();
                                  num_personas_2_1.Text = parada2_1.Personas.Num_Personsa_Espera.ToString();
                                  num_Camiones_2_1.Text = parada2_1.Num_Camiones_Espera.ToString();
                                  if (parada2_1.Camiones.Count > 0)
                                  {
                                      cap_auto_2_1.Text = parada2_1.Camiones[0].Num_Abordo.ToString();
                                      cap_max_auto_2_1.Text = parada2_1.Camiones[0].Total_Cap_Camion.ToString();
                                      num_linea_2_1.Text = parada2_1.Camiones[0].Numero_Camion.ToString();
                                  }
                                  else
                                  {
                                      num_linea_2_1.Text = "";
                                      cap_auto_2_1.Text = "0";
                                      cap_max_auto_2_1.Text = "0";
                                  }


                                  var parada2_2 = e.Where(p => p.Ruta == 2 && p.Parada == 2).First();
                                  num_personas_2_2.Text = parada2_2.Personas.Num_Personsa_Espera.ToString();
                                  num_Camiones_2_2.Text = parada2_2.Num_Camiones_Espera.ToString();
                                  if (parada2_2.Camiones.Count > 0)
                                  {
                                      cap_auto_2_2.Text = parada2_2.Camiones[0].Num_Abordo.ToString();
                                      cap_max_auto_2_2.Text = parada2_2.Camiones[0].Total_Cap_Camion.ToString();
                                      num_linea_2_2.Text = parada2_2.Camiones[0].Numero_Camion.ToString();
                                  }
                                  else
                                  {
                                      num_linea_2_2.Text = "";
                                      cap_auto_2_2.Text = "0";
                                      cap_max_auto_2_2.Text = "0";
                                  }


                                  var parada2_3 = e.Where(p => p.Ruta == 2 && p.Parada == 3).First();
                                  num_personas_2_3.Text = parada2_3.Personas.Num_Personsa_Espera.ToString();
                                  num_Camiones_2_3.Text = parada2_3.Num_Camiones_Espera.ToString();
                                  if (parada2_3.Camiones.Count > 0)
                                  {
                                      cap_auto_2_3.Text = parada2_3.Camiones[0].Num_Abordo.ToString();
                                      cap_max_auto_2_3.Text = parada2_3.Camiones[0].Total_Cap_Camion.ToString();
                                      num_linea_2_3.Text = parada2_3.Camiones[0].Numero_Camion.ToString();
                                  }
                                  else
                                  {
                                      num_linea_2_3.Text = "";
                                      cap_auto_2_3.Text = "0";
                                      cap_max_auto_2_3.Text = "0";
                                  }

                                  var parada3_1 = e.Where(p => p.Ruta == 3 && p.Parada == 1).First();
                                  num_personas_3_1.Text = parada3_1.Personas.Num_Personsa_Espera.ToString();
                                  num_Camiones_3_1.Text = parada3_1.Num_Camiones_Espera.ToString();
                                  if (parada3_1.Camiones.Count > 0)
                                  {
                                      cap_auto_3_1.Text = parada3_1.Camiones[0].Num_Abordo.ToString();
                                      cap_max_auto_3_1.Text = parada3_1.Camiones[0].Total_Cap_Camion.ToString();
                                      num_linea_3_1.Text = parada3_1.Camiones[0].Numero_Camion.ToString();
                                  }
                                  else
                                  {
                                      num_linea_3_1.Text = "";
                                      cap_auto_3_1.Text = "0";
                                      cap_max_auto_3_1.Text = "0"; 
                                  }

                                  var parada3_2 = e.Where(p => p.Ruta == 3 && p.Parada == 2).First();
                                  num_personas_3_2.Text = parada3_2.Personas.Num_Personsa_Espera.ToString();
                                  num_Camiones_3_2.Text = parada3_2.Num_Camiones_Espera.ToString();
                                  if (parada3_2.Camiones.Count > 0)
                                  {
                                      cap_auto_3_2.Text = parada3_2.Camiones[0].Num_Abordo.ToString();
                                      cap_max_auto_3_2.Text = parada3_2.Camiones[0].Total_Cap_Camion.ToString();
                                      num_linea_3_2.Text = parada3_2.Camiones[0].Numero_Camion.ToString();
                                  }
                                  else
                                  {
                                      num_linea_3_2.Text = "";
                                      cap_auto_3_2.Text = "0";
                                      cap_max_auto_3_2.Text = "0";
                                  }

                                  var parada3_3 = e.Where(p => p.Ruta == 3 && p.Parada == 3).First();
                                  num_personas_3_3.Text = parada3_3.Personas.Num_Personsa_Espera.ToString();
                                  num_Camiones_3_3.Text = parada3_3.Num_Camiones_Espera.ToString();
                                  if (parada3_3.Camiones.Count > 0)
                                  {
                                      cap_auto_3_3.Text = parada3_3.Camiones[0].Num_Abordo.ToString();
                                      cap_max_auto_3_3.Text = parada3_3.Camiones[0].Total_Cap_Camion.ToString();
                                      num_linea_3_3.Text = parada3_3.Camiones[0].Numero_Camion.ToString();
                                  }
                                  else
                                  {
                                      num_linea_3_3.Text = "";
                                      cap_auto_3_3.Text = "0";
                                      cap_max_auto_3_3.Text = "0";
                                  }

                              });
        }
        private void Conectar_grid_DAtos()
        {
            Animacion.Control_Global.Add(new ControlAnimacion
            {
                Ruta = 1,
                Parada = 1,
                Variables_Animaciones = new ObservableCollection<Control_int_Animacion> {
                    new Control_int_Animacion {Tipo = Control_Animacion.Enum_Destinos.Entrada},
                    new Control_int_Animacion {Tipo = Control_Animacion.Enum_Destinos.Salida}},
                Personas = new Control_Persona { Grid_Personas = cola_entrada_1_1, Grid_Personas_SALIDA = cola_salida_1_1 },
                Camiones = new ObservableCollection<Control_Camion>(),
                Grid_Camiones = camion_1_1

            });
            Animacion.Control_Global.Add(new ControlAnimacion
            {
                Ruta = 1,
                Parada = 2,
                Variables_Animaciones = new ObservableCollection<Control_int_Animacion> {
                    new Control_int_Animacion {Tipo = Control_Animacion.Enum_Destinos.Entrada},
                    new Control_int_Animacion {Tipo = Control_Animacion.Enum_Destinos.Salida}},
                Personas = new Control_Persona { Grid_Personas = cola_entrada_1_2, Grid_Personas_SALIDA = cola_salida_1_2 },
                Camiones = new ObservableCollection<Control_Camion>(),
                Grid_Camiones = camion_1_2
            });
            Animacion.Control_Global.Add(new ControlAnimacion
            {
                Ruta = 1,
                Parada = 3,
                Variables_Animaciones = new ObservableCollection<Control_int_Animacion> {
                    new Control_int_Animacion {Tipo = Control_Animacion.Enum_Destinos.Entrada},
                    new Control_int_Animacion {Tipo = Control_Animacion.Enum_Destinos.Salida}},
                Personas = new Control_Persona { Grid_Personas = cola_entrada_2_3, Grid_Personas_SALIDA = cola_salida_2_3 },
                Camiones = new ObservableCollection<Control_Camion>(),
                Grid_Camiones = camion_1_3
            });

            Animacion.Control_Global.Add(new ControlAnimacion
            {
                Ruta = 2,
                Parada = 1,
                Variables_Animaciones = new ObservableCollection<Control_int_Animacion> {
                    new Control_int_Animacion {Tipo = Control_Animacion.Enum_Destinos.Entrada},
                    new Control_int_Animacion {Tipo = Control_Animacion.Enum_Destinos.Salida}},
                Personas = new Control_Persona { Grid_Personas = cola_entrada_2_1, Grid_Personas_SALIDA = cola_salida_2_1 },
                Camiones = new ObservableCollection<Control_Camion>(),
                Grid_Camiones = camion_2_1
            });
            Animacion.Control_Global.Add(new ControlAnimacion
            {
                Ruta = 2,
                Parada = 2,
                Variables_Animaciones = new ObservableCollection<Control_int_Animacion> {
                    new Control_int_Animacion {Tipo = Control_Animacion.Enum_Destinos.Entrada},
                    new Control_int_Animacion {Tipo = Control_Animacion.Enum_Destinos.Salida}},
                Personas = new Control_Persona { Grid_Personas = cola_entrada_2_2, Grid_Personas_SALIDA = cola_salida_2_2 },
                Camiones = new ObservableCollection<Control_Camion>(),
                Grid_Camiones = camion_2_2
            });
            Animacion.Control_Global.Add(new ControlAnimacion
            {
                Ruta = 2,
                Parada = 3,
                Variables_Animaciones = new ObservableCollection<Control_int_Animacion> {
                    new Control_int_Animacion {Tipo = Control_Animacion.Enum_Destinos.Entrada},
                    new Control_int_Animacion {Tipo = Control_Animacion.Enum_Destinos.Salida}},
                Personas = new Control_Persona { Grid_Personas = cola_entrada_2_3, Grid_Personas_SALIDA = cola_salida_2_3 },
                Camiones = new ObservableCollection<Control_Camion>(),
                Grid_Camiones = camion_2_3
            });

            Animacion.Control_Global.Add(new ControlAnimacion
            {
                Ruta = 3,
                Parada = 1,
                Variables_Animaciones = new ObservableCollection<Control_int_Animacion> {
                    new Control_int_Animacion {Tipo = Control_Animacion.Enum_Destinos.Entrada},
                    new Control_int_Animacion {Tipo = Control_Animacion.Enum_Destinos.Salida}},
                Personas = new Control_Persona { Grid_Personas = cola_entrada_3_1, Grid_Personas_SALIDA = cola_salida_3_1 },
                Camiones = new ObservableCollection<Control_Camion>(),
                Grid_Camiones = camion_3_1
            });
            Animacion.Control_Global.Add(new ControlAnimacion
            {
                Ruta = 3,
                Parada = 2,
                Variables_Animaciones = new ObservableCollection<Control_int_Animacion> {
                    new Control_int_Animacion {Tipo = Control_Animacion.Enum_Destinos.Entrada},
                    new Control_int_Animacion {Tipo = Control_Animacion.Enum_Destinos.Salida}},
                Personas = new Control_Persona { Grid_Personas = cola_entrada_3_2, Grid_Personas_SALIDA = cola_salida_3_2 },
                Camiones = new ObservableCollection<Control_Camion>(),
                Grid_Camiones = camion_3_2
            });
            Animacion.Control_Global.Add(new ControlAnimacion
            {
                Ruta = 3,
                Parada = 3,
                Variables_Animaciones = new ObservableCollection<Control_int_Animacion> {
                    new Control_int_Animacion {Tipo = Control_Animacion.Enum_Destinos.Entrada},
                    new Control_int_Animacion {Tipo = Control_Animacion.Enum_Destinos.Salida}},
                Personas = new Control_Persona { Grid_Personas = cola_entrada_3_3, Grid_Personas_SALIDA = cola_salida_3_3 },
                Camiones = new ObservableCollection<Control_Camion>(),
                Grid_Camiones = camion_3_3
            });
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            data.Add(DateTime.Now.ToString());
            for (int i = 0; i < 75; i++)
            {
                anima.Agregar_Persona(1, 2);
                await Task.Delay(100);
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //anima.Quitar_Persona(ruta1_2, anima.Control_Global[0]);

            anima.Agregar_Camion(1,2,40,10,1253,1000,9);


        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            anima.Animacion_Entrar_Persona(1, 2, 50);
            //anima.Animacion_Salida_Persona( 1, 2, 5);
        }

        private void volumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
                generador.CambiarVelocidad(e.NewValue);
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            anima.Animacion_Salida_Persona(1, 2, 50);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            anima.Quitar_Camion(1, 2);
        }
    }
}
