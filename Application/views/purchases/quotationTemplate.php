<?php
require '../../vendor/autoload.php';

$CustomerSelect = $_POST['CustomerSelect'];
$CustomerID = $_POST['CustomerID'];
$UserID = $_POST['UserID'];
$UserName = $_POST['UserName'];
$PSurname = $_POST['PSurname'];
$MSurname = $_POST['MSurname'];
$Name = $_POST['Name'];
$address = $_POST['address'];
$house_number = $_POST['house_number'];
$apt_number = $_POST['apt_number'];
$neighborhood = $_POST['neighborhood'];
$zip_code = $_POST['zip_code'];
$catStatesId = $_POST['catStatesId'];
$Deputation = $_POST['Deputation'];  // Verifica si este campo tiene valor o está vacío
$catTownsId = $_POST['catTownsId'];
$CelPhone = $_POST['CelPhone'];
$Email = $_POST['Email'];
$social_reason = $_POST['social_reason'];  // Puede que no tenga valor
$RFCCURP = $_POST['RFCCURP'];
$DateOfBirth = $_POST['DateOfBirth'];
$CityOfBirth = $_POST['CityOfBirth'];
$CivilStatus = $_POST['CivilStatus'];
$Occupation = $_POST['Occupation'];
$Company = $_POST['Company'];
$PhoneCompany = $_POST['PhoneCompany'];
$AddressCompany = $_POST['AddressCompany'];
$ExtPhoneCompany = $_POST['ExtPhoneCompany'];
$StateAddressCompany = $_POST['StateAddressCompany'];
$MunicipalityAddressCompany = $_POST['MunicipalityAddressCompany'];
$CityAddressCompany = $_POST['CityAddressCompany'];  // Puede que no tenga valor
$Income = $_POST['Income'];
$stateName = $_POST['stateName'];
$townName = $_POST['townName'];
$stateCompanyName = $_POST['stateCompanyName'];
$cityCompanyName = $_POST['cityCompanyName'];
$CivilStatusName = $_POST['CivilStatusName'];

// Referencias del cliente
$ReferenceCustomer1 = $_POST['ReferenceCustomer1'];
$ReferenceCustomerPhone1 = $_POST['ReferenceCustomerPhone1'];
$ReferenceCustomer2 = $_POST['ReferenceCustomer2'];  // Puede que no tenga valor
$ReferenceCustomerPhone2 = $_POST['ReferenceCustomerPhone2'];  // Puede que no tenga valor

// Datos de la cripta
$cryptId = $_POST['cryptId'];
$cryptSpaces = $_POST['cryptSpaces'];
$discountId = $_POST['discountId'];
$federalTax = $_POST['federalTax'];

$planVenta = $_POST['planVenta'];

// Beneficiarios (arreglo)
$beneficiaries = [];
if (isset($_POST['beneficiaries'])) {
    foreach ($_POST['beneficiaries'] as $key => $beneficiary) {
        $beneficiaries[] = [
            'name' => $beneficiary['name'],
            'surnames' => $beneficiary['surnames'],
            'birthdate' => $beneficiary['birthdate'],
            'phone' => $beneficiary['phone'],
            'relationship' => $beneficiary['relationship'],
            'customerId' => $beneficiary['customerId']
        ];
    }
}

$purchaseId = $_POST['purchaseId'];

$TypePay = $_POST['TypePay'] ?? null;  // Tipo de pago
$monto = 0;
$paymentTypeDescription = '';
$check_number = $account_number = $bank = ''; // Inicializamos en vacío

// Inicializar variables
$paymentTypeDescription = '';
$montos = [];
$payments = $_POST['payments'] ?? []; // Recibir el arreglo de pagos

// Recorre los pagos recibidos
foreach ($payments as $payment) {
    $monto = $payment['paymentAmount'];
    $typePaymentId = $payment['typePaymentId'];
    
    // Dependiendo del tipo de pago, asigna la descripción y los detalles
    switch ($typePaymentId) {
        case 1:
            $paymentTypeDescription = 'CHEQUE';
            $montos[] = $monto;
            break;
        case 2:
            $paymentTypeDescription = 'TARJETA DE CREDITO/DEBITO';
            $montos[] = $monto;
            break;
        case 3:
            $paymentTypeDescription = 'TRANSFERENCIA';
            $montos[] = $monto;
            break;
        case 4:
            $paymentTypeDescription = 'EFECTIVO';
            $montos[] = $monto;
            break;
        case 5:
            $paymentTypeDescription = 'DEPOSITO EN EFECTIVO';
            $montos[] = $monto;
            break;
    }
}

