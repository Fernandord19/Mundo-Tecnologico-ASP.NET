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
            if (codigo != null)
            {
                return FiltrarProductos("nombre", "").Where(p => p.codigo == codigo).FirstOrDefault();
            }
            else
            {
                return null;
            }

        }

        Usuario BuscarUsuario(string codigo)
        {

            Usuario reg = null;

            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_USUARIO_VENTA", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigo", codigo);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    reg = new Usuario()
                    {
                        documento = dr.GetString(0),
                        celular = dr.GetString(1)
                    };

                }

                dr.Close();
                con.Close();
            }

            return reg;
        }

        IEnumerable<Distrito> ListarDistritos()
        {
            List<Distrito> lista = new List<Distrito>();

            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_listar_distritos", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Distrito reg = new Distrito();
                    reg.codigo = dr.GetString(0);
                    reg.nombre = dr.GetString(1);

                    lista.Add(reg);
                }

                dr.Close();
                con.Close();
            }

            return lista;
        }

        IEnumerable<Venta> ListarVentas()
        {
            List<Venta> lista = new List<Venta>();

            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_LISTAR_VENTAS", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Venta reg = new Venta()
                    {
                        codigo = dr.GetString(0),
                        fecha = dr.GetDateTime(1),
                        distrito = dr.GetString(2),
                        direccion = dr.GetString(3),
                        referencia = dr.GetString(4),
                        total = dr.GetDecimal(5),
                        estado = dr.GetByte(6),
                        usuario = dr.GetString(7),
                        codigoUsuario = dr.GetString(8),
                        celular = dr.GetString(9),
                        documento = dr.GetString(10)
                    };

                    lista.Add(reg);
                }
                dr.Close(); con.Close();
            }

            return lista;
        }

        IEnumerable<Venta> ListarVentasFechas(DateTime f1, DateTime f2)
        {
            List<Venta> lista = new List<Venta>();

            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_LISTAR_VENTAS_FECHAS", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("f1", f1);
                cmd.Parameters.AddWithValue("f2", f2);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Venta reg = new Venta()
                    {
                        codigo = dr.GetString(0),
                        fecha = dr.GetDateTime(1),
                        direccion = dr.GetString(2),
                        total = dr.GetDecimal(3),
                        estado = dr.GetByte(4),
                        usuario = dr.GetString(5),
                        distrito = dr.GetString(6),
                        celular = dr.GetString(7),
                        documento = dr.GetString(8)
                    };

                    lista.Add(reg);
                }
                dr.Close(); con.Close();
            }

            return lista;
        }

        IEnumerable<Venta> Ventas()
        {
            return ListarVentas().Where(v => v.estado == 1 || v.estado == 2);
        }

        IEnumerable<Item> DetallarVenta(string codigo)
        {
            List<Item> lista = new List<Item>();

            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("SP_LISTAR_DETALLE_VENTA", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigo", codigo);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Item reg = new Item()
                    {
                        nombre = dr.GetString(0),
                        cantidad = dr.GetInt32(1),
                        precio = dr.GetDecimal(2),
                        codigo = dr.GetString(3)
                    };

                    lista.Add(reg);
                }
                dr.Close(); con.Close();
            }

            return lista;
        }

        IEnumerable<Venta> VentaUsuario(string codigoUsuario)
        {
            return ListarVentas().Where(v => v.codigoUsuario == codigoUsuario).ToList();
        }

        string EditarVenta(string codigo, int estado)
        {
            string mensaje = "";

            using (SqlConnection con = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_EDITAR_VENTA", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@codigo", codigo);
                    cmd.Parameters.AddWithValue("@estado", estado);

                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    mensaje = "Venta " + codigo + " editada correctamente";

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

        string AgregarCarrito(string codigo, Int16 stock, int cantidad)
        {

            Producto reg = Buscar(codigo);
            Item it = new Item()
            {
                codigo = reg.codigo,
                nombre = reg.nombre,
                precio = reg.precio,
                stock = reg.stock,
                cantidad = cantidad
            };

            List<Item> temporal = (List<Item>)Session["carrito"];
            bool bandera = true;

            foreach (var item in temporal)
            {
                if (item.codigo == reg.codigo)
                {
                    if ((item.cantidad += cantidad) > reg.stock)
                        item.cantidad = reg.stock;
                    else
                        item.cantidad += cantidad;

                    bandera = false;
                    break;
                }
            }

            if (bandera)
                temporal.Add(it);

            return "Producto agregado al carrito";
        }

        void EditarCarrito(string codigo, int cantidad)
        {
            List<Item> temporal = Session["carrito"] as List<Item>;

            foreach (var item in temporal)
            {
                if (item.codigo == codigo)
                {
                    item.cantidad = cantidad;
                    break;
                }
            }
        }

        void EliminarCarrito(string codigo)
        {
            List<Item> temporal = Session["carrito"] as List<Item>;
            temporal.Remove(temporal.Where(i => i.codigo == codigo).FirstOrDefault());
        }

        void RestablecerStock(string codigo)
        {
            IEnumerable<Item> listaItems = DetallarVenta(codigo);
            using (SqlConnection con = new SqlConnection(cadena))
            {
                con.Open();

                foreach (Item it in listaItems)
                {
                    SqlCommand cmd = new SqlCommand("sp_rechazar_venta", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codigo", it.codigo);
                    cmd.Parameters.AddWithValue("@cantidad", it.cantidad);

                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
        }

        string autoGenerar()
        {
            string codigo = "";
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("select dbo.AutogeneraCodigoVenta()", cn);
                cn.Open();
                codigo = (string)cmd.ExecuteScalar();
                cn.Close();
            }

            return codigo;
        }

        public ActionResult Catalogo(string criterio = "nombre", string filtro = "", int pagInicio = 0, string f = "")
        {
            IEnumerable<Producto> listaProductos = FiltrarProductos(criterio, filtro);
            ViewBag.criterio = criterio;
            ViewBag.filtro = filtro;

            // para la paginación
            int filasPagina = 6;
            int totalRegistros = listaProductos.Count();
            int numPaginas = totalRegistros % filasPagina == 0 ? totalRegistros / filasPagina : totalRegistros / filasPagina + 1;

            // Establecer las condiciones de flecha
            switch (f)
            {
                case "derecha":
                    pagInicio++;
                    if (pagInicio > numPaginas - 1) pagInicio = numPaginas - 1;
                    if (pagInicio < 0) pagInicio = 0;
                    break;
                case "izquierda":
                    pagInicio--;
                    if (pagInicio < 0) pagInicio = 0;
                    break;
                case "inicio":
                    pagInicio = 0;
                    break;
                case "fin":
                    pagInicio = numPaginas - 1;
                    if (pagInicio < 0) pagInicio = 0; // valida si solo hay una página
                    break;
            }

            // ViewBag para almacenar la página de inicio y la cantidad de páginas
            ViewBag.pagInicio = pagInicio;
            ViewBag.numPaginas = numPaginas;

            return View(listaProductos.Skip(filasPagina * pagInicio).Take(filasPagina));
        }

        public ActionResult DetalleAgregar(string codigo = null)
        {
            Producto reg = Buscar(codigo);
            return View(reg);
        }

        [HttpPost]
        public ActionResult Agregar(string codigo, Int16 stock, int cantidad)
        {
            if (cantidad > stock)
            {
                TempData["mensaje"] = "Ingrese Una cantidad menor al stock";
                return RedirectToAction("DetalleAgregar", new { codigo = codigo });
            }

            TempData["mensaje"] = AgregarCarrito(codigo, stock, cantidad);
            return RedirectToAction("DetalleAgregar", new { codigo = codigo });
        }

        public ActionResult Carrito()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                List<Item> productos = Session["carrito"] as List<Item>;
                ViewBag.distritos = ListarDistritos();
                return View(productos);
            }

        }

        public ActionResult EditarCarr(string codigo, int cantidad)
        {
            EditarCarrito(codigo, cantidad);
            return RedirectToAction("Carrito");
        }
        public ActionResult Eliminar(string codigo)
        {
            EliminarCarrito(codigo);
            return RedirectToAction("Carrito");
        }

        public ActionResult Procesar(Venta reg)
        {
            string codigo = autoGenerar();
            string codigoUsuario = (Session["usuario"] as Usuario).codigo;
            string mensaje = "";

            SqlConnection cn = new SqlConnection(cadena);
            cn.Open();
            SqlTransaction tr = cn.BeginTransaction(IsolationLevel.Serializable);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_REGISTRAR_VENTA", cn, tr);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@direccion", reg.direccion);
                cmd.Parameters.AddWithValue("@referencia", reg.referencia);
                cmd.Parameters.AddWithValue("@total", reg.total);
                cmd.Parameters.AddWithValue("@estado", 1);
                cmd.Parameters.AddWithValue("@codigo_usuario", codigoUsuario);
                cmd.Parameters.AddWithValue("@codigo_distrito", reg.codigoDistrito);
                cmd.ExecuteNonQuery();

                foreach (Item it in (Session["carrito"] as List<Item>))
                {
                    cmd = new SqlCommand("SP_REGISTRAR_DETALLE_VENTA", cn, tr);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CODIGO_VENTA", codigo);
                    cmd.Parameters.AddWithValue("@CODIGO_PRODUCTO", it.codigo);
                    cmd.Parameters.AddWithValue("@cantidad", it.cantidad);

                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("sp_venta_producto", cn, tr);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codigo", it.codigo);
                    cmd.Parameters.AddWithValue("@cantidad", it.cantidad);

                    cmd.ExecuteNonQuery();
                }


                tr.Commit();
                mensaje = string.Format("Se ha generado la compra con código ' {0} '", codigo);
            }
            catch (SqlException ex)
            {
                mensaje = ex.Message;
                tr.Rollback();
            }
            finally
            {
                cn.Close();
            }

            return RedirectToAction("Aviso", new { m = mensaje });
        }

        public ActionResult Editar(string codigo, int estado)
        {
            TempData["mensaje"] = EditarVenta(codigo, estado);
            if (estado == 4)
            {
                RestablecerStock(codigo);
            }
            return RedirectToAction("ListadoVentas");
        }
        public ActionResult Aviso(string m)
        {
            ViewBag.mensaje = m;
            Session["carrito"] = new List<Item>();
            return View();
        }

        public ActionResult MisCompras()
        {
            IEnumerable<Venta> lista = VentaUsuario((Session["usuario"] as Usuario).codigo);
            return View(lista);
        }

        public ActionResult ListadoVentas()
        {
            IEnumerable<Venta> lista = Ventas();
            return View(lista);
        }

        public ActionResult DetalleVenta(string codigo)
        {
            IEnumerable<Item> lista = DetallarVenta(codigo);
            ViewBag.codigo = codigo;
            return View(lista);
        }

        public ActionResult ReporteVentas()
        {
            return View();
        }

        public void MostrarReporte(DateTime f1, DateTime f2)
        {
            try
            {
                IEnumerable<Venta> listaVentas = new List<Venta>();

                listaVentas = ListarVentasFechas(f1, f2);


                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "ReporteVentas.rpt"));
                rd.SetDataSource(listaVentas);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();


                rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "ventas");

            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

    }
}