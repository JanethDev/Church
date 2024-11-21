<?php
require_once('../../init.php');
require_once('auth/session.php');
require 'vendor/autoload.php';

use GuzzleHttp\Client;
use GuzzleHttp\Exception\RequestException;


if ($_SERVER['REQUEST_METHOD'] === 'POST') {

    $dataIntent = [
        'id' => (int) ($_POST['editId'] ?? 0),
        'intent_id' => (int) ($_POST['editCatIntents'] ?? 0),
        'misa_id' => (int) ($_POST['editCatMisas'] ?? 0),
        'date' => $_POST['editDate'] ?? null,
        'mention_person' => $_POST['editPersona'] ?? null,
        'applicant' => $_POST['editSolicitante'] ?? null,
        'phone' => $_POST['editPhone'] ?? null, // Mantener como string
        'donation' => (float) ($_POST['editDonation'] ?? 0),
        'exchange_rate' => (float) ($_POST['editRate'] ?? 0),// Forzar a float
        'decription' => $_POST['editDescription'] ?? null,
    ];

    // Elimina valores nulos
    $dataIntent = array_filter($dataIntent, fn($value) => $value !== null);

    // Verifica el JSON
    $jsonData = json_encode($dataIntent, JSON_PRETTY_PRINT);

    // Muestra el JSON para depuración
    echo $jsonData;

    // Configuración de Guzzle
    $headers = [
        'Authorization' => 'Bearer ' . $token,
        'Content-Type' => 'application/json',
    ];

    $client = new Client(['base_uri' => API_SERVICES]);

    try {
        $response = $client->request('POST', 'update/misa_intents', [
            'headers' => $headers,
            'body' => $jsonData,
        ]);

        $body = $response->getBody()->getContents();
        header('Content-Type: application/json');
        //echo $body;

    } catch (RequestException $e) {
        if ($e->hasResponse()) {
            $errorBody = $e->getResponse()->getBody()->getContents();
            echo json_encode(["error" => "Respuesta de la API: " . $errorBody]);
        } else {
            echo json_encode(["error" => "Error al realizar la solicitud: " . $e->getMessage()]);
        }
    }
} else {
    echo json_encode(["error" => "Método no permitido."]);
}
?>
