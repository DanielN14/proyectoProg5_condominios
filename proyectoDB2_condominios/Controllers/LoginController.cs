using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using proyecto_condominios.DatabaseHelper;
using System.Data.SqlClient;
using System.Data;
using System.Text.Json;
using System.Collections.Generic;
using proyectoDB2_condominios.Models;

namespace proyectoDB2_condominios.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult LoginUser(string email, string password)
        {
            Usuario? usuario = Obtener_Usuario(email, password);

            if (usuario != null)
            {
                HttpContext.Session.SetString("usuario", JsonSerializer.Serialize(usuario));
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = new Models.Error()
                {
                    Message = "Incorrect username or password",
                    BackUrl = "Login",
                    Text = "Try again?"
                };

                return View("Error");
            }
        }

        public Usuario? Obtener_Usuario(string email, string password)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@email", email),
                new SqlParameter("@password", password)
            };

            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ObtenerUsuarioLogin", param);

            if (ds.Rows.Count == 1)
            {
                int? idPers;
                if (ds.Rows[0]["idPersona"] == DBNull.Value)
                {
                    idPers = null;
                }
                else
                {
                    idPers = Convert.ToInt32(ds.Rows[0]["idPersona"]);
                }

                Usuario usuario = new Usuario()
                {
                    idUsuario = Convert.ToInt32(ds.Rows[0]["idUsuario"]),
                    email = ds.Rows[0]["email"].ToString(),
                    password = ds.Rows[0]["password"].ToString(),
                    idRolUsuario = Convert.ToInt32(ds.Rows[0]["idRolUsuario"]),
                    idPersona = idPers,
                };

                return usuario;
            }
            return null;
        }
    }
}
