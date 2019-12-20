using Engine.Actions;
using Engine.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Engine.Factories
{
    public static class ItemFactory
    {
        private const string GAME_DATA_FILENAME = @".\GameData\GameItems.json";

        private static readonly List<GameItem> _standardGameItems = new List<GameItem>();

        // Populate our list of game items. Runs the first time this static class is used
        static ItemFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                using (var reader = new StreamReader(GAME_DATA_FILENAME))
                {

                }
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }

        private static void BuildHealingItem(int id, string name, int price, int hitPointsToHeal)
        {
            GameItem item = new GameItem(GameItem.ItemCategory.Consumable, id, name, price);
            item.Action = new Heal(item, hitPointsToHeal);
            _standardGameItems.Add(item);
        }

        public static GameItem CreateGameItem(int itemTypeId) =>
            _standardGameItems.FirstOrDefault(i => i.ItemTypeId == itemTypeId)?.Clone();

        public static string ItemName(int itemId) =>
            _standardGameItems.FirstOrDefault(i => i.ItemTypeId == itemId)?.Name ?? "";

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