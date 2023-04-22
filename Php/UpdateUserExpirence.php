<?php
require_once('db_mysql.php');
require_once ('CurrentDateTime.php');
$params = json_decode(file_get_contents('php://input'));
$mysql = new MySqlDb();
$formattedDateTime = GetCurrentDateTime();
$mysql->updateData("system_users","Current_EXP = '$params->CurrentEXP', LastSavedTime = '$formattedDateTime'", "user_id = '$params->UserID'");
