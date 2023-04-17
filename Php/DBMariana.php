<?php

class DBMariana{

    public int $PointOfIntrestId;
    public string $name;

    //Might want to add lat, lon
    // Cos_lat, Cos_lng, Sin_lat, Sin_lng later on

    function __construct(int $PointOfIntrestId, string $name)
    {
        $this->name = $name;
        $this->PointOfIntrestId = $PointOfIntrestId;
    }
}