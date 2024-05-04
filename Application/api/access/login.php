<?php
require_once('../../init.php');


if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Obtener datos del formulario
    $email = $_POST["email"];
    $password = $_POST["password"];

    $data = array("email" => $email, "password" => $password);
    $url = API_SERVICES .'access/login' . '?' . http_build_query($data);

    $ch = curl_init($url);

    // Configurar cURL para una solicitud GET
    curl_setopt($ch, CURLOPT_HTTPGET, true);

    // Configurar para que cURL devuelva la respuesta en lugar de imprimirlo
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);

    // Ejecutar la solicitud cURL
    $response = curl_exec($ch);
 
    // Procesar la respuesta del servicio de login
    if ($response === false) {
        echo "Error al enviar la solicitud al servicio de login.";
    } else {
        // Obtener el código de estado HTTP de la respuesta
        $http_code = curl_getinfo($ch, CURLINFO_HTTP_CODE);

        // Verificar el código de estado HTTP
        if ($http_code == 200) {
            // Inicio de sesión exitoso
            session_start();
            $_SESSION['token'] = $response;
            $code=1;
            $mensaje = "";
            

        } elseif ($http_code == 400) {
            // Error de credenciales incorrectas
            $code = 2;
            $mensaje =  "Error: " . $response;
        } else {
            // Otro tipo de error
            $code = 2;
            $mensaje = "Error desconocido. Código de estado HTTP: " . $http_code;
        }
   }
    curl_close($ch); 
    // Cerrar la sesión cURL
    $json_obj = array("caso"=> $code,"mensaje" => $response);
    echo json_encode($json_obj, JSON_PRETTY_PRINT);
}

?>