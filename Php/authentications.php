<?php

require_once('db_mysql.php');

function Authenticate($Username,$Password){
    if(!isset($Username) || !isset($Password)){
        header("HTTP/1.0 401 Unauthorized");

        echo '"{"error": "Authentication failed."}';

        exit();
    }
    $mysql = new MySqlDb();

    $stmt = $mysql->getData("system_users", "username = '$Username'");

    $user_id = "";
    if(password_verify($Password, $stmt['hashedpwd']) == true){
        $user_id = $stmt['user_id'];
    } //The stored hash is pwd
    else{
        header("HTTP/1.0 401 Unauthorized");

        echo '{"error": "Invalid username or password."}';
        exit();
    }
    return $user_id;
}