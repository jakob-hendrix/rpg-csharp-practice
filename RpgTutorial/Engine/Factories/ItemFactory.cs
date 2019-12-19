using System.Collections.Generic;
using System.Linq;
using Engine.Actions;
using Engine.Models;

namespace Engine.Factories
{
    public static class ItemFactory
    {
        private static readonly List<GameItem> _standardGameItems = new List<GameItem>();

        // Populate our list of game items
        static ItemFactory()
        {
            // runs the first time this static class is used
            BuildWeapon(1001, "Pointy Stick", 1, 1, 2);
            BuildWeapon(1002, "Rusty Sword", 5, 1, 3);
            BuildWeapon(1003,"Vorpal Sword",100000,5,500);

            BuildWeapon(1501,"Snake fangs",0,0,2);
            BuildWeapon(1502, "Rat claws", 0,0,2);
            BuildWeapon(1503,"Spider fangs",0,0,4);

            BuildHealingItem(2001, "Granola bar", 5, 2);

            BuildMiscellaneousItem(9001, "Snake Fang", 1);
            BuildMiscellaneousItem(9002, "Snakeskin", 2);
            BuildMiscellaneousItem(9003, "Rat Tail", 1);
            BuildMiscellaneousItem(9004, "Rat Fur", 2);
            BuildMiscellaneousItem(9005, "Spider Fang", 1);
            BuildMiscellaneousItem(9006, "Spider Silk", 3);
        }

        private static void BuildHealingItem(int id, string name, int price, int hitPointsToHeal)
        {
            GameItem item = new GameItem(GameItem.ItemCategory.Consumable, id, name, price);
            item.Action = new Heal(item, hitPointsToHeal);
            _standardGameItems.Add(item);
        }

        public static GameItem CreateGameItem(int itemTypeId) =>
            _standardGameItems.FirstOrDefault(i => i.ItemTypeId == itemTypeId)?.Clone();

        private static void BuildMiscellaneousItem(int id, string name, int price) =>
            _standardGameItems.Add(new GameItem(GameItem.ItemCategory.Miscellaneous, id, name, price));

        private static void BuildWeapon(int id, string name, int price, 
            int minDamage, int maxDamage)
        {
            GameItem weapon = new GameItem(GameItem.ItemCategory.Weapon, id, name, price, true);
            weapon.Action = new AttackWithWeapon(weapon, minDamage, maxDamage);
            _standardGameItems.Add(weapon);
        }
    }
}