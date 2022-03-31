# ProjectGOS


Hello everyone. So, let's hop right into it.
About a year ago I tried my hand at this and well, It's a new day!
New game, new ambitions and a tighter focus, I have a much better idea on how I want the end goal for this game to be. This is what motivates me.
Project GOS is a solo project I've been working on since September 1st, and today I've decided to share the progress I've achieved so far.
Created in Unity using C#, and pixel art, I bring to you all(or few), my first, pre-alpha demo!
Without further ado, let's talk about what the game is about!

Project GOS is a turn based, strategic RPG. You embark on quests, complete levels, and earn rewards that can be used to unlock new spells and items for battles.
Delve in dungeons, unlock new characters to battle with, and battle others online. Yes, Online.
Collect lore pages and learn about the history of the world. Cook meals to sell for money, or eat to boost your health. Fish and mine for resources to upgrade facilities for idle gains.
And of course more that will slowly be revealed in time. Quite ambitious, but I'm happy to say I have a lot accomplished so far. So let's talk about what's in the demo, and whats coming next.


___________________________________________________________________________________________________________________________________________________
Demo Build 1.2

EDIT: Build 1.1 is broken and I'll probably remove it. 1.2 Should be working.

And I'm back, two weeks later to share today a new build!
So, without further ado, let's discuss what's been added over the past two weeks.
First things first, I've updated the 'hub' with a screenshots folder!
Want to skip the game? Maybe just a peek at what the game looks like? Take a look at the Screenshots folder!
(All sprites and artwork done by muah).

Anyways, onto what's new
-New Storage system! Players can now store extra items inside a storage back at the town(base).

-Storage handles up to 4 items and 10000 Solcs(In game currency). Future builds will have upgrade options.

-New 'Dump' System! Toss away unwanted items and acquire your Solcs based on the items and the amount tossed.

-This is essentially an NPC for selling items.

-Both new systems tied to the new Main Quest, "In Unison".

-This is the only new quest for now. Many more in future builds!

-Press the 'Q' Button to view quest progress.

-New Character Select! While in a town, press the 'G' Key to open up the Character Select.

-All characters are available in build 1.2. Character Select is only available while inside a town.

-Now that the character select is finished, players can now select up to 5 playable characters!

-Each character comes equipped with a unique passive. As more spells are released, more build paths will spawn.

-New Map!

-Tied to new quest. Build 1.2 features a new map!

-Many new Interactables. Each map will feature something new. As to not spoil it, you can see some previews of the new map in the Screenshots folder!

-New Enemies and drops. (Inside new map).

-Revamped Battle system look. (Polishing!)

A good two weeks if I say so myself! Build 1.2 will be up shortly after this readme is updated. As for B1.3 in approximately 2 weeks from today, you can look forward to:
-New Spells

-New Lore Section

-Relic Items

-Shops

-New Map

+More

In other news. I've been grinding tirelessly getting the game to a decent state as well as preparing social media pages. I've also been struggling with the music part a bit, only because my girlfriend broke my Midi Keyboard and all of my musical talents leaned heavily on that thing. I miss you babes(My Midi). And since I'm broke, replacing it has been put on pause. I'm preparing the Tiktoks with the music I have now. But uhhh, yea, promoting wooo. Very fun stuff this is.

See yall in 2





___________________________________________________________________________________________________________________________________________________
Demo Build 1

Build 1 only contains 1 level without sound. Lighting and sprites will continue to improve over time. 
Dialog between player and NPCs are available. Dialog choices are available, however for the tutorial level, will remain relatively limited.
Only 1 character. (More are done and ready, I just need to make a character select menu).
Interactable environment. (Including bushes, boulders and torches.)
Short Quests! Quests are in the game and ready! However, short quests will not show up in the player quest log, so for Demo 1, the quest log is irrelevant.
Short Quests NPCs appear on certain maps outside of the base town map. Accepting their quest will reveal their quest progress above their head, as opposed to within your quest log.
This is because regular quest log quests will be placed here, while short quests reset upon leaving the level.
Inventory system with consuming items and discarding items. Consuming items currently has no effect, but will be enabled in future builds. For now, I didn't want to overload players with information with a game that is only 10-20 minutes or so long.
Cooking! You can cook food in the demo! I've worked 3 days straight implementing this feature. This one is fun.
Spell selection. You may not be able to select your characters just yet, but you CAN select the spells you use. All available spells will be unlocked in Demo 1.
There may be a few more I missed, but this covers the main portion of the demo.

