<?php
//1. take in a bunch of plot Id's
//2. Verify if there are any structures if there is not don't throw an HTTP error just give a log and exit out.
//3. make an array
//4. loop through Id's
//5. get all structures that match OwningPlotID
//6. push that to array
//7. output array at the end.
require_once('db_mysql.php');
$parms = json_decode(file_get_contents('php://input'));
$mysql = new MySqlDb();
require_once ('StructureObj.php');
$StructureArray = array();
$IdArray = $parms->Ids;
foreach ($IdArray as $id){
    foreach ($mysql->GetRows("Structures", "OwningPlotID = '$id'") as $Structure){
        array_push($StructureArray, new StructureObj($Structure["Structure_ID"],
        $Structure["Structure_Type"], $Structure["OwningPlotID"],
        $Structure["X"], $Structure["Y"]));
    }
}
echo json_encode($StructureArray);
//if(!$mysql->DoesRowExist("Structures", "marinaId = '$parms->MarinaID' AND system_user_id = '$parms->UserID'")){
//    header("HTTP/1.0 513 Row Not Found In Database");
//    header("Error: Could not find user map");
//    exit();
//}