// Si se usaron múltiples métodos de pago, define el tipo como "MIXTO"
if (count($montos) > 1) {
    $paymentTypeDescription = 'MIXTO';
}
// Otros datos de la operación
$paymentPlan = $_POST['paymentPlan'];
$cryptKey = $_POST['cryptKey'];
$level = $_POST['level'];
$area = $_POST['area'];
$zone = $_POST['zone'];


function getNumericValue($key, $default = 0.0) {
    return isset($_POST[$key]) && is_numeric($_POST[$key]) ? (float)$_POST[$key] : $default;
}

// Ahora asigna los valores utilizando la función
$totalAmount = number_format(getNumericValue('totalAmount'), 2, '.', ',');
$appliedDiscount = number_format(getNumericValue('appliedDiscount'), 2, '.', ',');
$balance = number_format(getNumericValue('balance'), 2, '.', ',');
$initialPayment = number_format(getNumericValue('initialPayment'), 2, '.', ',');
$inMaintenance = getNumericValue('inMaintenance', 'False') ? number_format(getNumericValue('inMaintenance'), 2, '.', ',') : 'NO APLICA';
$inAshDeposit = getNumericValue('inAshDeposit', 'False') ? number_format(getNumericValue('inAshDeposit'), 2, '.', ',') : 'NO APLICA';
$inOtherFee = getNumericValue('inOtherFee', 'False') ? number_format(getNumericValue('inOtherFee'), 2, '.', ',') : 'NO APLICA';


$paymentType = isset($_POST['payment_type']) ? $_POST['payment_type'] : null;
$paymentAmountMes = isset($_POST['payment_amount']) ? $_POST['payment_amount'] : null;
error_log("Valor de paymentType: " . $paymentType);



$diaPrimerPago = isset($_POST['diaPrimerPago']) ? $_POST['diaPrimerPago'] : null;
$mesPrimerPago = isset($_POST['mesPrimerPago']) ? $_POST['mesPrimerPago'] : null;
$yPrimerPago = isset($_POST['yPrimerPago']) ? $_POST['yPrimerPago'] : null;

$diaUltimoPago = isset($_POST['diaUltimoPago']) ? $_POST['diaUltimoPago'] : null;
$mesUltimoPago = isset($_POST['mesUltimoPago']) ? $_POST['mesUltimoPago'] : null;
$yUltimoPago = isset($_POST['yUltimoPago']) ? $_POST['yUltimoPago'] : null;

 // Fecha actual
 $fechaActual = new DateTime();


 // Formatear la fecha para mostrar el día, mes y año
 $diaSolicitud = $fechaActual->format('d');
 $mesSolicitud = $fechaActual->format('m');
 $ySolicitud = $fechaActual->format('Y');

use Dompdf\Dompdf;

// Crear instancia de DOMPDF
$dompdf = new Dompdf();
$logoBase64 = base64_encode(file_get_contents('C:/xampp/htdocs/Church/Application/assets/img/LOGO_CATEDRAL_TIJUANA.png'));

$html = "

<!DOCTYPE html>
<html >
<head>
    <meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>
    <style>
         .container {
            display: flex;
            flex-wrap: nowrap; /* Evita que las tablas se vayan a la siguiente línea */
            justify-content: space-between; /* Asegura que se repartan de manera uniforme */
        }

        .container table {
            flex-shrink: 1; /* Permite que las tablas se reduzcan si el espacio es insuficiente */
            width: 48%; /* Limita el ancho de cada tabla para que quepan en una fila */
            margin-right: 10px; /* Añade espacio entre las tablas */
}

body {
            /*transform: scale(0.95);  Escala todo ligeramente para que ocupe menos espacio */
            transform-origin: top left;
            font-size: 4pt; /* Reduce el tamaño de la fuente */
            margin: 0; /* Márgenes mínimos */
            padding: 0; /* Sin padding */
        }
