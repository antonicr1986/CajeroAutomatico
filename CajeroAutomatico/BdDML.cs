using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CajeroAutomatico
{
    public class BdDML
    {
        public Conexion objetoConexion;

        private float saldo;
        private long numCuenta;

        public float ConsultaSaldo(string identificacion)
        {
            try
            {
                using (var context = new DBonlineEF()) 
                {
                    var saldo = context.Cajero_CuentaCorriente
                                       .Where(c => c.identificacion == identificacion)
                                       .Select(c => c.saldo)
                                       .FirstOrDefault(); // Obtiene el primer saldo o 0 si no hay coincidencias

                    // Verifica si se encontró un saldo y conviértelo a float
                    return saldo.HasValue ? Convert.ToSingle(saldo.Value) : 0f;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return 0f;
        }

        public long ConsultaNumCuenta (string identificacion)
        {
            try
            {
                using (var context = new DBonlineEF()) 
                {
                    var numCuenta = context.Cajero_CuentaCorriente
                                           .Where(c => c.identificacion == identificacion)
                                           .Select(c => c.numCuenta)
                                           .FirstOrDefault();

                    return numCuenta;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return 0;
        }

        public void RetirarSaldo(float cantidad, string identificacion)
        {
            try
            {
                using (var context = new DBonlineEF())
                {
                    var cuenta = context.Cajero_CuentaCorriente
                                        .FirstOrDefault(c => c.identificacion == identificacion);

                    if (cuenta != null)
                    {
                        cuenta.saldo -= cantidad;

                        context.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la cuenta con la identificación proporcionada.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public void IngresarSaldo(float cantidad, string identificacion)
        {
            try
            {
                using (var context = new DBonlineEF()) 
                {
                    // Busca la cuenta que coincida con la identificación
                    var cuenta = context.Cajero_CuentaCorriente
                                        .FirstOrDefault(c => c.identificacion == identificacion);

                    if (cuenta != null)
                    {
                        cuenta.saldo += cantidad;
                        context.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la cuenta con la identificación proporcionada.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
