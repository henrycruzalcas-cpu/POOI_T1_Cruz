$ErrorActionPreference = 'Stop'
Import-Module Microsoft.PowerShell.Utility
$root = Split-Path $PSScriptRoot -Parent
$app = Join-Path $root 'POOI_T2_CruzAlcas'
$json = Join-Path $app 'App_Data\alumnos.json'
$original = [IO.File]::ReadAllText($json)
$base = 'http://localhost:51841'
$session = [Microsoft.PowerShell.Commands.WebRequestSession]::new()
$script:total = 0
function Check($condition, $name) {
    if (-not $condition) { throw "FALLO WEB: $name" }
    $script:total++
    Write-Host "OK WEB: $name"
}
function GetPage($path) {
    Invoke-WebRequest "$base$path" -WebSession $session -TimeoutSec 15 -SkipHttpErrorCheck
}
function PostForm($path, $body, $html) {
    $match = [regex]::Match($html, 'name="__RequestVerificationToken"[^>]*value="([^"]+)"')
    if (-not $match.Success) { throw 'No se encontro el token del formulario.' }
    $body['__RequestVerificationToken'] = [Net.WebUtility]::HtmlDecode($match.Groups[1].Value)
    Invoke-WebRequest "$base$path" -Method Post -Body $body -WebSession $session -TimeoutSec 15 -SkipHttpErrorCheck
}
function Students { @(Get-Content -Raw $json | ConvertFrom-Json) }
$exe = Join-Path $env:ProgramFiles 'IIS Express\iisexpress.exe'
if (-not (Test-Path $exe)) { $exe = Join-Path ${env:ProgramFiles(x86)} 'IIS Express\iisexpress.exe' }
if (-not (Test-Path $exe)) { throw 'IIS Express no esta instalado en el verificador.' }
$server = $null
try {
    [IO.File]::WriteAllText($json, '[]')
    $server = Start-Process $exe -ArgumentList @("/path:$app", '/port:51841', '/systray:false') -PassThru -RedirectStandardOutput "$env:RUNNER_TEMP\t2-iis.log" -RedirectStandardError "$env:RUNNER_TEMP\t2-iis-error.log"
    $page = $null
    for ($i = 0; $i -lt 30; $i++) {
        try { $page = GetPage '/Alumno/Index'; break } catch { Start-Sleep -Seconds 1 }
    }
    Check ($null -ne $page -and $page.StatusCode -eq 200 -and $page.Content.Contains('Agregar alumno')) 'Listado inicial responde correctamente'
    Check ((GetPage '/Content/site.css').StatusCode -eq 200 -and (GetPage '/Scripts/site.js').Content.Contains('5000')) 'Estilos y script del mensaje disponibles'
    $page = GetPage '/Alumno/Agregar'
    Check ($page.StatusCode -eq 200 -and $page.Content.Contains('name="dni"')) 'Formulario Agregar disponible'
    $body = @{ dni='00123456'; nombres='Auditoria Uno'; apellidos='Prueba'; carrera='Computacion'; ciclo='3' }
    $page = PostForm '/Alumno/Agregar' $body $page.Content
    Check ($page.StatusCode -eq 200 -and $page.Content.Contains('Auditoria Uno') -and (Students).Count -eq 1) 'Alta real, redireccion y archivo JSON'
    Check ($page.Content.Contains('id="mensaje"')) 'Mensaje de resultado visible tras guardar'
    $page = GetPage '/Alumno/Agregar'
    $page = PostForm '/Alumno/Agregar' $body $page.Content
    Check ($page.StatusCode -eq 200 -and $page.Content.Contains('Ya existe un alumno') -and (Students).Count -eq 1) 'Formulario rechaza DNI duplicado'
    $page = GetPage '/Alumno/Detalles?dni=00123456'
    Check ($page.StatusCode -eq 200 -and $page.Content.Contains('Auditoria Uno')) 'Detalles muestra alumno correcto'
    $page = GetPage '/Alumno/Actualizar?dni=00123456'
    Check ($page.StatusCode -eq 200 -and $page.Content.Contains('name="dniOriginal" value="00123456"')) 'Editar conserva identidad original'
    $body = @{ dniOriginal='00123456'; dni='00123456'; nombres='Auditoria Editada'; apellidos='Prueba'; carrera='Computacion'; ciclo='4' }
    $page = PostForm '/Alumno/Actualizar' $body $page.Content
    Check ($page.StatusCode -eq 200 -and $page.Content.Contains('Auditoria Editada') -and (Students)[0].ciclo -eq 4) 'Actualizacion real con el mismo DNI'
    $page = GetPage '/Alumno/Agregar'
    $page = PostForm '/Alumno/Agregar' @{dni='87654321';nombres='Segundo';apellidos='Alumno';carrera='Computacion';ciclo='2'} $page.Content
    Check ((Students).Count -eq 2) 'Segundo alumno registrado'
    $page = GetPage '/Alumno/Actualizar?dni=00123456'
    $body.dni = '87654321'
    $page = PostForm '/Alumno/Actualizar' $body $page.Content
    Check ($page.StatusCode -eq 200 -and $page.Content.Contains('Ese DNI pertenece') -and (Students).Count -eq 2) 'Edicion rechaza DNI de otro alumno'
    $page = GetPage '/Alumno/Agregar'
    $page = PostForm '/Alumno/Agregar' @{dni='123';nombres='';apellidos='';carrera='';ciclo='0'} $page.Content
    Check ($page.StatusCode -eq 200 -and $page.Content.Contains('field-validation-error') -and (Students).Count -eq 2) 'Validacion del servidor rechaza formulario invalido'
    $page = GetPage '/Alumno/Index'
    $page = PostForm '/Alumno/Serializar' @{} $page.Content
    Check ($page.StatusCode -eq 200 -and $page.Content.Contains('id="mensaje"') -and (Students).Count -eq 2) 'Boton Serializar guarda y muestra mensaje'
    $page = PostForm '/Alumno/Eliminar' @{dni='00123456'} $page.Content
    Check ($page.StatusCode -eq 200 -and -not $page.Content.Contains('Auditoria Editada') -and (Students).Count -eq 1) 'Eliminar real y redireccion'
    Check ((GetPage '/Alumno/Detalles?dni=00123456').StatusCode -eq 404) 'Alumno eliminado devuelve 404 en Detalles'
    $page = GetPage '/Alumno/Index'
    $page = PostForm '/Alumno/Eliminar' @{dni='87654321'} $page.Content
    Check (([IO.File]::ReadAllText($json).Trim()) -eq '[]') 'Eliminar ultimo alumno restaura arreglo vacio'
    Write-Host "RESULTADO WEB: $script:total comprobaciones correctas."
}
finally {
    if ($server -and -not $server.HasExited) { Stop-Process -Id $server.Id -Force }
    [IO.File]::WriteAllText($json, $original)
}
