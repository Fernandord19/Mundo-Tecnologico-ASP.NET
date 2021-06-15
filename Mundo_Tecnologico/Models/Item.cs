using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mundo_Tecnologico.Models
{
    public class Item
    {

        public string codigo { get; set; }
        public string nombre { get; set; }
        public decimal precio { get; set; }
        public int stock { get; set; }
        public int cantidad { get; set; } // cantidad solicitada
        public decimal monto { get { return precio * cantidad; }} 

    }
}