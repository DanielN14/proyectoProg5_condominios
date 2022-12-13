using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using proyecto_condominios.DatabaseHelper;
using proyectoDB2_condominios.Models;
using System.Data.SqlClient;
using System.Drawing;
using System.Numerics;

namespace proyectoDB2_condominios.Controllers
{
    public class VisitantesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult Agregar()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                ViewBag.usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));

                return View();
            }
            else
                return RedirectToAction("Index","Login");
        }

        public ActionResult AgregarVisitante(string nombre, string primerApellido, string segundoApellido , string cedula,int switchfavorito)
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));

                List<SqlParameter> param = new List<SqlParameter>()
                {

                    new SqlParameter("@pNombre", nombre),
                    new SqlParameter("@pPrimerApellido", primerApellido),
                    new SqlParameter("@pSegundoApellido", segundoApellido),
                    new SqlParameter("@pCedula", cedula),
                    new SqlParameter("@pFavorito", switchfavorito),
                    new SqlParameter("@pidPersona", usuario!.idPersona),
                };

                DatabaseHelper.ExecStoreProcedure("SP_AgregarVisitante", param);

                return RedirectToAction("Agregar", "Visitantes");
            }
        }

    }
}
