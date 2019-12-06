# Planning Docs

## Version 1

These are the baseline features that will give us a basic, but playable game (MVP). 

### Features

* user can create a player
  * different classes (different bonuses for each class)
* player can move to locations
    * the location might have a monster to fight
        * if the player defeats the monster, they receive
            * exp
            * gold
            * random loot
        * if the player loses
            * return Home
            * are completely healed
    * the location may have a quest
        * completing the quest requires turning in an item
            * the item is from monster loot
        * upon turing in the required item linked to the quest
            * gain exp
            * gain gold
            * gain a reward item
    * the location may have a trader
        * player can buy items
        * player can save items
* player can save/load the game

## Future Versions

* add automated tests of the code
* support multiple languages
* improved graphisc (jrpg-style?)
* add a crafting system for items
* add crafting recipies
* player can lean and use spells
* scrolls
* potions
* add armor
* add jewelry
* add the ability to enchant items
* add pets
    * helps in combat? heal? attack?
* upgrade the combat system (apply bonus to items/characters)
* populate the world (locations/monsters/items/quests/etc) from disk (files or database)
* game creator apps to let players create their own locations, monsters, items, quests, etc without writing any code (the app handles persistence to the files or database)