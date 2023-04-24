<?php
class PlotObj{

    public int $Id;
    public int $OwningMarinaId;
    public string $Tile_Data;
    public int $Plot_index;

    function __construct(int $Id, int $OwningMarinaId, string $Tile_Data, int $Plot_index){
        $this->Id = $Id;
        $this->OwningMarinaId = $OwningMarinaId;
        $this->Tile_Data = $Tile_Data;
        $this->Plot_index = $Plot_index;
    }

}