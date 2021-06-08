RPG Creator and Runtime - Basic Explanation

Creating a Module - A module consists of 7 folders:

-Items
-Maps
-Moves
-Objects
-Scripts
-Sprites
-Tiles

This is where the runtime will search for things to run. All modules should be in the "modules" folder in the root of the runtime.

NOTE: Any time you're stuck on what to do, check out the default assets!
There are default assets in the folder labelled "defaultassets".

PART 1: Create Moves!

In the moves folder, simply create a text file labelled with the move name.

On the first line, define the name with two slashes:

//Attack Name

Code a move with anything you like! be sure to subtract from the target. if you need to access the target's stats, use $target.

For example:

//Basic Attack
$value = $this.attack
IF $value is less than 1 :
	$value = 1
$target.hp -= $value
PRINT "$this.name attacks for $value damage!"

PART 2: Create a character!

Generate art assets for your project. These should be saved as a .png image with a size of 32*32
Put those into the "Sprites" folder. The module will look for art in there, and only there. This rule 
applies to everything you create. 

Go into the NPCMaker folder. You can use the GUI to create an NPC. Submit a value for every primary value.
You can even add scripts to it to flesh out AI (Necessary for enemies, as there is no default AI).
Scripting commands will be explained below.

NOTE: As with everything in this system, more experienced users can create their own object.txt files from scratch.

Place the generated "objectname".txt into the Objects folder in your module. We're not ready to run just yet.

PART 3: Create a map!

Generate Art assets for your tiles. Again, save them as 32*32 pngs. For these, there are fixed filenames.

Here are the possible filenames:

Grass.png
Wall.png
Dirt.png
Water.png
BridgeUD.png
BridgeLR.png

NOTE: Your custom pngs do not necessarily have to BE "water" or "dirt" textures. Simply draw different items, if you wish.
The only constraint of the map editor is that "Wall" always has collision and other tiles do not.

the map maker generates two files: One for collision and one for art. They are placed in the Mapconfig folder.

Take "CollisionMap.txt", rename it to "(name)tile.txt"
Take "SpriteMap.txt", rename it to "(name)background.txt"

The first map in a module should be named "defaultbackground.txt" and "defaulttile.txt"

NOTE: These can be manually edited to call for more unique tiles. Replacing "Wall" with "Floor" in the spritemap will call "Floor.png" instead.

Object mapping must be done manually. Create a text file named "(name)object.txt"

Object maps allow one entry per line.

"objectname" X: 1 Y: 1 

if the object is the player, please label it as such.

"playername" X: 1 Y: 1 PLAYER

An Example:

"ness" X: 2 Y: 2 PLAYER
"orc" X: 4 Y: 4
"orc" X: 3 Y: 5
"Deacon" X: 4 Y: 5

In scripting, "ness" becomes "ness1"
"orc" becomes "orc1"
"orc" becomes "orc2"
"Deacon" becomes "Deacon1"

At this point the game should launch and work. There are two optional things you can do, though.

Part 4: Fun with Items and Scripts

SCRIPTS - In the script folder, any .txt file can be called. These use the same language as all other actions. Use these for common non-combat operations.

ITEMS - Items are also manually generated. The stats are defined above the actions. For example:

attack = 3
slot = 1
[ACTION:Eat Sword]
$this.hp += 5
PRINT "You ate the sword. It was spicy."
PRINT "5 hp has been restored."
TAKE sword

Part 5: Scripting commands, and how to use them.

GOTO (linenum) - Goes to the line in a script. The first line is 0.
GIVE (weaponfile.txt) - Gives the player an inventory item.
TAKE (weaponfile.txt) - Takes the item away from the player. Useful for single-use items.
SAY "Thing to say" - Makes this object say something. Can have a object name before it to specify an object.
ENDTURN - Ends the current turn.
ADDMOVE attackname.txt - Adds the move. Can have a object name before it to specify an object.
REMOVEMOVE attackname.txt - Removes the move. Can have a object name before it to specify an object.
AGGRO objectname - Makes an NPC pursue an object and attack it. Can have a object name before it to specify an object.
DEAGGRO - Removes Aggro. Can have a object name before it to specify an object.
SWITCHMAP map - Switches to the loaded map. be sure to not include .txt at the end.
RUN script.txt - Runs the script.
PRINT "Text" - Prints a string.
LOCK object - Locks an object in place.
UNLOCK object - unlocks an object
DISTANCE object variable = gets the distance to an object and loads it into the variable, Can have a object name before it to specify an object.
DESTROY object

IF true AND/OR false: - Checks the truth of statements. Can use IS GREATER THAN, IS LESS THAN, IS EQUAL TO, ect.
RAND 50: - checks a number between 1-100. The number is the chance, so 50 means 50%.
Use a tab to go to a deeper logic level, like python.

Dialogue:
NEWRESPONSE - starts a new response list.
ADDRESPONSE "response" - Adds a response to this list.
GETRESPONSE - Gets the player's response.

