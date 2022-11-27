using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace proyectoDB2_condominios.Controllers
{
    public class VisitasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
