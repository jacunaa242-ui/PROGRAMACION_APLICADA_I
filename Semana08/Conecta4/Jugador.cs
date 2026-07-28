using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Conecta4
{
    public abstract class Jugador
    {
        public int Id { get; }
        public string Nombre { get; }
        public Brush ColorFicha { get; }

        protected Jugador(int id, string nombre, Brush colorFicha)
        {
            Id = id;
            Nombre = nombre;
            ColorFicha = colorFicha;
        }

        public abstract bool EsIA { get; }
    }

    public class JugadorHumano : Jugador
    {
        public JugadorHumano(int id, string nombre, Brush colorFicha)
            : base(id, nombre, colorFicha) { }

        public override bool EsIA => false;
    }

    public class JugadorIA : Jugador
    {
        public JugadorIA(int id, string nombre, Brush colorFicha)
            : base(id, nombre, colorFicha) { }

        public override bool EsIA => true;

       
        public int ObtenerColumnaElegida(Tablero tablero)
        {
            var rnd = new System.Random();
            int columna;
            do
            {
                columna = rnd.Next(0, Tablero.COLUMNAS);
            } while (tablero.EstaColumnaLlena(columna));

            return columna;
        }
    }
}
