<?php
require_once('db_mysql.php');
$params = json_decode(file_get_contents('php://input'));
$mysql = new MySqlDb();
$DateTime = $mysql->getFieldsFromTableWhere("system_users", "user_id = '$params->UserID'", "LastSavedTime");
$DateTime = $DateTime->fetch_assoc();
//$a = array();
//array_push($a, $DateTime["LastSavedTime"]);
try {
//    var_dump($DateTime["LastSavedTime"]);
    $date = new DateTime($DateTime["LastSavedTime"]);
    $date->setTimezone(new DateTimeZone("UTC"));
//    var_dump($date);
    echo json_encode($date->format(DATE_ATOM));
} catch (Exception $e) {
    var_dump($e);
}
//$date = strtotime($DateTime["LastSavedTime"]);
//echo json_encode(date("y-m-d H:i:s",$date));