p, td {
    word-wrap: break-word; /* Para que las palabras largas se dividan en varias líneas si es necesario */
}
    

    h1 {
        color: black;
        font-family: Arial, sans-serif;
        font-style: normal;
        font-weight: bold;
        text-decoration: none;
        font-size: 8pt;
    }

    .s1 {
        color: black;
        font-family: Arial, sans-serif;
        font-style: normal;
        font-weight: normal;
        text-decoration: none;
        font-size: 6pt;
    }
    .s3 {
        color: black;
        font-family: Arial, sans-serif;
        font-style: normal;
        font-weight: normal;
        text-decoration: none;
        font-size: 5pt;
    }

    p {
        color: black;
        font-family: Arial, sans-serif;
        font-style: normal;
        font-weight: normal;
        text-decoration: none;
        font-size: 6pt;
        margin: 0pt;
    }

    .s2 {
        color: black;
        font-family: Arial, sans-serif;
        font-style: normal;
        font-weight: bold;
        text-decoration: none;
        font-size: 6pt;
    }

    h2 {
        color: black;
        font-family: Arial, sans-serif;
        font-style: normal;
        font-weight: bold;
        text-decoration: none;
        font-size: 6pt;
        margin: 0; 
        padding: 4pt 0 0 0; 
        text-align: center;
        padding-top: 5pt;

    
    }
    .watermark-text {
        position: absolute;
        top: 50%;
        left: 50%;
        font-size: 55pt;
        color: rgba(230, 60, 80, 0.3); 
        transform: translate(-50%, -50%) rotate(-45deg); 
        z-index: 0; 
        white-space: nowrap; 
    }
     .content {
            position: relative;
            z-index: 1; /* Asegura que el contenido esté sobre la marca de agua */
            padding: 20px;
        }

    .a,
    a {
        color: black;
        font-family: Arial, sans-serif;
        font-style: normal;
        font-weight: normal;
        text-decoration: none;
        font-size: 6pt;
    }

    .s3 {
        color: black;
        font-family: Arial, sans-serif;
        font-style: normal;
        font-weight: normal;
        text-decoration: none;
        font-size: 6pt;
    }

    .s4 {
        color: black;
        font-family: Arial, sans-serif;
        font-style: normal;
        font-weight: normal;
        text-decoration: none;
        font-size: 6pt;
    }

    table,
    tbody {
        vertical-align: top;
        overflow: visible;
    }
    </style>
</head>
    <div class='watermark-text'>NO TIENE VALIDEZ</div>

<div class='content'>

<table class='table' cellspacing='0' cellpadding='0' style='border: none !important; width: 100%;'>
    <tbody>
        <tr>
             <td rowspan='4' style='border: none !important; vertical-align: top; width: 40%; text-align: center; position: relative;'>
                <img width='140px' height='140px' src='data:image/png;base64,<?= $logoBase64 ?>' alt='Logo' style='position: absolute; top: -50px;' />
            </td>
        </tr>
        <tr>
            <td colspan='2' style='border: none !important; vertical-align: middle; width: 60%; '>
                <h1 style='margin: 0;'>PARROQUIA IGLESIA DE GUADALUPE DEL RIO EN TIJUANA, A.R.</h1>
                <p style='margin: 0;'>
                <br> PASEO CENTENARIO 10150 ZONA RIO, TIJUANA B.C. C.P. 22320 
                <br> TEL.(664) 607-37-75 Y (664) 607-38-67</t>
            </td>
        </tr>
    </tbody>
