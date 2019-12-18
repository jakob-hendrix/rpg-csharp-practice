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

            BuildMiscellaneousItem(9001, "Snake Fang", 1);
            BuildMiscellaneousItem(9002, "Snakeskin", 2);
            BuildMiscellaneousItem(9003, "Rat Tail", 1);
            BuildMiscellaneousItem(9004, "Rat Fur", 2);
            BuildMiscellaneousItem(9005, "Spider Fang", 1);
            BuildMiscellaneousItem(9006, "Spider Silk", 3);
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