<?php
require_once('../../init.php');
require_once('auth/session.php');
require 'vendor/autoload.php';
date_default_timezone_set('America/Tijuana');

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
    $response = $client->request('GET', 'misa_intents/all');
    $body = $response->getBody()->getContents();
    $data = json_decode($body, true);
   
    if (json_last_error() === JSON_ERROR_NONE) {
        header('Content-Type: application/json');
        echo json_encode($data); // Usamos $data para asegurarnos que es JSON válido
    } else {
        echo json_encode(["error" => "La respuesta de la API no es un JSON válido"]);
    }
} catch (RequestException $e) {
    if ($e->hasResponse()) {
        $errorBody = $e->getResponse()->getBody()->getContents();
        echo json_encode(["error" => "Error en la solicitud: " . $errorBody]);
    } else {
        echo json_encode(["error" => "Error en la solicitud: " . $e->getMessage()]);
    }
}
?>
