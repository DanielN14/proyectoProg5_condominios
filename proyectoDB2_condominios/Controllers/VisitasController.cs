using Microsoft.AspNetCore.Mvc;
using proyecto_condominios.DatabaseHelper;
using proyectoDB2_condominios.Models;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace proyectoDB2_condominios.Controllers
{
    public class VisitasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(string txtNombre, string txtPrimerApellido, string txtSegundoApellido, string txtCedula)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@nombre", txtNombre),
                new SqlParameter("@primerApellido", txtPrimerApellido),
                new SqlParameter("@segundoApellido", txtSegundoApellido),
                new SqlParameter("@cedula", txtCedula),
            };

            DatabaseHelper.ExecStoreProcedure("SP_AgregarVisitante", param);

            return RedirectToAction("Index", "Visitas");
        }

    }
}
