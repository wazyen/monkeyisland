*Warning: This game is designed to be played with a resolution of 640x400 pi­xels. It may not display properly with any other screen size.*

## Game description

This game is an emulation of an insult duel in *The Secret of Monkey Island* developed with the unique goal of training with Unity.

Demo: https://youtu.be/PUzjhO5Uuw0

Play Online: https://wazyen.itch.io/monkeyisland

## Game Dynamics

After starting the game, the first thing we will see is the main menu, where we will have the options to either start a new game or quit.

If we choose to start a new game, we will swap to a scene where we will see a pirate (controlled by AI) challenging Guybrush (controlled by us) to an insult duel. As soon as we answer him, the duel will begin.

At the beginning of the duel, one of the characters will start randomly the duel and, from that moment on, the one who wins each round will begin insulting in the next one.

Once the duel is over, that is, one of them loses 3 rounds, we will be given the options to either go back to the main menu, start a new game or quit.

## Game Implementation

The game logic is implemented using a Finite State Machine controlling in which state the game is at any time. Each state has a text history presenting it that is shown during the game, a set of possible answers for the player to pick, a set of nodes/states to which to jump depending on the answer chosen by the player, a boolean variable telling whether the state is a final state and an action to be executed whenever that state is visited.

Due to the humongous complexity that would entail elaborating the complete tree reuniting all possible combinations a game can develop depending on several factors, such as who starts insulting in the first round, each of the possible insults the player or the AI can use or who wins each round, the tree of states is going to be built on the fly as the game advances.

In the file *tree_of_states.png* we can observe a scheme of the structure that the previously mentioned tree of states will have. The game starts in the state *root*. Whenever we accept the duel, we will jump with a 50% chance to either the state *player_insults* or one of the states *computer_insults[i]*.

The state *player_insults* represents the state where the player has their turn to insult first. From that state, once we choose between the 16 possible insults, we will jump to one of the states *player_insulted[i]*, depending on the insult used by the player, which will be shown on the screen. Once we decide to move on, we will jump to one of the states *computer_answers[i]*, depending on the AI response, which will be shown on the screen. In order to make the game less unfair for the AI, it has been provided a chance of 50% of picking the right answer, together with a chance of 50% of picking randomly any of the 16 possible answers. Consequently, the AI will have a chance of 50% + 50% * 1/16 of winning the reound. From that point, we will have to go to the next round, which, depending on who won the previous one, will start in either the state *player_insults* or one of the states *computer_insults[i]*, as long as both duelers have won less than 3 rounds so far.

Each of the states *computer_insults[i]* represent each of the 16 possible insults chosen by the AI when it's its turn to insult first. In the case the game has to jump to any of these states, it will do it randomly between the 16 possible options. Once we are in any of these states, we will have the chance to pick between the 16 possible responses. Once we pick one, we will jump to one of the states *player_answered[i]*, depending on the insult chosen by the player, which will be shown on the screen. From that point, we will have to go to the next round, which, depending on who won the previous one, will start in either the state *player_insults* or one of the states *computer_insults[i]*, as long as both duelers have won less than 3 rounds so far.

Once either the player or the AI wins (or loses) 3 rounds, we will jump to either the state *player_wins* or *computer_wins*, depending on who won the game. In that state, we will be given the options to either go back to the main menu, start a new game or quit the game.

## Assets References

### Assets/Fonts/ :

##### "MonkeyIsland-1991-refined.ttf" obtained from:

https://www.ptless.org/sfomi/

### Assets/Images/ :

##### "arrows.png" drawn using GIMP from:

https://www.youtube.com/watch?v=hc-sxN1OnPg

##### "duel-background.png" obtained from:

https://ext.minijuegosgratis.com/monkeyisland/img/room049.png

##### "guybrush.png" obtained from:

https://ext.minijuegosgratis.com/monkeyisland/img/guybrush.png

##### "melee-island.gif" obtained from:

https://lparchive.org/The-Secret-of-Monkey-Island/Update%2021/1-somi_904.gif

##### "pirate.png" obtained from:

https://ext.minijuegosgratis.com/monkeyisland/img/pirata3.png

### Assets/Music/ :

##### "intro.mp3" obtained from:

https://scummbar.com/mi2/MI1-CD/01%20-%20Opening%20Themes%20-%20Introduction.mp3

##### "duel.mp3" obtained from:

https://scummbar.com/mi2/MI1-CD/23%20-%20Monkey%20Island%20Night%20(Ambient).mp3

##### "hit_sfx.mp3" obtained from:

https://ext.minijuegosgratis.com/monkeyisland/snd/hit1.mp3

### Assets/Resources/Images/ :

##### "heart-full.png" drawn using GIMP from:

https://www.wikihow.com/Make-a-Simple-Pixel-Art-Heart

##### "heart-empty.png" drawn using GIMP from:

https://www.wikihow.com/Make-a-Simple-Pixel-Art-Heart


### Assets/Resources/Text/ :

##### Insults in "insults.json" obtained from:

http://gamelosofy.com/los-insultos-de-el-secreto-de-monkey-island-1/
