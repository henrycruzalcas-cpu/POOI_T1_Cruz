using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using POOI_T2_CruzAlcas.Models;

namespace POOI_T2_CruzAlcas.Services
{
    public enum ResultadoOperacion { Correcto, Duplicado, NoEncontrado }

    public sealed class AlumnoJsonStore
    {
        // La colección se mantiene como texto JSON, según el enunciado.
        private string lista = @"[]";
        private static readonly object bloqueo = new object();
        private readonly string ruta;

        public AlumnoJsonStore(string rutaArchivo)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo))
                throw new ArgumentException("Se requiere una ruta de archivo.", "rutaArchivo");
            ruta = Path.GetFullPath(rutaArchivo);
        }

        public Alumno[] Listar()
        {
            lock (bloqueo) { return Deserializar(); }
        }

        public Alumno Buscar(string dni)
        {
            lock (bloqueo) { return Deserializar().FirstOrDefault(a => a.dni == dni); }
        }

        public ResultadoOperacion Agregar(Alumno alumno)
        {
            Validar(alumno);
            lock (bloqueo)
            {
                var alumnos = Deserializar();
                if (alumnos.Any(a => a.dni == alumno.dni)) return ResultadoOperacion.Duplicado;
                // Arreglo temporal para la operación; no se almacena una List<Alumno>.
                Serializar(alumnos.Concat(new[] { alumno }).ToArray());
                return ResultadoOperacion.Correcto;
            }
        }

        public ResultadoOperacion Actualizar(string dniOriginal, Alumno alumno)
        {
            Validar(alumno);
            lock (bloqueo)
            {
                var alumnos = Deserializar();
                int indice = Array.FindIndex(alumnos, a => a.dni == dniOriginal);
                if (indice < 0) return ResultadoOperacion.NoEncontrado;
                if (alumnos.Any(a => a.dni == alumno.dni && a.dni != dniOriginal))
                    return ResultadoOperacion.Duplicado;
                alumnos[indice] = alumno;
                Serializar(alumnos);
                return ResultadoOperacion.Correcto;
            }
        }

        public ResultadoOperacion Eliminar(string dni)
        {
            lock (bloqueo)
            {
                var alumnos = Deserializar();
                if (!alumnos.Any(a => a.dni == dni)) return ResultadoOperacion.NoEncontrado;
                Serializar(alumnos.Where(a => a.dni != dni).ToArray());
                return ResultadoOperacion.Correcto;
            }
        }

        public void Guardar()
        {
            lock (bloqueo) { Serializar(Deserializar()); }
        }

        private Alumno[] Deserializar()
        {
            lista = File.Exists(ruta) ? File.ReadAllText(ruta, Encoding.UTF8) : @"[]";
            var alumnos = JsonConvert.DeserializeObject<Alumno[]>(lista);
            if (alumnos == null || alumnos.Any(a => a == null))
                throw new InvalidDataException("El archivo debe contener un arreglo de alumnos.");
            // Si el archivo fue alterado o está dañado, no se sobrescribe.
            foreach (var alumno in alumnos) Validar(alumno);
            if (alumnos.Select(a => a.dni).Distinct().Count() != alumnos.Length)
                throw new InvalidDataException("El archivo contiene DNI duplicados.");
            return alumnos;
        }

        private static void Validar(Alumno alumno)
        {
            if (alumno == null) throw new ArgumentNullException("alumno");
            Validator.ValidateObject(alumno, new ValidationContext(alumno), true);
        }

        private void Serializar(Alumno[] alumnos)
        {
            string nuevoJson = JsonConvert.SerializeObject(alumnos, Formatting.Indented);
            Directory.CreateDirectory(Path.GetDirectoryName(ruta));
            string temporal = ruta + "." + Guid.NewGuid().ToString("N") + ".tmp";
            try
            {
                File.WriteAllText(temporal, nuevoJson, new UTF8Encoding(false));
                if (File.Exists(ruta)) File.Replace(temporal, ruta, null);
                else File.Move(temporal, ruta);
                lista = nuevoJson;
            }
            finally
            {
                if (File.Exists(temporal)) File.Delete(temporal);
            }
        }
    }
}
