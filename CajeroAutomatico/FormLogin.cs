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
        public Usuario Usuario {get;set;}
        private string[] CuentaTransferencias = new string[5];
        private int CuentaContador = 0;


        public Retiro Retiro { get; set; }

        Conexion objetoConexion = new Conexion();

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
                if (string.IsNullOrEmpty(textBoxIdentificacion.Text) || string.IsNullOrEmpty(textBoxPIN.Text))
                {
                    MessageBox.Show("Rellena el numero de tarjeta y el PIN");
                    return;
                }

                string numeroIdentificacionIngresado = textBoxIdentificacion.Text;
                int pinIngresado;
                if (!int.TryParse(textBoxPIN.Text, out pinIngresado))
                {
                    MessageBox.Show("El PIN debe ser un número válido");
                    return;
                }

                // *** A2: Verificar usuario en la base de datos usando Entity Framework
                using (var context = new DBonlineEF()) 
                {
                    // Verificar si existe el usuario con la identificación y PIN ingresados
                    var cuentaCliente = context.Cajero_CuentasClientes
                                               .FirstOrDefault(c => c.Identificacion == numeroIdentificacionIngresado && c.Pin == pinIngresado);

                    if (cuentaCliente != null)
                    {
                        // Usuario y PIN son correctos
                        this.Hide();

                        // Cargar la cuenta corriente usando el PIN
                        CuentaCorriente cuenta = new CuentaCorriente();
                        ComprobarCuentaUsuarioBD(pinIngresado); 

                        /* FormCajero cajero1 = new FormCajero(Usuario, cuenta, Retiro); */
                        FormCajero cajero1 = new FormCajero(numeroIdentificacionIngresado, Retiro, CuentaTransferencias, CuentaContador);
                        cajero1.Show();

                        MessageBox.Show("Comprobando usuario" +
                            "\nIdentificación usuario = " + cuenta.Identificacion +
                            "\nCuenta = " + cuenta +
                            "\nRetiro = " + Retiro);
                    }
                    else
                    {
                        // Usuario o PIN incorrectos
                        MessageBox.Show("El número de identificación o el PIN son incorrectos");
                    }
                }
            }
            catch(FormatException ex)
            {
                MessageBox.Show("El numero de tarjeta o el PIN tienen un formato incorrecto: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se produjo un error: " + ex.Message);
            }
        }

        public void ComprobarCuentaUsuarioBD(int pin)
        {
            try
            {
                using (var context = new DBonlineEF())
                { 
                    // Buscar la cuenta que coincide con el Pin
                    var cuentaCorriente = context.Cajero_CuentaCorriente
                                                 .FirstOrDefault(c => c.pin == pin);

                    if (cuentaCorriente != null)
                    {
                        // Aquí ya tienes los datos en la propiedad de cuentaCorriente
                        MessageBox.Show("Coprobando cuentacorriente" +
                            "\nSaldo: " + cuentaCorriente.saldo +
                            "\nIdentificación usuario: " + cuentaCorriente.identificacion +
                            "\nPIN: " + cuentaCorriente.pin +
                            "\nNumCuenta: " + cuentaCorriente.numCuenta);
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la cuenta.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
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
