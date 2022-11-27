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
            ViewBag.usuario =JsonConvert.DeserializeObject(HttpContext.Session.GetString("usuario"));
            ViewBag.Vehiculos = CargarVehiculos();
            return View();
        }

        public List<Vehiculo> CargarVehiculos()
        {
            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ObtenerVehiculos", null);
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

                });
            }

            return vehiculosList;
        }

        public ActionResult EliminarVehiculo(int idVehiculo)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@idVehiculo", idVehiculo)
            };

            DatabaseHelper.ExecStoreProcedure("SP_EliminarVehiculo", param);

            return RedirectToAction("Index", "Vehiculos");
        }

        public ActionResult Agregar()
        {
            ViewBag.usuario =JsonConvert.DeserializeObject(HttpContext.Session.GetString("usuario"));
            return View();
        }

        public ActionResult AgregarVehiculo(string placa, string marca, string modelo, string color)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@placa", placa),
                new SqlParameter("@marca", marca),
                new SqlParameter("@modelo", modelo),
                new SqlParameter("@color", color),
            };

            DatabaseHelper.ExecStoreProcedure("SP_AgregarVehiculo", param);


            return RedirectToAction("Index", "Vehiculos");
        }
        public ActionResult Editar(int idVehiculo)
        {
            ViewBag.usuario =JsonConvert.DeserializeObject(HttpContext.Session.GetString("usuario"));
            ViewBag.vehiculo = CargarVehiculo(idVehiculo);
            return View();
        }
        private List<Vehiculo> CargarVehiculo(int idVehiculo)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@idVehiculo", idVehiculo)
            };
            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ObtenerVehiculo", param);
            List<Vehiculo> vehiculoList = new List<Vehiculo>();

            foreach (DataRow row in ds.Rows)
            {
                vehiculoList.Add(new Vehiculo()
                {
                    idVehiculo = Convert.ToInt32(row["idVehiculo"]),
                    placa = row["placa"].ToString(),
                    marca = row["marca"].ToString(),
                    modelo = row["modelo"].ToString(),
                    color = row["color"].ToString(),
                });
            }

            return vehiculoList;
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
    }
}
