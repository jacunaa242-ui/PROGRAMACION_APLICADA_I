using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CentroOdontológico
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CargarPiezasDentales();
        }

        private void CargarPiezasDentales()
        {
            List<string> listaPiezas = new List<string>();
            for (int cuadrante = 1; cuadrante <= 4; cuadrante++)
            {
                for (int pieza = 1; pieza <= 8; pieza++)
                {
                    listaPiezas.Add($"Pieza {cuadrante}{pieza}");
                }
            }
            cmbPiezaDental.ItemsSource = listaPiezas;
        }

        private void btnCronogramar_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validaciones básicas de control
            if (string.IsNullOrWhiteSpace(txtPaciente.Text))
            {
                MessageBox.Show("Por favor, escriba el nombre del paciente.", "Faltan datos", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPaciente.Focus();
                return;
            }

            if (cmbTratamiento.SelectedItem == null)
            {
                MessageBox.Show("Por favor, seleccione un tratamiento clínico.", "Faltan datos", MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbTratamiento.Focus();
                return;
            }

            if (cmbPiezaDental.SelectedItem == null)
            {
                MessageBox.Show("Por favor, elija la pieza dental a tratar.", "Faltan datos", MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbPiezaDental.Focus();
                return;
            }

            if (!calFecha.SelectedDate.HasValue)
            {
                MessageBox.Show("Por favor, elija una fecha en el calendario.", "Faltan datos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. Captura y procesamiento de datos
            string paciente = txtPaciente.Text.Trim();
            string tratamiento = ((ComboBoxItem)cmbTratamiento.SelectedItem).Content.ToString();
            string piezaDental = cmbPiezaDental.SelectedItem.ToString();

            // Fechas de la cita actual
            DateTime fechaCita = calFecha.SelectedDate.Value;
            string fechaCorta = fechaCita.ToString("dd/MM/yyyy");
            string fechaLarga = fechaCita.ToString("dddd, d 'de' MMMM 'de' yyyy");

            // 3. CÁLCULO DE LA PRÓXIMA CITA (Automáticamente en 14 días)
            DateTime proximaCita = fechaCita.AddDays(14);
            string fechaProximaCorta = proximaCita.ToString("dd/MM/yyyy");
            string fechaProximaLarga = proximaCita.ToString("dddd, d 'de' MMMM 'de' yyyy");

            // 4. Construcción de la respuesta organizada por renglones utilizando saltos de línea (\n)
            string registroTarjeta = $"Paciente: {paciente}  |  {piezaDental}  |   Tratamiento: {tratamiento}\n" +
                                     $"Cita Actual: {fechaCorta} — ({fechaLarga})\n" +
                                     $"Próxima Cita: {fechaProximaCorta} — ({fechaProximaLarga})";

            // 5. Agregar el elemento estructurado al ListBox
            lstResultados.Items.Add(registroTarjeta);

            // 6. Limpieza para la siguiente captura
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            txtPaciente.Clear();
            cmbTratamiento.SelectedIndex = -1;
            cmbPiezaDental.SelectedIndex = -1;
            calFecha.SelectedDate = null;
        }
    }
}