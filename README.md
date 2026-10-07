# POOI T2 — Gestión de alumnos

**Alumno:** Henry Santiago Cruz Alcas  
**Curso:** Programación Orientada a Objetos I — Cibertec  
**Proyecto:** POOI_T2_CruzAlcas

Aplicación C# con **ASP.NET MVC 5 sobre .NET Framework 4.8**. Permite listar, agregar, ver detalles, actualizar y eliminar alumnos. Los datos se serializan en un archivo JSON.

## Abrir y ejecutar

1. En Windows, usar Visual Studio con la carga **Desarrollo de ASP.NET y web**, IIS Express y el paquete de destino de .NET Framework 4.8.
2. Extraer **todo** el ZIP en una carpeta local.
3. Abrir `POOI_T2_CruzAlcas.sln`.
4. Si se obtiene desde GitHub, hacer clic derecho en la solución y seleccionar **Restaurar paquetes NuGet**. El ZIP incluye la carpeta `packages` para facilitar la apertura.
5. Establecer `POOI_T2_CruzAlcas` como proyecto de inicio y presionar **Ctrl+F5** (IIS Express).
6. Se abrirá el listado de alumnos. La ruta inicial es `/Alumno/Index`.

No se necesita SQL Server. La carpeta `App_Data` debe permitir escritura al usuario que ejecuta la aplicación.

## Cumplimiento del ejercicio

| Requisito | Implementación |
| --- | --- |
| Clase y constructor de Alumno | `Models/Alumno.cs`: dni, nombres, apellidos, carrera y ciclo |
| Cadena JSON inicial vacía | `Services/AlumnoJsonStore.cs`: `private string lista = @"[]";` |
| Index | Listado de la colección y estado vacío |
| Agregar | Registro con validación del DNI único |
| Eliminar | Eliminación por DNI y redirección a Index |
| Serializar | Botón Guardar JSON y guardado automático al agregar, actualizar o eliminar |
| Mensaje por 5 segundos | `TempData` y `Scripts/site.js` con 5000 ms |
| Detalles | Ficha completa del alumno seleccionado |
| Actualizar | Modifica el alumno original, permite corregir su DNI y evita duplicados |
| Vistas | Index, Agregar, Detalles y Actualizar; formulario parcial compartido |

La fuente persistente es `POOI_T2_CruzAlcas/App_Data/alumnos.json`, entregada con `[]`. Cada operación lee el JSON y lo deserializa a un **arreglo temporal**; no se utiliza una `List<Alumno>` como almacenamiento. Tras una modificación se serializa nuevamente y se reemplaza el archivo. Los registros permanecen al reiniciar la aplicación.

Para Actualizar se interpreta la operación como la modificación de un alumno existente; no se agrega otro registro. La validación de duplicados excluye al alumno que se está editando.

## Validaciones y protección de los datos

- DNI de ocho dígitos, conservado como texto para admitir ceros iniciales.
- Nombres, apellidos y carrera obligatorios; ciclo entero entre 1 y 10.
- Cambios mediante POST y token antifalsificación.
- Confirmación antes de eliminar.
- Escritura temporal y sustitución del JSON para reducir el riesgo de escritura parcial.
- Un archivo inválido no se reemplaza por una colección vacía silenciosamente.

## Comprobación rápida

1. Abrir el listado vacío y agregar un alumno de prueba.
2. Intentar repetir su DNI: debe rechazarse.
3. Consultar Detalles y modificar nombres/carrera con el mismo DNI: debe guardarse.
4. Agregar otro alumno e intentar asignarle el DNI del primero: debe rechazarse.
5. Reiniciar la aplicación: ambos registros deben mantenerse.
6. Eliminar un registro: debe desaparecer del listado y del archivo.
7. Pulsar Guardar JSON: el mensaje desaparece a los cinco segundos.

Los datos usados en las pruebas no están incluidos en la colección inicial.

## Entrega

Enlace de la T2: https://github.com/henrycruzalcas-cpu/POOI_T1_Cruz/tree/entrega-t2

La T2 está en la rama `entrega-t2`; la rama `main` conserva la T1. La alternativa de entrega es **Cruz.zip**. En Blackboard debe pegarse el enlace **completo**, incluida la rama, para abrir directamente esta tarea.
