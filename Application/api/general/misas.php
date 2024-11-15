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
    // Obtener el ID del día enviado desde el frontend
    $day_id = isset($_GET['day_id']) ? $_GET['day_id'] : null;

    // Realiza la solicitud GET a la API de horas
    $response = $client->request('GET', 'misas');
    $body = $response->getBody()->getContents();
    $data = json_decode($body, true);

    // Verificar si los datos fueron decodificados correctamente
    if ($data === null && json_last_error() !== JSON_ERROR_NONE) {
        throw new Exception("Error de decodificación JSON: " . json_last_error_msg());
    }

    // Filtrar las horas por el día si se envió el parámetro
    if ($day_id) {
        $data = array_filter($data, function($item) use ($day_id) {
            return $item['day_id'] == $day_id;
        });
    }

    // Envuelve los datos en el formato JSON esperado
    echo json_encode(array_values($data)); // Reindexa el array

} catch (RequestException $e) {
    if ($e->hasResponse()) {
        $errorBody = $e->getResponse()->getBody()->getContents();
        echo json_encode(["error" => "Error en la solicitud: " . $errorBody]);
    } else {
        echo json_encode(["error" => "Error en la solicitud: " . $e->getMessage()]);
    }
} catch (Exception $e) {
    // Maneja errores adicionales, como problemas de decodificación JSON
    echo json_encode(["error" => "Excepción capturada: " . $e->getMessage()]);
}
