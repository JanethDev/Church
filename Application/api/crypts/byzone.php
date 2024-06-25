<?php
require_once('../../init.php');
require_once('auth/session.php');
require 'vendor/autoload.php';

use GuzzleHttp\Client;
use GuzzleHttp\Exception\RequestException;

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    if (isset($_POST['href']) && !empty($_POST['href'])) {
        $href = isset($_POST['href']) ? trim($_POST['href']) : '';
        $zone = str_replace('#', '', $href); 
        $headers = [
            'Authorization' => 'Bearer '.$token,
        ];

        $client = new Client([
            'base_uri' => API_SERVICES,
            'headers' => $headers,
        ]);

        try {
            // Realiza la solicitud GET a la API con la zona especificada
            $response = $client->request('GET', 'crypts/byzone', [
                'query' => [
                    'zone' => $zone,
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
        // Maneja caso en que 'href' no esté presente o esté vacío
        echo json_encode(["error" => "Parámetro 'href' no válido o no proporcionado."]);
    }
} else {
    // Maneja caso en que el método no sea POST
    echo json_encode(["error" => "Método no permitido."]);
}
?>