</table>
<table style='width:100%; border-collapse: collapse;margin-bottom: 15px;margin-top: 45px;'>
    <tr>
        <td style='vertical-align: top; width: auto;'>
            <!-- Primera tabla aquí -->
            <table style='border-collapse:collapse;' cellspacing='0'>
                <tr style='height:13pt'>
                    <td style='background-color:#b0d5f7; width:80pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
                        colspan='3'>
                        <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>FECHA</p>
                    </td>
                </tr>
                <tr style='height:13pt'>
                    <td style='width:25pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>DÍA</p>
                    </td>
                    <td style='width:30pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>MES</p>
                    </td>
                    <td style='width:25pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>AÑO</p>
                    </td>
                </tr>
                <tr style='height:10pt'>
                    <td style='width:25pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s1' style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$diaSolicitud</p>
                    </td>
                    <td style='width:30pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s1' style='padding-top: 1pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$mesSolicitud</p>
                    </td>
                    <td style='width:25pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s1' style='padding-top: 1pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$ySolicitud</p>
                    </td>
                </tr>
            </table>
        </td>
        <td style='vertical-align: top; text-align: right;'>
            <!-- Segunda tabla aquí -->
            <table style='border-collapse:collapse; float: right;' cellspacing='0'>
                <tr style='height:10pt'>
                    <td style='background-color:#b0d5f7; width:80pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s1' style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>SOLICITUD No.</p>
                    </td>
                    <td style='width:100pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p style='text-indent: 0pt;text-align: left;'>$purchaseId</p>
                    </td>
                </tr>
                <tr style='height:10pt'>
                    <td style='background-color:#b0d5f7; width:80pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s1' style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>CLIENTE No.</p>
                    </td>
                    <td style='width:100pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s1' style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>N/A</p>
                    </td>
                </tr>
                <tr style='height:10pt'>
                    <td style='background-color:#b0d5f7; width:80pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s1' style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>CONTRATO No.</p>
                    </td>
                    <td style='width:100pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s1' style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>N/A</p>
                    </td>
                </tr>
                <tr style='height:10pt'>
                    <td style='background-color:#b0d5f7; width:80pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s1' style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>VENDEDOR</p>
                    </td>
                    <td style='width:100pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s1' style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$UserName</p>
                    </td>
                </tr>
                <tr style='height:10pt'>
                    <td style='background-color:#b0d5f7; width:80pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s1' style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>SERVIDOR</p>
                    </td>
                    <td style='width:100pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s1' style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$UserName</p>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>

<h2 style='padding-top:8pt!important; text-indent: 0pt;text-align: center;'>DATOS DEL SOLICITANTE
</h2>
<table style='border-collapse:collapse;width:100%;' cellspacing='0'>
    <tr style='height:13pt'>
        <td
            style='background-color:#b0d5f7; width:150pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2'
                style='padding-top: 1pt;padding-left: 57pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                APELLIDO PATERNO</p>
        </td>
        <td
            style='background-color:#b0d5f7; width:150pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>MATERNO</p>
        </td>
        <td
            style='background-color:#b0d5f7; width:150pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>NOMBRE(S)</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td
            style='width:150pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$PSurname
            </p>
        </td>
        <td
            style='width:150pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$MSurname
                </p>
        </td>
        <td
            style='width:150pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$Name</p>
        </td>
    </tr>
</table>
<h2 style='padding-bottom: 1pt;text-indent: 0pt;text-align: center;'>DOMICILIO
    PARTICULAR</h2>
<table style='border-collapse:collapse;width:100%;' cellspacing='0'>
    <tr style='height:10pt'>
        <td colspan = '2'
            style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>CALLE, AV.,
                BLVD. CALZ</p>
        </td>
        <td
            style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>NUMERO</p>
        </td>
        <td
            style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>INTERIOR
            </p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td colspan = '2'
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p style='text-indent: 0pt;text-align: left;'>$address</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p style='text-indent: 0pt;text-align: left;'>$house_number </p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p style='text-indent: 0pt;text-align: left;'>$apt_number</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td colspan = '2'
            style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>COLONIA</p>
        </td>
        <td style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            colspan='2'>
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>CODIGO POSTAL</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td colspan = '2'
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p style='text-indent: 0pt;text-align: left;'>$neighborhood</p>
        </td>
        <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            colspan='2'>
            <p style='text-indent: 0pt;text-align: left;'>$zip_code</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td
            style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>CIUDAD</p>
        </td>
        <td
            style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2'style='padding-top: 1pt;padding-left: 33pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>DELEGACION</p>
        </td>
        <td colspan = '2'
            style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>ESTADO</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                $townName</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p style='text-indent: 0pt;text-align: left;'>$neighborhood</p>
        </td>
        <td colspan = '2'
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                $stateName</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td colspan = '2'
            style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>TELEFONO
                PARTICULAR</p>
        </td>
        <td style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            colspan='2'>
            <p class='s2'
                style='padding-top: 1pt;padding-left: 70pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                CORREO ELECTRONICO</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td colspan = '2'
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                $CelPhone</p>
        </td>
        <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            colspan='2'>
            <p style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'><a
                    href='mailto:$Email' class='s3'>$Email</a></p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td
            style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>R.F.C./C.U.R.P.</p>
        </td>
        <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            colspan='3'>
            <p style='text-indent: 0pt;text-align: left;'>$RFCCURP</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td
            style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>FECHA DE
                NACIMIENTO</p>
        </td>
        <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            colspan='3'>
            <p style='text-indent: 0pt;text-align: left;'>$DateOfBirth</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td
            style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>LUGAR DE
                NACIMIENTO</p>
        </td>
        <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            colspan='3'>
            <p style='text-indent: 0pt;text-align: left;'>$CityOfBirth</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td
            style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>ESTADO
                CIVIL</p>
        </td>
        <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            colspan='3'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                $CivilStatusName</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td
            style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>OCUPACION
            </p>
        </td>
        <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            colspan='3'>
            <p style='text-indent: 0pt;text-align: left;'>$Occupation</p>
        </td>
    </tr>
