function cerrarSesion() {
    
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "auth/logout.php", true);
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
          
            window.location.href = "../login.php";
        }
    };
    xhr.send();
}