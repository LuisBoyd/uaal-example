<?php
require_once('db_mysql.php');
$params = json_decode(file_get_contents('php://input'));
$mysql = new MySqlDb();
$UserData = $mysql->getFieldsFromTableWhere("system_users",
"user_id = '$params->UserID'", "user_id", "username",
"Level", "Current_EXP", "Freemium_Currency", "Premium_Currency");
$UserData = $UserData->fetch_assoc();
if(!isset($UserData)){
    header("HTTP/1.0 401 Unauthorized");
    header("Error: Could not find Data");
    exit();
}
echo json_encode($UserData);