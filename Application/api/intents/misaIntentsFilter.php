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

    $dateReq = isset($_GET['dateReq']) ? $_GET['dateReq'] : date('Y-m-d');
    $catIntents = isset($_GET['catIntents']) ? $_GET['catIntents'] : null;
    $timeSelect = isset($_GET['timeSelect']) ? $_GET['timeSelect'] : null;

    $filteredData = array_filter($data, function($item) use ($dateReq, $catIntents, $timeSelect) {
        error_log("Filtrando item: " . print_r($item, true));
        error_log("dateReq: $dateReq, catIntents: $catIntents, timeSelect: $timeSelect");
    
        // Normalizar las fechas para comparar solo la parte de fecha
        $dateMatch = isset($item['date']) && date('Y-m-d', strtotime($item['date'])) === $dateReq;
    
        // Filtrar por categoría
        $categoryMatch = !$catIntents || (isset($item['intent_id']) && $item['intent_id'] == $catIntents);
    
        // Filtrar por hora
        $hourMatch = !$timeSelect || (isset($item['misa_hour']) && $item['misa_hour'] === $timeSelect);
    
        return $dateMatch && $categoryMatch && $hourMatch;
    });

    echo json_encode([
        "data" => array_values($filteredData)
    ]);

} catch (RequestException $e) {
    if ($e->hasResponse()) {
        $errorBody = $e->getResponse()->getBody()->getContents();
        echo json_encode(["error" => "Error en la solicitud: " . $errorBody]);
    } else {
        echo json_encode(["error" => "Error en la solicitud: " . $e->getMessage()]);
    }
}
?>
