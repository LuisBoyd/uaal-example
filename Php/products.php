<?php



require_once('db_mysql.php');
require_once('system_permissions.php');

$http_verb = $_SERVER['REQUEST_METHOD'];

$params    = json_decode(file_get_contents('php://input'));



require_once('authentications.php');

$user_id = Authenticate($params["username"], $params["password"]);

if(checkPermissions($user_id,1) == false){
    header("HTTP/1.0 403 Forbidden");
    echo $user_id;
    echo '{"error": "You do not have permissions to create a product."}' . '\n';

    exit();
}

