# CS426 Final Project - The Great Race!
## By: Rahul Chatterjee, Allen Breyer, Aakash Kotak

## Design Document
https://docs.google.com/document/d/1s_18H70d4SwaF5gHm3IApc3wV1Rdj42UssKQFosjxxE/edit?ts=5e6a8d7e

## Story
In the near future a virus by the name of COVID-19 has spread across the globe and caused the infected to turn into zombie-like creatures. This pandemic has led to a war for resources leaving those without any supplies vulnerable to the virus. The clock is counting down between two superpowers to fight over the remaining resources which will ultimately lead to the winner surviving the virus!

## Controls
 - Movement - W, A, S, D
 - Jump - SPACEBAR
 - Run - SHIFT
 - Firing weapon & Grab Bridge Piece- LMB
 - Aim - RMB
 - Reload - R

## Design and Rationale
Realistic island floor plan to enhance immersion.

Each area of the island has its own advantages and disadvantages:
 - Forest area (better hiding and cover from enemy players because of trees, but zombies lurk there and can be hard to see)
 - Outer area (the beach, little-to-no-cover from enemy players, but can easily see and escape from zombies)
 - Military base (area is safe from zombies but not other enemy players, the player can replenish supplies here)
 
Player and player team members are a military unit. The military unit is outfitted with military uniforms to identify team members and enemies.
Zombies are NPC’s that were soldiers before turning into zombies. Zombies are outfitted with a tattered military uniform.


## AI
 - Zombie character uses Navigation Mesh pathfinding to locate and chase the player on the island (Aakash).
 - Zombie character uses a FSM to determine when it can “see” the player, how to approach the player, and when to attack the player when it’s close enough (Rahul).
 - The bot player uses Navigation Mesh pathfinding to follow the player. It also uses a FSM to determine if it can “see” the player, whether to run or walk towards the player, and if it starts getting “stuck” on an obstacle it will attempt to jump over it (Allen).

## Mecanim
  - Player character (idle, walk, run, jump, shooting) (Rahul)
  - Zombie character (idle, zombie walk, zombie swing attack) (Aakash)
  - Player Bot (idle, walk, run, jump) (Allen)
  
## UI
  - There is a title screen now that explains the story and instructions of the game, as well as player controls. When the player is ready, they can click "PLAY". This makes it feel more like a game with a goal because there is now a story and objective given for the player instead of having the player jump right into the game without explaining any rules.
  - Created a new health, armor and stamina bar with icons to indicate which parts are for what.
  - Added a lerping speed so that there's a delay when the player takes damage instead of having the damage show up on the bars instantly. It looks cooler, many popular games have this implementation, and it doesn't make the UI look clunky.
  - The colors for each survival bar gets darker when the bars are running low. For example, health bar is bright red at 100 health, but will become a darker red at 20 health. Good to have an indicator for when bars are running low.
  - Added a green-yellow background color for all the bars to make it look nicer and not blend in with the background so the player can see them easily.
  
## Sound Design
 - Added footstep sand SFX helps immerse the player into thinking that they are walking on a sandy beach. We plan on adding different kinds of footstep sounds depending on where the player is walking.
 - Added a water dive/splashing SFX so that the player will know that they are going into deep water.
 - Added a Zombie growl SFX to alert the player when a zombie is nearby.
 - Added a Zombie snarl SFX when the zombie starts attacking to give the player an audio clue that they are in an attack state.
 - Added a hit SFX when the player gets hit by a zombie to give audio confirmation that they got attacked.
 - Added a gun shot SFX to give audio confirmation to the player that they fired their weapon.
 - Added a fanfare theme when the player wins the game (getting to the other island with the supplies).
 
## Shader Design
 - Added a blood splatter effect that uses transparency and bump maps to indicate that the zombie took damage from the gun. (Allen)
 - Fog effects enhance the game world’s atmosphere to make it feel more mysterious and dark, and make the world look more realistic. (Aakash)
 - Bridge pieces are not shiny anymore since we changed the texture and removed its specular mapping to make it look more like bridge pieces and match with the world's texture. (Rahul)


## Modifications in Response to the Alpha Feedback
 - We labeled the UI bars so that the player can easily tell what each bar is (health, stamina, armor)
 - We scripted a settings menu that includes options for both volume and graphics quality (the graphics quality can be lowered to help with lagging issues on less powerful machines)
 - The playtesters hardly had any interactions with the zombies, so we scripted zombie spawners so that more will appear as time goes on.
 - Fixed zombie colliders so that they don’t slide like crazy or teleport in the water
 - Zombies can now wander around so that they’ll be more likely to engage with the player
 - Zombies can now die when you shoot them and they play a death animation before disappearing
 - Scattered more bridge pieces around so that the player doesn’t finish the game as fast
 - Started implementing networking features which involves players starting on alternative islands when they join



