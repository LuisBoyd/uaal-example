<?php
require_once('db_mysql.php');
require_once('MarinaObj.php');
$params = json_decode(file_get_contents('php://input'));
$mysql = new MySqlDb();
//Get all Rows in User_marinas that belong to the systemUserID
$OwnedMarianaIds = $mysql->getFieldsFromTableWhere("user_marinas","system_user_id = '$params->UserID'", "marinaId");
//var_dump(implode("+", $OwnedMarianaIds));
$a = array();
$listOfOwnedPOIids = array();
foreach ($OwnedMarianaIds as $item) foreach ($item as $key=> $value){
    $OwnedPointsOfIntrest = $mysql->getData("pointsofinterest", "pointOfInterestId = '$value'");
    array_push($listOfOwnedPOIids,$OwnedPointsOfIntrest["pointOfInterestId"]);
    array_push($a, new MarinaObj($OwnedPointsOfIntrest["id"],
        $OwnedPointsOfIntrest["pointOfInterestId"], $OwnedPointsOfIntrest["name"],
        $OwnedPointsOfIntrest["buycost"],$OwnedPointsOfIntrest["basesellcost"],
    true));
}

$allEngWalesMarianas = null;
if(count($listOfOwnedPOIids) != 0){
    $allEngWalesMarianas = $mysql->getFieldsFromTableWhere("pointsofinterest","pointOfInterestId NOT IN (".implode(",",$listOfOwnedPOIids).")" ,"id", "pointOfInterestId", "name", "buycost", "basesellcost");
}
else{
    $allEngWalesMarianas = $mysql->getFieldsFromTable("pointsofinterest","id", "pointOfInterestId", "name", "buycost", "basesellcost");
}
foreach ($allEngWalesMarianas as $marina){
    array_push($a, new MarinaObj($marina["id"], $marina["pointOfInterestId"], $marina["name"], $marina["buycost"], $marina["basesellcost"],
    false));
}
//afterwards send it back
echo json_encode($a);

