using Microsoft.AspNetCore.Mvc;
using POOI_T1_Cruz.Models;

namespace POOI_T1_Cruz.Controllers
{
    public class AdministrativoController : Controller
    {
        [HttpGet]
        public IActionResult RegistrarAdministrativo()
        {
            return View(new Administrativo());
        }

        [HttpPost]
        public IActionResult RegistrarAdministrativo(Administrativo administrativo)
        {
            return View(administrativo);
        }
    }
}