</table>
<h2 style='padding-bottom: 0pt;text-indent: 0pt;text-align: center;'>DATOS DE LA
    EMPRESA DONDE PRESTA SUS SERVICIOS</h2>
<table style='border-collapse:collapse;width:100%;' cellspacing='0'>
    <tr style='height:10pt'>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                NOMBRE DE LA COMPAÑIA</p>
        </td>
        <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            colspan='2'>
            <p style='text-indent: 0pt;text-align: left;'>$Company</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            colspan='2'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                DOMICILIO</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                TELEFONO: $PhoneCompany</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            colspan='2'>
            <p style='text-indent: 0pt;text-align: left;'>$AddressCompany</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>EXT: $ExtPhoneCompany
            </p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                CIUDAD</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                MUNICIPIO</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                ESTADO</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                $cityCompanyName</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p style='text-indent: 0pt;text-align: left;'><br /></p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                $stateCompanyName</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            colspan='2'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                INGRESO PROMEDIO MENSUAL (Incluye a su conyuge)</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p style='text-indent: 0pt;text-align: left;'>$Income</p>
        </td>
    </tr>
</table>
<h2 style='padding-bottom: 0pt;text-indent: 0pt;text-align: center;'>REFERENCIAS
</h2>
<table style='border-collapse:collapse;width:100%;' cellspacing='0'>
    <tr style='height:10pt'>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>1)
                $ReferenceCustomer1</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>TEL
            </p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                $ReferenceCustomerPhone1</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>2)
                $ReferenceCustomer2
            </p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>TEL
            </p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p style='text-indent: 0pt;text-align: left;'>$ReferenceCustomerPhone2</p>
        </td>
    </tr>
