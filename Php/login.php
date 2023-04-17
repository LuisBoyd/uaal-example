<?php
require_once('db_mysql.php');
require_once('UserLoginResponse.php');
$http_verb = $_SERVER['REQUEST_METHOD'];
$parms = json_decode(file_get_contents('php://input'));
$mysql = new MySqlDb();

$userDetails = $mysql->getData("system_users", "username = '$parms->username'");
//var_dump($userDetails);
//if the user details is not set we know there is no user with that username in the db
if(!isset($userDetails)){
    header("HTTP/1.0 401 Unauthorized");
    header("Error: username or password is invalid");
    echo json_encode(new UserLoginResponse(-1, "", -1,
    -1,-1,-1,-1,-1));
    exit();
}

//Check password or hash if the (hashed+salted) password entered
//and the (hashed+salted) stored is not the same in which case error.
if(!password_verify($parms->password, $userDetails["hashedpwd"])){
    header("HTTP/1.0 401 Unauthorized");
    header("Error: username or password is invalid");
    echo json_encode(new UserLoginResponse(-1, "", -1,
        -1,-1,-1,-1,-1));
    exit();
}
else{
    echo json_encode(new UserLoginResponse("Success: Logged in", true));
}