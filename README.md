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

-BUG - if you dont click anything, no one wins the round (no effect), should default to enemy win
-BUG - whenyou click to restart after a battle, the last entry persists in the log
-clear the output text after each round
-better helpers to reset vars.


