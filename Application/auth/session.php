<?php
require_once("api/access/user_data.php");
session_start();

function validarSesion() {
    return isset($_SESSION['token']);
}

function getSessionToken() {
    return $_SESSION['token'] ?? null;
}

function setSessionToken($token) {
    $_SESSION['token'] = $token;
}

if (!validarSesion()) {
    header("Location: ../login.php");
    exit;
}

$token = getSessionToken();

try {
    $result = UserDataRequest::encode($token);

    if ($result !== null && is_array($result)) {
        $role_id = $result['role_id'];
        $name = $result['name'];
        $email = $result['email'];
        $role = $result['role'];
        $user_id = $result['user_id'];
    } else {
        throw new Exception("No se pudo obtener la información del token.");
    }
} catch (Exception $e) {
    echo "Error: " . $e->getMessage();
}
?>
