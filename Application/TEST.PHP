<?PHP
$path = 'C:/xampp/htdocs/Church/Application/assets/img/LOGO_CATEDRAL_TIJUANA.png';
$base64 = base64_encode(file_get_contents($path));
echo $base64; // Copia este resultado

?>