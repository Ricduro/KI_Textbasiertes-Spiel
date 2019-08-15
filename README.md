# KI_Textbasiertes-Spiel (C#)
## "Conquered Kingdom" a text-based Unity game

Text-based game made during an artificial intelligence uni course. 
Player battles an AI opponent in a 1 versus 1 melee fight until one is defeated. Attacks happen automatically every 3 seconds, during this timeframe both fighters choose an attack type. Player can choose with "A", "S" and "D" keys, otherwise the previous attack is repeated.
Similar to rock-paper-scissors, the comparison of the choice of both fighters decides who does damage, with added (somewhat random) chance to
dodge an attack, based on the two fighters being in different weight classes. The higher & lower weightclass is randomly assigned every 
playthrough.

**AI information:**
The enemy AI pursues a winning strategy by learning which attack is consistently effective. This is made possible by a neural network with 
multi-layer perceptron, and the AI trains against a computer opponent so that it's biases are set when playing against a human enemy.
The AI continues to adjust itself while it's fighting the player character.

**Resources used:**
http://ai-junkie.com/
