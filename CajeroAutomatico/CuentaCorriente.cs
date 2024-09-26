using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CajeroAutomatico
{
    public class CuentaCorriente
    {
        private double Saldo { get; set; }
        private long NumCuenta { get; set; }
        private string Identificacion { get; set; }
        private int PIN { get; set; }
        private String[] Transferencias { get; set; }
        private int Contador { get; set; }

        public CuentaCorriente(double saldo, long numCuenta,string identificacion, int pin)
        {
            Saldo = saldo;
            NumCuenta = numCuenta;
            Identificacion = identificacion;
            PIN = pin;
            Transferencias = new string[5];
        }

        public CuentaCorriente()
        {
        }

        public double ConsultarSaldo()
        {
            return Saldo;
        }

        public long ConsultarNumCuenta()
        {
            return NumCuenta;
        }
    }
}