</table>
<h2 style='padding-bottom: 0pt;text-indent: 0pt;text-align: center;'>BENEFICIARIOS</h2>
<table style='border-collapse:collapse;width:100%;' cellspacing='0'>
    <tr style='height:10pt'>
        <td style=' background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt' colspan='2'>
            <p style='text-indent: 0pt;text-align: left;'><br /></p>
        </td>
        <td style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt' colspan='3'>
            <p class='s2' style='padding-top: 1pt;padding-left: 25pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>FECHA NAC.</p>
        </td>
        <td style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt' colspan='2'>
            <p class='s2' style='padding-top: 1pt;padding-left: 8pt;text-indent: 0pt;line-height: 6pt;text-align: left;'><br/></p>
        </td>
 
    </tr>
    <tr style='height:12pt'>
        <td style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;text-align: center;'>APELLIDOS</p>
        </td>
        <td style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;text-align: center;'>NOMBRES(S)</p>
        </td>
        <td style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s4' style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 8pt;text-align: left;'>DIA</p>
        </td>
        <td style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s4' style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 8pt;text-align: left;'>MES</p>
        </td>
        <td style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s4' style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 8pt;text-align: left;'>AÑO</p>
        </td>
        <td style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p style='text-indent: 0pt;text-align: left;'>PARENTESCO</p>
        </td>
        <td style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p style='text-indent: 0pt;text-align: left;'>TELEFONO</p>
        </td>
    </tr>"; 
    // Contar el número de beneficiarios existentes
    $beneficiaryCount = count($beneficiaries);

    // Generar filas para los beneficiarios existentes
    foreach ($beneficiaries as $beneficiary) {
        $birthdate = date('d-m-Y', strtotime($beneficiary['birthdate']));
        list($day, $month, $year) = explode('-', $birthdate);

        $html .= "
        <tr style='height:12pt'>
            <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                <p style='text-indent: 0pt;text-align: center;'>{$beneficiary['surnames']}</p>
            </td>
            <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                <p style='text-indent: 0pt;text-align: center;'>{$beneficiary['name']}</p>
            </td>
            <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                <p style='text-indent: 0pt;text-align: center;'>$day</p>
            </td>
            <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                <p style='text-indent: 0pt;text-align: center;'>$month</p>
            </td>
            <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                <p style='text-indent: 0pt;text-align: center;'>$year</p>
            </td>
            <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                <p style='text-indent: 0pt;text-align: center;'>{$beneficiary['relationship']}</p>
            </td>
            <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                <p style='text-indent: 0pt;text-align: center;'>{$beneficiary['phone']}</p>
            </td>
        </tr>";
    }

    // Si hay menos de 3 beneficiarios, agregar filas vacías
    for ($i = $beneficiaryCount; $i < 3; $i++) {
        $html .= "
        <tr style='height:12pt'>
            <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                <p style='text-indent: 0pt;text-align: center;'><br/></p>
            </td>
            <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                <p style='text-indent: 0pt;text-align: center;'><br/></p>
            </td>
            <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                <p style='text-indent: 0pt;text-align: center;'><br/></p>
            </td>
            <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                <p style='text-indent: 0pt;text-align: center;'><br/></p>
            </td>
            <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                <p style='text-indent: 0pt;text-align: center;'><br/></p>
            </td>
            <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                <p style='text-indent: 0pt;text-align: center;'><br/></p>
            </td>
            <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                <p style='text-indent: 0pt;text-align: center;'><br/></p>
            </td>
        </tr>";
    }

$html .= "</table>
<h2 style='padding-bottom: 1pt;text-indent: 0pt;text-align: center;'>CONDICIONES
    ECONOMICAS DE LA OPERACION</h2>
<table style='border-collapse:collapse;width:100%;' cellspacing='0'>
    <tr style='height:10pt'>
        <td colspan='2'
            style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2'
                style='padding-top: 1pt;padding-left: 20pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>PLAN DE VENTA</p>
        </td>
        <td
            style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2'
                style='padding-top: 1pt;padding-left: 12pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                CLAVE DE LA CRIPTA</p>
        </td>
        <td
            style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>NIVEL</p>
        </td>
        <td 
            style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>AREA</p>
        </td>
        <td
            style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>ZONA</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td colspan ='2'
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                $planVenta</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                $cryptKey</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$level</p>
        </td>
        <td 
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                $area</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p style='text-indent: 0pt;text-align: left;'>$zone</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td colspan='2' style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            >
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>IMPORTE TOTAL</p>
        </td>
        <td colspan='2' style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            >
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>PAGO INICIAL</p>
        </td>
        <td colspan='2'
            style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>SALDO</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td colspan='2' style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            >
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                $$totalAmount M.N.</p>
        </td>
        <td colspan='2' style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            >
            <p class='s1'
                style='padding-top: 1pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                $$initialPayment M.N.</p>
        </td>
        <td colspan='2'
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$$balance
                M.N.</p>
        </td>
    </tr>";



