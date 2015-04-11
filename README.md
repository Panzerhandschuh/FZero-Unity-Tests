# FZero-Unity-Tests
Tests for F-Zero GX file formats

Full Unity project: http://www.mediafire.com/download/k9x15vd9l3ijze3/FZeroTest.7z

Requires Unity 4.6+ though it might still work on older versions with some missing features. 

This tool reads and displays all of the info in a coli file that we understand currently. There's also a save feature that will update the coli file with updated values changed from Unity's inspector. Currently, only track spline and object edits are saved. I recommend reading the source code if you want to get a better understanding of the structure of coli files.

Instructions: 
- Open the project in Unity
- Load the Scene file
- Click Main Camera in the object hierarchy and change the course number value to select a course
- If you are missing scripts on the main camera, select EditorCamera.cs, DebugHelper.cs, and CollisionParser.cs for the missing scripts
- Make all edits in Unity's edit mode (game mode only lets you look around)

Here's a coli excel file for course03 (Mute City Serial Gaps) that's almost fully labelled using discovered patterns and offsets: http://www.mediafire.com/view/4a9mnychlndc7eb/stage_03_00000000-00035B97.xlsx
