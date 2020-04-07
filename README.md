# CS426 Final Project - The Great Race!
## By: Rahul Chatterjee, Allen Breyer, Aakash Kotak

## Story
In the near future a virus by the name of COVID-19 has spread across the globe and caused the infected to turn into zombie-like creatures. This pandemic has led to a war for resources leaving those without any supplies vulnerable to the virus. The clock is counting down between two superpowers to fight over the remaining resources which will ultimately lead to the winner surviving the virus!

## Controls
 - Movement - W, A, S, D
 - Jump - SPACEBAR
 - Run - SHIFT
 - Firing weapon & Grab Bridge Piece- LMB
 - Aim - RMB

## Design and Rationale
Realistic island floor plan to enhance immersion
Each area of the island has its own advantages and disadvantages
 - Forest area (better hiding and cover from enemy players because of trees, but zombies lurk there and can be hard to see)
 - Outer area (the beach, little-to-no-cover from enemy players, but can easily see and escape from zombies)
 - Military base (area is safe from zombies but not other enemy players, the player can replenish supplies here)
 
Player and player team members are a military unit. The military unit is outfitted with military uniforms to identify team members and enemies.
Zombies are NPC’s that were soldiers before turning into zombies. Zombies are outfitted with a tattered military uniform.


## AI
 - Zombie character uses Navigation Mesh pathfinding to locate and chase the player on the island (Aakash).
 - Zombie character uses a FSM to determine when it can “see” the player, how to approach the player, and when to attack the player when it’s close enough (Rahul).
 - The bot player uses Navigation Mesh pathfinding to follow the player. It also uses a FSM to determine if it can “see” the player, whether to run or walk towards the player, and if it starts getting “stuck” on an obstacle it will attempt to jump over it (Allen).

## Mechanim
  - Player character (idle, walk, run, jump, shooting) (Rahul)
  - Zombie character (idle, zombie walk, zombie swing attack) (Aakash)
  - Player Bot (idle, walk, run, jump) (Allen)