if (($TypePay)!= 1) {
    $html .= "
    <tr style='height:10pt'>
        <td colspan='6'
            style=' border-top-width:4pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt;border-top:none!important;'>
            <p class='s2'
                style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>
                EL SALDO SERÁ LIQUIDADO EN &nbsp;
                <strong>{$planVenta}</strong> 
                &nbsp;EN ABONOS:
                
                <!-- Checkbox para abonos mensuales (seleccionado por defecto si $paymentType es 'mensuales') -->
                &nbsp;&nbsp;<input style='vertical-align: middle; transform: scale(0.8);' type='checkbox' name='payment_type' value='mensuales' id='mensuales_checkbox' " . 
                ($paymentType === 'mensuales' ? 'checked' : '') . ">&nbsp;MENSUALES
                
                <!-- Checkbox para abonos semanales -->
                &nbsp;&nbsp;<input style='vertical-align: middle; transform: scale(0.8);' type='checkbox' name='payment_type' value='semanales' id='semanales_checkbox' " . 
                ($paymentType === 'semanales' ? 'checked' : '') . ">&nbsp;SEMANALES
                
                <!-- Mostrar el valor calculado como texto -->
                &nbsp;&nbsp;POR LA CANTIDAD DE: 
                &nbsp;$ &nbsp;$paymentAmountMes M.N.
            </p>
        </td>
    </tr>";
}else{
    $html .= "
    <tr style='height:10pt'>
        <td colspan='6'
            style=' border-top-style:solid;border-top-width:4pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2'
                style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>
                 &nbsp;
            </p>
        </td>
    </tr>";

    }
 $html .= "
    
</table>
<table style='border-collapse:collapse;width:100%;border-top: none!important;' cellspacing='0'>
    <tr style='height:10pt'>
        <td colspan='2'
            style=' border-top-style:solid;border-top:none;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2'><br/>
            </p>
        </td>
        <td style='background-color:#b0d5f7; border-top-style:solid;border-top:none;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            >
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>DIA</p>
        </td>
        <td style='background-color:#b0d5f7; border-top-style:solid;border-top:none;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            >
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>MES</p>
        </td>
        <td style='background-color:#b0d5f7; border-top-style:solid;border-top:none;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            >
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>AÑO</p>
        </td>
        <td colspan='2'
            style=' border-top-style:solid;border-top:none;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2'><br/>
            </p>
        </td>
        <td style='border-top-style:solid;border-top:none;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            >
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>DIA</p>
        </td>
        <td style=' border-top-style:solid;border-top:none;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            >
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>MES</p>
        </td>
        <td style='border-top-style:solid;border-top:none;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            >
            <p class='s2' style='padding-top: 1pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>AÑO</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td colspan='2'
            style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt; text-align: right;'>
            <p class='s2'>SIENDO EL PRIMERO DE ELLOS EN</p>
        </td>
        <td style=' border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            >
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$diaPrimerPago</p>
        </td>
        <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            >
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$mesPrimerPago</p>
        </td>
        <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            >
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$yPrimerPago</p>
        </td>
        <td colspan='2'
            style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt; text-align: right;'>
            <p class='s2'> Y EL ULTIMO EN
            </p>
        </td>
        <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            >
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$diaUltimoPago</p>
        </td>
        <td style=' border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            >
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$mesUltimoPago</p>
        </td>
        <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            >
           <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$yUltimoPago</p>
        </td>
    </tr>
</table>
<h2 style='padding-bottom: 1pt;text-indent: 0pt;text-align: center;'>ADICIONAL
</h2>
<table style='border-collapse:collapse;width:100%;' cellspacing='0'>
    <tr style='height:10pt'>
        <td
            style='background-color:#b0d5f7;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>CUOTA
                DE MANTENIMIENTO ANUAL</p>
        </td>
        <td
            style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                DEPÓSITO DE CENÍZAS</p>
        </td>";
        if ($inOtherFee !== 'False' && is_numeric($inOtherFee) && $inOtherFee > 0) {
            $html .= "
            <td
                style='background-color:#b0d5f7; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                <p class='s1'
                    style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                    OTRO </p>
            </td>";
        }

    
    $html .= "</tr><tr style='height:10pt'>";

// Condición para inMaintenance
if ($inMaintenance !== 'False' && is_numeric($inMaintenance) && $inMaintenance > 0) {
    $html .= "
    <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
        <p class='s1' style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
            APLICA - " . number_format($inMaintenance, 2) . " M.N.
        </p>
    </td>";
} else {
    $html .= "
    <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
        <p class='s1' style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
            NO APLICA
        </p>
    </td>";
}

// Condición para inAshDeposit
if ($inAshDeposit !== 'False' && is_numeric($inAshDeposit) && $inAshDeposit > 0) {
    $html .= "
    <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
        <p class='s1' style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
            APLICA - " . number_format($inAshDeposit, 2) . " M.N.
        </p>
    </td>";
} else {
    $html .= "
    <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
        <p class='s1' style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
            NO APLICA
        </p>
    </td>";
}

