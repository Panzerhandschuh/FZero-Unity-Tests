# FZeroGXEditor
Course editor for F-Zero GX

This editor reads and displays all of the info in a coli file that we understand currently. There is a save feature that will update the coli file with updated values changed from Unity's inspector. Currently, only checkpoint and object edits are saved.

# Dependencies
- [FZeroGXTools](https://github.com/Panzerhandschuh/FZeroGXTools) (included in this repository)

# Instructions
1. Use [GCRebuilder](https://gamebanana.com/tools/6410) to extract game files from an F-Zero GX iso
   - Move contents within root folder into Assets/GameData
2. Open the project in Unity
3. Load the Scene file (Scenes/Editor.unity)
4. Use the "CourseLoader" object to select and load a course

# Research
Here's a coli excel file for course03 (Mute City Serial Gaps) that's almost fully labelled using discovered patterns and offsets: http://www.mediafire.com/view/4a9mnychlndc7eb/stage_03_00000000-00035B97.xlsx
