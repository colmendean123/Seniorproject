How to Start Editing Your Own Map!!!
	First off launch the Mapcreator.exe and you will be greeted by a nice welcome screen that 
	asks for your input of a width and height! The Max width and height is 150x150! if you go
	any higher than that the sprites will display a little bit broken but on saves the data 
	will still work as normal. The input for width and height is limited to 999 and 999.
	Below are the controls for using the Mapcreator!

Controls:
        Left Mouse Button:
            Sets the sprite currently selected (by defualt it is None).

        Q:
			Pressing Q will set the sprite to place as None
		W:
			Pressing W will set the sprite to place as Wall
		E:
			Pressing E will set the sprite to place as Dirt
		R:
			Pressing R will set the sprite to place as Grass
		T:
			Pressing T will set the sprite to place as Water
		Y:
			Pressing Y will set the sprite to place as BridgeUD,
			which is a Bridge sprite that is in the direction Up and Down
		U:
			Pressing U will set the sprite to place as BridgeLR,
			which is a Bridge sprite that is in the direction Left and Right
		I:
			Pressing O will set the sprite to place as Floor,
			which is a franite floor sprite.
		O:
			Pressing I will set the sprite to place as Water
        F1:
			Pressing F1 will save the collision map which will hold the 
			collsion data for the main game. The CollisionMap.txt file
			created from pressing this button will overwrite anything 
			in the file and write new info for the current map
			(it will look like alot of 0's and 1's)
			Pressing F1 will also save the sprite map which will hold sprite
			info for the main game. The SpriteMap.txt file created from
			pressing F1 will overwrite anything in the file and write 
			new info for the current map. (The data for this map will 
			be a list of names for each sprite)
		F2:
			Will load your map from earlier in the session if you want to revert
        Escape:
			Pressing the Escape key will boot you back to the main menu
			without saving progress be careful.
		Arrow keys:
			The arrow keys move the camera around allowing you to be able to pan
			to different parts of the map if the map is too big to fit on the screen