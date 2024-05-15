<?php
require_once("api/access/user_data.php");
session_start();

function validarSesion() {
    return isset($_SESSION['token']);
}

if(!validarSesion()) {
    header("Location: ../login.php");
    exit;
}

$token = $_SESSION['token'];

try {
    $result = UserDataRequest::encode($token);

    if ($result !== null && is_array($result)) {
        $role_id = $result['role_id'];
        $name = $result['name'];
        $email = $result['email'];
        $role = $result['role'];
        $user_id = $result['user_id'];
    } else {
        echo "Error: No se pudo obtener la información del token.";
    }
} catch (Exception $e) {
    echo "Error: " . $e->getMessage();
}
?>