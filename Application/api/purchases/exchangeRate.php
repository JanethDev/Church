<?php
// Función para obtener el tipo de cambio actual
function obtenerTipoCambio() {
    // URL de ejemplo para obtener el tipo de cambio desde una API
    $url = "https://open.er-api.com/v6/latest/USD"; // Actualiza a una API válida.

    // Inicializamos cURL para hacer la solicitud HTTP
    $ch = curl_init();
    curl_setopt($ch, CURLOPT_URL, $url);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_TIMEOUT, 10); // Agregar un timeout de 10 segundos
    $response = curl_exec($ch);
    
    // Verificar si ocurrió un error en la conexión
    if (curl_errno($ch)) {
        curl_close($ch);
        return 'Error al conectar con la API';
    }

    curl_close($ch);

    // Decodificar la respuesta JSON
    $data = json_decode($response, true);

    // Verificar si obtuvimos la tasa de cambio correctamente
    if (isset($data['rates']['MXN'])) {
        return $data['rates']['MXN'];
    } else {
        return 'Error al obtener el tipo de cambio';
    }
}

$tipoCambio = obtenerTipoCambio();

if (is_numeric($tipoCambio)) {
    echo json_encode(['tipo_cambio' => $tipoCambio]);
} else {
    echo json_encode(['error' => $tipoCambio]);
}
?>
