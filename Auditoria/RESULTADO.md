# Auditoría de la T2

Fecha: 6 de octubre de 2026 (Perú).

## Resultado

- Compilación de la solución en Windows con MSBuild y .NET Framework: correcta.
- Precompilación de todas las vistas Razor con aspnet_compiler: correcta.
- 22 comprobaciones de las operaciones JSON: correctas, también en Windows.
- Archivos XML de configuración y referencias del proyecto: correctos.
- Sintaxis JavaScript: correcta; temporizador del mensaje configurado en 5000 ms.
- Colección entregada: `[]`, sin datos de prueba.
- Archivo ZIP: integridad verificada; incluye solución, fuentes y paquetes NuGet.

Evidencia reproducible: [Verificación en GitHub Actions](https://github.com/henrycruzalcas-cpu/POOI_T1_Cruz/actions/runs/37557400039).

## Casos comprobados

Colección vacía, serialización vacía, alta, conservación de ceros del DNI, rechazo de duplicados sin modificar el archivo, persistencia al crear una nueva instancia, caracteres Unicode, edición con el mismo DNI, segundo registro, rechazo de DNI ajeno, cambio de DNI sin duplicar, actualización de un registro inexistente, búsqueda inexistente, eliminación persistente, eliminación inexistente, cuatro entradas inválidas, eliminación del último alumno, protección de JSON dañado, rechazo de `null` y detección de duplicados en un archivo alterado.

## Alcance

Se verificaron la compilación del proyecto y las vistas, y la ejecución de la lógica de datos. No se realizó una prueba manual interactiva del navegador ni una ejecución en la laptop del alumno. La entrega en Blackboard no fue realizada por estas pruebas. El README contiene las instrucciones de apertura y una comprobación manual breve.
