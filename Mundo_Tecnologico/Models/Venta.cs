using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mundo_Tecnologico.Models
{
    public class Venta
    {
        public string codigo { get; set; }
        public DateTime fecha { get; set; }
        public string direccion { get; set; }
        public string referencia { get; set; }
        public byte estado { get; set; }
        public decimal total { get; set; }
        public string codigoUsuario { get; set; }
        public string codigoDistrito { get; set; }

        // Llaves foráneas
        public string usuario { get; set; }
        public string distrito { get; set; }
        public string celular { get; set; }
        public string documento { get; set; }
    }
}