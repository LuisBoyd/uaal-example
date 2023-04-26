<?php
class PlotObj{

    public int $Id;
    public int $OwningMarinaId;
    public string $Tile_Data;
    public int $Plot_index_X;
    public int $Plot_index_Y;

    function __construct(int $Id, int $OwningMarinaId, string $Tile_Data, int $Plot_index_X,
    int $Plot_index_Y){
        $this->Id = $Id;
        $this->OwningMarinaId = $OwningMarinaId;
        $this->Tile_Data = $Tile_Data;
        $this->Plot_index_X = $Plot_index_X;
        $this->Plot_index_Y = $Plot_index_Y;
    }

}