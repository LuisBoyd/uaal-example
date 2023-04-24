<?php
//1.Verification of owned map check. (pass in Marina Id and User ID)
//2. if the check of getting a row from the "user_marinas" by passing in the Marina ID and Uer ID is successful.
//3. hold the User Marina ID in a varible
//4. Get all tables from "Plots" from where the "OwningMarinaID" == "User Marina ID" from point 3.
//5. loop through each plot and send that back as json_encode in an array. I can create the plot object in php
require_once('db_mysql.php');
$parms = json_decode(file_get_contents('php://input'));
$mysql = new MySqlDb();

//Do user_marina Verification Check Start
if(!$mysql->DoesRowExist("user_marinas", "marinaId = '$parms->MarinaID' AND system_user_id = '$parms->UserID'")){
    header("HTTP/1.0 513 Row Not Found In Database");
    header("Error: Could not find user map");
    exit();
}
//Do user_marina Verification Check End

//Get User map ID Start
$User_Mairna_ID = $mysql->getFieldsFromTableWhere("user_marinas", "marinaId = '$parms->MarinaID' AND system_user_id = '$parms->UserID'",
"Id");
$User_Mairna_ID = $User_Mairna_ID->fetch_assoc();
if(!isset($User_Mairna_ID)){
    header("HTTP/1.0 500 Internal Server Error");
    header("Error: Could not get User_map_id");
    exit();
}
$User_Mairna_ID = $User_Mairna_ID["Id"];
//Get User map ID End
require_once ('PlotObj.php'); //Include Plot Obj
//Get all Rows from Plots Table Start
$PlotObjs = array();
$PlotObjsRequest = $mysql->GetRows("Plots", "OwningMarinaID = '$User_Mairna_ID'");
foreach ($PlotObjsRequest as $Plot){
    array_push($PlotObjs, new PlotObj($Plot["Plot_ID"], $Plot["OwningMarinaID"],
        $Plot["TileData"],$Plot["Plot_Index"]));
}
//Get all Rows from Plots Table End

echo json_encode($PlotObjs);