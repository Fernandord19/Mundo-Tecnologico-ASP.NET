using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mundo_Tecnologico.Models;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Mundo_Tecnologico.Controllers
{
    public class EcommerceController : Controller
    {
        string cadena = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        IEnumerable<Producto> FiltrarProductos(string criterio, string filtro)
        {
            List<Producto> lista = new List<Producto>();

            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_filtrar_productos", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@criterio", criterio);
                cmd.Parameters.AddWithValue("@filtro", filtro);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Producto reg = new Producto()
                    {
                        codigo = dr.GetString(0),
                        foto = dr.GetString(1),
                        nombre = dr.GetString(2),
                        descripcion = dr.GetString(3),
                        precio = dr.GetDecimal(4),
                        stock = dr.GetInt32(5),
                    };

                    lista.Add(reg);
                }

                dr.Close();
                con.Close();

            }

            return lista;

        }

        Producto Buscar(string codigo)
        {
            if(codigo != null)
            {
                return FiltrarProductos("nombre", "").Where(p => p.codigo == codigo).FirstOrDefault();
            }
            else
            {
                return null;
            }
            
        }

        public ActionResult Catalogo(string criterio = "nombre", string filtro = "")
        {
            IEnumerable<Producto> listaProductos = FiltrarProductos(criterio, filtro);
            return View(listaProductos);
        }

        public ActionResult Detalle(string codigo = null)
        {
            Producto reg = Buscar(codigo);
            return View(reg);
        }
    }
}