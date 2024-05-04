<?php
session_start();

function validarSesion() {
    if(isset($_SESSION['token'])) {
        
        return true;
    } else {
        return false;
    }
}

if(!validarSesion()) {
    header("Location: ../login.php");
    exit;
}
if(validarSesion()) {
    $token = $_SESSION['token'];
}
?>