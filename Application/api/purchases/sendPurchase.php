<?php
require_once('../../init.php');
require_once('auth/session.php');
require 'vendor/autoload.php';

use GuzzleHttp\Client;
use GuzzleHttp\Exception\RequestException;

// Asegúrate de que el método de solicitud sea POST
if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    // Aquí recibes todos los datos del formulario
    $dataPurchase = [
        'tuition' => 'C-JC-001215-A/F',
        'cryptId' => isset($_POST['cryptId']) ? (int)$_POST['cryptId'] : null, 
        'cryptSpaces' => isset($_POST['cryptSpaces']) ? (int)$_POST['cryptSpaces'] : null, 
        'maintenanceFee' => isset($_POST['inMaintenance']) ? (float)$_POST['inMaintenance'] : null, 
        'federalTax' => isset($_POST['federalTax']) ? (float)$_POST['federalTax'] : null, 
        'discountId' => isset($_POST['discountId']) ? (int)$_POST['discountId'] : null, 
        'ashDeposit' => isset($_POST['inAshDeposit']) ? (float)$_POST['inAshDeposit'] : null, 
        'customerId' => isset($_POST['CustomerID']) ? (int)$_POST['CustomerID'] : null, 
        'monthlyPayments' => isset($_POST['paymentPlan']) ? (int)$_POST['paymentPlan'] : null, 
        'referencePerson1' => $_POST['ReferenceCustomer1'] ?? null,
        'referencePersonPhone1' => $_POST['ReferenceCustomerPhone1'] ?? null,
        'referencePerson2' => $_POST['ReferenceCustomer2'] ?? null,
        'referencePersonPhone2' => $_POST['ReferenceCustomerPhone2'] ?? null,
    ];
    
    // Aquí recibes la información de los pagos
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
    
    // Agrega los pagos al arreglo de datos
    $dataPurchase['payments'] = $payments;
    
    // Convertir el arreglo a JSON
    $jsonData = json_encode($dataPurchase);
    echo '<script> console.log('.$jsonData.');</script>';
    // Configuración de Guzzle
    $headers = [
        'Authorization' => 'Bearer ' . $token,
        'Content-Type' => 'application/json', // Indica que el cuerpo es JSON
    ];

    $client = new Client([
        'base_uri' => API_SERVICES,
        'headers' => $headers,
    ]);

    try {
        // Realiza la solicitud POST a la API
        $response = $client->request('POST', 'purchase/create', [
            'json' => $jsonData // Usa 'json' para enviar datos en formato JSON
        ]);


        // Obtiene el contenido de la respuesta de la API
        $body = $response->getBody()->getContents();

        header('Content-Type: application/json');
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
    // Manejar el caso en que el método de solicitud no es POST
    echo json_encode(["error" => "Método no permitido."]);
}


/*

$dataPurchase = [
        'CustomerSelect' => $_POST['CustomerSelect'] ?? null,
        'CustomerID' => $_POST['CustomerID'] ?? null,
        'UserID' => $_POST['UserID'] ?? null,
        'PSurname' => $_POST['PSurname'] ?? null,
        'MSurname' => $_POST['MSurname'] ?? null,
        'Name' => $_POST['Name'] ?? null,
        'CelPhone' => $_POST['CelPhone'] ?? null,
        'Email' => $_POST['Email'] ?? null,
        'BusinessName' => $_POST['BusinessName'] ?? null,
        'RFCCURP' => $_POST['RFCCURP'] ?? null,
        'DateOfBirth' => $_POST['DateOfBirth'] ?? null,
        'CityOfBirth' => $_POST['CityOfBirth'] ?? null,
        'CivilStatus' => $_POST['CivilStatus'] ?? null,
        'Occupation' => $_POST['Occupation'] ?? null,
        'Company' => $_POST['Company'] ?? null,
        'PhoneCompany' => $_POST['PhoneCompany'] ?? null,
        'AddressCompany' => $_POST['AddressCompany'] ?? null,
        'ExtPhoneCompany' => $_POST['ExtPhoneCompany'] ?? null,
        'StateAddressCompany' => $_POST['StateAddressCompany'] ?? null,
        'MunicipalityAddressCompany' => $_POST['MunicipalityAddressCompany'] ?? null,
        'CityAddressCompany' => $_POST['CityAddressCompany'] ?? null,
        'Income' => $_POST['Income'] ?? 0.00,
        'ReferenceCustomer1' => $_POST['ReferenceCustomer1'] ?? null,
        'ReferenceCustomerPhone1' => $_POST['ReferenceCustomerPhone1'] ?? null,
        'ReferenceCustomer2' => $_POST['ReferenceCustomer2'] ?? null,
        'ReferenceCustomerPhone2' => $_POST['ReferenceCustomerPhone2'] ?? null,
        'cryptId' => $_POST['cryptId'] ?? null,
        'cryptSpaces' => $_POST['cryptSpaces'] ?? null,
        'discountId' => $_POST['discountId'] ?? null,
        'federalTax' => $_POST['federalTax'] ?? null,
        'inMaintenance' => $_POST['inMaintenance'] ?? null,
        'inAshDeposit' => $_POST['inAshDeposit'] ?? null,
        'TypePay' => $_POST['TypePay'] ?? null,
        'amount_cash_deposit' => $_POST['amount_cash_deposit'] ?? null,
        'TicketCashDeposit' => $_POST['TicketCashDeposit'] ?? null,
        'amount_cash' => $_POST['amount_cash'] ?? null,
        'paymentPlan' => $_POST['paymentPlan'] ?? null,
        'cryptKey' => $_POST['cryptKey'] ?? null,
        'level' => $_POST['level'] ?? null,
        'area' => $_POST['area'] ?? null,
        'zone' => $_POST['zone'] ?? null,
        'totalAmount' => $_POST['totalAmount'] ?? null,
        'appliedDiscount' => $_POST['appliedDiscount'] ?? null,
        'initialPayment' => $_POST['initialPayment'] ?? null,
        'balance' => $_POST['balance'] ?? null,
    ];
    // Aquí recibes la información de los beneficiarios
    $beneficiaries = [];
    if (isset($_POST['beneficiaries']) && is_array($_POST['beneficiaries'])) {
        foreach ($_POST['beneficiaries'] as $beneficiary) {
            $beneficiaries[] = [
                'name' => $beneficiary['name'] ?? null,
                'surnames' => $beneficiary['surnames'] ?? null,
                'birthdate' => $beneficiary['birthdate'] ?? null,
                'phone' => $beneficiary['phone'] ?? null,
                'relationship' => $beneficiary['relationship'] ?? null,
                'customerId' => $beneficiary['customerId'] ?? null,
            ];
        }
    }

    // Agrega los beneficiarios al arreglo de datos
    $data['beneficiaries'] = $beneficiaries;

    // Aquí recibes la información de los pagos
    $payments = [];
    if (isset($_POST['payments']) && is_array($_POST['payments'])) {
        foreach ($_POST['payments'] as $payment) {
            $payments[] = [
                'paymentAmount' => $payment['paymentAmount'] ?? null,
                'concept' => $payment['concept'] ?? null,
                'typePaymentId' => $payment['typePaymentId'] ?? null,
                'currencyId' => $payment['currencyId'] ?? null,
            ];
        }
    }

    // Agrega los pagos al arreglo de datos
    $data['payments'] = $payments;

    */
?>

