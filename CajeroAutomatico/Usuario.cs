using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CajeroAutomatico
{
    public class Usuario
    {
        public string Nombre { get;private set; }
        public string Identificacion { get; private set; }
        public int PIN { get; private set; }

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
