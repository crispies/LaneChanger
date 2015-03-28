Lane control for Cities: Skylines - Early Beta Update 1

Updated March 28, 2015 

Considerable fixes and improvements from previous release. All intersection types are supported now, up to 8-way. The UI has changed somewhat, see the usage notes below. If you have issues with the mod, please restart your game after disabling it. This is still a beta, although performance and stability should be much, much better. 

Take control of your intersections: 
-No left or right turn for any type of intersection. 
-Correct lane markings for most commmon cases.

How to use: 
-Click on "Lanes" button at top left. 
-Click on a road segment, the segment will turn dark blue. Click the "Enable Lane Control" button at the top left to turn on lane control for that segment, right-click to cancel your selection.
-Connected segments will turn green, click on the green segments to toggle them to red. Traffic from the blue segment is permitted to move to the green segments, but not the red segments. 
-Click on the "Disable Lane Control" button to turn off lane control for a segment, reverting to stock behaviour. 
-PLEASE NOTE: Existing traffic that is on the road and has worked out its path, will not be effected by the changes, so it will take a few minutes (or more in a big city) for lane changes to take effect. (In a city of ~50,000 this is about 3 mins at max game speed) 
-If you are not sure if you have set up the intersection the way you want, you can set a bus route through the intersection, the buses will obey the lane control, and you can see if they will turn at a particular intersection

Bugs fixed: 
-Public transit no longer locks up. Buses respect custom intersecion rules. 
-Service building/vehicles no longer freeze up. 
-Left-hand drive is supported, although lane markings may not be correct in some situations. This is still being worked on. 
-Considerable performance improvment. 
-Button moved to improve compatibility with traffic query tool. 
-Should no longer conflict with toggle traffic lights.

Warning: 
-This mod will conflict with other mods that modify the stock pathfinder. The only one I know of at the moment is Traffic++, which should not be used with Lane Changer. 
-There will definitely be a performance hit running this mod, especially in its current not-quite-beta state, though it is considerably improved from initial release. 
-Shouldn't change anything that would make it impossible to load a save without it, but no guarantees. Back up your save files and proceed with caution. You will likely need to restart the game after disabling the mod to get the stock pathfinder back.

What works: 
-No left/right/any direction, you can permit or deny movement from any segment to any other attached segment 
-Road marking should update correctly for no-left and no-right turn. Setting no straight through will work (cars will no go straight through), but lane markers will not change

What doesn't work: 
-Lane markings for more complex intersections (5-way+), cars will obey your settings, but lane markings are wrong 
-Car lane choices moving through edited intersections are not always optimal 
-Lane markings will sometimes get out of sync with your selections in the tool. Toggling Lane Control off and back on for that segment will reset the markings.

TODO: 
-Fix fugly UI 
-Improve lane markings to be more consistent 
-Add appropriate props (no turn signs, etc) 
-Add the option to exempt service and/or emergency vehicles from traffic rules. 
-For later on: Further intersection enhancements: light controls, service vehicle lanes, etc.
