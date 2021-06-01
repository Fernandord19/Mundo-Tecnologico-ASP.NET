using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mundo_Tecnologico.Models
{
    public class Producto
    {
        public string codigo { get; set; }
        public string foto { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public decimal precio { get; set; }
        public int stock { get; set; }
        public byte estado { get; set; }
        public string codigoMarca { get; set; }
        public string codigoCategoria { get; set; }


        // Atributos para las relaciones foráneas
        public string nombreMarca { get; set; }
        public string nombreCategoria { get; set; }
    }
}