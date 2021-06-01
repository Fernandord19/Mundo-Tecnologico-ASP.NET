using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Mundo_Tecnologico.Models;


namespace Mundo_Tecnologico.Controllers
{
    public class MarcasController : Controller
    {

        string cadena = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        IEnumerable<Marca> ListarMarcas()
        {
            List<Marca> lista = new List<Marca>();

            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_listar_marcas", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Marca reg = new Marca()
                    {
                        codigo = dr.GetString(0),
                        nombre = dr.GetString(1),
                        estado = dr.GetByte(2)
                    };

                    lista.Add(reg);
                }

                dr.Close();
                con.Close();

            }

            return lista;
        }

        Marca BuscarMarca(string codigo)
        {
            if (codigo != null)
            {
                return ListarMarcas().Where(m => m.codigo == codigo).FirstOrDefault();
            }
            else
            {
                return null;
            }

        }

        string EditarMarca(Marca mar)
        {
            string mensaje = "";

            using (SqlConnection con = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_editar_marca", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@cod", mar.codigo);
                    cmd.Parameters.AddWithValue("@nom", mar.nombre);
                    cmd.Parameters.AddWithValue("@estado", mar.estado);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    mensaje = "Marca editada con exito";
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }
                finally
                {
                    con.Close();
                }

            }

            return mensaje;
        }

        string RegistrarMarca(Marca mar)
        {
            string mensaje = "";
            using (SqlConnection con = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_registrar_marca", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nom", mar.nombre);
                    cmd.Parameters.AddWithValue("@estado", mar.estado);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    mensaje = "Se registro la marca existosamente";
                }
                catch (SqlException ex)
                {
                    mensaje = ex.Message;

                }
                finally
                {
                    con.Close();
                }

            }
            return mensaje;
        }

        string EliminarMarca(string codigo)
        {
            string mensaje = "";
            using (SqlConnection con = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_eliminar_marca", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cod", codigo);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    mensaje = "Se elimino la marca existosamente";
                }
                catch (SqlException ex)
                {
                    mensaje = ex.Message;
                }
                finally
                {
                    con.Close();
                }
            }
            return mensaje;
        }

        public ActionResult Listado()
        {
            IEnumerable<Marca> lista = ListarMarcas();
            return View(lista);
        }

        public ActionResult Registrar()
        {
            return View(new Marca());
        }

        [HttpPost]
        public ActionResult Registrar(Marca mar)
        {
            TempData["mensaje"] = RegistrarMarca(mar);
            return RedirectToAction("Listado");
        }

        public ActionResult Editar(string codigo = "")
        {
            Marca m = BuscarMarca(codigo);
            return View(m);
        }

        [HttpPost]
        public ActionResult Editar(Marca mar)
        {
            TempData["mensaje"] = EditarMarca(mar);
            return RedirectToAction("Listado");
        }

        public ActionResult Detalle(string codigo = "")
        {
            Marca mar = BuscarMarca(codigo);
            return View(mar);
        }

        public ActionResult Eliminar(string codigo = "")
        {
            TempData["mensaje"] = EliminarMarca(codigo);
            return RedirectToAction("Listado");
        }
    }
}
