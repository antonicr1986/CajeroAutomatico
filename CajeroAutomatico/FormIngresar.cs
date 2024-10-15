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

        private double cuentaSaldo;
        private long numCuenta;
        private string cuentaUsuario;
        private int cuentaPin;
        private string cuentaIdentificacion;
        private int cuentaContador;
        private string[] cuentaTransferencias;

        public FormIngresar(FormCajero formCajero, double cuentaSaldo, long cuentaNumCuenta, string cuentaUsuario, int cuentaPin, string cuentaIdentificacion, int cuentaContador, string[] cuentaTrasnferencias)
        {
            InitializeComponent();
            FormCajero = formCajero;

            this.cuentaSaldo = cuentaSaldo;
            numCuenta = cuentaNumCuenta;
            this.cuentaUsuario = cuentaUsuario;
            this.cuentaPin = cuentaPin;
            this.cuentaIdentificacion = cuentaIdentificacion;
            this.cuentaContador = cuentaContador;
            cuentaTransferencias = cuentaTrasnferencias;

        }

        private void ButtonConfirmar_Click(object sender, EventArgs e)
        {
            float cantidadIngresar;
            float.TryParse(textBoxIngresar.Text,out cantidadIngresar);

            if (cantidadIngresar == 0)
            {
                MessageBox.Show($"La cantidad ingresada no puede ser de 0 €");
            }

            else if (cuentaContador < cuentaTransferencias.Length)
            {
                bdDMl.IngresarSaldo(cantidadIngresar, cuentaIdentificacion);
                cuentaSaldo = bdDMl.ConsultaSaldo(cuentaIdentificacion);
                MessageBox.Show($"La cantidad ingresada ha sido de {cantidadIngresar} € y el saldo total de la cuenta es de {cuentaSaldo} €");
                cuentaTransferencias[cuentaContador] = $"Ingreso: {cantidadIngresar} €";
                cuentaContador++;
                FormCajero.CuentaContador = cuentaContador;
            }
            else
            {
                cuentaContador = 0;
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
