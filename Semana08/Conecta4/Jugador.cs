using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Conecta4
{

    public enum NivelDificultad
    {
        Facil,
        Medio,
        Dificil
    }

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
        public NivelDificultad Dificultad { get; set; }

        public JugadorIA(int id, string nombre, Brush colorFicha, NivelDificultad dificultad)
            : base(id, nombre, colorFicha)
        {
            Dificultad = dificultad;
        }

        public override bool EsIA => true;

        public int ObtenerColumnaElegida(Tablero tablero, int idOponente)
        {
            switch (Dificultad)
            {
                case NivelDificultad.Facil:
                    return ObtenerColumnaFacil(tablero);

                case NivelDificultad.Medio:
                    return ObtenerColumnaMedio(tablero, idOponente);

                case NivelDificultad.Dificil:
                    return ObtenerColumnaMiniMax(tablero, idOponente);

                default:
                    return ObtenerColumnaFacil(tablero);
            }
        }

        // Dificultad Fácil: Elección totalmente aleatoria
        private int ObtenerColumnaFacil(Tablero tablero)
        {
            Random rnd = new Random();
            int col;
            do
            {
                col = rnd.Next(0, Tablero.COLUMNAS);
            } while (tablero.EstaColumnaLlena(col));

            return col;
        }

        // Dificultad Medio: Ataca si gana, bloquea si el rival gana, o aleatorio
        private int ObtenerColumnaMedio(Tablero tablero, int idOponente)
        {
            // 1. ¿Puedo ganar en esta jugada?
            for (int c = 0; c < Tablero.COLUMNAS; c++)
            {
                if (!tablero.EstaColumnaLlena(c))
                {
                    Tablero sim = tablero.Clonar();
                    sim.ColocarFicha(c, Id);
                    if (sim.ComprobarVictoria(Id)) return c;
                }
            }

            // 2. ¿Debo bloquear al oponente para que no gane?
            for (int c = 0; c < Tablero.COLUMNAS; c++)
            {
                if (!tablero.EstaColumnaLlena(c))
                {
                    Tablero sim = tablero.Clonar();
                    sim.ColocarFicha(c, idOponente);
                    if (sim.ComprobarVictoria(idOponente)) return c;
                }
            }

            return ObtenerColumnaFacil(tablero);
        }

        // Dificultad Difícil: Algoritmo MiniMax con profundidad de búsqueda
        private int ObtenerColumnaMiniMax(Tablero tablero, int idOponente)
        {
            int mejorPuntaje = int.MinValue;
            int mejorColumna = -1;
            int profundidad = 4; // Profundidad de simulación a futuro

            for (int c = 0; c < Tablero.COLUMNAS; c++)
            {
                if (!tablero.EstaColumnaLlena(c))
                {
                    Tablero sim = tablero.Clonar();
                    sim.ColocarFicha(c, Id);

                    int puntaje = MiniMax(sim, profundidad - 1, false, Id, idOponente);
                    if (puntaje > mejorPuntaje)
                    {
                        mejorPuntaje = puntaje;
                        mejorColumna = c;
                    }
                }
            }

            return mejorColumna != -1 ? mejorColumna : ObtenerColumnaFacil(tablero);
        }

        private int MiniMax(Tablero tab, int profundidad, bool esMaximizando, int idIA, int idHumano)
        {
            if (tab.ComprobarVictoria(idIA)) return 1000 + profundidad;
            if (tab.ComprobarVictoria(idHumano)) return -1000 - profundidad;
            if (tab.EstaLleno() || profundidad == 0) return 0;

            if (esMaximizando)
            {
                int maxEval = int.MinValue;
                for (int c = 0; c < Tablero.COLUMNAS; c++)
                {
                    if (!tab.EstaColumnaLlena(c))
                    {
                        Tablero sim = tab.Clonar();
                        sim.ColocarFicha(c, idIA);
                        int eval = MiniMax(sim, profundidad - 1, false, idIA, idHumano);
                        maxEval = Math.Max(maxEval, eval);
                    }
                }
                return maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                for (int c = 0; c < Tablero.COLUMNAS; c++)
                {
                    if (!tab.EstaColumnaLlena(c))
                    {
                        Tablero sim = tab.Clonar();
                        sim.ColocarFicha(c, idHumano);
                        int eval = MiniMax(sim, profundidad - 1, true, idIA, idHumano);
                        minEval = Math.Min(minEval, eval);
                    }
                }
                return minEval;
            }
        }
    }
}
