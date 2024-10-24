<?php
require '../../vendor/autoload.php';

$CustomerSelect = $_POST['CustomerSelect'];
$CustomerID = $_POST['CustomerID'];
$UserID = $_POST['UserID'];
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
$inMaintenance = $_POST['inMaintenance'] == 'False' ? false : true;
$inAshDeposit = $_POST['inAshDeposit'] == 'False' ? false : true;
$inOtherFee = $_POST['inOtherFee'] == 'False' ? false : true;
$TypePay = $_POST['TypePay'];
$amount_cash = $_POST['amount_cash'];

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

// Pagos (arreglo)
$payments = [];
if (isset($_POST['payments'])) {
    foreach ($_POST['payments'] as $key => $payment) {
        $payments[] = [
            'paymentAmount' => $payment['paymentAmount'],
            'concept' => $payment['concept'],
            'typePaymentId' => $payment['typePaymentId'],
            'currencyId' => $payment['currencyId']
        ];
    }
}

// Otros datos de la operación
$paymentPlan = $_POST['paymentPlan'];
$cryptKey = $_POST['cryptKey'];
$level = $_POST['level'];
$area = $_POST['area'];
$zone = $_POST['zone'];
$totalAmount = $_POST['totalAmount'];
$appliedDiscount = $_POST['appliedDiscount'];
$initialPayment = $_POST['initialPayment'];
$balance = $_POST['balance'];

 // Fecha actual
 $fechaActual = new DateTime();


 // Formatear la fecha para mostrar el día, mes y año
 $diaSolicitud = $fechaActual->format('d');
 $mesSolicitud = $fechaActual->format('m');
 $ySolicitud = $fechaActual->format('Y');

use Dompdf\Dompdf;

