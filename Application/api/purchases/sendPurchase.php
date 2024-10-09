<?php

if (isset($_POST['beneficiaries'])) {
    $beneficiaries = $_POST['beneficiaries'];
    foreach ($beneficiaries as $beneficiary) {
        // Procesa cada beneficiario
        $name = $beneficiary['name'];
        $surnames = $beneficiary['surnames'];
        // etc.
    }
    // Convierte el arreglo de beneficiarios a JSON y lo imprime
    header('Content-Type: application/json');
    echo json_encode($beneficiaries);
}


