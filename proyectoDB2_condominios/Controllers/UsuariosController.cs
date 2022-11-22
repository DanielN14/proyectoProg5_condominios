using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using proyecto_condominios.DatabaseHelper;
using proyectoDB2_condominios.Models;

namespace proyectoDB2_condominios.Controllers
{
    public class UsuariosController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.usuario = JsonSerializer.Deserialize<Usuario>(HttpContext.Session.GetString("usuario"));
            ViewBag.Usuarios = CargarUsuarios();
            return View();
        }

        private List<Usuario> CargarUsuarios()
        {
            DataTable ds = DatabaseHelper.ExecuteSelect(
                "SELECT * FROM VW_ObtenerUsuarios;",
                null
            );
            List<Usuario> listadoUsuarios = new List<Usuario>();

            foreach (DataRow row in ds.Rows)
            {
                listadoUsuarios.Add(
                    new Usuario()
                    {
                        idUsuario = Convert.ToInt32(row["idUsuario"]),
                        nombre = row["nombre"].ToString(),
                        primerApellido = row["primerApellido"].ToString(),
                        segundoApellido = row["segundoApellido"].ToString(),
                        cedula = row["cedula"].ToString(),
                        email = row["email"].ToString(),
                        nombreRol = row["nombreRol"].ToString(),
                        nombreCondominio = row["nombreCondominio"].ToString(),
                        idPersona = Convert.ToInt32(row["idPersona"]),
                    }
                    
                );
            }

            return listadoUsuarios;
        }

        public IActionResult Agregar()
        {
            ViewBag.usuario = JsonSerializer.Deserialize<Usuario>(HttpContext.Session.GetString("usuario"));
            ViewBag.rolesUsuarios = CargarRolesUsuarios();
            ViewBag.condominios = CargarCondominios();
            return View();
        }

        private List<RolUsuario> CargarRolesUsuarios()
        {
            DataTable ds = DatabaseHelper.ExecuteSelect("SELECT * FROM rolesUsuarios WHERE nombreRol != 'Administrador'", null);
            List<RolUsuario> listadoRolesUsuario = new List<RolUsuario>();

            foreach (DataRow row in ds.Rows)
            {
                listadoRolesUsuario.Add(
                    new RolUsuario()
                    {
                        idRolUsuario = Convert.ToInt32(row["idRolUsuario"]),
                        nombreRol = row["nombreRol"].ToString(),
                    }
                );
            }

            return listadoRolesUsuario;
        }

        private List<Condominio> CargarCondominios()
        {
            DataTable ds = DatabaseHelper.ExecuteSelect("SELECT idProyectoHabitacional, nombre FROM proyectosHabitacionales", null);
            List<Condominio> listadoCondominios = new List<Condominio>();

            foreach (DataRow row in ds.Rows)
            {
                listadoCondominios.Add(
                    new Condominio()
                    {
                        idProyectoHabitacional = Convert.ToInt32(row["idProyectoHabitacional"]),
                        nombre = row["nombre"].ToString(),
                    }
                );
            }

            return listadoCondominios;
        }

        public IActionResult AgregarUsuario(string txtNombre, string txtPApellido, string txtSApellido, 
            string txtCedula, IFormFile inputPhoto, string txtEmail, string txtPassword, string selectRol,
            string selectCondominio
        )
        {
            string photoPath;

            if (inputPhoto != null)
            {
                photoPath =
                    "/images/fotos_usuarios/"
                    + Guid.NewGuid().ToString()
                    + new FileInfo(inputPhoto.FileName).Extension;

                using (
                    var stream = new FileStream(
                        Directory.GetCurrentDirectory() + "/wwwroot/" + photoPath,
                        FileMode.Create
                    )
                )
                {
                    inputPhoto.CopyTo(stream);
                }
            }
            else
            {
                photoPath = "/images/fotos_usuarios/defaultAvatar.jpg";
            }

            DatabaseHelper.ExecStoreProcedure(
                "SP_AgregarUsuario",
                new List<SqlParameter>()
                {
                    new SqlParameter("@pNombre", txtNombre),
                    new SqlParameter("@pPrimerApellido", txtPApellido),
                    new SqlParameter("@pSegundoApellido", txtSApellido),
                    new SqlParameter("@pCedula", txtCedula),
                    new SqlParameter("@pFoto", photoPath),
                    new SqlParameter("@pEmail", txtEmail),
                    new SqlParameter("@pPassword", txtPassword),
                    new SqlParameter("@pIdRolUsuario", selectRol),
                    new SqlParameter("@pIdProyectoHabitacional", selectCondominio),
                }
            );

            return RedirectToAction("Index", "Usuarios");
        }
        public ActionResult Editar(int idPersona)
        {
            ViewBag.usuario = JsonSerializer.Deserialize<Usuario>(HttpContext.Session.GetString("usuario"));
            ViewBag.rolesUsuarios = CargarRolesUsuarios();
            ViewBag.usuario = CargarUsuario(idPersona);
            return View();
        }
        
        private Usuario CargarUsuario(int idPersona)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@idPersona", idPersona)
            };
            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ObtenerUsuario", param);

            Usuario usuario = new Usuario(){
                idPersona = Convert.ToInt32(ds.Rows[0]["idPersona"]),
                nombre = ds.Rows[0]["nombre"].ToString(),
                primerApellido = ds.Rows[0]["primerApellido"].ToString(),
                segundoApellido = ds.Rows[0]["segundoApellido"].ToString(),
                cedula = ds.Rows[0]["cedula"].ToString(),
                foto = ds.Rows[0]["foto"].ToString(),
                email = ds.Rows[0]["email"].ToString(),
                password = ds.Rows[0]["password"].ToString(),
                nombreRol = ds.Rows[0]["nombreRol"].ToString(),
                idRolUsuario = Convert.ToInt32(ds.Rows[0]["idRolUsuario"]),
                idUsuario = Convert.ToInt32(ds.Rows[0]["idUsuario"]),
            };
 
            return usuario;
        }
        
        public ActionResult UpdateUsuario(int idPersona, int idUsuario, string txtNombre, 
            string txtPApellido, string txtSApellido, string txtCedula, IFormFile inputPhoto, 
            string txtEmail,string txtPassword, string selectRol)
        {

            string? photoPath = null;

            if (inputPhoto != null)
            {
                photoPath =
                    "/images/fotos_usuarios/"
                    + Guid.NewGuid().ToString()
                    + new FileInfo(inputPhoto.FileName).Extension;

                using (
                    var stream = new FileStream(
                        Directory.GetCurrentDirectory() + "/wwwroot/" + photoPath,
                        FileMode.Create
                    )
                )
                {
                    inputPhoto.CopyTo(stream);
                }
            }

            List<SqlParameter> param = new List<SqlParameter>()
            {
                    new SqlParameter("@idPersona", idPersona),
                    new SqlParameter("@idUsuario", idUsuario),
                    new SqlParameter("@Nombre", txtNombre),
                    new SqlParameter("@PrimerApellido", txtPApellido),
                    new SqlParameter("@SegundoApellido", txtSApellido),
                    new SqlParameter("@Cedula", txtCedula),
                    new SqlParameter("@Foto", photoPath), 
                    new SqlParameter("@Email", txtEmail),
                    new SqlParameter("@Password", txtPassword),
                    new SqlParameter("@IdRolUsuario", selectRol),
            };

            DatabaseHelper.ExecStoreProcedure("SP_UpdateUsuario", param);

            return RedirectToAction("Index", "Usuarios");
        }
        
        public ActionResult EliminarUsuario(int idUsuario)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                 new SqlParameter("@idUsuario", idUsuario)
            };

            DatabaseHelper.ExecStoreProcedure("SP_EliminarUsuario", param);

            return RedirectToAction("Index", "Usuarios");
        }
    }
}
