<?php

function CheckStringContains($str, array $arr_CharWords){

    foreach ($arr_CharWords as $a){
        if(stripos($str, $a)!== false) return true;
    }
    return false;
}