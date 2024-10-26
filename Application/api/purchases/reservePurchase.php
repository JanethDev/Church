<?php
require_once('../../init.php');
require_once('auth/session.php');
require 'vendor/autoload.php';

use GuzzleHttp\Client;
use GuzzleHttp\Exception\RequestException;

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $idUser = $_POST['UserID'];
    $clientId = !empty($_POST['CustomerID']) ? (int)$_POST['CustomerID'] : 0;
    $referencePersonPhone1 = $_POST['ReferenceCustomerPhone1'] ?? null;
    $referencePersonPhone2 = $_POST['ReferenceCustomerPhone2'] ?? null;
    $customerPhone= $_POST['CelPhone'] ?? null;
    $formattedPhone1 = preg_replace('/\D/', '', $referencePersonPhone1);
    $formattedPhone2 = preg_replace('/\D/', '', $referencePersonPhone2);
    $phone = preg_replace('/\D/', '', $customerPhone);

    $dataPurchase = [
        'tuition' => '',
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

    $client = new Client([
        'base_uri' => API_SERVICES,  // Debe ser la URL base de la API
        'headers' => $headers,
    ]);

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
    }else{
        $customerData = [
            'id' => $clientId ?? '',
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
    
        try {
            // Realiza la solicitud PUT o POST para actualizar el cliente
            $response = $client->request('POST', 'customer/update', [
                'headers' => $headers,
                'body' => $jsonCustomerData,
            ]);
    
            // Obtén la respuesta del servidor
            $body = $response->getBody()->getContents();
            //echo '<script> console.log(JSON.stringify(' . $jsonCustomerData . ', null, 2)); </script>';
    
        } catch (RequestException $e) {
            if ($e->hasResponse()) {
                $errorBody = $e->getResponse()->getBody()->getContents();
                echo json_encode(["error" => "Error al actualizar cliente: " . $errorBody]);
                exit;
            } else {
                echo json_encode(["error" => "Error al actualizar cliente: " . $e->getMessage()]);
                exit;
            }
        }
    }
    try {
        // Consultar beneficiarios y referencias existentes
        $response = $client->request('GET', 'customer/consult/beneficiaries', [
            'query' => ['customerId' => $clientId]
        ]);
        $existingBeneficiaries = json_decode($response->getBody()->getContents(), true);
    
        // Verifica si hay beneficiarios en la solicitud antes de procesarlos
        $receivedBeneficiaryIds = [];
        if (!empty($_POST['beneficiaries']) && is_array($_POST['beneficiaries'])) {
            $receivedBeneficiaryIds = array_filter(
                array_column($_POST['beneficiaries'], 'idBeneficiary'),
                fn($id) => !empty($id) && $id !== 'undefined'
            );
        }
    
        $receivedReferenceIds = array_filter(
            [$_POST['idReference1'] ?? null, $_POST['idReference2'] ?? null],
            fn($id) => !empty($id) && $id !== 'undefined'
        );
    
        // Eliminar beneficiarios y referencias que ya existen en la base de datos pero no están en los datos recibidos
        foreach ($existingBeneficiaries as $beneficiary) {
            if (!in_array($beneficiary['id'], $receivedBeneficiaryIds) && !in_array($beneficiary['id'], $receivedReferenceIds)) {
                try {
                    $client->request('GET', 'customer/delete/beneficiarie', [
                        'query' => ['beneficiarieId' => $beneficiary['id']]
                    ]);
                } catch (RequestException $e) {
                    handleException($e, "Error al eliminar beneficiario/referencia");
                }
            }
        }
    
        // Procesar beneficiarios recibidos para actualizar o crear solo si existen
        if (!empty($_POST['beneficiaries']) && is_array($_POST['beneficiaries'])) {
            foreach ($_POST['beneficiaries'] as $beneficiary) {
                if (empty($beneficiary['idBeneficiary']) || $beneficiary['idBeneficiary'] === 'undefined') {
                    // Insertar si no tiene un id válido
                    $url = 'customer/create/beneficiarie';
                    $singleBeneficiary = [
                        'customerId' => $beneficiary['customerId'] ?? null,
                        'name' => $beneficiary['name'] ?? null,
                        'lastname' => $beneficiary['surnames'] ?? null,
                        'phone' => preg_replace('/\D/', '', $beneficiary['phone'] ?? ''),
                        'birthdate' => $beneficiary['birthdate'] ?? null,
                        'relationship' => $beneficiary['relationship'] ?? null,
                        'user_id' => $idUser ?? null,
                        'type' => 1,
                    ];
                } else {
                    // Actualizar si tiene un id válido
                    $url = 'customer/update/beneficiarie';
                    $singleBeneficiary = [
                        'id' => $beneficiary['idBeneficiary'],
                        'name' => $beneficiary['name'] ?? null,
                        'lastname' => $beneficiary['surnames'] ?? null,
                        'phone' => preg_replace('/\D/', '', $beneficiary['phone'] ?? ''),
                        'birthdate' => $beneficiary['birthdate'] ?? null,
                        'relationship' => $beneficiary['relationship'] ?? null,
                    ];
                }
    
                try {
                    $client->request('POST', $url, [
                        'headers' => $headers,
                        'body' => json_encode($singleBeneficiary)
                    ]);
                } catch (RequestException $e) {
                    handleException($e, "Error al procesar beneficiario");
                }
            }
        }
    
        // Procesar referencias recibidas para actualizar, crear o eliminar
        for ($i = 1; $i <= 2; $i++) {
            $referenceId = $_POST["idReference$i"] ?? null;
            $name = $_POST["ReferenceCustomer$i"] ?? null;
            $phone = preg_replace('/\D/', '', $_POST["ReferenceCustomerPhone$i"] ?? '');
    
            if (!empty($name) && !empty($phone)) {
                $referenceData = [
                    'id' => $referenceId,
                    'customerId' => $clientId,
                    'name' => $name,
                    'lastname' => "",
                    'phone' => $phone,
                    'relationship' => "N/A"
                ];
                try {
                    $url = $referenceId ? 'customer/update/beneficiarie' : 'customer/create/references';
                    $client->request('POST', $url, [
                        'headers' => $headers,
                        'body' => json_encode($referenceData)
                    ]);
                } catch (RequestException $e) {
                    handleException($e, "Error al procesar referencia");
                }
            } elseif ($referenceId) {
                // Eliminar referencia si tiene un id pero los datos están vacíos
                try {
                    $client->request('GET', 'customer/delete/beneficiarie', [
                        'query' => ['beneficiarieId' => $referenceId]
                    ]);
                } catch (RequestException $e) {
                    handleException($e, "Error al eliminar referencia");
                }
            }
        }
    } catch (RequestException $e) {
        handleException($e, "Error en la solicitud");
    }
    
    // Función para manejo de excepciones
    function handleException($e, $message) {
        $errorBody = $e->hasResponse() ? $e->getResponse()->getBody()->getContents() : $e->getMessage();
        echo json_encode(["error" => "$message: " . $errorBody]);
        exit;
    }
    
    // Ahora realiza la solicitud POST para la compra
    $jsonData = json_encode($dataPurchase, JSON_PRETTY_PRINT);
    try {
        $response = $client->request('POST', 'purchase/reserve', [
            'headers' => $headers,
            'body' => $jsonData,
        ]);

        $body = $response->getBody()->getContents();

        //var_dump($body);

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
