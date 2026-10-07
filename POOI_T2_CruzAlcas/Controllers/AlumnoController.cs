using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;
using POOI_T2_CruzAlcas.Models;
using POOI_T2_CruzAlcas.Services;

namespace POOI_T2_CruzAlcas.Controllers
{
    public class AlumnoController : Controller
    {
        private AlumnoJsonStore almacen;
        private AlumnoJsonStore Almacen
        {
            get { return almacen ?? (almacen = new AlumnoJsonStore(Server.MapPath("~/App_Data/alumnos.json"))); }
        }

        [HttpGet]
        public ActionResult Index() { return View(Almacen.Listar()); }

        [HttpGet]
        public ActionResult Agregar() { return View(new Alumno { ciclo = 1 }); }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Agregar(Alumno alumno)
        {
            if (!ModelState.IsValid) return View(alumno);
            var nuevo = new Alumno(alumno.dni, alumno.nombres.Trim(), alumno.apellidos.Trim(), alumno.carrera.Trim(), alumno.ciclo);
            if (Almacen.Agregar(nuevo) == ResultadoOperacion.Duplicado)
            {
                ModelState.AddModelError("dni", "Ya existe un alumno con ese DNI.");
                return View(alumno);
            }
            TempData["Mensaje"] = "Alumno agregado y guardado en JSON correctamente.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Detalles(string dni)
        {
            var alumno = Almacen.Buscar(dni);
            if (alumno == null) return HttpNotFound("No se encontró al alumno.");
            return View(alumno);
        }

        [HttpGet]
        public ActionResult Actualizar(string dni)
        {
            var alumno = Almacen.Buscar(dni);
            if (alumno == null) return HttpNotFound("No se encontró al alumno.");
            ViewBag.DniOriginal = alumno.dni;
            return View(alumno);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Actualizar(string dniOriginal, Alumno alumno)
        {
            ViewBag.DniOriginal = dniOriginal;
            if (string.IsNullOrWhiteSpace(dniOriginal)) return new HttpStatusCodeResult(400);
            if (!ModelState.IsValid) return View(alumno);
            var modificado = new Alumno(alumno.dni, alumno.nombres.Trim(), alumno.apellidos.Trim(), alumno.carrera.Trim(), alumno.ciclo);
            var resultado = Almacen.Actualizar(dniOriginal, modificado);
            if (resultado == ResultadoOperacion.NoEncontrado) return HttpNotFound("No se encontró al alumno.");
            if (resultado == ResultadoOperacion.Duplicado)
            {
                ModelState.AddModelError("dni", "Ese DNI pertenece a otro alumno.");
                return View(alumno);
            }
            TempData["Mensaje"] = "Alumno actualizado y guardado en JSON correctamente.";
            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Eliminar(string dni)
        {
            var resultado = Almacen.Eliminar(dni);
            TempData["Mensaje"] = resultado == ResultadoOperacion.Correcto
                ? "Alumno eliminado. El archivo JSON fue actualizado."
                : "El alumno ya no existe en la colección.";
            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Serializar()
        {
            Almacen.Guardar();
            TempData["Mensaje"] = "Colección serializada y guardada en alumnos.json correctamente.";
            return RedirectToAction("Index");
        }

        protected override void OnException(ExceptionContext contexto)
        {
            var error = contexto.Exception;
            if (error is IOException || error is UnauthorizedAccessException || error is JsonException || error is ValidationException)
            {
                Trace.TraceError(error.ToString());
                contexto.ExceptionHandled = true;
                Response.StatusCode = 500;
                Response.TrySkipIisCustomErrors = true;
                contexto.Result = new ViewResult { ViewName = "Error" };
            }
            else base.OnException(contexto);
        }
    }
}
