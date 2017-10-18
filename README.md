# Battle Microgame
-A simple click-timer rock-paper-scissors game in unity
-could be adopted / extended into a larger game with RPG elements


-----

2017-07-10 Plan / Notes

x-timer that moves a drag UI element
x-button - click and measure tiem with dragger

x-have downtime between clicks
x-translate the clickvalue to a percent to hit

x-need a small buffer after slider complete (have some buffer room built into the SliderValue?) so you can click right at 100?

x-translate to crit/hit/miss values

2017-07-11

x-create three buttons - attack, dodge, counter

attack < counter < special

x-create grid of outcomes 
	Win, Lose, Draw
	Crit, Hit, Miss, Fail

x-create data structure to hold enemy attack ordering - call in loop (attack type + timer value)

-Record the attack type and outcome at top of screen (Attack + Win, Counter + Draw, etc)

x-should only be able to click button once per round.

2017-07-14

x-BUG - if you dont click anything, no one wins the round (no effect), should default to enemy win
x-BUG - whenyou click to restart after a battle, the last entry persists in the log
x-clear the output text after each round
x-better helpers to reset vars.
x-countdown till battle starts: 3...2...1... Fight!
x-show damage done on each side.

-should damage be more of a continuous function of accuracy?  plus some randomness?

2017-07-18
x-usable items (healing potions, poisons, etc) that can be clicked in between regular attacks.
	-buttons tied to actual items in inventory.  using items removes them.
		-displays counts as part of button.

-lifetap effect


-various attacks (attack, counter, special) have effects attached.

x-start / select enemy screen
x-player stats screen.
	x-tied to player object

-level up mechanics
	x-gain xp on win
	x-xp per level curve
	-generate enemies per level.

x-items display damage / healing.

x-run on iphone for testing:
	https://unity3d.com/learn/tutorials/topics/mobile-touch/building-your-unity-game-ios-device-testing


-07-30-2017
	x-procedurally generate enemies based on current player level - easy, medium, hard challenges.

	x-test unity scene

	-hook up unit tests framework?

-08-20-2017
	x-splash screen (Player vs Goblin etc.)
	x-top of battle screen - have ? boxes that get filled as the turns progress.  just loop a cursor around the current one.
		-when we start the battle, instantiate all of these as ?
		-once the turn completes, update the icon
		-put a cursor on the current turn icon.


09-03-2017
	x-more varied enemies (difficulty per level + boss)
	-better turn icon indicator
	x-gain xp / level up
	x-win battle splash screen (xp, loot gained)
	-save stats on iphone (with reset option)
	x-testing - level up button - to test enemy generation
	
	-random items
		x-loot
		-weapons - give varying dmg + effets for each of the attack types (attack, counter, special)
	x-inventory 
		x-show current weapon / armor on that screen.
		x-limit list to 15 items
		x-sorting.

		x-items - seperate sublist for usable items
		x-remove on use.
		x-better random items / abilities
	-Store screen.


MVP
	x-udate to latest unity version?
	x-skinning UI
	x-avatars for players / enemies
	x-icons for items
	x-store
	-sound
	-enemy selection (select from 3)
	-save progress with iphone localdata.

	-basic map populated with locdata + timestamp + perlin noise?
	
Store
	-pass in level and variance to populate store
	x-similar interface to inventory - buy/sell
	x-show player's money.

