using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using POOI_T2_CruzAlcas.Models;
using POOI_T2_CruzAlcas.Services;

public static class PruebasJson
{
    private static int total;
    private static void Verificar(bool cumple, string caso)
    {
        if (!cumple) throw new Exception("FALLO: " + caso);
        total++;
        Console.WriteLine("OK: " + caso);
    }

    private static Alumno Crear(string dni, string nombres = "Lucía")
    {
        return new Alumno(dni, nombres, "Pérez Ñúñez", "Computación e Informática", 3);
    }

    public static int Main()
    {
        string carpeta = Path.Combine(Path.GetTempPath(), "t2-auditoria-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(carpeta);
        string ruta = Path.Combine(carpeta, "alumnos.json");
        try
        {
            var datos = new AlumnoJsonStore(ruta);
            Verificar(datos.Listar().Length == 0, "Colección inicialmente vacía");
            datos.Guardar();
            Verificar(File.ReadAllText(ruta).Trim() == "[]", "Serializar colección vacía");
            Verificar(datos.Agregar(Crear("00123456")) == ResultadoOperacion.Correcto, "Agregar alumno");
            Verificar(datos.Buscar("00123456").dni == "00123456", "DNI conserva ceros iniciales");
            string original = File.ReadAllText(ruta);
            Verificar(datos.Agregar(Crear("00123456")) == ResultadoOperacion.Duplicado && File.ReadAllText(ruta) == original, "Duplicado no modifica el archivo");
            Verificar(new AlumnoJsonStore(ruta).Buscar("00123456").apellidos == "Pérez Ñúñez", "Persistencia y caracteres Unicode");
            Verificar(datos.Actualizar("00123456", Crear("00123456", "María")) == ResultadoOperacion.Correcto && datos.Buscar("00123456").nombres == "María", "Actualizar conservando DNI");
            Verificar(datos.Agregar(Crear("87654321")) == ResultadoOperacion.Correcto, "Agregar segundo alumno");
            original = File.ReadAllText(ruta);
            Verificar(datos.Actualizar("00123456", Crear("87654321")) == ResultadoOperacion.Duplicado && File.ReadAllText(ruta) == original, "Actualizar rechaza DNI ajeno");
            Verificar(datos.Actualizar("00123456", Crear("11223344")) == ResultadoOperacion.Correcto && datos.Buscar("00123456") == null && datos.Listar().Length == 2, "Cambiar DNI sin duplicar registro");
            Verificar(datos.Actualizar("00000000", Crear("22334455")) == ResultadoOperacion.NoEncontrado && datos.Listar().Length == 2, "Actualizar inexistente no agrega");
            Verificar(datos.Buscar("00000000") == null, "Buscar inexistente");
            Verificar(datos.Eliminar("11223344") == ResultadoOperacion.Correcto && new AlumnoJsonStore(ruta).Buscar("11223344") == null, "Eliminar y persistir");
            Verificar(datos.Eliminar("11223344") == ResultadoOperacion.NoEncontrado, "Eliminar inexistente");
            foreach (var invalido in new[] { Crear("123"), Crear("ABCDEFGH"), Crear("12345678", "   "), new Alumno("12345678", "A", "B", "C", 0) })
            {
                bool rechazado = false;
                try { datos.Agregar(invalido); } catch (ValidationException) { rechazado = true; }
                Verificar(rechazado, "Validación de datos incorrectos");
            }
            datos.Eliminar("87654321");
            Verificar(datos.Listar().Length == 0 && File.ReadAllText(ruta).Trim() == "[]", "Eliminar último alumno deja arreglo vacío");
            File.WriteAllText(ruta, "{archivo inválido");
            bool protegido = false;
            try { datos.Agregar(Crear("12345678")); } catch (JsonException) { protegido = true; }
            Verificar(protegido && File.ReadAllText(ruta) == "{archivo inválido", "No sobrescribe JSON dañado");
            File.WriteAllText(ruta, "null");
            protegido = false;
            try { datos.Listar(); } catch (InvalidDataException) { protegido = true; }
            Verificar(protegido, "Rechaza null como colección");
            File.WriteAllText(ruta, JsonConvert.SerializeObject(new[] { Crear("12345678"), Crear("12345678") }));
            protegido = false;
            try { datos.Guardar(); } catch (InvalidDataException) { protegido = true; }
            Verificar(protegido, "Detecta duplicados en archivo alterado");
            Console.WriteLine("RESULTADO: " + total + " comprobaciones correctas.");
            return 0;
        }
        catch (Exception error) { Console.Error.WriteLine(error); return 1; }
        finally { Directory.Delete(carpeta, true); }
    }
}