// Crear instancia de DOMPDF
$dompdf = new Dompdf();
//$logoBase64 = base64_encode(file_get_contents("C:/xampp/htdocs/Church/Application/assets/img/LOGO_CATEDRAL_TIJUANA.png"));

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
            font-size: 5pt; /* Reduce el tamaño de la fuente */
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
        font-size: 9pt;
    }

    .s1 {
        color: black;
        font-family: Arial, sans-serif;
        font-style: normal;
        font-weight: normal;
        text-decoration: none;
        font-size: 6pt;
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
        font-size: 8pt;
        margin: 0; 
        padding: 4pt 0 0 0; 
        text-align: center;
    
    }
    .watermark-text {
        position: absolute;
        top: 50%;
        left: 50%;
        font-size: 50pt;
        color: rgba(230, 60, 80, 0.5); 
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
        font-size: 8pt;
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
            <td rowspan='4' style='border: none !important; vertical-align: top; width: 40%; text-align: center;'>
                <img width='100px' height='100px' src='C:/xampp/htdocs/Church/Application/assets/img/LOGO_CATEDRAL_TIJUANA.png' alt='Logo'/>
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
<table style='width:100%; border-collapse: collapse;margin-bottom: 15px;'>
    <tr>
        <td style='vertical-align: top; width: auto;'>
            <!-- Primera tabla aquí -->
            <table style='border-collapse:collapse;' cellspacing='0'>
                <tr style='height:13pt'>
                    <td style='background-color:#9fc5e8; width:100pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
                        colspan='3'>
                        <p class='s2' style='padding-top: 5pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>FECHA</p>
                    </td>
                </tr>
                <tr style='height:13pt'>
                    <td style='width:30pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s2' style='padding-top: 5pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>DÍA</p>
                    </td>
                    <td style='width:35pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s2' style='padding-top: 5pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>MES</p>
                    </td>
                    <td style='width:35pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s2' style='padding-top: 5pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>AÑO</p>
                    </td>
                </tr>
                <tr style='height:10pt'>
                    <td style='width:55pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s1' style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$diaSolicitud</p>
                    </td>
                    <td style='width:56pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s1' style='padding-top: 2pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$mesSolicitud</p>
                    </td>
                    <td style='width:55pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s1' style='padding-top: 2pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$ySolicitud</p>
                    </td>
                </tr>
            </table>
        </td>
        <td style='vertical-align: top; text-align: right;'>
            <!-- Segunda tabla aquí -->
            <table style='border-collapse:collapse; float: right;' cellspacing='0'>
                <tr style='height:10pt'>
                    <td style='background-color:#9fc5e8; width:80pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s1' style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>SOLICITUD No.</p>
                    </td>
                    <td style='width:100pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p style='text-indent: 0pt;text-align: left;'><br /></p>
                    </td>
                </tr>
                <tr style='height:10pt'>
                    <td style='background-color:#9fc5e8; width:80pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s1' style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>CLIENTE No.</p>
                    </td>
                    <td style='width:100pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s1' style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>Adriana Quintero Pérez</p>
                    </td>
                </tr>
                <tr style='height:10pt'>
                    <td style='background-color:#9fc5e8; width:80pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s1' style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>CONTRATO No.</p>
                    </td>
                    <td style='width:100pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s1' style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>C-JC-000176-A/C</p>
                    </td>
                </tr>
                <tr style='height:10pt'>
                    <td style='background-color:#9fc5e8; width:80pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s1' style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>VENDEDOR No.</p>
                    </td>
                    <td style='width:100pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s1' style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>Adriana Quintero Pérez</p>
                    </td>
                </tr>
                <tr style='height:10pt'>
                    <td style='background-color:#9fc5e8; width:80pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s1' style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>SERVIDOR</p>
                    </td>
                    <td style='width:100pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                        <p class='s1' style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>Adriana Quintero Pérez</p>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>

<h2 style='padding-top: 1pt;padding-bottom: 1pt;text-indent: 0pt;text-align: center;'>DATOS DEL SOLICITANTE
</h2>
<table style='border-collapse:collapse;width:100%;' cellspacing='0'>
    <tr style='height:13pt'>
        <td
            style='background-color:#9fc5e8; width:150pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2'
                style='padding-top: 5pt;padding-left: 57pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                APELLIDO PATERNO</p>
        </td>
        <td
            style='background-color:#9fc5e8; width:150pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 5pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>MATERNO</p>
        </td>
        <td
            style='background-color:#9fc5e8; width:150pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 5pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>NOMBRE(S)</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td
            style='width:150pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$PSurname
            </p>
        </td>
        <td
            style='width:150pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$MSurname
                </p>
        </td>
        <td
            style='width:150pt;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$Name</p>
        </td>
    </tr>
</table>
<h2 style='padding-top: 1pt;padding-bottom: 1pt;text-indent: 0pt;text-align: center;'>DOMICILIO
    PARTICULAR</h2>
<table style='border-collapse:collapse;width:100%;' cellspacing='0'>
    <tr style='height:10pt'>
        <td colspan = '2'
            style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 2pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>CALLE, AV.,
                BLVD. CALZ</p>
        </td>
        <td
            style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 2pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>NUMERO</p>
        </td>
        <td
            style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 2pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>INTERIOR
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
            style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 2pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>COLONIA</p>
        </td>
        <td style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            colspan='2'>
            <p class='s2' style='padding-top: 2pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>CODIGO POSTAL</p>
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
            style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 2pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>CIUDAD</p>
        </td>
        <td
            style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2'style='padding-top: 2pt;padding-left: 33pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>DELEGACION</p>
        </td>
        <td colspan = '2'
            style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 2pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>ESTADO</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                AGUASCALIENTES</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p style='text-indent: 0pt;text-align: left;'><br /></p>
        </td>
        <td colspan = '2'
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                AGUASCALIENTES</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td colspan = '2'
            style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 2pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>TELEFONO
                PARTICULAR</p>
        </td>
        <td style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            colspan='2'>
            <p class='s2'
                style='padding-top: 2pt;padding-left: 70pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                CORREO ELECTRONICO</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td colspan = '2'
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                $CelPhone</p>
        </td>
        <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            colspan='2'>
            <p style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'><a
                    href='mailto:$Email' class='s3'>$Email</a></p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td
            style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 2pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>R.F.C./C.U.R.P.</p>
        </td>
        <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            colspan='3'>
            <p style='text-indent: 0pt;text-align: left;'>$RFCCURP</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td
            style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 2pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>FECHA DE
                NACIMIENTO</p>
        </td>
        <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            colspan='3'>
            <p style='text-indent: 0pt;text-align: left;'>$DateOfBirth</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td
            style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 2pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>LUGAR DE
                NACIMIENTO</p>
        </td>
        <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            colspan='3'>
            <p style='text-indent: 0pt;text-align: left;'>$CityOfBirth</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td
            style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 2pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>ESTADO
                CIVIL</p>
        </td>
        <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            colspan='3'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                $CivilStatus</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td
            style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 2pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>OCUPACION
            </p>
        </td>
        <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            colspan='3'>
            <p style='text-indent: 0pt;text-align: left;'>$Occupation</p>
        </td>
    </tr>
</table>
<h2 style='padding-top: 1pt;padding-bottom: 0pt;text-indent: 0pt;text-align: center;'>DATOS DE LA
    EMPRESA DONDE PRESTA SUS SERVICIOS</h2>
<table style='border-collapse:collapse;width:100%;' cellspacing='0'>
    <tr style='height:10pt'>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
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
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                DOMICILIO</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
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
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>EXT: $ExtPhoneCompany
            </p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                CIUDAD</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                MUNICIPIO</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                ESTADO</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                AGUASCALIENTES</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p style='text-indent: 0pt;text-align: left;'><br /></p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                AGUASCALIENTES</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            colspan='2'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                INGRESO PROMEDIO MENSUAL (Incluye a su conyuge)</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p style='text-indent: 0pt;text-align: left;'>$Income</p>
        </td>
    </tr>
</table>
<h2 style='padding-top: 1pt;padding-bottom: 0pt;text-indent: 0pt;text-align: center;'>REFERENCIAS
</h2>
<table style='border-collapse:collapse;width:100%;' cellspacing='0'>
    <tr style='height:10pt'>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>1)
                $ReferenceCustomer1</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>TEL
            </p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                $ReferenceCustomerPhone1</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>2)
                $ReferenceCustomer2
            </p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>TEL
            </p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p style='text-indent: 0pt;text-align: left;'>$ReferenceCustomerPhone2</p>
        </td>
    </tr>
