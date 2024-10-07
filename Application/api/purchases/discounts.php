<?php
require_once('../../init.php');
require_once('auth/session.php');
require 'vendor/autoload.php';

use GuzzleHttp\Client;
use GuzzleHttp\Exception\RequestException;

$headers = [
    'Authorization' => 'Bearer '.$token,
];

$client = new Client([
    'base_uri' => API_SERVICES,
    'headers' => $headers,
]);

try {
    // Realiza la solicitud GET a la API con la zona especificada
    $response = $client->request('GET', 'discounts/active');

    // Obtiene el contenido de la respuesta de la API
    $body = $response->getBody()->getContents();

    header('Content-Type: application/json');
    echo $body;

} catch (RequestException $e) {
    // Maneja errores de solicitud Guzzle
    if ($e->hasResponse()) {
        $errorBody = $e->getResponse()->getBody()->getContents();
        echo json_encode(["error" => "Error en la solicitud: " . $errorBody]);
    } else {
        echo json_encode(["error" => "Error en la solicitud: " . $e->getMessage()]);
    }
}
    
?>
