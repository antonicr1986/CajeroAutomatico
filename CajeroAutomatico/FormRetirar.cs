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
    public partial class FormRetirar : Form
    {
        BdDML bdDMl = new BdDML();

        private double cuentaSaldo;
        private long numCuenta;
        private string cuentaUsuario;
        private int cuentaPin;
        private string cuentaIdentificacion;
        private int cuentaContador;
        private string[] cuentaTransferencias;

        private FormCajero formCajero;
        private Retiro retiro;


        //CONSTANTES
        private readonly int maxRetirar = 1000;
        private readonly int numMaxRetiros = 10;
        private readonly int cantidadMaxRetiradaHoy = 3000;

        public FormRetirar(FormCajero formCajero, double cuentaSaldo, string cuentaIdentificacion,Retiro retiro, string [] cuentaTransferencias, int cuentaContador)
        {
            InitializeComponent();
            this.formCajero = formCajero;

            this.cuentaSaldo = cuentaSaldo;
            this.cuentaIdentificacion = cuentaIdentificacion;
            this.retiro = retiro;
            this.cuentaTransferencias = cuentaTransferencias;
            this.cuentaContador = cuentaContador;
            
        }

        private void ButtonConfirmarRetiro_Click(object sender, EventArgs e)
        {
            float cantidadRetirar;
            float.TryParse(textBoxRetirar.Text, out cantidadRetirar);

            cuentaSaldo = bdDMl.ConsultaSaldo(cuentaIdentificacion);
            
            if (cantidadRetirar == 0)
            {
                MessageBox.Show($"Indique un valor mayor que 0 para retirar saldo de la cuenta." +
                    "\n\nTOTAL CUENTA: " + cuentaSaldo);
                return;
            }

            if (cantidadRetirar > bdDMl.ConsultaSaldo(cuentaIdentificacion))
            {
                MessageBox.Show($"La cantidad no se puede retirar porque es mas grande que el total del saldo de la cuenta." +
                    "\n\nTOTAL CUENTA: " + cuentaSaldo);
                return;
            }
            if (cantidadRetirar > maxRetirar)
            {
                MessageBox.Show($"La cantidad ha retirar no puede ser mas grande de: {maxRetirar} €");
                return;
            }

            //Para controlar que los retiros de hoy no superen los 3000 €
            if (DateTime.Now.Date != retiro.Fecha.Date)
            {
                retiro.RetirosHoyEuros = 0;
            }

            if (retiro.RetirosHoyEuros >= cantidadMaxRetiradaHoy)
            {
                MessageBox.Show($"Has superado el importe maximo de retiros de hoy: {cantidadMaxRetiradaHoy}");
                return;
            }

            //Para controlar que los retiros de hoy se pongan a 0 cuando la fecha sea un nuevo dia
            if (DateTime.Now.Date != retiro.Fecha.Date)
            {
                retiro.RetirosHoyNum = 0;
            }

            if (retiro.RetirosHoyNum >= numMaxRetiros)
            {
                MessageBox.Show($"Has superado el maximo de retiros de hoy: {numMaxRetiros}");
                return;
            }
                
            if (cuentaContador < cuentaTransferencias.Length) //TODO
            {
                bdDMl.RetirarSaldo(cantidadRetirar, cuentaIdentificacion);
                retiro.RetirosHoyNum++;
                retiro.RetirosHoyEuros += cantidadRetirar;
                cuentaSaldo = bdDMl.ConsultaSaldo(cuentaIdentificacion);
                MessageBox.Show($"La cantidad retirada ha sido de {cantidadRetirar} € y el saldo total de la cuenta es de { cuentaSaldo } €");
                cuentaTransferencias[cuentaContador] = $"Retiro: {cantidadRetirar} €";
                cuentaContador++;
                formCajero.CuentaContador = cuentaContador;
            }
            else
            {
                cuentaContador = 0;
                this.ButtonConfirmarRetiro_Click(sender, e);
            }    
        }

        private void TextBoxRetirar_TextChanged(object sender, EventArgs e)
        {
            textBoxRetirar.Text = textBoxRetirar.Text.Replace('.', ',');

            // Establecer la posición del cursor al final del texto
            textBoxRetirar.SelectionStart = textBoxRetirar.Text.Length;
            textBoxRetirar.SelectionLength = 0;
        }
    }
}
