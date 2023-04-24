<?php
require_once('db_mysql.php');
$parms = json_decode(file_get_contents('php://input'));
$mysql = new MySqlDb();

//TODO change this to plots later as well as when someone buys a plot copy data over
if(!$mysql->DoesRowExist("DefaultMarinaPlots", "pointOfInterestId = '$parms->MarinaID'")){
    header("HTTP/1.0 513 Row Not Found In Database");
    header("Error: Already Own A Marina Of This Type");
    exit();
}

$PlotData = $mysql->getFieldsFromTableWhere("DefaultMarinaPlots", "pointOfInterestId = '$parms->MarinaID'",
"default_plot_data");
$PlotData = $PlotData->fetch_assoc();
if(!isset($PlotData)){
    header("HTTP/1.0 500 Internal Server Error");
    header("Error: Could not find Default Plot");
    exit();
}

$PlotData = $PlotData["default_plot_data"];

print_r($PlotData);
