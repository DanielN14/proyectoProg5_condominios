using Microsoft.AspNetCore.Mvc;
using proyecto_condominios.DatabaseHelper;
using proyectoDB2_condominios.Models;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace proyectoDB2_condominios.Controllers
{
    public class VehiculosController : Controller
    {
        public ActionResult Index()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
                ViewBag.Vehiculos = CargarVehiculosCondomino();
                return View();
            }
        }

        public List<Vehiculo> CargarVehiculosCondomino()
        {
            var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));

            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ObtenerVehiculosCondomino", new List<SqlParameter>()
            {
                new SqlParameter("@pIdPersona", usuario!.idPersona),
            });

            List<Vehiculo> vehiculosList = new List<Vehiculo>();

            foreach (DataRow row in ds.Rows)
            {
                vehiculosList.Add(new Vehiculo()
                {
                    idVehiculo = Convert.ToInt32(row["idVehiculo"]),
                    placa = row["placa"].ToString(),
                    marca = row["marca"].ToString(),
                    modelo = row["modelo"].ToString(),
                    color = row["color"].ToString(),
                    accesoLibre = Convert.ToInt32(row["accesoLibre"]),
                });
            }

            return vehiculosList;
        }

        public ActionResult EliminarVehiculo(int idVehiculo, string tipoPropietario)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@idVehiculo", idVehiculo),
                new SqlParameter("@tipoPropietario", tipoPropietario),
            };

            DatabaseHelper.ExecStoreProcedure("SP_EliminarVehiculo", param);

            return RedirectToAction("Index", "Vehiculos");
        }

        public ActionResult Agregar()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
                return View();
            }
        }

        public ActionResult AgregarVehiculoCondomino(string placa, string marca, string modelo, string color, int switchAcceso)
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
                    new SqlParameter("@placa", placa),
                    new SqlParameter("@marca", marca),
                    new SqlParameter("@modelo", modelo),
                    new SqlParameter("@color", color),
                    new SqlParameter("@idCondomino", usuario!.idPersona),
                    new SqlParameter("@AccesoLibre", switchAcceso),
                };

                DatabaseHelper.ExecStoreProcedure("SP_AgregarVehiculoCondomino", param);

                return RedirectToAction("Index", "Vehiculos");
            }
        }

        public ActionResult Editar(int idVehiculo)
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
                ViewBag.vehiculo = CargarVehiculoCondomino(idVehiculo);
                return View();
            }
        }

        private Vehiculo CargarVehiculoCondomino(int idVehiculo)
        {
            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ObtenerVehiculoCondomino", new List<SqlParameter>()
            {
                new SqlParameter("@idVehiculo", idVehiculo)
            });

            Vehiculo vehiculo = new Vehiculo()
            {
                idVehiculo = Convert.ToInt32(ds.Rows[0]["idVehiculo"]),
                placa = ds.Rows[0]["placa"].ToString(),
                marca = ds.Rows[0]["marca"].ToString(),
                modelo = ds.Rows[0]["modelo"].ToString(),
                color = ds.Rows[0]["color"].ToString(),
                accesoLibre = Convert.ToInt32(ds.Rows[0]["accesoLibre"]),
            };

            return vehiculo;
        }

        public ActionResult UpdateVehiculo(int idVehiculo, string placa, string marca, string modelo, string color)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@idVehiculo", idVehiculo),
                new SqlParameter("@placa", placa),
                new SqlParameter("@marca", marca),
                new SqlParameter("@modelo", modelo),
                new SqlParameter("@color", color)
            };

            DatabaseHelper.ExecStoreProcedure("SP_UpdateVehiculo", param);

            return RedirectToAction("Index", "Vehiculos");
        }

        [HttpPost]
        public ActionResult UpdateAccesoLibre(int accesoLibre, int idVehiculo)
        {
            DatabaseHelper.ExecStoreProcedure("SP_UpdateAccesoLibre", new List<SqlParameter>()
            {
                new SqlParameter("@pAccesoLibre", accesoLibre),
                new SqlParameter("@pIdVehiculo", idVehiculo),
            });

            return Ok();
        }
    }
}
