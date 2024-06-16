/*
 * 
 * Shortdoc Viguar Industries Unity Engine Tools
 * 
 * __________[ 1 ]_[ Utility Assets ]
 * |
 * |[  ]
 * |
 * |
 * 
 * __________[ 2 ]_[ Editor Utility Assets ]
 * |     
 * |[ RenderToImage.cs ]
 * |           
 * |    The RenderToImage.cs script is capable of generating images of any desired resolution, within the capabilities of the Unity Game Engine and the machine it is running on.
 * |    When invoked, The script creates a .png file of the camera object that it is attached to.  
 * |        
 * |    There are two exposed variables controlling the image generation:
 * |        Vector2 resolution - The image resolution in pixels, where resolution.x is Width and resolution.y is Height of the image.
 * |        string path - The path where the image will be stored. If the path directory is left empty, the image will be stored at the project root.
 * |
 * |    The script works both Offline (A) and at Runtime (B). 
 * |       A: Offline, it can be invoked through the Inspector GUI Button, or by script by calling the "RenderToImage.SaveToFile();" function.
 * |       B: At Runtime, it can be invoked by calling the "RenderToImage.SaveToFile();" function.
 * |    
 * 
 * 
 * 
 * 
 */ 