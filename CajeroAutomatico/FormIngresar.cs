using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CajeroAutomatico
{
    public partial class FormIngresar : Form
    {
        FormCajero FormCajero;
        BdDML bdDMl = new BdDML();

        private double CuentaSaldo;
        private long NumCuenta;
        private string CuentaUsuario;
        private int CuentaPin;
        private string CuentaIdentificacion;
        private int CuentaContador;
        private string[] CuentaTransferencias;

        public FormIngresar(FormCajero formCajero, double cuentaSaldo, long cuentaNumCuenta, string cuentaUsuario, int cuentaPin, string cuentaIdentificacion, int cuentaContador, string[] cuentaTrasnferencias)
        {
            InitializeComponent();
            FormCajero = formCajero;

            CuentaSaldo = cuentaSaldo;
            NumCuenta = cuentaNumCuenta;
            CuentaUsuario = cuentaUsuario;
            CuentaPin = cuentaPin;
            CuentaIdentificacion = cuentaIdentificacion;
            CuentaContador = cuentaContador;
            CuentaTransferencias = cuentaTrasnferencias;

            //TODELETE
            string mensaje = "";
            for (int i = 0; i < cuentaTrasnferencias.Length; i++)
            {
                mensaje += cuentaTrasnferencias[i] + "\n";
            }
            MessageBox.Show(mensaje);
        }

        private void ButtonConfirmar_Click(object sender, EventArgs e)
        {
            float cantidadIngresar;
            float.TryParse(textBoxIngresar.Text,out cantidadIngresar);

            if (cantidadIngresar == 0)
            {
                MessageBox.Show($"La cantidad ingresada no puede ser de 0 €");
            }

            else if (CuentaContador < CuentaTransferencias.Length)
            {
                bdDMl.IngresarSaldo(cantidadIngresar, CuentaIdentificacion);
                CuentaSaldo = bdDMl.ConsultaSaldo(CuentaIdentificacion);
                MessageBox.Show($"La cantidad ingresada ha sido de {cantidadIngresar} € y el saldo total de la cuenta es de {CuentaSaldo} €");
                CuentaTransferencias[CuentaContador] = $"Ingreso: {cantidadIngresar} €";
                CuentaContador++;
                FormCajero.CuentaContador = CuentaContador;
            }
            else
            {
                CuentaContador = 0;
                this.ButtonConfirmar_Click(sender,e);
            }
        }

        private void TextBoxIngresar_TextChanged(object sender, EventArgs e)
        {
            textBoxIngresar.Text = textBoxIngresar.Text.Replace('.', ',');

            // Establecer la posición del cursor al final del texto
            textBoxIngresar.SelectionStart = textBoxIngresar.Text.Length;
            textBoxIngresar.SelectionLength = 0;
        }
    }
}
