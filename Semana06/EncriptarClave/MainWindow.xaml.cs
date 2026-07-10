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

namespace EncriptarClave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnEncriptar_Click(object sender, RoutedEventArgs e)
        {
            string clave = txtClave.Text;

            if (string.IsNullOrWhiteSpace(clave))
            {
                MessageBox.Show("Ingrese una clave.");
                return;
            }

            StringBuilder encriptada = new StringBuilder();

            foreach (char c in clave)
            {
                encriptada.Append((char)(c + 3));
            }

            txtResultado.Text = encriptada.ToString();
        }

        private void btnDesencriptar_Click(object sender, RoutedEventArgs e)
        {
            string clave = txtResultado.Text;

            if (string.IsNullOrWhiteSpace(clave))
            {
                MessageBox.Show("No existe una clave encriptada.");
                return;
            }

            StringBuilder desencriptada = new StringBuilder();

            foreach (char c in clave)
            {
                desencriptada.Append((char)(c - 3));
            }

            txtResultado.Text = desencriptada.ToString();
        }
    }
}