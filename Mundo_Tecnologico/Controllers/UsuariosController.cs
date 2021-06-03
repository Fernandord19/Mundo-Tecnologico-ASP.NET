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
    public class UsuariosController : Controller
    {

        string cadena = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        IEnumerable<Usuario> ListarUsuarios()
        {
            List<Usuario> lista = new List<Usuario>();

            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_listar_usuarios", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Usuario reg = new Usuario()
                    {
                        codigo = dr.GetString(0),
                        nombre = dr.GetString(1),
                        apellidos = dr.GetString(2),
                        correo = dr.GetString(3),
                        estado = dr.GetByte(4),
                        tipoUsuario = dr.GetString(5)
                    };

                    lista.Add(reg);
                }
                dr.Close(); con.Close();
            }

            return lista;
        }

        Usuario Buscar(string codigo)
        {
            if (codigo != null)
            {
                return ListarUsuarios().Where(u => u.codigo == codigo).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        Usuario ValidarAcceso(string correo, string clave)
        {
            Usuario reg = null;

            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_validar_acceso", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@correo", correo);
                cmd.Parameters.AddWithValue("@clave", Usuario.GetSHA256(clave));
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    reg = new Usuario()
                    {
                        codigo = dr.GetString(0),
                        nombre = dr.GetString(1),
                        apellidos = dr.GetString(2),
                        correo = dr.GetString(3),
                        estado = dr.GetByte(4),
                        tipoUsuario = dr.GetString(5)
                    };

                }
                dr.Close(); con.Close();
            }

            return reg;
        }

        void IniciarSesion(Usuario reg)
        {
            Session["usuario"] = reg;
            Session.Timeout = 30; // 30 minutos de inactividad
        }

        void CerrarSesion()
        {
            Session.Abandon();
        }

        string RegistrarUsuario(Usuario reg)
        {
            string mensaje = "";

            using (SqlConnection con = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_registrar_usuario", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@nom", reg.nombre);
                    cmd.Parameters.AddWithValue("@ape", reg.apellidos);
                    cmd.Parameters.AddWithValue("@correo", reg.correo);
                    cmd.Parameters.AddWithValue("@clave", Usuario.GetSHA256(reg.clave));
                    cmd.Parameters.AddWithValue("@estado", reg.estado);
                    cmd.Parameters.AddWithValue("@cod_tip_usu", reg.codigoTipoUsuario);

                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    mensaje = "Usuario " + reg.codigo + " registrado correctamente";
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

        string EditarUsuario(Usuario reg)
        {
            string mensaje = "";
            using (SqlConnection con = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_editar_usuario", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@cod", reg.codigo);
                    cmd.Parameters.AddWithValue("@nom", reg.nombre);
                    cmd.Parameters.AddWithValue("@ape", reg.apellidos);
                    cmd.Parameters.AddWithValue("@corre", reg.correo);
                    cmd.Parameters.AddWithValue("@clav", reg.clave);
                    cmd.Parameters.AddWithValue("@esta", reg.estado);
                    cmd.Parameters.AddWithValue("@cod_tip_usu", reg.codigoTipoUsuario);

                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    mensaje = "Usuario " + reg.codigo + " editado correctamente";
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

        string EliminarUsuario(string codigo)
        {
            string mensaje = "";

            using (SqlConnection con = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_eliminar_usuario", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@cod", codigo);

                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    mensaje = "Usuario " + codigo + " eliminado correctamente";

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
            IEnumerable<Usuario> lista = ListarUsuarios();
            return View(lista);
        }

        public ActionResult Registrar()
        {
            return View(new Usuario());
        }

        [HttpPost]
        public ActionResult Registrar(Usuario reg)
        {
            TempData["mensaje"] = RegistrarUsuario(reg);
            return RedirectToAction("Listado");

        }
        
        public ActionResult Editar(string codigo = null)
        {
            Usuario reg = Buscar(codigo);
            return View(reg);
        }

        [HttpPost]
        public ActionResult Editar(Usuario reg)
        {
            TempData["mensaje"] = EditarUsuario(reg);
            return RedirectToAction("Listado");
        }
        
        public ActionResult Eliminar(string codigo = null)
        {
            TempData["mensaje"] = EliminarUsuario(codigo);
            return RedirectToAction("Listado");
        }

        public ActionResult Validar(string correo, string clave)
        {
            Usuario reg = ValidarAcceso(correo, clave);
            if (reg != null)
            {
                IniciarSesion(reg);
                return RedirectToAction("Index", "Home");
            } else
            {
                TempData["mensaje"] = "Credenciales Incorrectas o Usuario eliminado";
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult Cerrar()
        {
            CerrarSesion();
            return RedirectToAction("Login", "Home");
        }
    }
}