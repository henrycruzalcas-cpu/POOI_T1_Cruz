using Microsoft.AspNetCore.Mvc;
using POOI_T1_Cruz.Models;

namespace POOI_T1_Cruz.Controllers
{
    public class EmpleadoController : Controller
    {
        [HttpGet]
        public IActionResult RegistrarEmpleado()
        {
            return View(new Empleado());
        }
        [HttpPost]
        public IActionResult RegistrarEmpleado(Empleado empleado)
        {
            return View(empleado);
        }
    }
}
