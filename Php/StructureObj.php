<?php
class StructureObj{

    public int $Id;
    public int $Structure_type;
    public int  $OwningPlotID;
    public int $X;
    public int $Y;

    function __construct(int $Id, int $Structure_type, int $OwningPlotID,int $X, int $Y){
        $this->Id = $Id;
        $this->Structure_type = $Structure_type;
        $this->OwningPlotID = $OwningPlotID;
        $this->X = $X;
        $this->Y = $Y;
    }

}