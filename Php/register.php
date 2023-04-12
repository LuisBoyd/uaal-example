<?php
require_once('db_mysql.php');
require_once ('Response.php');
$http_verb = $_SERVER['REQUEST_METHOD'];
$parms = json_decode(file_get_contents('php://input'));
$mysql = new MySqlDb();
$passwordError = false;

//Minimum password attribute count for password to succeed.
$PassAttributeCount = getenv("passwordAttributesLength");
$passAttributeCurrentCount = 0;
//Make sure username is atleast 4 characters Long
$minimumChar = getenv("username_min_char");

//make sure password is X in length
$passXLength = getenv("pass_x_length");
if(!isset($passXLength)) $passXLength = 0;

//Make Checks
if($passXLength != 0){
    if(strlen($parms->password) < $passXLength){
        $passwordError = true;
    }
}
//Check if password contains both characters and strings
if(preg_match('/\d/', $parms->password)){
    $passAttributeCurrentCount += 1;
}
if(preg_match('/[a-z]/', $parms->password)){
    $passAttributeCurrentCount += 1;
}

if(preg_match("/[a-z]/", $parms->password)){
    $passAttributeCurrentCount += 1;
}
//Check if character contains special character
if(!preg_match('/[\'^£$%&*()}{@#~?><>,|=_+¬-]/', $parms->password)){
    $passAttributeCurrentCount += 1;
}

if($passAttributeCurrentCount < $PassAttributeCount){
    $passwordError = true;
}

if(preg_match('/[\'^£$%&*()}{@#~?><>,|=_+¬-]/', $parms->username)){
    header("HTTP/1.0 500 Internal Server Error");
    echo json_encode(new Response("error: username can not contain special characters", false));
    exit();
}

if(strlen($parms->username) < $minimumChar){
    header("HTTP/1.0 500 a username must contain '$minimumChar' characters");
    echo json_encode(new Response("error: a username must contain '$minimumChar' characters", false));
    exit();
}

//See if a user with the same username already exists.
$DuplicateUsernameCount = $mysql->GetCount("system_users","username = '$parms->username'");
if($DuplicateUsernameCount > 0){
    header("HTTP/1.0 500 please choose another username");
    echo json_encode(new Response("error: please choose another username", false));
    exit();
}

if($passwordError){
    header("HTTP/1.0 500 Internal Server Error");
    echo json_encode(new Response("error: Passwords must have at least '$passXLength' characters and \n
    contain at least two of the following: uppercase letters,\n
    lowercase letters, numbers, and symbols", false));
    exit();
}

//HashPassword
$SaltAndHashedPwd = password_hash($parms->password, PASSWORD_DEFAULT);
//Store data in DB if not return an error.
if($mysql->createData("system_users",
        "(username, hashedpwd)", "('$parms->username', '$SaltAndHashedPwd')") == null){

    header("HTTP/1.0 500 Server could not create user");
    echo json_encode(new Response("error: Server could not create user", false));
    exit();
}else{
    echo json_encode(new Response("Success: Server created user", true));
}

