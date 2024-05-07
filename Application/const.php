<?php

define('ROOT_PATH', __DIR__);
define('PUBLIC_PATH', __DIR__);
define('DS', DIRECTORY_SEPARATOR);
define('EOL', PHP_EOL);

if (!isset($argv)) {
	$argv = [isset($_SERVER['SCRIPT_FILENAME']) ? $_SERVER['SCRIPT_FILENAME'] : __FILE__];
	$args_list = isset($_GET['argv']) && is_array($_GET['argv']) ? $_GET['argv'] : $_GET;

	foreach ($args_list as $key => $value) {
		if ($value) $argv[] = $value;
	}
}

$document_root = realpath($_SERVER['DOCUMENT_ROOT']);
$server_name = $_SERVER['SERVER_NAME'];
$request_uri = $_SERVER['REQUEST_URI'];
$request_method = $_SERVER['REQUEST_METHOD'];
$current_page = basename($_SERVER['PHP_SELF']);
$services = "https://37e3-2806-2f0-1000-fb9b-cded-15f6-ec5c-6112.ngrok-free.app/";


$document_url = (isset($_SERVER['HTTPS']) && $_SERVER['HTTPS'] == 'on' ? 'https' : 'http').'://'.$server_name;

$app_path = $document_root;
$app_url = $document_url;

$app_uri = '/';
if (strpos(PUBLIC_PATH, $document_root) === 0) {
	$app_uri = substr(PUBLIC_PATH, strlen($document_root));
	$app_path .= $app_uri;
	$app_url .= str_replace(DS, '/', $app_uri);
}

define('DOCUMENT_ROOT', $document_root);
define('SERVER_NAME', $server_name);
define('SERVER_URL', $document_url);
define('SERVER_REQUEST', $document_url.$request_uri);
define('API_SERVICES', $services);

define('REQUEST_URI', $request_uri);
define('REQUEST_METHOD', $request_method);

define('APP_PATH', $app_path);
define('APP_URL', $app_url);
define('APP_URI', $app_uri);
define('APP_CURRPAGE', $current_page);

define('APP_LOGS_PATH', ROOT_PATH.'/logs');
define('APP_ARCHIVE_PATH', ROOT_PATH.'/archive');
define('APP_CACHE_PATH', ROOT_PATH.'/cache');
define('APP_TMP_PATH', ROOT_PATH.'/tmp');

define('ASSETS_URL', APP_URL.'/assets');
define('ASSETS_PATH', APP_PATH.'/assets');
define('DOCS_PATH', APP_PATH.'/docs');
define('INCLUDES_PATH', APP_PATH.'/inc');

define('API_URL', APP_URL.'/api');
define('API_URI', APP_URI.'/api');

define('AJAX_URL', APP_URL.'/ajax');
