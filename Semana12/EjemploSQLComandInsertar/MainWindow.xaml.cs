using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

namespace EjemploSQLComandInsertar
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            string cn = ConfigurationManager.ConnectionStrings["EjemploSQLComandInsertar.Properties.Settings.Northwind"];
            using (SqlConnection conex =new SqlConnection(cn))
            {
                string query = "INSERT INTO ";
                SqlCommand cmd = new SqlCommand(query, conex);
                cmd.Parameters.Add("@Id",SqlDbType.NChar, 5).Value = txtIdcliente.Text;
                cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 40).Value=TXTnOMBRE.Text;

                conex.Open();
                int filasafectadas = cmd.ExecuteNonQuery();


            }

        }
    }
}
