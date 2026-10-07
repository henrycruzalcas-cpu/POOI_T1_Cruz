(function () {
    "use strict";
    var mensaje = document.getElementById("mensaje");
    if (mensaje) {
        window.setTimeout(function () { mensaje.remove(); }, 5000);
    }
    document.querySelectorAll(".delete-form").forEach(function (form) {
        form.addEventListener("submit", function (event) {
            if (!window.confirm("¿Deseas eliminar este alumno?")) event.preventDefault();
        });
    });
}());
