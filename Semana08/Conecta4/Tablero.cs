using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conecta4
{
    public class Tablero
    {
        public const int FILAS = 6;
        public const int COLUMNAS = 7;

        private readonly int[,] celdas;

        public Tablero()
        {
            celdas = new int[FILAS, COLUMNAS];
            Reiniciar();
        }

        public void Reiniciar()
        {
            for (int f = 0; f < FILAS; f++)
            {
                for (int c = 0; c < COLUMNAS; c++)
                {
                    celdas[f, c] = 0; 
                }
            }
        }

        public int ObtenerFichaEn(int fila, int columna)
        {
            return celdas[fila, columna];
        }

        public bool EstaColumnaLlena(int columna)
        {
            return celdas[0, columna] != 0;
        }


        public int ColocarFicha(int columna, int idJugador)
        {
            if (columna < 0 || columna >= COLUMNAS || EstaColumnaLlena(columna))
                return -1; 

            for (int f = FILAS - 1; f >= 0; f--)
            {
                if (celdas[f, columna] == 0)
                {
                    celdas[f, columna] = idJugador;
                    return f; 
                }
            }

            return -1;
        }

        public bool EstaLleno()
        {
            for (int c = 0; c < COLUMNAS; c++)
            {
                if (!EstaColumnaLlena(c)) return false;
            }
            return true;
        }

        public bool ComprobarVictoria(int idJugador)
        {
            // 1. Horizontal
            for (int f = 0; f < FILAS; f++)
            {
                for (int c = 0; c <= COLUMNAS - 4; c++)
                {
                    if (celdas[f, c] == idJugador &&
                        celdas[f, c + 1] == idJugador &&
                        celdas[f, c + 2] == idJugador &&
                        celdas[f, c + 3] == idJugador)
                        return true;
                }
            }

            // 2. Vertical
            for (int f = 0; f <= FILAS - 4; f++)
            {
                for (int c = 0; c < COLUMNAS; c++)
                {
                    if (celdas[f, c] == idJugador &&
                        celdas[f + 1, c] == idJugador &&
                        celdas[f + 2, c] == idJugador &&
                        celdas[f + 3, c] == idJugador)
                        return true;
                }
            }

            // 3. Diagonal Ascendente 
            for (int f = 3; f < FILAS; f++)
            {
                for (int c = 0; c <= COLUMNAS - 4; c++)
                {
                    if (celdas[f, c] == idJugador &&
                        celdas[f - 1, c + 1] == idJugador &&
                        celdas[f - 2, c + 2] == idJugador &&
                        celdas[f - 3, c + 3] == idJugador)
                        return true;
                }
            }

            // 4. Diagonal Descendente
            for (int f = 0; f <= FILAS - 4; f++)
            {
                for (int c = 0; c <= COLUMNAS - 4; c++)
                {
                    if (celdas[f, c] == idJugador &&
                        celdas[f + 1, c + 1] == idJugador &&
                        celdas[f + 2, c + 2] == idJugador &&
                        celdas[f + 3, c + 3] == idJugador)
                        return true;
                }
            }

            return false;
        }
    }
}
