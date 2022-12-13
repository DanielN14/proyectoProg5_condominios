using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using proyecto_condominios.DatabaseHelper;
using proyectoDB2_condominios.Models;
using System.Data.SqlClient;
using System.Data;

namespace proyectoDB2_condominios.Controllers
{
    public class SeguridadController : Controller
    {
        public IActionResult Index()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
                ViewBag.Condominios = CargarCondominios();
                return View();
            }
        }

        public IActionResult Visitas_QR()
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

        public IActionResult Vehiculos_Condominos(string busqueda)
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
                ViewBag.vehiculos = CargarVehiculos_Condominos(busqueda);
                return View();
            }
        }

        public List<Vehiculo> CargarVehiculos_Condominos(string busqueda)
        {
            if (busqueda == null)
            {
                busqueda = "";
            }

            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ObtenerVehiculosResidentes", new List<SqlParameter>(){
                new SqlParameter("@busqueda", busqueda),
            });

            List<Vehiculo> VehiculosList = new List<Vehiculo>();

            foreach (DataRow row in ds.Rows)
            {
                VehiculosList.Add(new Vehiculo()
                {
                    idVehiculo = Convert.ToInt32(row["idVehiculo"]),
                    placa = row["placa"].ToString(),
                    marca = row["marca"].ToString(),
                    modelo = row["modelo"].ToString(),
                    color = row["color"].ToString(),
                });
            }

            return VehiculosList;
        }

        public ActionResult Validacion(int CodigoQR)
        {
            var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ValidarQR", new List<SqlParameter>(){
                new SqlParameter("@CodigoQR", CodigoQR),
            });

            List<QR> qrList = new List<QR>();
            foreach (DataRow row in ds.Rows)
            {
                qrList.Add(new QR()
                {
                    codigo = Convert.ToInt32(row["codigo"]),
                });
            }

            if (qrList.Count() != 0)
            {
                TempData["Mensaje"] = "Codigo correcto";
                return RedirectToAction("Visitas_QR", "Seguridad");
            }
            else
            {
                TempData["Error"] = "Codigo incorrecto";
                return RedirectToAction("Visitas_QR");
            }
        }

        public ActionResult MarcarSalida(int idVisita, int idProyectoHabitacional)
        {
            DatabaseHelper.ExecStoreProcedure("SP_UpdateFechaSalida", new List<SqlParameter>()
            {
                new SqlParameter("@idVisita", idVisita),
            });

            return RedirectToAction("Visitas_Guarda", new { idProyectoHabitacional = idProyectoHabitacional });
        }

        public List<Condominio> CargarCondominios()
        {
            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ObtenerCondominios", null);
            List<Condominio> condominioList = new List<Condominio>();

            foreach (DataRow row in ds.Rows)
            {
                condominioList.Add(new Condominio()
                {
                    idProyectoHabitacional = Convert.ToInt32(row["idProyectoHabitacional"]),
                    logo = row["logo"].ToString(),
                    codigo = row["codigo"].ToString(),
                    nombre = row["nombre"].ToString(),
                    direccion = row["direccion"].ToString(),
                    telefonoOficina = row["telefonoOficina"].ToString(),
                });
            }

            return condominioList;
        }
        
        public ActionResult Visitas_Guarda(int idProyectoHabitacional, string busquedaTradicional, string busquedaDelivery)
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
                ViewBag.VisitasTradicionales_Guarda = CargarVisitasTradicionales_Guarda(idProyectoHabitacional, busquedaTradicional);
                ViewBag.VisitasDelivery_Guarda = CargarVisitasDelivery_Guarda(idProyectoHabitacional, busquedaDelivery);
                return View();
            }
        }

        public List<VisitasTradicionales_Guarda> CargarVisitasTradicionales_Guarda(int idProyectoHabitacional, string busquedaTradicional)
        {
            if (busquedaTradicional == null)
            {
                busquedaTradicional = "";
            }
            
            var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));

            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ObtenerVisitasTradicional_Guarda", new List<SqlParameter>()
            {
                new SqlParameter("@idProyectoHabitacional", idProyectoHabitacional),
                new SqlParameter("@busquedaTradicional", busquedaTradicional),
            });

            List<VisitasTradicionales_Guarda> listVisitasTradicionales = new List<VisitasTradicionales_Guarda>();

            foreach (DataRow row in ds.Rows)
            {
                listVisitasTradicionales.Add(new VisitasTradicionales_Guarda()
                {
                    idVisita = Convert.ToInt32(row["idVisita"]),
                    FechaEntrada = Convert.ToDateTime(row["fechaHoraEntrada"]),
                    numeroVivienda = row["numeroVivienda"].ToString(),
                    nombreResidente = row["nombreResidente"].ToString(),
                    nombreVisitante = row["nombreVisitante"].ToString(),
                    cedulaVisitante = row["cedulaVisitante"].ToString(),
                    placaVehiculo = row["placa"].ToString(),
                    marcaVehiculo = row["marca"].ToString(),
                    condominio = row["condominio"].ToString(),
                });
            }
            ViewData["idProyectoHabitacional"] = idProyectoHabitacional;
            return listVisitasTradicionales;
        }

        public List<VisitasDelivery_Guarda> CargarVisitasDelivery_Guarda(int idProyectoHabitacional, string busquedaDelivery)
        {
            if (busquedaDelivery == null)
            {
                busquedaDelivery = "";
            }

            var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));

            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ObtenerVisitasDelivery_Guarda", new List<SqlParameter>(){
                new SqlParameter("@idProyectoHabitacional", idProyectoHabitacional),
                new SqlParameter("@busquedaDelivery", busquedaDelivery),
            });

            List<VisitasDelivery_Guarda> listVisitasDelivery = new List<VisitasDelivery_Guarda>();

            foreach (DataRow row in ds.Rows)
            {
                listVisitasDelivery.Add(new VisitasDelivery_Guarda()
                {
                    idVisita = Convert.ToInt32(row["idVisita"]),
                    FechaEntrada = Convert.ToDateTime(row["fechaHoraEntrada"]),
                    numeroVivienda = row["numeroVivienda"].ToString(),
                    nombreResidente = row["nombreResidente"].ToString(),
                    proveedorDelivery = row["proveedorDelivery"].ToString(),
                    condominio = row["condominio"].ToString(),
                });
            }
            ViewData["idProyectoHabitacional"] = idProyectoHabitacional;
            return listVisitasDelivery;
        }

    }
}
