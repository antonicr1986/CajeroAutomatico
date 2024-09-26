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

        private float CuentaSaldo;
        private long CuentaNumCuenta;
        private string CuentaUsuario;
        private int CuentaPin;
        private string CuentaIdentificacion;
        public int CuentaContador { get; set;}
        private string[] Transferencias = new string[5];

        private Retiro Retiro;

        public FormCajero(string identificacion, Retiro retiro, string[] cuentaTransferencias, int cuentaContador)
        {
            InitializeComponent();

            CuentaIdentificacion = identificacion;
            Retiro = retiro;
            Transferencias = cuentaTransferencias;
            CuentaContador = cuentaContador;

            //TODELETE
            string mensaje = "";
            for (int i = 0; i < cuentaTransferencias.Length; i++)
            {
                mensaje += cuentaTransferencias[i] + "\n";
            }
            MessageBox.Show(mensaje);
        }

        private void FormCajero_Load(object sender, EventArgs e)
        {
        }

        private void ButtonConsultaSaldo_Click(object sender, EventArgs e)
        {
            CuentaSaldo = bdDMl.ConsultaSaldo(CuentaIdentificacion);
            MessageBox.Show($"El saldo total de su cuenta es: {CuentaSaldo} €");
        }

        private void ButtonRetirarSaldo_Click(object sender, EventArgs e)
        {
            CuentaSaldo = bdDMl.ConsultaSaldo(CuentaIdentificacion);
            FormRetirar retirar = new FormRetirar(this,CuentaSaldo, CuentaIdentificacion, Retiro, Transferencias, CuentaContador);
            retirar.ShowDialog();
        }

        private void ButtonIngresarSaldo_Click(object sender, EventArgs e)
        {
            CuentaSaldo = bdDMl.ConsultaSaldo(CuentaIdentificacion);
            FormIngresar ingresar = new FormIngresar(this,CuentaSaldo, CuentaNumCuenta, CuentaUsuario, CuentaPin, CuentaIdentificacion, CuentaContador, Transferencias);
            ingresar.ShowDialog();
        }

        private void ButtonVerNumCuenta_Click(object sender, EventArgs e)
        {
            CuentaNumCuenta = bdDMl.ConsultaNumCuenta(CuentaIdentificacion);
            MessageBox.Show("El numero de cuenta es " + CuentaNumCuenta);
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
                if (!string.IsNullOrWhiteSpace(Transferencias[0]))

                {
                    MessageBox.Show($"Las ultimas transferencias son:\n{Transferencias[0]}\n{Transferencias[1]}\n" +
                    $"{Transferencias[2]}\n{Transferencias[3]}\n{Transferencias[4]}");
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
            FormCambioPIN formPIN = new FormCambioPIN(CuentaIdentificacion);
            formPIN.ShowDialog();
        }
    }
}
