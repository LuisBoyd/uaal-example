<?php
require_once('db_mysql.php');
require_once('DBMariana.php');

$mysql = new MySqlDb();

//$MarianaSQLCall = $mysql->RawStatement("SELECT id, pointOfInterestId, name FROM `pointsofinterest`");
$MarianaSQLCall = $mysql->getFieldsFromTable("pointsofinterest", "id", "pointOfInterestId", "name", "buycost","basesellcost");
echo json_encode($MarianaSQLCall);