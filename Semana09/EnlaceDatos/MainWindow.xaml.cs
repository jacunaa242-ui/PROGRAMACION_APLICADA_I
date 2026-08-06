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

namespace EnlaceDatos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Producto producto;

        public MainWindow()
        {
            InitializeComponent();

            producto = new Producto();
            producto.nombre = "Café";
            this.DataContext = producto;


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("EL precio es:" + producto.Precio);
        }
    }
}
