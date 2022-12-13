using Microsoft.AspNetCore.Mvc;
using proyecto_condominios.DatabaseHelper;
using proyectoDB2_condominios.Models;
using System.Data;
using System.Data.SqlClient;
using proyectoDB2_condominios.Controllers;
using Newtonsoft.Json;

namespace proyectoDB2_condominios.Controllers
{
    public class CondominiosController : Controller
    {
        public ActionResult Index()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                return RedirectToAction("Index","Login");
            }
            else
            {
                ViewBag.usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
                ViewBag.Condominios = CargarCondominios();
                return View();
            }
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

        public ActionResult Editar(int idProyectoHabitacional)
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                return RedirectToAction("Index","Login");
            }
            else
            {
                ViewBag.usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
                ViewBag.condominio = CargarCondominio(idProyectoHabitacional);
                ViewBag.viviendas = ViviendasController.CargarViviendas(idProyectoHabitacional);
                return View();
            }
        }

        private Condominio CargarCondominio(int idProyectoHabitacional)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@idProyectoHabitacional", idProyectoHabitacional)
            };

            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ObtenerCondominio", param);

            Condominio condominio = new Condominio()
            {
                idProyectoHabitacional = Convert.ToInt32(ds.Rows[0]["idProyectoHabitacional"]),
                logo = ds.Rows[0]["logo"].ToString(),
                codigo = ds.Rows[0]["codigo"].ToString(),
                nombre = ds.Rows[0]["nombre"].ToString(),
                direccion = ds.Rows[0]["direccion"].ToString(),
                telefonoOficina = ds.Rows[0]["telefonoOficina"].ToString(),
            };

            return condominio;
        }

        public ActionResult UpdateCondominio(IFormFile inputPhoto, int idProyectoHabitacional, string nombre, string direccion, string telefonoOficina)
        {

            string? photoPath = null;

            if (inputPhoto != null)
            {
                photoPath =
                    "/images/fotos_condominios/"
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
                new SqlParameter("@idProyectoHabitacional", idProyectoHabitacional),
                new SqlParameter("@logo", photoPath),
                new SqlParameter("@nombre", nombre),
                new SqlParameter("@direccion", direccion),
                new SqlParameter("@telefonoOficina", telefonoOficina)
            };

            DatabaseHelper.ExecStoreProcedure("SP_UpdateCondominio", param);

            return RedirectToAction("Index", "Condominios");
        }

        public ActionResult EliminarCondominio(int idProyectoHabitacional)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@idProyectoHabitacional", idProyectoHabitacional)
            };

            DatabaseHelper.ExecStoreProcedure("SP_EliminarCondominio", param);

            return RedirectToAction("Index", "Condominios");
        }

        public ActionResult Agregar()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                return RedirectToAction("Index","Login");
            }
            else
            {
                ViewBag.usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
                return View();
            }
        }

        public ActionResult AgregarCondominio(IFormFile inputPhoto, string codigo, string nombre, string direccion, string telefonoOficina, string selectNumViviendas)
        {
            string photoPath;

            if (inputPhoto != null)
            {
                photoPath =
                    "/images/fotos_condominios/"
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
                photoPath = "/images/fotos_condominios/defaultCondominio.png";
            }

            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@logo", photoPath),
                new SqlParameter("@codigo", codigo),
                new SqlParameter("@nombre", nombre),
                new SqlParameter("@direccion", direccion),
                new SqlParameter("@telefonoOficina", telefonoOficina),
                new SqlParameter("@numeroViviendas", selectNumViviendas),
            };

            DatabaseHelper.ExecStoreProcedure("SP_AgregarCondominio", param);


            return RedirectToAction("Index", "Condominios");
        }
    }
}
