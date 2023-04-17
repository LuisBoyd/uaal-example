<?php
require_once('db_mysql.php');
require_once('UserLoginResponse.php');
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
    header("HTTP/1.0 400 Bad Request");
    header("Error: username can not contain special characters");
    exit();
}

if(strlen($parms->username) < $minimumChar){
    header("HTTP/1.0 400 Bad Request");
    header("Error: a username must contain '$minimumChar' characters");
    exit();
}

//See if a user with the same username already exists.
$DuplicateUsernameCount = $mysql->GetCount("system_users","username = '$parms->username'");
if($DuplicateUsernameCount > 0){
    header("HTTP/1.0 409 Conflict");
    header("Error: please choose another username");
    exit();
}

if($passwordError){
    header("HTTP/1.0 400 Bad Request");
    header("Error: Passwords must have at least '$passXLength' characters and \n
    contain at least two of the following: uppercase letters,\n
    lowercase letters, numbers, and symbols");
    exit();
}

//HashPassword
$SaltAndHashedPwd = password_hash($parms->password, PASSWORD_DEFAULT);
//Store data in DB if not return an error.
if($mysql->createData("system_users",
        "(username, hashedpwd)", "('$parms->username', '$SaltAndHashedPwd')") == null){

    header("HTTP/1.0 500 Internal Server Error");
    header("Error: Server could not create user");
    exit();
}else{
    //Something may need to be here.
}

