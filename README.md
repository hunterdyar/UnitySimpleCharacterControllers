# UnitySimpleCharacterControllers
Sample 2D Character Controllers, for students to reference.

All contained in the "Blooper" namespace.

# Grid Snap Controller
A controller for moving a player in a grid-style game, with instant snapping. "position" based movement.
The level is created with Unity's [tilemap](https://learn.unity.com/tutorial/introduction-to-tilemaps#) system.

# Asteroids Character Controller
A controller that uses rigidbody2D forces to accelerate, rotate a player. "Acceleration" based movement.

# Platformer Character Controller
A pretty bad platformer controller, missing lots of features and is buggy. But demonstrates how one would get started using raycasts instead of a rigidbody to move a player in this way. It'll take work to make it feel good, but there are bones here.

# Twin Stick Character Controller
Uses a rigidbody2D. Uses .MovePosition and .MoveRotation functions in FixedUpdate to move.

Also turns the player to face the mouse instantly.

This example also has shooting. There is a bullet manager, bullets, and enemies. The code demonstrates an architecture where things tend to "handle themselves" and minimize dependencies. Notice how each script is actually fairly small, and doesn't do much that's really complicated as a script. The complexity comes from how the scripts all talk to each other. 

# GGJ Platformer
Platformer script I wrote for a project for Global Game Jam 2020. Doesn't use rigidbody, I do collisions and physics myself because it just works better and is easier to adjust, IMHO. I cleaned it up a bit, making it a bit more universal. It works pretty well, although still has some quirks. Collider shape is just a bounds that has to be set (I added tools to do this using a boxCollider2D or spriteRenderer).

The example input uses Input.GetAxisRaw() which is good for fast and snappy keyboard input; there is no smoothing function on it. 