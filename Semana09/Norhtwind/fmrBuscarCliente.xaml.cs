using Microsoft.Data.SqlClient;
using System;
using System.Configuration;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Reflection.PortableExecutable;
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

namespace Norhtwind
{
    /// <summary>
    /// Lógica de interacción para fmrBuscarCliente.xaml
    /// </summary>
    public partial class fmrBuscarCliente : Window
    {

        string cadenaConexion = @"server=(localdb)\MSSQLLocalDB; Database=Northwind; Integrated Security=true; TrustServerCertificate=true; Encrypt=true";
        public fmrBuscarCliente()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string query = "select distinct Country From Customers order by Country";

            using (SqlConnection con = new SqlConnection(cadenaConexion))
            using (SqlCommand command = new SqlCommand(query, con))
            {
                try
                {
                    con.Open();

                    using (SqlDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        cbxpais.Items.Clear();
                        while (reader.Read())
                        {
                            cbxpais.Items.Add(reader.GetString(0));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error de conexion: {ex.Message}");
                }
            }

        }

        private void cbxpais_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            if (cbxpais.SelectedItem == null)
                return;

            string pais = cbxpais.SelectedItem.ToString();
            string query = "select CustomerID, CompanyName, ContactName, Country from customers where Country = @Country";

            using (SqlConnection con = new SqlConnection(cadenaConexion))
            using (SqlCommand command = new SqlCommand(query, con))
            {
                command.Parameters.AddWithValue("@Country", pais);

                try
                {
                    con.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<Cliente> lstClientes = new List<Cliente>();
                        while (reader.Read())
                        {
                            Cliente cliente = new Cliente();
                            cliente.CustomerID = reader.GetString(0);
                            cliente.CompanyName = reader.GetString(1);
                            cliente.ContactName = reader.GetString(2);
                            cliente.Country = reader.GetString(3);

                            lstClientes.Add(cliente);
                        }

                        lvCliente.ItemsSource = lstClientes;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar los clientes: {ex.Message}");
                }
            }
        }
    }
}
