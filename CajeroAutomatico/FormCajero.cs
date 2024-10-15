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
    public partial class FormCajero : Form
    {
        private const string MensajeErrorGeneral = "Error: ";
        private const string MensajeErrorSinTransferencias = "No hay ninguna transferencia registrada en esta cuenta";

        BdDML bdDMl = new BdDML();

        private float cuentaSaldo;
        private long cuentaNumCuenta;
        private string cuentaUsuario;
        private int cuentaPin;
        private string cuentaIdentificacion;
        public int CuentaContador { get; set;}
        private string[] transferencias = new string[5];

        private Retiro Retiro;

        public FormCajero(string identificacion, Retiro retiro, string[] cuentaTransferencias, int cuentaContador)
        {
            InitializeComponent();

            cuentaIdentificacion = identificacion;
            Retiro = retiro;
            transferencias = cuentaTransferencias;
            CuentaContador = cuentaContador;
        }

        private void FormCajero_Load(object sender, EventArgs e)
        {
        }

        private void ButtonConsultaSaldo_Click(object sender, EventArgs e)
        {
            cuentaSaldo = bdDMl.ConsultaSaldo(cuentaIdentificacion);
            MessageBox.Show($"El saldo total de su cuenta es: {cuentaSaldo} €");
        }

        private void ButtonRetirarSaldo_Click(object sender, EventArgs e)
        {
            cuentaSaldo = bdDMl.ConsultaSaldo(cuentaIdentificacion);
            FormRetirar retirar = new FormRetirar(this,cuentaSaldo, cuentaIdentificacion, Retiro, transferencias, CuentaContador);
            retirar.ShowDialog();
        }

        private void ButtonIngresarSaldo_Click(object sender, EventArgs e)
        {
            cuentaSaldo = bdDMl.ConsultaSaldo(cuentaIdentificacion);
            FormIngresar ingresar = new FormIngresar(this,cuentaSaldo, cuentaNumCuenta, cuentaUsuario, cuentaPin, cuentaIdentificacion, CuentaContador, transferencias);;
            ingresar.ShowDialog();
        }

        private void ButtonVerNumCuenta_Click(object sender, EventArgs e)
        {
            cuentaNumCuenta = bdDMl.ConsultaNumCuenta(cuentaIdentificacion);
            MessageBox.Show("El numero de cuenta es " + cuentaNumCuenta);
        }

        private void FormCajero_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }

        private void ButtonTransferencias_Click(object sender, EventArgs e)
        {//TODO Comprobar porque no me carga bien las 5 transferencias
            try
            {
                if (!string.IsNullOrWhiteSpace(transferencias[0]))

                {
                    MessageBox.Show($"Las ultimas transferencias son:\n{transferencias[0]}\n{transferencias[1]}\n" +
                    $"{transferencias[2]}\n{transferencias[3]}\n{transferencias[4]}");
                }
                else
                    MessageBox.Show(MensajeErrorSinTransferencias);
            }
            catch (Exception ex)
            {
                MessageBox.Show(MensajeErrorGeneral + ex);
            }
        }

        private void ButtonCerrarSesion_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormLogin formLogin = new FormLogin();
            formLogin.ShowDialog();
        }

        private void buttonCambiarPIN_Click(object sender, EventArgs e)
        {
            FormCambioPIN formPIN = new FormCambioPIN(cuentaIdentificacion);
            formPIN.ShowDialog();
        }
    }
}
