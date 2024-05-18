# Spyro: A Hero's Tail - Text Editor
Editor for the text.edb file from Spyro: A Hero's Tail for PS2, GameCube and Xbox. Written in C# on the .NET framework and using Windows Forms.<br><br>
text.edb contains a spreadsheet of all the game's text, as well as some extra data denoting which character the text strings belong to and which section of the file they are stored in. Sections are loaded depending on what is needed by the game.

## Support
Only the GameCube version has been tested so far, but the tool should work with all versions of the game.

## How To Use
To find text.edb, the game's filelist must first be extracted. [Eurochef](https://github.com/eurotools/eurochef)'s command-line version can be used for this. The filelist must also be repacked after replacing the text.edb file.<br>
Open the text.edb file with the "Open File" button, and the list of sections should fill up. Double-click on the section you wish to load, and you'll see all the text item stored within.<br>
Click the "Edit Selected" button after highlighting a text item, or simply double-click on the item, and a window will pop up letting you edit the text and what character is assigned to it. Any text item that has been edited in the current session will be marked bold in the list.<br>
When you're done editing, click "Export File" and choose a location for it. Before repacking the filelist with Eurochef, make sure the file is named "text.edb", and replace the original file.