</table>
<h2 style='padding-top: 1pt;padding-bottom: 0pt;text-indent: 0pt;text-align: center;'>BENEFICIARIOS</h2>
<table style='border-collapse:collapse;width:100%;' cellspacing='0'>
    <tr style='height:10pt'>
        <td style=' background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt' colspan='2'>
            <p style='text-indent: 0pt;text-align: left;'><br /></p>
        </td>
        <td style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt' colspan='3'>
            <p class='s2' style='padding-top: 2pt;padding-left: 25pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>FECHA NAC.</p>
        </td>
        <td style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt' colspan='2'>
            <p class='s2' style='padding-top: 2pt;padding-left: 8pt;text-indent: 0pt;line-height: 6pt;text-align: left;'><br/></p>
        </td>
 
    </tr>
    <tr style='height:12pt'>
        <td style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 2pt;text-indent: 0pt;text-align: center;'>APELLIDOS</p>
        </td>
        <td style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 2pt;text-indent: 0pt;text-align: center;'>NOMBRES(S)</p>
        </td>
        <td style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s4' style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 8pt;text-align: left;'>DIA</p>
        </td>
        <td style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s4' style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 8pt;text-align: left;'>MES</p>
        </td>
        <td style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s4' style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 8pt;text-align: left;'>AÑO</p>
        </td>
        <td style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p style='text-indent: 0pt;text-align: left;'>PARENTESCO</p>
        </td>
        <td style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p style='text-indent: 0pt;text-align: left;'>TELEFONO</p>
        </td>
    </tr>"; 

    if (!empty($beneficiaries)) {
        // Si hay beneficiarios, recorrerlos y generar filas
        foreach ($beneficiaries as $beneficiary) {
            // Convertir la fecha de nacimiento en formato día, mes, año
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
    } else {
        // Si no hay beneficiarios, mostrar una fila vacía
        $html .= "
        <tr style='height:12pt'>
            <td colspan='7' style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                <p style='text-indent: 0pt;text-align: center;'>Ningún beneficiario registrado</p>
            </td>
        </tr>";
    }

$html .= "</table>
<h2 style='padding-top: 1pt;padding-bottom: 1pt;text-indent: 0pt;text-align: center;'>CONDICIONES
    ECONOMICAS DE LA OPERACION</h2>
<table style='border-collapse:collapse;width:100%;' cellspacing='0'>
    <tr style='height:10pt'>
        <td
            style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2'
                style='padding-top: 2pt;padding-left: 20pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>PLAN
                DE VENTA</p>
        </td>
        <td
            style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2'
                style='padding-top: 2pt;padding-left: 12pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                CLAVE DE LA CRIPTA</p>
        </td>
        <td
            style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 2pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>NIVEL</p>
        </td>
        <td
            style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 2pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>AREA</p>
        </td>
        <td
            style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 2pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>ZONA</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                $paymentPlan</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                $cryptKey</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$level</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                $area</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p style='text-indent: 0pt;text-align: left;'>$zone</p>
        </td>
    </tr>

</table>
<table>
    <tr style='height:10pt'>
        <td style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            >
            <p class='s2' style='padding-top: 2pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>IMPORTETOTAL</p>
        </td>
        <td style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            >
            <p class='s2' style='padding-top: 2pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>PAGO
                INICIAL</p>
        </td>
        <td
            style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s2' style='padding-top: 2pt;text-indent: 0pt;line-height: 6pt;text-align: center;'>SALDO</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            >
            <p class='s1'
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                $$totalAmount M.N.</p>
        </td>
        <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
            >
            <p class='s1'
                style='padding-top: 2pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                $$initialPayment M.N.</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 1pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>$$balance
                M.N.</p>
        </td>
    </tr>
</table>
<table>";



if (($TypePay)!= 1) {
     $html .= "
    <tr>
                           
        <td >EL SALDO SERA LIQUIDADO EN <?= $selectedPaymentDescription; ?> EN ABONOS DE: $ <?= number_format($mensualidades,2); ?> MXN. C/U</td>
        
    </tr>";
}else{
    $html .= "
    <tr>
                           
        <td >EL SALDO SERA LIQUIDADO EN <?= $selectedPaymentDescription; ?> EN ABONOS DE: $ <?= number_format($mensualidades,2); ?> MXN. C/U</td>
        
    </tr>";

    }
 $html .= "</table>
<h2 style='padding-top: 1pt;padding-bottom: 1pt;text-indent: 0pt;text-align: center;'>ADICIONAL
</h2>
<table style='border-collapse:collapse;width:100%;' cellspacing='0'>
    <tr style='height:10pt'>
        <td
            style='background-color:#9fc5e8;border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>CUOTA
                DE MANTENIMIENTO ANUAL</p>
        </td>
        <td
            style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                DEPÓSITO DE CENÍZAS</p>
        </td>
    </tr>
    <tr style='height:10pt'>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>NO
                APLICA</p>
        </td>
        <td
            style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
            <p class='s1'
                style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                APLICA - $920.00 M.N.</p>
        </td>
    </tr>
</table>
<p style='text-indent: 0pt;text-align: left;'><br /></p>
<div class='container'>
    
    <table style='border-collapse:collapse;width:100%;' cellspacing='0'>
        <tr style='height:10pt'>
             <td rowspan='2'
                style='background-color:#9fc5e8; border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                <p class='s1' style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                    FORMA DEL PAGO INICIAL</p>
            </td>
            <td
                style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                <p style='text-indent: 0pt;text-align: left;'><br /></p>
            </td>
            <td
                style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                <p class='s1'
                    style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                    CANTIDAD</p>
            </td>
            <td
                style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                <p class='s1'
                    style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>No. CHEQUE</p>
            </td>
            <td
                style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                <p class='s1'
                    style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>No.
                    DE CUENTA</p>
            </td>
            <td
                style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                <p class='s1'
                    style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>BANCO
                </p>
            </td>
        </tr>
        <tr style='height:10pt'>
            <td
                style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'>
                <p class='s1'
                    style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                    EFECTIVO</p>
            </td>
            <td style='border-top-style:solid;border-top-width:1pt;border-left-style:solid;border-left-width:1pt;border-bottom-style:solid;border-bottom-width:1pt;border-right-style:solid;border-right-width:1pt'
                colspan='4'>
                <p class='s1'
                    style='padding-top: 2pt;padding-left: 2pt;text-indent: 0pt;line-height: 6pt;text-align: left;'>
                    $600.00 M.N.</p>
            </td>
        </tr>
    </table>
</div>
<p style='text-indent: 0pt;text-align: left;'><br /></p>
<div class='textbox' style='border:0.5pt solid #000000;display:block;min-height:34.0pt;top:0.2pt;'>
    <h2 style='text-indent: 0pt;text-align: center;'>FIRMAS DEL REPRESENTANTE Y VENDEDOR</h2>
    <p style='padding-top: 2pt;text-indent: 0pt;text-align: center;'>Manifiesto que he recibido el pago inicial correspondiente a la presente operación de acuerdo a la forma que se especifica en la presente solicitud.</p>
    <p style='text-indent: 0pt;text-align: left;'><br /></p>
    <p style='text-indent: 0pt;text-align: left;'><br /></p>
    <table style='border-collapse: separate; border-spacing: 10px; width: 60%; margin: 0 auto; margin-top: 15px;'>
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
    <p class='s1' style='padding-top: 2pt;text-indent: 0pt;line-height: 87%;text-align: left;'>
        Con la aceptación de la presente solicitud me comprometo a firmar el contrato correspondiente una vez transcurrido el plazo de 15 días hábiles contados a partir de la 
        firma de esta solicitud y no habiendo hecho el uso del derecho de revocar mi consentimiento por la firma del contrato de cesión de derechos de uso mortuorio a 
        perpetuidad que ampara la presente operación, cuyos principales términos y condiciones son los establecidos en esta solicitud. Lo anterior sin responsabilidad 
        alguna que establece el Art. 48 de la Ley Federal de Protección al Consumidor. Igualmente me obligo al suscribir al momento de firmar el mencionado 
        contrato, el pagaré correspondiente a la presente operación para garantizar el saldo del valor o precio convenido. Declaro estar de acuerdo con la presente 
        solicitud, así como la información que he proporcionado es real y verídica.
        En caso de cancelación de compra antes de la firma del contrato, se aplicará el reembolso del 50% del pago realizado. Vigencia 15 días a partir de la firma de esta solicitud.
    </p>
</div>

<table style='border-collapse: separate; border-spacing: 10px; width: 40%; margin: 0 auto; margin-top: 20px;'>
    <tr>
        <td style='border-top: 1px solid black; text-align: center; padding: 5px;'>
        <h1 style='padding-top: 1pt;text-indent: 0pt;text-align: center;'>NOMBRE Y FIRMA DEL ADQUIRIENTE</h1>
        </td>
       
    </tr>
</table>

<!--<p style='padding-top: 3pt;padding-left: 8pt;text-indent: 0pt;text-align: center;'>
    Original: Expediente &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    Copia Azul: Adquiriente &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    Copia Rosa: Contabilidad
</p>-->
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