using Microsoft.AspNetCore.Mvc;
using proyecto_condominios.DatabaseHelper;
using proyectoDB2_condominios.Models;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace proyectoDB2_condominios.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                ViewBag.usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));

                return View();
            }
            else
            {
                ViewBag.Error = new Error()
                {
                    Message = "You must LogIn first",
                    BackUrl = "Login",
                    Text = "Go back to LogIn"
                };

                return View("Error");
            }
        }

        public ActionResult ObtenerPaginas(string id)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@idRolUsuario", id),
            };

            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_PaginaXRol", param);

            List<Paginas> paginas = new List<Paginas>();

            foreach (DataRow dr in ds.Rows)
            {
                paginas.Add(new Paginas()
                {
                    Pagina = dr["Pagina"].ToString()
                });
            }

            return Json(paginas);
        }
    }
}