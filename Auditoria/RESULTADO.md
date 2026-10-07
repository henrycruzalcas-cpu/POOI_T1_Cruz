# Auditoría de la T2

Fecha: 6 de octubre de 2026 (Perú).

## Resultado

- Compilación de la solución en Windows con MSBuild y .NET Framework: correcta.
- Precompilación de todas las vistas Razor con aspnet_compiler: correcta.
- 22 comprobaciones de las operaciones JSON: correctas en Windows.
- 16 comprobaciones HTTP contra la aplicación ejecutada en IIS Express: correctas.
- Total: 38 comprobaciones funcionales correctas, además de la compilación y revisión de archivos.
- Archivos XML de configuración y referencias del proyecto: correctos.
- Sintaxis JavaScript: correcta; temporizador del mensaje configurado en 5000 ms.
- Colección entregada: `[]`, sin datos de prueba.
- Archivo ZIP: integridad verificada; incluye solución, fuentes y paquetes NuGet.

Evidencia reproducible: [Verificación en GitHub Actions](https://github.com/henrycruzalcas-cpu/POOI_T1_Cruz/actions/runs/37557927687).

## Casos comprobados

Colección vacía, serialización vacía, alta, conservación de ceros del DNI, rechazo de duplicados sin modificar el archivo, persistencia al crear una nueva instancia, caracteres Unicode, edición con el mismo DNI, segundo registro, rechazo de DNI ajeno, cambio de DNI sin duplicar, actualización de un registro inexistente, búsqueda inexistente, eliminación persistente, eliminación inexistente, cuatro entradas inválidas, eliminación del último alumno, protección de JSON dañado, rechazo de `null` y detección de duplicados en un archivo alterado.

## Alcance

Se verificaron la compilación del proyecto y las vistas, la lógica de datos y solicitudes HTTP reales a IIS Express: listado, recursos CSS/JS, formularios, token antifalsificación, alta, duplicados, detalles, actualización, errores de validación, serialización, mensajes, eliminación y respuesta 404. No se realizó una inspección visual manual en navegador ni una ejecución en la laptop del alumno. La entrega en Blackboard no fue realizada por estas pruebas.

La segunda auditoría no encontró errores en la aplicación ni requisitos faltantes frente a la rúbrica proporcionada. Se ajustó únicamente la inicialización del módulo HTTP del verificador. La calificación corresponde al docente; los resultados no constituyen garantía de una nota específica.

