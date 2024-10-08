<?php
// Función para obtener el tipo de cambio actual
function obtenerTipoCambio() {

    $token = '0f3d70f1cf4fd8f916a9d7736a2882a26ba374273d4defcb118a73e0336fea01';
    $url = 'https://www.banxico.org.mx/SieAPIRest/service/v1/series/SF43718/datos/oportuno?token=' . $token;


    // Inicializamos cURL para hacer la solicitud HTTP
    $ch = curl_init();
    // Configurar opciones de cURL
    curl_setopt($ch, CURLOPT_URL, $url);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);

    // Ejecutar la solicitud
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
    if (isset($data['bmx']['series'][0]['datos'][0]['dato'])) {
        return $data['bmx']['series'][0]['datos'][0]['dato'];
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
