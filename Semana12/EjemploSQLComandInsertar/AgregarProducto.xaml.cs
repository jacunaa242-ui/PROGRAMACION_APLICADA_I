using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace EjemploSQLComandInsertar
{
    /// <summary>
    /// Lógica de interacción para AgregarProducto.xaml
    /// </summary>
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await CargarProductos();
    }


    private async System.Threading.Tasks.Task CargarProductos()
    {
        string cadena = ConfigurationManager.ConnectionStrings["EjemploSQLCommandInsertar.Properties.Settings.Northwind"].ConnectionString;

        try
        {
            using (SqlConnection conn = new SqlConnection(cadena))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SP_ListarProductos";
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        DataTable tabla = new DataTable();
                        tabla.Load(reader);
                        dgProductos.ItemsSource = tabla.DefaultView;
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            MessageBox.Show($"Error SQL {ex.Number}, {ex.Message}", "Error SQL", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }


    private async void btnRegistrar_Click(
        object sender,
        RoutedEventArgs e)
    {
        btnRegistrar.IsEnabled = false;

        string cadena = ConfigurationManager.ConnectionStrings["EjemploSQLCommandInsertar.Properties.Settings.Northwind"].ConnectionString;

        try
        {
            using (SqlConnection conn =
                new SqlConnection(cadena))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SP_AgregarProducto";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 40).Value = txtNombre.Text;
                    cmd.Parameters.Add("@Precio", SqlDbType.Money).Value = decimal.Parse(txtPrecio.Text);
                    cmd.CommandTimeout = 60;

                    await cmd.ExecuteNonQueryAsync();

                    MessageBox.Show(
                        "Producto agregado correctamente.",
                        "Registro",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );

                    Limpiar();

                    await CargarProductos();
                }
            }
        }
        catch (FormatException)
        {
            MessageBox.Show(
                "El precio debe ser un número válido.",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Warning
            );
        }
        catch (SqlException ex)
        {
            MessageBox.Show(
                $"Error SQL {ex.Number}, {ex.Message}",
                "Error SQL",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error: {ex.Message}",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }
        finally
        {
            btnRegistrar.IsEnabled = true;
        }
    }


    private async void btnAgregarCategoria_Click(
        object sender,
        RoutedEventArgs e)
    {
        btnAgregarCategoria.IsEnabled = false;

        string nombreCategoria =
            txtCategoria.Text.Trim();


        if (string.IsNullOrWhiteSpace(nombreCategoria))
        {
            MessageBox.Show(
                "Ingrese el nombre de la categoría.",
                "Validación",
                MessageBoxButton.OK,
                MessageBoxImage.Warning
            );

            btnAgregarCategoria.IsEnabled = true;
            txtCategoria.Focus();

            return;
        }


        if (nombreCategoria.Length > 15)
        {
            MessageBox.Show(
                "El nombre de la categoría no puede tener más de 15 caracteres.",
                "Validación",
                MessageBoxButton.OK,
                MessageBoxImage.Warning
            );

            btnAgregarCategoria.IsEnabled = true;
            txtCategoria.Focus();

            return;
        }


        string cadena = ConfigurationManager
            .ConnectionStrings[
                "EjemploSQLCommandInsertar.Properties.Settings.Northwind"
            ]
            .ConnectionString;


        try
        {
            using (SqlConnection conn =
                new SqlConnection(cadena))
            {
                await conn.OpenAsync();


                using (SqlCommand cmd =
                    new SqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText =
                        "SP_AgregarCategoria";

                    cmd.CommandType =
                        CommandType.StoredProcedure;


                    cmd.Parameters.Add(
                        "@NombreCategoria",
                        SqlDbType.NVarChar,
                        15
                    ).Value = nombreCategoria;


                    using (SqlDataReader reader =
                        await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            int categoryID =
                                Convert.ToInt32(
                                    reader["CategoryID"]
                                );


                            bool nuevaCategoria =
                                Convert.ToBoolean(
                                    reader["NuevaCategoria"]
                                );


                            if (nuevaCategoria)
                            {
                                MessageBox.Show(
                                    $"La categoría \"{nombreCategoria}\" " +
                                    $"se agregó correctamente.\n\n" +
                                    $"ID de categoría: {categoryID}",
                                    "Categoría agregada",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information
                                );
                            }
                            else
                            {
                                MessageBox.Show(
                                    $"La categoría \"{nombreCategoria}\" " +
                                    $"ya existe.\n\n" +
                                    $"ID de categoría: {categoryID}",
                                    "Categoría existente",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information
                                );
                            }
                        }
                    }
                }
            }


            txtCategoria.Clear();
            txtCategoria.Focus();
        }
        catch (SqlException ex)
        {
            MessageBox.Show(
                $"Error SQL {ex.Number}, {ex.Message}",
                "Error SQL",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error: {ex.Message}",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }
        finally
        {
            btnAgregarCategoria.IsEnabled = true;
        }
    }


    private void Limpiar()
    {
        txtNombre.Clear();
        txtPrecio.Clear();
        txtCategoria.Clear();

        txtNombre.Focus();
    }


    private void btnNuevo_Click(
        object sender,
        RoutedEventArgs e)
    {
        Limpiar();
    }
}
}
