using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CajeroAutomatico
{
    public class Usuario
    {
        private string Nombre { get;set; }
        private string Identificacion { get; set; }
        private int PIN { get; set; }

        public Usuario(string nombre, string identificacion, int pin)
        {
            Nombre = nombre;
            Identificacion = identificacion;
            PIN = pin;
        }

        public bool VerificarUsuario(string identificacion, int pin)
        {
            return identificacion == Identificacion && pin == PIN;
        }
    }
}
