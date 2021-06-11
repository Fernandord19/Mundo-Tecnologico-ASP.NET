using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mundo_Tecnologico.Models;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;

namespace Mundo_Tecnologico.Controllers
{
    public class ProductosController : Controller
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

        IEnumerable<Producto> ListarProductos()
        {
            List<Producto> lista = new List<Producto>();

            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_listar_productos", con);
                cmd.CommandType = CommandType.StoredProcedure;
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
                        estado = dr.GetByte(6),
                        nombreMarca = dr.GetString(7),
                        nombreCategoria = dr.GetString(8)
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
            if (codigo != null)
            {
                return ListarProductos().Where(p => p.codigo == codigo).FirstOrDefault();
            } else
            {
                return null;
            }
            
        }

        string EditarProducto(Producto reg, string rutaFoto, HttpPostedFileBase foto)
        {
            string mensaje = "";

            using (SqlConnection con = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_editar_producto", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@cod", reg.codigo);
                    cmd.Parameters.AddWithValue("@foto", rutaFoto);
                    cmd.Parameters.AddWithValue("@nom", reg.nombre);
                    cmd.Parameters.AddWithValue("@des", reg.descripcion);
                    cmd.Parameters.AddWithValue("@pre", reg.precio);
                    cmd.Parameters.AddWithValue("@stock", reg.stock);
                    cmd.Parameters.AddWithValue("@estado", reg.estado);
                    cmd.Parameters.AddWithValue("@codmar", reg.codigoMarca);
                    cmd.Parameters.AddWithValue("@codcat", reg.codigoCategoria);

                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    mensaje = "Producto " + reg.codigo + " editado correctamente";

                    if (i == 1 && foto!= null)
                    {
                        foto.SaveAs(Server.MapPath(rutaFoto));
                    }
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

        string AutoGenerarCodigo()
        {
            string codigo = "";

            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("select dbo.AutogeneraCodigoProducto()", con);
                con.Open();
                codigo = (string)cmd.ExecuteScalar();
                con.Close();

            }

            return codigo;
        }

        string RegistrarProducto(Producto reg, string rutaFoto, HttpPostedFileBase foto)
        {
            string mensaje = "";

            using (SqlConnection con = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_registrar_producto", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@foto", rutaFoto);
                    cmd.Parameters.AddWithValue("@nom", reg.nombre);
                    cmd.Parameters.AddWithValue("@des", reg.descripcion);
                    cmd.Parameters.AddWithValue("@pre", reg.precio);
                    cmd.Parameters.AddWithValue("@stock", reg.stock);
                    cmd.Parameters.AddWithValue("@est", reg.estado);
                    cmd.Parameters.AddWithValue("@codmar", reg.codigoMarca);
                    cmd.Parameters.AddWithValue("@codcat", reg.codigoCategoria);

                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    mensaje = "Producto " + reg.codigo + " registrado correctamente";

                    if (i == 1)
                    {
                        foto.SaveAs(Server.MapPath(rutaFoto));
                    }
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

        string EliminarProducto(string codigo)
        {
            string mensaje = "";

            using (SqlConnection con = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_eliminar_producto", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@cod", codigo);

                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    mensaje = "Producto " + codigo + " eliminado correctamente";

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
            IEnumerable<Producto> lista = ListarProductos();
            return View(lista);
        }

        public ActionResult Registrar()
        {
            ViewBag.marcas = ListarMarcas().Where(m => m.estado == 1).ToList();
            ViewBag.categorias = ListarCategorias().Where(c => c.estado == 1).ToList();
            return View(new Producto());
        }

        [HttpPost]
        public ActionResult Registrar(Producto reg, HttpPostedFileBase foto)
        {
            // Creamos la ruta virtual del archivo seleccionado en Img
            string ruta = "~/Img/Productos/" + AutoGenerarCodigo() + ".jpg";

            ViewBag.marcas = ListarMarcas().Where(m => m.estado == 1).ToList();
            ViewBag.categorias = ListarCategorias().Where(c => c.estado == 1).ToList();
            TempData["mensaje"] = RegistrarProducto(reg, ruta, foto);
            return RedirectToAction("Listado");

        }

        public ActionResult Editar(string codigo = null)
        {
            Producto reg = Buscar(codigo);
            ViewBag.marcas = ListarMarcas().Where(m => m.estado == 1).ToList();
            ViewBag.categorias = ListarCategorias().Where(c => c.estado == 1).ToList();
            return View(reg);
        }

        [HttpPost]
        public ActionResult Editar(Producto reg, HttpPostedFileBase foto)
        {
            // Si es NULL, mantiene su foto original
            // Creamos la ruta virtual del archivo seleccionado en Img
            string ruta = "~/Img/Productos/" + reg.codigo + ".jpg";

            ViewBag.marcas = ListarMarcas().Where(m => m.estado == 1).ToList();
            ViewBag.categorias = ListarCategorias().Where(c => c.estado == 1).ToList();
            TempData["mensaje"] = EditarProducto(reg, ruta, foto);
            return RedirectToAction("Listado");
        }

        public ActionResult Detalle(string codigo = null)
        {
            Producto reg = Buscar(codigo);
            return View(reg);
        }

        public ActionResult Eliminar(string codigo = null)
        {
            TempData["mensaje"] = EliminarProducto(codigo);
            return RedirectToAction("Listado");
        }

        
    }
}