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

            // Determinar configuración desde la UI
            bool vsIA = cmbModoJuego.SelectedIndex == 0;
            NivelDificultad dif = (NivelDificultad)cmbDificultad.SelectedIndex;

            // Mostrar u ocultar el combo de dificultad si es PvP
            if (lblDificultad != null && cmbDificultad != null)
            {
                lblDificultad.Visibility = vsIA ? Visibility.Visible : Visibility.Collapsed;
                cmbDificultad.Visibility = vsIA ? Visibility.Visible : Visibility.Collapsed;
            }

            // Instanciamos los jugadores según el modo
            jugadores = new Jugador[2];
            jugadores[0] = new JugadorHumano(1, "Jugador 1 (Rojo)", Brushes.Red);

            if (vsIA)
            {
                jugadores[1] = new JugadorIA(2, "Computadora (Amarillo)", Brushes.Gold, dif);
            }
            else
            {
                jugadores[1] = new JugadorHumano(2, "Jugador 2 (Amarillo)", Brushes.Gold);
            }

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
                Width = 48,
                Height = 48
            };
        }

        private async void BtnCasilla_Click(object sender, RoutedEventArgs e)
        {
            if (juegoFinalizado || jugadores[indiceJugadorActual].EsIA) return;

            Button btnPulsado = (Button)sender;
            int columnaSeleccionada = (int)btnPulsado.Tag;

            await  ProcesarJugada(columnaSeleccionada);
        }

        private async Task ProcesarJugada(int columna)
        {
            Jugador jugadorActual = jugadores[indiceJugadorActual];


            // 1. Validar y colocar ficha en el modelo de datos
            int filaResultado = tablero.ColocarFicha(columna, jugadorActual.Id);

            if (filaResultado == -1)
            {
                lblEstado.Text = "¡Columna llena! Selecciona otra.";
                return;
            }


            // 2. Reflejar en la UI
            matrizBotonesUI[filaResultado, columna].Content = CrearCirculoFicha(jugadorActual.ColorFicha);

            // 3. Evaluar condición de victoria
            if (tablero.ComprobarVictoria(jugadorActual.Id))
            {
                lblEstado.Text = $"¡GANADOR: {jugadorActual.Nombre}!";
                lblTurno.Text = "¡FIN DEL JUEGO!";
                lblTurno.Foreground = jugadorActual.ColorFicha;
                juegoFinalizado = true;
                MessageBox.Show($"¡Felicidades {jugadorActual.Nombre}! Has ganado.", "Victoria", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // 4. Evaluar empate
            if (tablero.EstaLleno())
            {
                lblEstado.Text = "¡Empate! Tablero lleno.";
                juegoFinalizado = true;
                return;
            }

            // 5. Siguiente turno
            indiceJugadorActual = (indiceJugadorActual + 1) % jugadores.Length;
            ActualizarEstadoTurno();

            // 6. Si le toca a la IA, procesar automáticamente tras un pequeño delay táctico
            if (jugadores[indiceJugadorActual] is JugadorIA iaPlayer && !juegoFinalizado)
            {
                lblEstado.Text = "La computadora está pensando...";
                await Task.Delay(400); // Pausa visual para mejor experiencia

                int idOponente = jugadores[(indiceJugadorActual + 1) % 2].Id;
                int columnaIA = iaPlayer.ObtenerColumnaElegida(tablero, idOponente);

                await ProcesarJugada(columnaIA);
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

        private void Opciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                InicializarJuego();
            }
        }

    }
}
