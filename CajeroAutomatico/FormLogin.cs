using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CajeroAutomatico
{
    public partial class FormLogin : Form
    {
        private const string MensajeErrorRellenaNumTarjYPin = "Rellena el número de tarjeta y el PIN";
        private const string MensajeErrorPin = "El PIN debe ser un número válido";
        private const string MensajeUsuarioIncorrecto = "El número de identificación o el PIN son incorrectos";
        private const string MensajeErrorFormato = "El número de tarjeta o el PIN tienen un formato incorrecto: ";
        private const string MensajeErrorGeneral = "Se produjo un error: ";
        private const string MensajeErrorRellenaTarjetaYPin = "Rellena el número de tarjeta y el PIN";
        private const string MensajeErrorPinFormatoNoValido = "El PIN debe ser un número válido";

        private Usuario Usuario {get;set;}
        private string[] UltimasTransferencias = new string[5];
        private int CuentaContador = 0;

        private Retiro Retiro { get; set; }

        public FormLogin()
        {
            InitializeComponent();
            this.Retiro = new Retiro();
        }

        public FormLogin(Usuario usuario)
        {
            InitializeComponent();
            this.Retiro = new Retiro();
            this.Usuario = usuario;
        }

        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }

        private void ButtonComprobar_Click(object sender, EventArgs e)
        {
            try
            {
                string numeroIdentificacionIngresado = textBoxIdentificacion.Text;
                int pinIngresado;

                IsEntradaValida(out pinIngresado);

                BdDML.VerificarUsuario(numeroIdentificacionIngresado, pinIngresado, this, Retiro, UltimasTransferencias, CuentaContador);
            }
            catch (FormatException ex)
            {
                MessageBox.Show(MensajeErrorFormato + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(MensajeErrorGeneral + ex.Message);
            }
        }

        private bool IsEntradaValida(out int pinIngresado)
        {
            pinIngresado = 0;

            if (string.IsNullOrEmpty(textBoxIdentificacion.Text) || string.IsNullOrEmpty(textBoxPIN.Text))
            {
                MessageBox.Show(MensajeErrorRellenaTarjetaYPin);
                return false;
            }

            if (!int.TryParse(textBoxPIN.Text, out pinIngresado))
            {
                MessageBox.Show(MensajeErrorPinFormatoNoValido);
                return false;
            }

            return true;
        }


        private void TextBoxPIN_KeyPress(object sender, KeyPressEventArgs e)
        {
        if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }
    }
}
