<?php
if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $to = $_POST['email']; // Correo del destinatario
    $subject = "Confirmación de Compra";
    $message = "Gracias por su compra. Adjuntamos el documento correspondiente.";
    $headers = "From: no-reply@catedraltijuana.org";

    // Adjuntar el PDF si es necesario
    if (isset($_FILES['pdf'])) {
        $filePath = $_FILES['pdf']['tmp_name'];
        $fileName = $_FILES['pdf']['name'];

        // Boundary para el mensaje multipart
        $boundary = md5(time());
        $headers .= "\r\nContent-Type: multipart/mixed; boundary=\"{$boundary}\"";

        // Mensaje en formato multipart
        $message = "--{$boundary}\r\n";
        $message .= "Content-Type: text/plain; charset=\"utf-8\"\r\n\r\n";
        $message .= "Gracias por su compra. Adjuntamos el documento correspondiente.\r\n";

        // Adjuntar el archivo PDF
        $fileData = file_get_contents($filePath);
        $message .= "--{$boundary}\r\n";
        $message .= "Content-Type: application/pdf; name=\"{$fileName}\"\r\n";
        $message .= "Content-Disposition: attachment; filename=\"{$fileName}\"\r\n";
        $message .= "Content-Transfer-Encoding: base64\r\n\r\n";
        $message .= chunk_split(base64_encode($fileData));
        $message .= "--{$boundary}--";
    }

    // Enviar el correo
    if (mail($to, $subject, $message, $headers)) {
        echo json_encode(['status' => 'success', 'message' => 'Correo enviado con éxito.']);
    } else {
        echo json_encode(['status' => 'error', 'message' => 'Error al enviar el correo.']);
    }
} else {
    echo json_encode(['status' => 'error', 'message' => 'Método no permitido.']);
}
?>
