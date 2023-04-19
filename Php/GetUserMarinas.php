<?php
require_once('db_mysql.php');
$params = json_decode(file_get_contents('php://input'));
$mysql = new MySqlDb();
//Get all Rows in User_marinas that belong to the systemUserID
$SystemUserOwned = $mysql->getData("user_marinas", "system_user_id = '$params->UserID'");
//afterwards send it back
echo json_encode($SystemUserOwned);

