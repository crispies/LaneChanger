# LaneChanger
Lane control for Cities: Skylines

*Take control of your intersections:*
- No left turn for 3- and 4-way intersections.
- No right turn for 4-way intersections.

How to use:
- Click on "Lanes" button at top left.
- Click on a road segment, and a pop-up will appear with buttons to toggle Left, Forward, and Right for each lane.
- Two rows of buttons will appear, one for each end of the road segment.
- Existing traffic that is on the road and has worked out its path, will not be effected by the changes, so it will take a few minutes (or more in a big city) for lane changes to take effect.

Warning:
- This mod will conflict with other mods that modify the stock pathfinder.  Mods that modify only vehicle AI should be fine.
- There will definitely be a performance hit running this mod, especially in its current not-quite-beta state.
- Shouldn't change anything that would make it impossible to load a save without it, but no guarantees.  Back up your save files and proceed with caution.

What works:
- Editing lane flags to select Left/Right/Forward.  This updates the graphics.
- Setting No-Left turn on any 3- or 4- way intersection
- Setting No-Right turn on any 4-way intersection
- No-Right in a 3-way will work in some situations, but generally Right and Forward are treated the same way at 3-way intersections
- Any unsupported situations are handled by the stock code

What doesn't work:
- Any intersections larger than 4-way will display stock behaviour, regardless of what you set the lanes to in the UI
- Highways aren't supported right now, you can change the arrows, but cars will ignore them.
- Cars coming from a one-way street onto a two-way street with a no left turn use odd lanes, sometimes turning off the one-way in inappropriate lanes
- Possible issue with lane markers not drawing on initial load of map.  This has happened a couple of times in testing, but I have not been able to reproduce consistently.
- A segment will have its lanes reset to default if you upgrade it, or build or upgrade any road segments that are attached to it.

TODO:
- Fix fugly UI
- Add no-right turn support in general case
- Add appropriate props (no turn signs, etc)
- Support larger intersections (5-way+)
- For later on: Further intersection enhancements: light controls, service vehicle lanes, etc.

Github:
https://github.com/crispies/LaneChanger

What you can do:
- Feedback, bug reports, pull requests, want to help?... all very welcome!
