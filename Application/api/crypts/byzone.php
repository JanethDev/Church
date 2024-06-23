<?php
require_once('../../init.php');
require_once('auth/session.php');
require 'vendor/autoload.php'; 

use GuzzleHttp\Client;
use GuzzleHttp\Exception\RequestException;

if ($_SERVER["REQUEST_METHOD"] != "POST") {
    $zone = "ah07u01";
  
    $headers = [
        'Authorization' => 'Bearer '.$token,
    ];
    

    $client = new Client([
        'base_uri' => API_SERVICES,  
        'headers' => $headers, 
    ]);

    try {
       
        $response = $client->request('GET', 'crypts/byzone', [
            'query' => [
                'zone' => $zone, 
            ],
        ]);

      
        $body = $response->getBody()->getContents();

   
        echo "Respuesta de la API: " . $body;


    } catch (RequestException $e) {
  
        if ($e->hasResponse()) {
            $errorBody = $e->getResponse()->getBody()->getContents();
            echo "Error en la solicitud: " . $errorBody;
        } else {
            echo "Error en la solicitud: " . $e->getMessage();
        }
    }
}
?>
