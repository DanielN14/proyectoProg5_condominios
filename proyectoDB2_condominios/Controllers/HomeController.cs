using Microsoft.AspNetCore.Mvc;
using proyecto_condominios.DatabaseHelper;
using proyectoDB2_condominios.Models;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace proyectoDB2_condominios.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                ViewBag.usuario = JsonSerializer.Deserialize<Usuario>(HttpContext.Session.GetString("usuario"));

                return View();
            }
            else
            {
                ViewBag.Error = new Models.Error()
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