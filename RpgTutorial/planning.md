# Planning Docs

## Version 1

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