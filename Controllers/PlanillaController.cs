using Microsoft.AspNetCore.Mvc;
using POOI_T1_Cruz.Models;

namespace POOI_T1_Cruz.Controllers
{
    public class PlanillaController : Controller
    {
        // Coleccion de demostracion para la evaluacion.
        private static readonly List<Empleado> Planilla = new List<Empleado>
        {
            new Empleado { idEmpleado = "E001", nomapeEmpleado = "Ana Torres", categoriaEmpleado = "E1", nHijos = 2, tipoContrato = "Indefinido" },
            new Empleado { idEmpleado = "E002", nomapeEmpleado = "Luis Perez", categoriaEmpleado = "E2", nHijos = 1, tipoContrato = "Contratado" },
            new Empleado { idEmpleado = "E003", nomapeEmpleado = "Maria Rojas", categoriaEmpleado = "E3", nHijos = 0, tipoContrato = "Indefinido" },
            new Empleado { idEmpleado = "E004", nomapeEmpleado = "Carlos Diaz", categoriaEmpleado = "Otra", nHijos = 3, tipoContrato = "Contratado" }
        };

        [HttpGet]
        public IActionResult RegistrarPlanilla()
        {
            ViewBag.Planilla = Planilla;
            return View(new Empleado());
        }

        [HttpPost]
        public IActionResult RegistrarPlanilla(Empleado empleado)
        {
            Planilla.Add(empleado);
            ViewBag.Planilla = Planilla;
            return View(new Empleado());
        }

        [HttpGet]
        public IActionResult Nuevo()
        {
            return RedirectToAction(nameof(RegistrarPlanilla));
        }
    }
}
