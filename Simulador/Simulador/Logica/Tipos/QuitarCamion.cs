namespace Simulador.Logica
{
    public class QuitarCamion
    {
        public int Ruta { get; set; }
        public int Parada { get; set; }
        public QuitarCamion(int ruta, int parada)
        {
            Ruta = ruta;
            Parada = parada;
        }
    }
}
