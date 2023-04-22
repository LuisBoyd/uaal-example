<?php
function GetCurrentDateTime(){
    $CurrentDateTime = new DateTimeImmutable();
    $formattedDateTime = $CurrentDateTime->format("y-m-d H:i:s");
    return $formattedDateTime;
}