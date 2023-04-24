<?php
require_once('db_mysql.php');
$params = json_decode(file_get_contents('php://input'));
$mysql = new MySqlDb();
$marina = $mysql->getFieldsFromTableWhere("pointsofinterest",
"pointOfInterestId = '$params->MarinaID'", "id","pointOfInterestId",
"name","buycost","basesellcost");
$marina = $marina->fetch_assoc();
if (!isset($marina)){
    header("HTTP/1.0 401 Unauthorized");
    header("Error: Could Not Find Marina with ID");
    exit();
}
echo json_encode($marina);