<?php

include "db_mysql.php";


$mysql = new MySqlDb();

$insert  = $mysql->createData("Levels", "(Id, EXPToLevel, RewardID)",
    "(1, 2756, 2456)");

print_r($insert);
