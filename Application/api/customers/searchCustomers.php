<?php
require_once('../../init.php');
require_once('auth/session.php');
require 'vendor/autoload.php';

use GuzzleHttp\Client;
use GuzzleHttp\Exception\RequestException;

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    if (isset($_POST['value']) && !empty($_POST['value'])) {
        // Recoge el valor del cliente buscado
        $customer = isset($_POST['value']) ? trim($_POST['value']) : '';

        // Prepara los headers de la solicitud con el token de autorización
        $headers = [
            'Authorization' => 'Bearer '.$token,
        ];

        // Instancia de Guzzle Client para hacer la solicitud a la API
        $client = new Client([
            'base_uri' => API_SERVICES,  // Debe ser la URL base de la API
            'headers' => $headers,
        ]);

        try {
            // Realiza la solicitud GET a la API para buscar al cliente
            $response = $client->request('GET', 'customer/search', [
                'query' => [
                    'value' => $customer, // Aquí va el parámetro de búsqueda
                ],
            ]);

            // Obtiene el contenido de la respuesta de la API
            $body = $response->getBody()->getContents();

            // Devuelve los datos como JSON
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
    } else {
        // Maneja caso en que 'Customer' no esté presente o esté vacío
        echo json_encode(["error" => "Parámetro 'Customer' no válido o no proporcionado."]);
    }
} else {
    // Maneja caso en que el método no sea POST
    echo json_encode(["error" => "Método no permitido."]);
}
?>
