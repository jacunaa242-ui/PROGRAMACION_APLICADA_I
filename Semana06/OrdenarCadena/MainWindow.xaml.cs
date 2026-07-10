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

namespace OrdenarCadena
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string[] ListaNombres = Array.Empty<string>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnListar_Click(object sender, RoutedEventArgs e)
        {
            string nombres = txtCadena.Text;

            if (string.IsNullOrWhiteSpace(nombres))
            {
                MessageBox.Show("Ingrese la cadena de nombres",
                                "Validación",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                return;
            }

            lstNombres.Items.Clear();
            lstResultado.Items.Clear();

            ListaNombres = nombres.Split(new char[] { ' ' },
                                         StringSplitOptions.RemoveEmptyEntries);

            foreach (string nombre in ListaNombres)
            {
                lstNombres.Items.Add(nombre);
            }

            txtCantidad1.Text = ListaNombres.Length.ToString();
            txtCantidad2.Text = "0";

        }

        private void btnPasar_Click(object sender, RoutedEventArgs e)
        {
            lstResultado.Items.Clear();

            if (string.IsNullOrWhiteSpace(txtLetra.Text))
            {
                MessageBox.Show("Ingrese una letra",
                                "Validación",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                txtLetra.Focus();
                return;
            }

            char letra = char.ToUpper(txtLetra.Text[0]);

            int contador = 0;

            foreach (string nombre in ListaNombres)
            {
                if (char.ToUpper(nombre[0]) == letra)
                {
                    lstResultado.Items.Add(nombre);
                    contador++;
                }
            }

            txtCantidad2.Text = contador.ToString();
        }
    }
}