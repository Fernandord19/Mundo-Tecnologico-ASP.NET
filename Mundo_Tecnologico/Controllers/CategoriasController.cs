using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mundo_Tecnologico.Models;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;

namespace Mundo_Tecnologico.Controllers
{
    public class CategoriasController : Controller
    {
        string cadena = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        IEnumerable<Categoria> ListarCategorias()
        {
            List<Categoria> lista = new List<Categoria>();
            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_listar_categorias", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Categoria reg = new Categoria()
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
        Categoria BuscarCategoria(string codigo)
        {
            if (codigo != null)
            {
                return ListarCategorias().Where(p => p.codigo == codigo).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        string EditarCategoria(Categoria reg)
        {
            string mensaje = "";
            using (SqlConnection con = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_editar_categoria", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cod", reg.codigo);
                    cmd.Parameters.AddWithValue("@nom", reg.nombre);
                    cmd.Parameters.AddWithValue("@est", reg.estado);
                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    mensaje = "Categoria " + reg.codigo + " editada correctamente";
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
        string RegistrarCategoria(Categoria reg)
        {
            string mensaje = "";
            using (SqlConnection con = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_registrar_categoria", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nom", reg.nombre);
                    cmd.Parameters.AddWithValue("@estado", reg.estado);
                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    mensaje = "Categoria " + reg.codigo + " registrada correctamente";
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
        string EliminarCategoria(string codigo)
        {
            string mensaje = "";
            using (SqlConnection con = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_eliminar_categoria", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cod", codigo);
                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    mensaje = "Categoria " + codigo + " eliminada correctamente";
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
        public ActionResult Listado()
        {
            IEnumerable<Categoria> lista = ListarCategorias();
            return View(lista);
        }

        public ActionResult Registrar()
        {
            return View(new Categoria());
        }

        [HttpPost]
        public ActionResult Registrar(Categoria reg)
        {
            TempData["mensaje"] = RegistrarCategoria(reg);
            return RedirectToAction("Listado");
        }

        public ActionResult Editar(string codigo = null)
        {
            Categoria reg = BuscarCategoria(codigo);
            return View(reg);
        }

        [HttpPost]
        public ActionResult Editar(Categoria reg)
        {
            TempData["mensaje"] = EditarCategoria(reg);
            return RedirectToAction("Listado");
        }

        public ActionResult Detalle(string codigo = null)
        {
            Categoria reg = BuscarCategoria(codigo);
            return View(reg);
        }

        public ActionResult Eliminar(string codigo = null)
        {
            TempData["mensaje"] = EliminarCategoria(codigo);
            return RedirectToAction("Listado");
        }

        public void ReporteCategorias()
        {
            try
            {
                IEnumerable<Categoria> listaCategorias = new List<Categoria>();
                listaCategorias = ListarCategorias();

                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "ReporteCategorias.rpt"));
                rd.SetDataSource(listaCategorias);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "categoria");

            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

    }
}