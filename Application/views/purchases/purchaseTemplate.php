<?php
require '../../vendor/autoload.php';


$purchaseId = $_POST['purchaseId'];
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
$Deputation = $_POST['Deputation'];
$catTownsId = $_POST['catTownsId'];
$CelPhone = $_POST['CelPhone'];
$Email = $_POST['Email'];
$social_reason = $_POST['social_reason'];
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
$CityAddressCompany = $_POST['CityAddressCompany'];
$Income = $_POST['Income'];
$ReferenceCustomer1 = $_POST['ReferenceCustomer1'];
$idReference1 = $_POST['idReference1'];
$ReferenceCustomerPhone1 = $_POST['ReferenceCustomerPhone1'];
$idReference2 = $_POST['idReference2'];
$ReferenceCustomer2 = $_POST['ReferenceCustomer2'];
$ReferenceCustomerPhone2 = $_POST['ReferenceCustomerPhone2'];
$paymentPlanLabelDesc = $_POST['paymentPlanLabelDesc'];
$cryptKeyLabel = $_POST['cryptKeyLabel'];
$levelLabel = $_POST['levelLabel'];
$areaLabel = $_POST['areaLabel'];
$zoneLabel = $_POST['zoneLabel'];
$payment_type = $_POST['payment_type'];
$inMaintenance = $_POST['inMaintenance'];
$inAshDeposit = $_POST['inAshDeposit'];
$tuition = "";

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
//$logoBase64 = base64_encode(file_get_contents('C:/xampp/htdocs/Church/Application/assets/img/LOGO_CATEDRAL_TIJUANA.png'));

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

<div class='content'>

<table class='table' cellspacing='0' cellpadding='0' style='border: none !important; width: 100%;'>
    <tbody>
        <tr>
             <td rowspan='4' style='border: none !important; vertical-align: top; width: 40%; text-align: center; position: relative;'>
                <img width='140px' height='140px' src='data:image/png;base64,<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>' alt='Logo' style='position: absolute; top: -50px;' />
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
                        <p class='s1' style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$tuition</p>
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
                $catTownsId</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p style='text-indent: 0pt;text-align: left;'>$neighborhood</p>
        </td>
        <td colspan = '2'
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                $catStatesId</p>
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
                $CivilStatus</p>
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
                $paymentPlanLabelDesc</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                $cryptKeyLabel</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$levelLabel</p>
        </td>
        <td 
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                $areaLabel</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p style='text-indent: 0pt;text-align: left;'>$zoneLabel</p>
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
                $ M.N.</p>
        </td>
        <td colspan='2' style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            >
            <p class='s1'
                style='padding-top: 1pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                $ M.N.</p>
        </td>
        <td colspan='2'
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 1pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$
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
                <strong></strong> 
                &nbsp;EN ABONOS:
                
                <!-- Checkbox para abonos mensuales (seleccionado por defecto si  es 'mensuales') -->
                &nbsp;&nbsp;<input style='vertical-align: middle; transform: scale(0.8);' type='checkbox' name='payment_type' value='mensuales' id='mensuales_checkbox' 
                MENSUALES
                
                <!-- Checkbox para abonos semanales -->
                &nbsp;&nbsp;<input style='vertical-align: middle; transform: scale(0.8);' type='checkbox' name='payment_type' value='semanales' id='semanales_checkbox' 
                
                <!-- Mostrar el valor calculado como texto -->
                &nbsp;&nbsp;POR LA CANTIDAD DE: 
                &nbsp;$ &nbsp; M.N.
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
                    </p>
            </td>
            <td style='border:solid 1pt;'>
                <p class='s1' style='padding-top: 1pt; padding-left: 2pt; text-indent: 0pt; line-height: 6pt; text-align: left;'>$ M.N.</p>
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
// Generar el PDF sin guardarlo en el servidor
$dompdf->loadHtml($html);
$dompdf->setPaper('A4', 'portrait');
$dompdf->render();

// Enviar el PDF directamente al navegador para descarga
header('Content-Type: application/pdf');
header('Content-Disposition: attachment; filename="solicitud.pdf"');
echo $dompdf->output();
exit();