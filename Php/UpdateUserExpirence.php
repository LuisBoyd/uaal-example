<?php
require_once('db_mysql.php');
$params = json_decode(file_get_contents('php://input'));
$mysql = new MySqlDb();
$mysql->updateData("system_users","Current_EXP = '$params->CurrentEXP'", "user_id = '$params->UserID'");
