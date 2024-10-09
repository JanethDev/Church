<?php
// Obtén el tipo de documento a descargar
$document = isset($_GET['doc']) ? $_GET['doc'] : '';

$filePath = '../uploads/docs/ReglamentoDelColumbario.pdf'; // Ruta del PDF

// Verifica si el archivo existe
if (file_exists($filePath)) {
    // Establece los encabezados para la descarga
    header('Content-Description: File Transfer');
    header('Content-Type: application/pdf');
    header('Content-Disposition: attachment; filename="'.basename($filePath).'"');
    header('Expires: 0');
    header('Cache-Control: must-revalidate');
    header('Pragma: public');
    header('Content-Length: ' . filesize($filePath));
    
    // Limpia el búfer de salida y lee el archivo
    ob_clean();
    flush();
    readfile($filePath);
    exit;
} else {
    die('El archivo no existe.'); // Manejo de errores
}
?>
