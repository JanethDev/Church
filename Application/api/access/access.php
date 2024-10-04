<?php
require_once '../../init.php';
require 'vendor/autoload.php'; 

session_start();

use GuzzleHttp\Client;
use GuzzleHttp\Exception\RequestException;

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Obtener datos del formulario
    $email = $_POST["email"];
    $password = $_POST["password"];

    $data = array("email" => $email, "password" => $password);
    $url = API_SERVICES . 'access/login?' . http_build_query($data);

    $client = new Client();

    try {
        $response = $client->get($url);
        $http_code = $response->getStatusCode();
        $responseBody = $response->getBody()->getContents();

        if ($http_code == 200 && !empty($responseBody)) {
            // Inicio de sesión exitoso
            //sleep(2);
            $_SESSION['token'] = $responseBody;
            $code = 1;
            $mensaje = "";
        } else {
            // Error de credenciales incorrectas o error desconocido
            $code = 2;
            $mensaje = "Error: " . ($http_code == 400 ? $responseBody : "Código de estado HTTP: " . $http_code);
        }
    } catch (RequestException $e) {
        // Manejar errores en la solicitud
        $mensaje = $e->getMessage();
        $code = 2;
        $responseBody = null;
    }

    // Preparar la respuesta JSON
    $json_obj = array("caso" => $code, "token" => $responseBody ?? null, "mensaje" => $mensaje);
    echo json_encode($json_obj, JSON_PRETTY_PRINT);
}
