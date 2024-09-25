using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CajeroAutomatico
{
    public class BdDML
    {
        public static void ComprobarCuentaUsuarioBD(int pin)
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

        public void ConsultarCuentaCorriente(string CuentaIdentificacion)
        {
            try
            {
                using (var context = new DBonlineEF())
                {
                    var cuenta = context.Cajero_CuentaCorriente
                                        .Where(c => c.identificacion == CuentaIdentificacion)
                                        .Select(c => new
                                        {
                                            c.saldo,
                                            c.numCuenta,
                                            c.usuario,
                                            c.pin
                                        })
                                        .FirstOrDefault();

                    if (cuenta != null)
                    {
                        double saldo = (double)cuenta.saldo;
                        long numCuenta = cuenta.numCuenta;
                        string usuario = cuenta.usuario;
                        int pin = cuenta.pin;
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

        public void CambioPin(string nuevoPin, string IdentificacionUsuario)
        {
            try
            {
                using (var context = new DBonlineEF())
                {
                    // Actualiza el PIN en Cajero_CuentasClientes
                    var cuentaCliente = context.Cajero_CuentasClientes
                                               .FirstOrDefault(c => c.Identificacion == IdentificacionUsuario);
                    if (cuentaCliente != null)
                    {
                        cuentaCliente.Pin = int.Parse(nuevoPin);
                        context.SaveChanges();
                        MessageBox.Show("PIN actualizado en Cajero_CuentasClientes.");
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la cuenta del cliente.");
                    }

                    // Actualiza el PIN en Cajero_CuentaCorriente
                    var cuentaCorriente = context.Cajero_CuentaCorriente
                                                 .FirstOrDefault(c => c.identificacion == IdentificacionUsuario);
                    if (cuentaCorriente != null)
                    {
                        cuentaCorriente.pin = int.Parse(nuevoPin);
                        context.SaveChanges();
                        MessageBox.Show("PIN actualizado en Cajero_CuentaCorriente.");
                        MessageBox.Show("El nuevo PIN es: " + nuevoPin);
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la cuenta corriente.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar la base de datos: " + ex.Message);
            }
        }      
    }
}
