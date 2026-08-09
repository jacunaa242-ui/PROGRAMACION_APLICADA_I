using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Votantes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TextBox[,] matrizCajas;
        private TextBlock[] totalesPartidos;
        private TextBlock[] totalesZonas;

        private string[] nombresPartidos = { "Buhito", "Aguila", "Torito", "Lorito" };
        private string[] nombresZonas = { "A", "B", "C", "D" };


        public MainWindow()
        {
            InitializeComponent();
            btnCalcular.Click += btnCalcular_Click;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            matrizCajas = new TextBox[4, 4] {
                { txt00, txt01, txt02, txt03 },
                { txt10, txt11, txt12, txt13 },
                { txt20, txt21, txt22, txt23 },
                { txt30, txt31, txt32, txt33 }
            };

            totalesPartidos = new TextBlock[] { tbTotalP0, tbTotalP1, tbTotalP2, tbTotalP3 };
            totalesZonas = new TextBlock[] { tbTotalZ0, tbTotalZ1, tbTotalZ2, tbTotalZ3 };

            Random rnd = new Random();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    matrizCajas[i, j].Text = rnd.Next(100, 500).ToString();
                }
            }
        }

        private void btnCalcular_Click(object sender, RoutedEventArgs e)
        {
            int[] sumaPorPartido = new int[4];
            int[] sumaPorZona = new int[4];
            int totalVotantesGeneral = 0;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {

                    if (int.TryParse(matrizCajas[i, j].Text, out int votos))
                    {
                        sumaPorPartido[i] += votos; 
                        sumaPorZona[j] += votos;
                        totalVotantesGeneral += votos;
                    }
                    else
                    {
                        MessageBox.Show($"Por favor, ingrese un número válido para el partido {nombresPartidos[i]} en la zona {nombresZonas[j]}.");
                        matrizCajas[i, j].Focus();
                        return;
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                totalesPartidos[i].Text = sumaPorPartido[i].ToString();
                totalesZonas[i].Text = sumaPorZona[i].ToString();
            }
            tbTotalVotantes.Text = totalVotantesGeneral.ToString();

            int maxVotosPartido = -1;
            int indiceGanador = 0;

            for (int i = 0; i < 4; i++)
            {
                if (sumaPorPartido[i] > maxVotosPartido)
                {
                    maxVotosPartido = sumaPorPartido[i];
                    indiceGanador = i;
                }
            }
            lblCandidatoGanador.Text = nombresPartidos[indiceGanador];

            int maxVotosZona = -1;
            int indiceZonaMax = 0;

            for (int j = 0; j < 4; j++)
            {
                if (sumaPorZona[j] > maxVotosZona)
                {
                    maxVotosZona = sumaPorZona[j];
                    indiceZonaMax = j;
                }
            }
            lblZonaMax.Text = nombresZonas[indiceZonaMax];
        }

        private void btnCalcular_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
