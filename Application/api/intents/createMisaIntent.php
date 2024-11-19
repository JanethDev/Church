<?php
require_once('../../init.php');
require_once('auth/session.php');
require 'vendor/autoload.php';

use GuzzleHttp\Client;
use GuzzleHttp\Exception\RequestException;

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $phone = $_POST['CelPhone'] ?? null;
    $phoneFormatted = preg_replace('/\D/', '', $phone);
    $dataIntent = [
        'intent_id' => (int) ($_POST['createCatIntents'] ?? 0),
        'misa_id' => (int) ($_POST['createCatMisas'] ?? 0),
        'date' => $_POST['createDateReq'] ?? null,
        'mention_person' => $_POST['persona'] ?? null,
        'applicant' => $_POST['solicitante'] ?? null,
        'phone' => $phoneFormatted, // Mantener como string
        'donation' => (float) ($_POST['donativo'] ?? 0),
        'exchange_rate' => (float) obtenerTipoCambio(), // Forzar a float
        'description' => $_POST['Description'] ?? null,
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
        $response = $client->request('POST', 'create/misa_intents', [
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

function obtenerTipoCambio() {
    $token = '0f3d70f1cf4fd8f916a9d7736a2882a26ba374273d4defcb118a73e0336fea01';
    $url = 'https://www.banxico.org.mx/SieAPIRest/service/v1/series/SF43718/datos/oportuno?token=' . $token;

    $ch = curl_init();
    curl_setopt($ch, CURLOPT_URL, $url);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);

    $response = curl_exec($ch);

    if (curl_errno($ch)) {
        curl_close($ch);
        return 'Error al conectar con la API';
    }

    curl_close($ch);

    $data = json_decode($response, true);

    if (isset($data['bmx']['series'][0]['datos'][0]['dato'])) {
        return $data['bmx']['series'][0]['datos'][0]['dato'];
    } else {
        return 'Error al obtener el tipo de cambio';
    }
}
