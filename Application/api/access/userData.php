<?php
require_once 'vendor/autoload.php'; 
use \Firebase\JWT\JWT;
use Firebase\JWT\Key;
class UserDataRequest
{
    public static function encode($token){
        $secretKey = "CHURCHmnjhbvgfcrdexcfrvgbhnjmhgvfrcvgbhnyv234dfg";
        $result = array();
        try {
            // Decodifica el token y obtén los claims
            $decoded = JWT::decode($token, new Key($secretKey, 'HS256'));
        
            // Accede a los claims del token decodificado
            $result['role_id'] = isset($decoded->role_id) ? intval($decoded->role_id) : 0;
            $result['name'] = isset($decoded->name) ? $decoded->name : '';
            $result['email'] = isset($decoded->email) ? $decoded->email : '';
            $result['role'] = isset($decoded->role) ? $decoded->role : '';
            $result['user_id'] = isset($decoded->user_id) ? intval($decoded->user_id) : 0;
        
            // Retorna el array con las variables
            return $result;
            
        
        } catch (Exception $e) {
            // En caso de error al decodificar el token
            $result = "Error: " . $e->getMessage();
        }
    }
}


?>