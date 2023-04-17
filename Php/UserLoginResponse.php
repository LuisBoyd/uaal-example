<?php

class UserLoginResponse{
    public int $user_id;
    public string $username;
    public int $level;
    public int $current_exp;
    public int $taken_slots;
    public int  $free_slots;
    public int $freemium_currency;
    public int $premium_currency;

    function __construct(int $user_id, string $username,
        int $level, int $current_exp, int $taken_slots,
                         int  $free_slots,int $freemium_currency,
                         int $premium_currency
    ){
        $this->$user_id = $user_id;
        $this->$username = $username;
        $this->$level = $level;
        $this->$current_exp = $current_exp;
        $this->taken_slots = $taken_slots;
        $this->free_slots = $free_slots;
        $this->freemium_currency = $freemium_currency;
        $this->premium_currency = $premium_currency;
    }

}