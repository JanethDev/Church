<?php
require_once('../../init.php');
require_once('auth/session.php');
require 'vendor/autoload.php';

use GuzzleHttp\Client;
use GuzzleHttp\Exception\RequestException;

$headers = [
    'Authorization' => 'Bearer ' . $token,
];

$client = new Client([
    'base_uri' => API_SERVICES,
    'headers' => $headers,
]);

try {
    // Realiza la solicitud GET a la API para obtener estados y municipios
    $response = $client->request('GET', 'state_towns');

    // Obtiene el contenido de la respuesta de la API
    $body = $response->getBody()->getContents();

    // Establece el tipo de contenido y devuelve la respuesta
    header('Content-Type: application/json');
    echo $body;

} catch (RequestException $e) {
    // Maneja errores de solicitud Guzzle
    if ($e->hasResponse()) {
        $errorBody = $e->getResponse()->getBody()->getContents();
        http_response_code($e->getResponse()->getStatusCode());
        echo json_encode(["error" => "Error en la solicitud: " . $errorBody]);
    } else {
        http_response_code(500);
        echo json_encode(["error" => "Error en la solicitud: " . $e->getMessage()]);
    }
}
?>
