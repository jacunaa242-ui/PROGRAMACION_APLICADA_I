using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Conecta4
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Tablero tablero;
        private Jugador[] jugadores;
        private int indiceJugadorActual;
        private Button[,] matrizBotonesUI; 
        private bool juegoFinalizado;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InicializarJuego();
        }

        private void InicializarJuego()
        {
            tablero = new Tablero();
            juegoFinalizado = false;

            jugadores = new Jugador[]
            {
                new JugadorHumano(1, "Jugador 1 (Rojo)", Brushes.Red),
                new JugadorHumano(2, "Jugador 2 (Amarillo)", Brushes.Gold)
            };

            indiceJugadorActual = 0;
            ConstruirTableroUI();
            ActualizarEstadoTurno();
        }


        private void ConstruirTableroUI()
        {
            gridTablero.Children.Clear();
            matrizBotonesUI = new Button[Tablero.FILAS, Tablero.COLUMNAS];

            for (int f = 0; f < Tablero.FILAS; f++)
            {
                for (int c = 0; c < Tablero.COLUMNAS; c++)
                {
                    Button btn = new Button
                    {
                        Margin = new Thickness(4),
                        Background = Brushes.Transparent,
                        BorderThickness = new Thickness(0),
                        Tag = c // Guardamos la columna a la que pertenece el botón
                    };

                    // Dibujamos un círculo representando la casilla vacía
                    btn.Content = CrearCirculoFicha(Brushes.White);
                    btn.Click += BtnCasilla_Click;

                    matrizBotonesUI[f, c] = btn;
                    gridTablero.Children.Add(btn);
                }
            }
        }

        private Ellipse CrearCirculoFicha(Brush color)
        {
            return new Ellipse
            {
                Fill = color,
                Width = 50,
                Height = 50
            };
        }

        private void BtnCasilla_Click(object sender, RoutedEventArgs e)
        {
            if (juegoFinalizado) return;

            Button btnPulsado = (Button)sender;
            int columnaSeleccionada = (int)btnPulsado.Tag;

            ProcesarJugada(columnaSeleccionada);
        }

        private void ProcesarJugada(int columna)
        {
            Jugador jugadorActual = jugadores[indiceJugadorActual];

            int filaResultado = tablero.ColocarFicha(columna, jugadorActual.Id);

            if (filaResultado == -1)
            {
                lblEstado.Text = "¡Columna llena! Intenta en otra columna.";
                return;
            }


            matrizBotonesUI[filaResultado, columna].Content = CrearCirculoFicha(jugadorActual.ColorFicha);


            if (tablero.ComprobarVictoria(jugadorActual.Id))
            {
                lblEstado.Text = $"¡GANADOR: {jugadorActual.Nombre}!";
                lblTurno.Text = "¡FIN DEL JUEGO!";
                lblTurno.Foreground = jugadorActual.ColorFicha;
                juegoFinalizado = true;
                MessageBox.Show($"¡Felicidades {jugadorActual.Nombre}! Has ganado la partida.", "Victoria", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }


            if (tablero.EstaLleno())
            {
                lblEstado.Text = "¡Empate! El tablero está lleno.";
                juegoFinalizado = true;
                return;
            }


            indiceJugadorActual = (indiceJugadorActual + 1) % jugadores.Length;
            ActualizarEstadoTurno();


            if (jugadores[indiceJugadorActual] is JugadorIA iaPlayer && !juegoFinalizado)
            {
                int columnaIA = iaPlayer.ObtenerColumnaElegida(tablero);
                ProcesarJugada(columnaIA);
            }
        }

        private void ActualizarEstadoTurno()
        {
            Jugador actual = jugadores[indiceJugadorActual];
            lblTurno.Text = actual.Nombre;
            lblTurno.Foreground = actual.ColorFicha;
            lblEstado.Text = $"Turno de {actual.Nombre}. Haz clic en una columna.";
        }

        private void btnReiniciar_Click(object sender, RoutedEventArgs e)
        {
            InicializarJuego();
        }
    }
}