// Condición para inOtherFee
if ($inOtherFee !== 'False' && is_numeric($inOtherFee) && $inOtherFee > 0) {
    $html .= "
    <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
        <p class='s1' style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
            APLICA - " . number_format($inOtherFee, 2) . " M.N.
        </p>
    </td>";
}

$html .= "</tr>";
$html .= "</table>
<p style='text-indent: 0pt;text-align: left;'><br /></p>
<div class='container'>
    
    <table style='border-collapse:collapse;width:100%;' cellspacing='0'>
        <tr style='height:10pt'>
            <td rowspan='2' style='background-color:#b0d5f7; border:solid 1pt;'>
                <p class='s1' style='padding-top: 1pt; padding-left: 2pt; text-indent: 0pt; line-height: 6pt; text-align: left;'>
                    A PAGAR</p>
            </td>
            <td style='border:solid 1pt;'>
                <p style='text-indent: 0pt; text-align: left;'>FORMA DE PAGO INICIAL</p>
            </td>
            <td style='border:solid 1pt;'>
                <p class='s1' style='padding-top: 1pt; padding-left: 2pt; text-indent: 0pt; line-height: 6pt; text-align: left;'>CANTIDAD</p>
            </td>
           
        </tr>
        <tr style='height:10pt'>
            <td style='border:solid 1pt;'>
                <p class='s1' style='padding-top: 1pt; padding-left: 2pt; text-indent: 0pt; line-height: 6pt; text-align: left;'>
                    $paymentTypeDescription</p>
            </td>
            <td style='border:solid 1pt;'>
                <p class='s1' style='padding-top: 1pt; padding-left: 2pt; text-indent: 0pt; line-height: 6pt; text-align: left;'>$$initialPayment M.N.</p>
            </td>
            
        </tr>
    </table>
</div>
<p style='text-indent: 0pt;text-align: left;'><br /></p>
<div class='textbox' style='border:0.5pt solid #000000;display:block;min-height:34.0pt;top:0.2pt;'>
    <h2 style='text-indent: 0pt;text-align: center;'>FIRMAS </h2>
    <p style='text-indent: 0pt;text-align: left;'><br /></p>
    <table style='border-collapse: separate; border-spacing: 10px; width: 60%; margin: 0 auto; margin-top: 10px;'>
        <tr>
            <td style='border-top: 1px solid black; text-align: center; padding: 5px;'>
                <p style='text-indent: 0pt; line-height: 7pt;'>NOMBRE Y FIRMA DEL VENDEDOR</p>
            </td>
            <td style='border-top: 1px solid black; text-align: center; padding: 5px;'>
                <p style='text-indent: 0pt; line-height: 7pt;'>Vo. Bo. GERENCIA DE VENTAS</p>
            </td>
        </tr>
    </table>
</div>

<div class='textbox' style='border-top: none; border-bottom: 0.5pt solid #000; border-left: 0.5pt solid #000; border-right: 0.5pt solid #000; display: block; padding: 5px;'>
    <p class='s3' style='padding-top: 1pt;text-indent: 0pt;line-height: 87%;text-align: left;'>
        Con la aceptación de la presente solicitud me comprometo a firmar el contrato correspondiente. Declaro estar de acuerdo con la presente 
        solicitud, así como la información que he proporcionado es real y verídica.
        
    </p>
</div>

<table style='position:absolute; border-collapse: separate; border-spacing: 10px; width: 40%; margin: 0 auto; margin-top: 30px;'>
    <tr>
        <td style='border-top: 1px solid black; text-align: center; padding: 5px;'>
        <h1 style='padding-top: 1pt;text-indent: 0pt;text-align: center;'>NOMBRE Y FIRMA DEL ADQUIRIENTE</h1>
        </td>
       
    </tr>
</table>

</body>

</html>
";
// Cargar el HTML en DOMPDF
$dompdf->loadHtml($html);

// Configurar el tamaño de la página y la orientación
$dompdf->setPaper('A4', 'portrait');

// Renderizar el PDF
$dompdf->render();

// Enviar el PDF al navegador
header('Content-Type: application/pdf');
header('Content-Disposition: attachment; filename="solicitud.pdf"');
echo $dompdf->output();