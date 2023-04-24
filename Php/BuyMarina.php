<?php
require_once('db_mysql.php');
$parms = json_decode(file_get_contents('php://input'));
$mysql = new MySqlDb();

//Check if Marina in pointsofinterest table exists.
if(!$mysql->DoesRowExist("pointsofinterest", "pointOfInterestId = '$parms->MarinaID'")){
    header("HTTP/1.0 513 Row Not Found In Database");
    header("Error: Already Own A Marina Of This Type");
    exit();
}

//Check first of all if we already own this marina in which case respond with a custom Error
if($mysql->DoesRowExist("user_marinas", "marinaId = '$parms->MarinaID'",
"system_user_id = '$parms->UserID'")){
    header("HTTP/1.0 512 Resource In Database");
    header("Error: Already Own A Marina Of This Type");
    exit();
}

//Check if row Exists for default plot
if(!$mysql->DoesRowExist("DefaultMarinaPlots", "pointOfInterestId = '$parms->MarinaID'")){
    header("HTTP/1.0 513 Row Not Found In Database");
    header("Error: There is no Default Plot Assigned");
    exit();
}

//Get User username
$Username = $mysql->getFieldsFromTableWhere("system_users","user_id = '$parms->UserID'",
"username");
$Username = $Username->fetch_assoc();
if(!isset($Username)){
    header("HTTP/1.0 500 Internal Server Error");
    header("Error: Could not find Username");
    exit();
}
$Username = $Username["username"];


//First get the Default plot for that specified marina.
$defaultMarinaPlot = $mysql->getFieldsFromTableWhere("DefaultMarinaPlots", "pointOfInterestId = '$parms->MarinaID'",
    "default_plot_data");
$defaultMarinaPlot = $defaultMarinaPlot->fetch_assoc();
if(!isset($defaultMarinaPlot)){
    header("HTTP/1.0 500 Internal Server Error");
    header("Error: Could not find Default Plot");
    exit();
}
$defaultMarinaPlot = $defaultMarinaPlot["default_plot_data"];
var_dump("This is the Default Marina Plot '$defaultMarinaPlot'");
//Create a User Map for the Inputted userID and Marina ID.
$data = $mysql->createData("user_marinas", "(marinaId, system_user_id, user_username)",
    "('$parms->MarinaID', '$parms->UserID', '$Username')");

var_dump($data);
if($data == null){
    header("HTTP/1.0 500 Internal Server Error");
    header("Error: Failed to Create Table");
    exit();
}
$MarinaID = $mysql->getFieldsFromTableWhere("user_marinas", "marinaId = '$parms->MarinaID' AND 
system_user_id = '$parms->UserID'", "id");
$MarinaID = $MarinaID->fetch_assoc();
if(!isset($MarinaID)){
    header("HTTP/1.0 500 Internal Server Error");
    header("Error: Could not find Marina ID");
    exit();
}
$MarinaID = $MarinaID["id"];
var_dump("This is the ID '$MarinaID'");

if($PlotData = $mysql->createData("Plots", "(OwningMarinaID, TileData, Plot_Index)",
    "('$MarinaID', '$defaultMarinaPlot', 0)") == null){
    header("HTTP/1.0 500 Internal Server Error");
    header("Error: Failed to Create Table");
    exit();
}
//Create A initial first plot for the newly bought plot

