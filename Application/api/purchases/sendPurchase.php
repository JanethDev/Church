<?php
require_once('../../init.php');
require_once('auth/session.php');
require 'vendor/autoload.php';

use GuzzleHttp\Client;
use GuzzleHttp\Exception\RequestException;

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $clientId = isset($_POST['CustomerID']) ? (int)$_POST['CustomerID'] : null;
    $referencePersonPhone1 = $_POST['ReferenceCustomerPhone1'] ?? null;
    $referencePersonPhone2 = $_POST['ReferenceCustomerPhone2'] ?? null;
    $formattedPhone1 = preg_replace('/\D/', '', $referencePersonPhone1);
    $formattedPhone2 = preg_replace('/\D/', '', $referencePersonPhone2);

    $dataPurchase = [
        'tuition' => '001215',
        'cryptId' => isset($_POST['cryptId']) ? (int)$_POST['cryptId'] : null,
        'cryptSpaces' => isset($_POST['cryptSpaces']) ? (int)$_POST['cryptSpaces'] : null,
        'maintenanceFee' => isset($_POST['inMaintenance']) ? (float)$_POST['inMaintenance'] : null,
        'federalTax' => isset($_POST['federalTax']) ? (float)$_POST['federalTax'] : null,
        'discountId' => isset($_POST['discountId']) ? (int)$_POST['discountId'] : null,
        'ashDeposit' => isset($_POST['inAshDeposit']) ? (float)$_POST['inAshDeposit'] : null,
        'customerId' => $clientId,
        'monthlyPayments' => isset($_POST['paymentPlan']) ? (int)$_POST['paymentPlan'] : null,
        'referencePerson1' => $_POST['ReferenceCustomer1'] ?? null,
        'referencePersonPhone1' => $formattedPhone1,
        'referencePerson2' => $_POST['ReferenceCustomer2'] ?? null,
        'referencePersonPhone2' => $formattedPhone2,
    ];

    $payments = [];
    if (isset($_POST['payments']) && is_array($_POST['payments'])) {
        foreach ($_POST['payments'] as $payment) {
            $payments[] = [
                'paymentAmount' => isset($payment['paymentAmount']) ? (float)$payment['paymentAmount'] : null,
                'concept' => $payment['concept'] ?? null,
                'typePaymentId' => isset($payment['typePaymentId']) ? (int)$payment['typePaymentId'] : null,
                'currencyId' => isset($payment['currencyId']) ? (int)$payment['currencyId'] : null,
            ];
        }
    }
    $dataPurchase['payments'] = $payments;

    // Configuración de Guzzle
    $headers = [
        'Authorization' => 'Bearer ' . $token,
        'Content-Type' => 'application/json',
    ];

    $client = new Client(['base_uri' => API_SERVICES]);

    // Si el customerId es null, creamos el cliente
    if ($clientId === null) {
        $customerData = [
            'email' => $_POST['Email'] ?? '',
            'name' => $_POST['Name'] ?? '',
            'father_last_name' => $_POST['PSurname'] ?? '',
            'mother_last_name' => $_POST['MSurname'] ?? '',
            'phone' => $formattedPhone1,
            'rfc' => $_POST['RFCCURP'] ?? '',
            'zip_code' => $_POST['zip_code'] ?? 0,
            'address' => $_POST['address'] ?? '',
            'catStatesId' => $_POST['catStatesId'] ?? 0,
            'catTownsId' => $_POST['catTownsId'] ?? 0,
            'social_reason' => $_POST['social_reason'] ?? '',
            'birthdate' => $_POST['birthdate'] ?? '',
            'birth_place' => $_POST['birth_place'] ?? '',
            'civil_status' => $_POST['civil_status'] ?? '',
            'occupation' => $_POST['occupation'] ?? '',
            'business_name' => $_POST['business_name'] ?? '',
            'business_address' => $_POST['business_address'] ?? '',
            'business_city' => $_POST['business_city'] ?? '',
            'business_municipality' => $_POST['business_municipality'] ?? '',
            'business_state' => $_POST['business_state'] ?? '',
            'business_phone' => $_POST['business_phone'] ?? '',
            'business_ext' => $_POST['business_ext'] ?? '',
            'deputation' => $_POST['deputation'] ?? '',
            'average_income' => $_POST['average_income'] ?? 0,
        ];

        $jsonCustomerData = json_encode($customerData);

        try {
            // Realiza la solicitud POST para crear el cliente
            $response = $client->request('POST', 'customer/create', [
                'headers' => $headers,
                'body' => $jsonCustomerData,
            ]);

            // Obtén el ID del nuevo cliente de la respuesta
            $body = $response->getBody()->getContents();
            $newCustomer = json_decode($body, true);
            $clientId = $newCustomer['customerId'];  // Asegúrate de que este campo exista en la respuesta

            // Agrega el nuevo customerId al arreglo de la compra
            $dataPurchase['customerId'] = $clientId;

        } catch (RequestException $e) {
            if ($e->hasResponse()) {
                $errorBody = $e->getResponse()->getBody()->getContents();
                echo json_encode(["error" => "Error al crear cliente: " . $errorBody]);
                exit;
            } else {
                echo json_encode(["error" => "Error al crear cliente: " . $e->getMessage()]);
                exit;
            }
        }
    }

    // Ahora realiza la solicitud POST para la compra
    $jsonData = json_encode($dataPurchase, JSON_PRETTY_PRINT);

    try {
        $response = $client->request('POST', 'purchase/create', [
            'headers' => $headers,
            'body' => $jsonData,
        ]);

        $body = $response->getBody()->getContents();
        header('Content-Type: application/json');
        echo $body;

    } catch (RequestException $e) {
        if ($e->hasResponse()) {
            $errorBody = $e->getResponse()->getBody()->getContents();
            echo json_encode(["error" => "Error en la solicitud: " . $errorBody]);
        } else {
            echo json_encode(["error" => "Error en la solicitud: " . $e->getMessage()]);
        }
    }
} else {
    echo json_encode(["error" => "Método no permitido."]);
}
