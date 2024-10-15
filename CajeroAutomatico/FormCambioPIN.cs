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
    public partial class FormCambioPIN : Form
    { 
        public string identificacionUsuario;

        BdDML bdDMl = new BdDML();

        public FormCambioPIN(string identificacion)
        {
            InitializeComponent();
            identificacionUsuario = identificacion;
        }

        private void ButtonConfirmarNuevoPIN_Click(object sender, EventArgs e)
        {
            string nuevoPin;

            if (string.IsNullOrWhiteSpace(textBoxNuevoPIN.Text) || (string.IsNullOrWhiteSpace(textBoxNuevoPINbis.Text)))
            {
                MessageBox.Show("Rellena las dos casillas con el nuevo PIN");
                return;
            }
                if (textBoxNuevoPIN.Text != textBoxNuevoPINbis.Text)
            {
                MessageBox.Show("Los PIN introducidos han de coincidir");
                return;
            }

            nuevoPin = textBoxNuevoPIN.Text;

            bdDMl.CambioPin(nuevoPin, identificacionUsuario);
           
        }

        private void CheckBoxVerPIN_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxVerPIN.Checked)
            {
                textBoxNuevoPIN.PasswordChar = '\0';
                textBoxNuevoPINbis.PasswordChar = '\0';
            }
            else
            {
                textBoxNuevoPIN.PasswordChar = '*';
                textBoxNuevoPINbis.PasswordChar = '*';
            }
        }

        private void FormCambioPIN_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }

        private void TextBoxNuevoPIN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void TextBoxNuevoPINbis_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }
    }
}
