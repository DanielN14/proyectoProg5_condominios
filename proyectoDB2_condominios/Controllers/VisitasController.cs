using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using proyecto_condominios.DatabaseHelper;
using QRCoder;
using System;
using System.IO;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using proyectoDB2_condominios.Models;
using System.Data;
using System.Numerics;

namespace proyectoDB2_condominios.Controllers
{
    public class VisitasController : Controller
    {
        public IActionResult Index()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
                ViewBag.usuario = usuario;
                ViewBag.visitasTradicionales = obtenerVisitasTradicionales();
                ViewBag.visitasDelivery = obtenerVisitasDelivery();
                return View();
            }
        }

        public IActionResult Editar_Visitas_Delivery(int idVisita)
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
                ViewBag.usuario = usuario;
                ViewBag.visitasDelivery = ObtenerVisitaDelivery(idVisita);
                return View();
            }
        }

        public List<VisitasDelivery> ObtenerVisitaDelivery(int idVisita)
        {

            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ObtenerVisitaDeliveryEditar", new List<SqlParameter>()
            {
                new SqlParameter("@idVisita", idVisita)
            });

            List<VisitasDelivery> ListVisitasDelivery = new List<VisitasDelivery>();

            foreach (DataRow row in ds.Rows)
            {
                ListVisitasDelivery.Add(new VisitasDelivery()
                {
                    idVisita = Convert.ToInt32(row["idVisita"]),
                    FechaEntrada = Convert.ToDateTime(row["fechaHoraEntrada"]),
                    proveedorDelivery = row["proveedorDelivery"].ToString(),
                }
                );
            }

            return ListVisitasDelivery;
        }

        public ActionResult UpdateVisita_Delivery(DateTime fechaEntrada, int idVisita, string selectDelivery)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@idVisita", idVisita),
                new SqlParameter("@fechaEntrada", fechaEntrada),
                new SqlParameter("@proveedorDelivery", selectDelivery)
            };

            DatabaseHelper.ExecStoreProcedure("SP_UpdateVisitaDelivery", param);

            return RedirectToAction("Index", "Visitas");
        }

        public IActionResult Editar_Visitas_Tradicional(int idVisita)
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
                ViewBag.usuario = usuario;
                ViewBag.visitasTradicional = ObtenerVisitaTradional(idVisita);
                return View();
            }

        }

        public List<VisitasTradicionales> ObtenerVisitaTradional(int idVisita)
        {
            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ObtenerVisitaTradicionalEditar", new List<SqlParameter>()
            {
                new SqlParameter("@idVisita", idVisita)
            });

            List<VisitasTradicionales> ListVisitasTradicionales = new List<VisitasTradicionales>();

            foreach (DataRow row in ds.Rows)
            {
                ListVisitasTradicionales.Add(new VisitasTradicionales()
                {
                    idVisita = Convert.ToInt32(row["idVisita"]),
                    FechaEntrada = Convert.ToDateTime(row["fechaHoraEntrada"]),
                });
            }

            return ListVisitasTradicionales;
        }

        public ActionResult UpdateVisita_Tradicional(int idVisita, DateTime fechaEntrada)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@idVisita", idVisita),
                new SqlParameter("@fechaEntrada", fechaEntrada),
            };

            DatabaseHelper.ExecStoreProcedure("SP_UpdateVisitaTradicional", param);

            return RedirectToAction("Index", "Visitas");
        }
        
        public List<VisitasDelivery> obtenerVisitasDelivery()
        {
            var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));

            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ObtenerVisitasDelivery", new List<SqlParameter>()
            {
                new SqlParameter("@pIdPersona", usuario!.idPersona)
            });

            List<VisitasDelivery> ListVisitasDelivery = new List<VisitasDelivery>();

            foreach (DataRow row in ds.Rows)
            {
                ListVisitasDelivery.Add(new VisitasDelivery()
                {
                    idVisita = Convert.ToInt32(row["idVisita"]),
                    FechaEntrada = Convert.ToDateTime(row["fechaHoraEntrada"]),
                    proveedorDelivery = row["proveedorDelivery"].ToString(),
                }
                );
            }

            return ListVisitasDelivery;
        }

        public List<VisitasTradicionales> obtenerVisitasTradicionales()
        {
            var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));

            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ObtenerVisitasTradicional", new List<SqlParameter>()
            {
                new SqlParameter("@pIdPersona", usuario!.idPersona)
            });

            List<VisitasTradicionales> ListVisitasTradicionales = new List<VisitasTradicionales>();

            foreach (DataRow row in ds.Rows)
            {
                ListVisitasTradicionales.Add(new VisitasTradicionales()
                {
                    idVisita = Convert.ToInt32(row["idVisita"]),
                    FechaEntrada = Convert.ToDateTime(row["fechaHoraEntrada"]),
                    nomCompletoVisitante = row["nombreVisitante"].ToString(),
                    cedula = row["cedula"].ToString(),
                }
                );
            }

            return ListVisitasTradicionales;
        }

        public ActionResult Agregar(int idPersona)
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.usuario = JsonConvert.DeserializeObject(HttpContext.Session.GetString("usuario"));
                ViewBag.visitantes = CargarVisitantes();
                ViewBag.tipoVisita = CargarTipoVisitas();
                ViewBag.visitantesFav = CargarVisitantesFav();
                return View();
            }
        }

        [HttpPost]
        public ActionResult AgregarVisita(DateTime fechaEntrada, int selectTipo,
        int? selectVisitanteExistente, int? selectVisitantefavoritos, string? selectDelivery,
        string? txtNombre, string? txtPApellido, string? txtSApellido, string? txtCedula, int? switchfavorito,
        string? txtPlaca, string? txtMarca, string? txtModelo, string? txtColor, int? switchEnVehiculo)
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
                    new SqlParameter("@fechaHoraEntrada", fechaEntrada),
                    new SqlParameter("@idTipoVisita", selectTipo),
                    new SqlParameter("@idPersona", usuario!.idPersona),
                };

                // Visita de tipo delivery
                if (selectTipo == 1)
                {
                    param.Add(new SqlParameter("@proveedorDelivery", selectDelivery));
                }

                // Visita de tipo Tradicional 
                if (selectTipo == 2)
                {
                    // Nuevo visitante
                    if (selectVisitanteExistente == null && selectVisitantefavoritos == null)
                    {
                        param.Add(new SqlParameter("@nombreVisitante", txtNombre));
                        param.Add(new SqlParameter("@primerApellido", txtPApellido));
                        param.Add(new SqlParameter("@segundoApellido", txtSApellido));
                        param.Add(new SqlParameter("@cedula", txtCedula));
                        param.Add(new SqlParameter("@favorito", switchfavorito));

                        // Vehículo
                        if (switchEnVehiculo == 1)
                        {
                            param.Add(new SqlParameter("@EnVehiculo", switchEnVehiculo));
                            param.Add(new SqlParameter("@placaVehiculo", txtPlaca));
                            param.Add(new SqlParameter("@marcaVehiculo", txtMarca));
                            param.Add(new SqlParameter("@modeloVehiculo", txtModelo));
                            param.Add(new SqlParameter("@colorVehiculo", txtColor));
                        }
                    }
                    else if (selectVisitantefavoritos == null)
                    // Visitante existente
                    {
                        param.Add(new SqlParameter("@idVisitante", selectVisitanteExistente));
                    }
                    else
                    // Visitante favoritos
                    {
                        param.Add(new SqlParameter("@idVisitante", selectVisitantefavoritos));
                    }
                }


                DatabaseHelper.ExecStoreProcedure("SP_AgregarVisita", param);

                return RedirectToAction("Index", "Visitas");
            }
        }

        public ActionResult EasyPass()
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

        public IActionResult AgregarQR()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("usuario")))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
                ViewBag.usuario = usuario;

                Random r = new Random();
                int number = r.Next(1000, 10000);

                QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
                QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(number.ToString(), QRCodeGenerator.ECCLevel.Q);
                QRCode qRCode = new QRCode(qRCodeData);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (Bitmap bitmap = qRCode.GetGraphic(20))
                    {
                        bitmap.Save(ms, ImageFormat.Png);
                        ViewBag.QRCodeImage = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                        ViewBag.numero = number.ToString();
                    }
                }
                DatabaseHelper.ExecStoreProcedure("SP_AgregarQR",
                    new List<SqlParameter>()
                    {
                        new SqlParameter("@idPersona", usuario!.idPersona),
                        new SqlParameter("@codigo", number),
                    }
                );

                return View();
            }

        }

        private List<Visitante> CargarVisitantes()
        {
            var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@idPersona", usuario!.idPersona)
            };
            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ObtenerVisitantesxPersona", param);

            List<Visitante> ListVisitantes = new List<Visitante>();

            foreach (DataRow row in ds.Rows)
            {
                ListVisitantes.Add(new Visitante()
                {
                    idVisitante = Convert.ToInt32(row["idVisitante"]),
                    nombre = row["nombre"].ToString(),
                    primerApellido = row["primerApellido"].ToString(),
                    segundoApellido = row["segundoApellido"].ToString(),
                }
                );
            }

            return ListVisitantes;
        }

        private List<Visitante> CargarVisitantesFav()
        {
            var usuario = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("usuario"));
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@idPersona", usuario!.idPersona)
            };
            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ObtenerVisitantesFavxPersona", param);

            List<Visitante> ListVisitantes = new List<Visitante>();

            foreach (DataRow row in ds.Rows)
            {
                ListVisitantes.Add(new Visitante()
                {
                    idVisitante = Convert.ToInt32(row["idVisitante"]),
                    nombre = row["nombre"].ToString(),
                    primerApellido = row["primerApellido"].ToString(),
                    segundoApellido = row["segundoApellido"].ToString(),
                }
                );
            }

            return ListVisitantes;
        }

        private List<TipoVisita> CargarTipoVisitas()
        {
            DataTable ds = DatabaseHelper.ExecuteStoreProcedure("SP_ObtenerTiposVisitas", null);

            List<TipoVisita> TipoVisitaList = new List<TipoVisita>();

            foreach (DataRow row in ds.Rows)
            {
                TipoVisitaList.Add(new TipoVisita()
                {
                    idTipoVisita = Convert.ToInt32(row["idTipoVisita"]),
                    nombreTipo = row["nombreTipo"].ToString(),
                }
                );
            }

            return TipoVisitaList;
        }

        public ActionResult EliminarVisita(int idVisita)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                 new SqlParameter("@idVisita", idVisita)
            };

            DatabaseHelper.ExecStoreProcedure("SP_EliminarVisita", param);

            return RedirectToAction("Index", "Visitas");
        }
    }
}
