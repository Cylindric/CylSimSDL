@ECHO OFF
CD %~dp0

SET BUILDROOT=%1
SET BUILDROOT=%BUILDROOT:"=%

SET TP="C:\Program Files\CodeAndWeb\TexturePacker\bin\TexturePacker.exe"
SET BASEOUTPUTPATH=%BUILDROOT%\assets\base
SET OPTIONS=--format xml --trim-sprite-names --extrude 0 --algorithm Basic 
SET OPTIONS=%OPTIONS% --trim-mode None --png-opt-level 0 --disable-auto-alias 
SET OPTIONS=%OPTIONS% --disable-rotation --size-constraints POT --force-word-aligned

ECHO Generating into %BASEOUTPUTPATH%

MD %BASEOUTPUTPATH%

SET OUTPUTPATH=%BASEOUTPUTPATH%\characters
%TP% %OPTIONS% --data %OUTPUTPATH%\colonist.xml --sheet %OUTPUTPATH%\colonist.png characters\colonist-assets

SET OUTPUTPATH=%BASEOUTPUTPATH%\furniture
%TP% %OPTIONS% --data %OUTPUTPATH%\furn_wall_steel.xml --sheet %OUTPUTPATH%\furn_wall_steel.png furniture\furn_wall_steel-assets
%TP% %OPTIONS% --data %OUTPUTPATH%\furn_door_heavy.xml --sheet %OUTPUTPATH%\furn_door_heavy.png furniture\furn_door_heavy-assets
%TP% %OPTIONS% --data %OUTPUTPATH%\furn_door.xml --sheet %OUTPUTPATH%\furn_door.png furniture\furn_door-assets
%TP% %OPTIONS% --data %OUTPUTPATH%\furn_oxygen.xml --sheet %OUTPUTPATH%\furn_oxygen.png furniture\furn_oxygen-assets
%TP% %OPTIONS% --data %OUTPUTPATH%\furn_mining_station.xml --sheet %OUTPUTPATH%\furn_mining_station.png furniture\furn_mining_station-assets

SET OUTPUTPATH=%BASEOUTPUTPATH%\backgrounds
MD %OUTPUTPATH%
XCOPY /Y Backgrounds\starfield.jpg %OUTPUTPATH%

SET OUTPUTPATH=%BASEOUTPUTPATH%\tiles
MD %OUTPUTPATH%
%TP% %OPTIONS% --data %OUTPUTPATH%\floor.xml --sheet %OUTPUTPATH%\floor.png Tiles\floor-assets


