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

namespace AdivinaEdad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int edadMinima;
        private int edadMaxima;
        private int edadPropuesta;
        private int contadorIntentos; 

        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnIntento_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(lbinferior.Text, out edadMinima))
            {
                MessageBox.Show("Ingrese una edad mínima válida.",
                    "Validación", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(lbsuperior.Text, out edadMaxima))
            {
                MessageBox.Show("Ingrese una edad máxima válida.",
                    "Validación", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (edadMinima > edadMaxima)
            {
                MessageBox.Show("La edad mínima no puede ser mayor que la edad máxima.",
                    "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            edadPropuesta = random.Next(edadMinima, edadMaxima + 1);
            contadorIntentos++;

            txtedadadivinada.Text = edadPropuesta.ToString();

        }

        private void btnincorrecto_Click(object sender, RoutedEventArgs e)
        {
            if (contadorIntentos == 0)
            {
                MessageBox.Show("Primero presione el botón 'Primer Intento'.",
                    "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            edadPropuesta = random.Next(edadMinima, edadMaxima + 1);
            contadorIntentos++;

            txtedadadivinada.Text = edadPropuesta.ToString();
        }

        private void btncorrecto_Click(object sender, RoutedEventArgs e)
        {
            if (contadorIntentos == 0)
            {
                MessageBox.Show("Primero presione el botón 'Primer Intento'.",
                    "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show(
                $"¡Edad correcta! La computadora adivinó en {contadorIntentos} intento(s).",
                "Juego terminado",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

    }
}