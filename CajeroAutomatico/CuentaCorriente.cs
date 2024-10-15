using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CajeroAutomatico
{
    public class CuentaCorriente
    {
        private double saldo;
        private long numCuenta;
        private string identificacion;
        private int pin;
        private String[] transferencias;
        private int contador; //?Borrar??

        public CuentaCorriente(double saldo, long numCuenta,string identificacion, int pin)
        {
            this.saldo = saldo;
            this.numCuenta = numCuenta;
            this.identificacion = identificacion;
            this.pin = pin;
            transferencias = new string[5];
        }

        public CuentaCorriente()
        {
        }

        public double ConsultarSaldo()
        {
            return saldo;
        }

        public long ConsultarNumCuenta()
        {
            return numCuenta;
        }
    }
}
