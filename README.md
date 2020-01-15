# UnitySimpleCharacterControllers
Sample 2D Character Controllers, for students to reference.

All contained in the "Blooper" namespace.

# Grid Snap Controller
A controller for moving a player in a grid-style game, with instant snapping. "position" based movement.

# AsteroidsCharacterController
A controller that uses rigidbody2D forces to accelerate, rotate a player. "Acceleration" based movement.

# Platformer Character Controller
A pretty bad platformer controller, missing lots of features and is buggy. But demonstrates how one would get started using raycasts instead of a rigidbody to move a player in this way. It'll take work to make it feel good, but there are bones here.

# Twin Stick Character Controller
Uses a rigidbody2D, but editing it's velocity directly to move.
Also turns the player to face the mouse instantly.

This example also has shooting. There is a bullet manager, bullets, and enemies. The code demonstrates an architecture where things tend to "handle themselves" and minimize dependencies. Notice how each script is actually fairly small, and doesn't do much that's really complicated as a script. The complexity comes from how the scripts all talk to each other. 
