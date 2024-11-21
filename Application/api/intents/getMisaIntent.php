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

$id = isset($_GET['idIntent']) ? $_GET['idIntent'] : null; // ID a filtrar

try {
    // Realiza la solicitud GET a la API para obtener catálogo de intenciones
    $response = $client->request('GET', 'misa_intents/all');

    // Obtiene el contenido de la respuesta de la API
    $body = $response->getBody()->getContents();

    // Decodifica el JSON en un array asociativo
    $data = json_decode($body, true);

    if ($id !== null) {
        // Filtra los resultados por idIntent
        $filteredData = array_filter($data, function ($item) use ($id) {
            return isset($item['id']) && $item['id'] == $id; // Filtrar por idIntent
        });

        // Re-indexa el array filtrado
        $filteredData = array_values($filteredData);

        // Devuelve solo los datos filtrados
        header('Content-Type: application/json');
        echo json_encode($filteredData);
    } else {
        // Si no se proporciona un idIntent, devuelve todos los datos
        header('Content-Type: application/json');
        echo json_encode($data);
    }
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