_________________________________________________________________________________________________________________________________________________________________________________
So what's coming in the near future?
Storage System! - A very important feature. Currently working on its UI.
Character Select - Obviously also very important. Should be done by the next build.
In Battle Spell Descriptions - While in battle, an image displaying the casted spells information should be revealed. This has already been added, but is temporarily shut off due to a lot of UI changes that made it look out of place.
The Town. Also already done; some NPCs just need to be given their functionality.
More Spells!
Relics. (Items that give characters a secondary passive effect)
More levels. (Level Select UI is complete. More levels coming soon!)

_________________________________________________________________________________________________________________________________________________________________________________
What's coming Later


PVP. (Soooo cool news! I actually have player v player working. The UI needs to be tidied up, and the timers between games need to be slightly better synced, otherwise this may be a feature I add sooner rather than later.)
Mining and Fishing. (Sprites done, just need coding)
DailyEvents.
_________________________________________________________________________________________________________________________________________________________________________________
Anywaysss, I'm thinking about posting an update every other week or so; maybe a new build a month, leading up to the game's release?
Not sure yet.
Making this game has been a fun challenge.  From the coding, to the artwork(The Indie staple, Pixel Art of course), to the sound, animations, story and dialog... I do it all baby!
For now this game will be PC only, but overtime, if reception is well, I may port it over to phones.


Finally, for those looking to play the game, simply run the 'Build1.1' installer located within the builds folder. Once the game starts, click, 'Begin', input a username at the top(At least 3 letters), click Enter and enjoy!
_________________________________________________________________________________________________________________________________________________________________________________
Before I go, let's briefly discuss the battle mechanics.
While inside a battle, the game plays similarly to Rock-Paper-Scissors. Each spell has a type associated with that 'Style', (Rock-Paper-Scissors). You battle opponents, and those who cast a spell that that beats another (I.E. Rock > Scissors), will deal a bonus effect. You and your opponent will keep battling until someones Health falls below 0.
Simple right? 
The goal was to make a simplistic battle system, intricate and deep 'deck building'. Easy to learn, hard to master sort of thing.
Of course this is just a surface level explanation of the spells. There are spells that summon allies, recover health, grant armor, boost damage, poison, burn, bleed etc.
Within the demo, there are spells that grant armor,heal, deal damage, bleed and poison, grant bonus damage, and summon allies! (Burn will be introduced later).
Furthermore, there are two ways to win a round; through Style Wins or Block Breaks. Each spell has a win type of one or the other(sometime both).
Style win effects only trigger when you beat an opponent who used a spell type (Paper > Rock). Block break win effects only trigger against an opponent who blocks.
Players can hold (must hold) 2 spells from each category.
Lastly, there are the characters. Characters(Stylists) each hold their own unique passive, separating them apart from each other.
Stylist 1, Mikhael, the one available in the demo, will gain a stack of 'The Creeping Pack' whenever you Style Win/Block Break. Every 2 stacks, Mikhael will summon a wolf ally who attacks and deals 5 damage, while bleeding x1 each round.
Stlylist 3 for instance, instead recovers health and gains a large damage boost whenever you successfully block.
Each Stylist is different, promoting different build/play patterns, while driving the desire to unlock them all! muahaha
_________________________________________________________________________________________________________________________________________________________________________________
Anyways, if anyone wants to reach me with any questions, advice, feedback, or perhaps you found a bug (I know of a few), don't be afraid to email me at - Assante.waldron@gmail.com
Other socials coming soon.
See you next Update!
