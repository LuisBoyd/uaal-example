<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.9" tiledversion="1.9.2" name="PathTiles" tilewidth="512" tileheight="512" tilecount="44" columns="11">
 <image source="Texture2D/PathTiles.png" width="5632" height="2048"/>
 <tile id="36">
  <properties>
   <property name="AddresablePath" value="Assets/2D/Tiles/LogicTiles/PathTiles/NewPathTiles.asset"/>
  </properties>
 </tile>
 <wangsets>
  <wangset name="Grass" type="corner" tile="-1">
   <wangcolor name="Path" color="#ff0000" tile="36" probability="1"/>
   <wangtile tileid="4" wangid="0,0,0,1,0,0,0,0"/>
   <wangtile tileid="5" wangid="0,0,0,0,0,1,0,0"/>
   <wangtile tileid="6" wangid="0,1,0,1,0,1,0,0"/>
   <wangtile tileid="15" wangid="0,1,0,0,0,0,0,0"/>
   <wangtile tileid="16" wangid="0,0,0,0,0,0,0,1"/>
   <wangtile tileid="17" wangid="0,0,0,1,0,1,0,1"/>
   <wangtile tileid="26" wangid="0,1,0,1,0,0,0,0"/>
   <wangtile tileid="27" wangid="0,0,0,0,0,1,0,1"/>
   <wangtile tileid="28" wangid="0,1,0,0,0,1,0,1"/>
   <wangtile tileid="36" wangid="0,1,0,1,0,1,0,1"/>
   <wangtile tileid="37" wangid="0,0,0,1,0,1,0,0"/>
   <wangtile tileid="38" wangid="0,1,0,0,0,0,0,1"/>
   <wangtile tileid="39" wangid="0,1,0,1,0,0,0,1"/>
  </wangset>
 </wangsets>
</tileset>
