<?php

class MarinaObj{

    public int $PKid;
    public int $POIid;
    public string $Name;
    public int $BuyCost;
    public int $BaseSellCost;
    public bool $OwnStatus;

    function __construct(int $primaryKeyId, int $PointofInterestID, string $marianaName, int $marinaBuyCost,
    int $marinaBaseSellCost, bool $ownerShipStatus){
        $this->PKid = $primaryKeyId;
        $this->POIid = $PointofInterestID;
        $this->Name = $marianaName;
        $this->BuyCost = $marinaBuyCost;
        $this->BaseSellCost = $marinaBaseSellCost;
        $this->OwnStatus = $ownerShipStatus;

    }

}