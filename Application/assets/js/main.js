function cerrarSesion() {
    // Realiza una solicitud AJAX al script de logout en el servidor
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "auth/logout.php", true);
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            // Redirige a la página de inicio de sesión una vez que se ha cerrado la sesión correctamente
            window.location.href = "../login.php";
        }
    };
    xhr.send();
}