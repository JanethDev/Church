<?php
require_once("api/access/userData.php");
session_start();

// Función para validar si hay una sesión activa
function validarSesion() {
    return isset($_SESSION['token']);
}

// Función para obtener el token de la sesión
function getSessionToken() {
    return $_SESSION['token'] ?? null;
}

// Función para establecer el token de la sesión
function setSessionToken($token) {
    $_SESSION['token'] = $token;
}

// Redirigir al login si no hay sesión activa
if (!validarSesion()) {
    header("Location: ../login.php");
    exit;
}

$token = getSessionToken();

try {
    // Obtener la información del usuario desde el token
    $result = UserDataRequest::encode($token);

    // Verificar si el resultado es válido
    if ($result !== null && is_array($result)) {
        $role_id = $result['role_id'];
        $name = $result['name'];
        $email = $result['email'];
        $role = $result['role'];
        $user_id = $result['user_id'];
    } else {
        // Si no se pudo obtener la información del token, redirigir al login
        throw new Exception("No se pudo obtener la información del token.");
    }
} catch (Exception $e) {
    // En caso de error, eliminar el token de la sesión y redirigir al login
    unset($_SESSION['token']);
    header("Location: ../login.php");
    exit;
}
?>
