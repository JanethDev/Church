<?php
require_once('../../init.php');
require_once('auth/session.php');
require 'vendor/autoload.php';

use GuzzleHttp\Client;
use GuzzleHttp\Exception\RequestException;

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $clientId = !empty($_POST['CustomerID']) ? (int)$_POST['CustomerID'] : 0;
    $referencePersonPhone1 = $_POST['ReferenceCustomerPhone1'] ?? null;
    $referencePersonPhone2 = $_POST['ReferenceCustomerPhone2'] ?? null;
    $customerPhone= $_POST['CelPhone'] ?? null;
    $formattedPhone1 = preg_replace('/\D/', '', $referencePersonPhone1);
    $formattedPhone2 = preg_replace('/\D/', '', $referencePersonPhone2);
    $phone = preg_replace('/\D/', '', $customerPhone);

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
    if ($clientId === 0) {
        $customerData = [
            'email' => $_POST['Email'] ?? '',
            'name' => $_POST['Name'] ?? '',
            'father_last_name' => $_POST['PSurname'] ?? '',
            'mother_last_name' => $_POST['MSurname'] ?? '',
            'phone' => $phone,
            'rfc' => $_POST['RFCCURP'] ?? '',
            'zip_code' =>  isset($_POST['zip_code']) ? (int)$_POST['zip_code'] : 0, 
            'address' => $_POST['address'] ?? '',
            'catStatesId' => isset($_POST['catStatesId']) ? (int)$_POST['catStatesId'] : 0,  
            'catTownsId' => isset($_POST['catTownsId']) ? (int)$_POST['catTownsId'] : 0,
            'social_reason' => $_POST['social_reason'] ?? '',
            'birthdate' => $_POST['DateOfBirth'] ?? '',
            'birth_place' => $_POST['CityOfBirth'] ?? '',
            'civil_status' => $_POST['CivilStatus'] ?? '',
            'occupation' => $_POST['Occupation'] ?? '',
            'business_name' => $_POST['Company'] ?? '',
            'business_address' => $_POST['AddressCompany'] ?? '',
            'business_city' => $_POST['CityAddressCompany'] ?? '',
            'business_municipality' => $_POST['MunicipalityAddressCompany'] ?? '',
            'business_state' => $_POST['StateAddressCompany'] ?? '',
            'business_phone' => $_POST['PhoneCompany'] ?? '',
            'business_ext' => $_POST['ExtPhoneCompany'] ?? '',
            'deputation' => $_POST['Deputation'] ?? '',
            'house_number' => $_POST['house_number'] ?? '',
            'apt_number' => $_POST['apt_number'] ?? '',
            'neighborhood' => $_POST['neighborhood'] ?? '',
            'average_income' => isset($_POST['Income']) ? (float)$_POST['Income'] : 0.0,

        ];

        $jsonCustomerData = json_encode($customerData, JSON_PRETTY_PRINT);

        //echo '<script> console.log(JSON.stringify(' . $jsonCustomerData . ', null, 2)); </script>';

        try {
            // Realiza la solicitud POST para crear el cliente
            $response = $client->request('POST', 'customer/create', [
                'headers' => $headers,
                'body' => $jsonCustomerData,
            ]);

            // Obtén el ID del nuevo cliente de la respuesta
            $body = $response->getBody()->getContents();
            $newCustomer = json_decode($body, true);
            
            $clientId = $newCustomer;  

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
    $beneficiaries = [];

    if (isset($_POST['beneficiaries']) && is_array($_POST['beneficiaries'])) {
        foreach ($_POST['beneficiaries'] as $beneficiary) {
    
            // Limpiar el número de teléfono, removiendo caracteres no numéricos
            $phoneBeneficiary = isset($beneficiary['phone']) ? preg_replace('/\D/', '', $beneficiary['phone']) : null;
    
            // Construimos el array del beneficiario
            $singleBeneficiary = [
                'customerId' => $beneficiary['customerId'] ?? null,
                'name' => $beneficiary['name'] ?? null,
                'lastname' => $beneficiary['surnames'] ?? null,
                'phone' => $phoneBeneficiary,
                'birthdate' => $beneficiary['birthdate'] ?? null,
                'relationship' => $beneficiary['relationship'] ?? null,
            ];
    
            // Convertimos el beneficiario a JSON
            $jsonBeneficiaryData = json_encode($singleBeneficiary, JSON_PRETTY_PRINT);
    
            try {
                // Realiza la solicitud POST para cada beneficiario
                $response = $client->request('POST', 'customer/create/beneficiarie', [
                    'headers' => $headers,
                    'body' => $jsonBeneficiaryData,
                ]);
    
                // Obtén la respuesta del servidor
                $body = $response->getBody()->getContents();
                echo '<script> console.log(JSON.stringify(' . $jsonBeneficiaryData  . ', null, 2)); </script>';
                echo json_encode(["message" => "Beneficiario creado: " . $body]);
    
            } catch (RequestException $e) {
                if ($e->hasResponse()) {
                    $errorBody = $e->getResponse()->getBody()->getContents();
                    echo json_encode(["error" => "Error al crear beneficiario: " . $errorBody]);
                    exit;
                } else {
                    echo json_encode(["error" => "Error al crear beneficiario: " . $e->getMessage()]);
                    exit;
                }
            }
        }
    }



    // Ahora realiza la solicitud POST para la compra
    $jsonData = json_encode($dataPurchase, JSON_PRETTY_PRINT);
    try {
        $response = $client->request('POST', 'purchase/reserve', [
            'headers' => $headers,
            'body' => $jsonData,
        ]);

        $body = $response->getBody()->getContents();
        header('Content-Type: application/json');
        echo json_encode($body);

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
