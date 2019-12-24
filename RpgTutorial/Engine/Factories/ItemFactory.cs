using System;
using Engine.Actions;
using Engine.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

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
                    var json = reader.ReadToEnd();
                    var dataObject = JObject.Parse(json);
                    var gameItems = dataObject["GameItems"];

                    BuildItemsFromJson(gameItems, "Weapons",GameItem.ItemCategory.Weapon);
                    BuildItemsFromJson(gameItems, "HealingItems",GameItem.ItemCategory.Miscellaneous);
                    BuildItemsFromJson(gameItems, "MiscellaneousItems",GameItem.ItemCategory.Consumable);
                }
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }

        public static void BuildItemsFromJson(JToken itemData, string jsonKey, GameItem.ItemCategory itemCategory)
        {
            var items = itemData[jsonKey];
            foreach (var item in items)
            {
                int id = (int) item["Id"];
                string name = (string) item["Name"];
                int price = (int) item["Price"];

                switch (itemCategory)
                {
                    case GameItem.ItemCategory.Consumable:
                        BuildHealingItem(id, name, price, (int) item["HitPointsToHeal"]);
                        break;
                    case GameItem.ItemCategory.Miscellaneous:
                        BuildMiscellaneousItem(id, name, price);
                        break;
                    case GameItem.ItemCategory.Weapon:
                        BuildWeapon(id, name, price, (int)item["MinimumDamage"], (int)item["MaximumDamage"]);
                        break;
                    default:
                        throw new InvalidEnumArgumentException();
                }
            }
        }

        private static void BuildHealingItem(int id, string name, int price, int hitPointsToHeal)
        {
            GameItem item = new GameItem(GameItem.ItemCategory.Consumable, id, name, price);
            item.Action = new Heal(item, hitPointsToHeal);
            _standardGameItems.Add(item);
        }

        private static void BuildMiscellaneousItem(int id, string name, int price) =>
            _standardGameItems.Add(new GameItem(GameItem.ItemCategory.Miscellaneous, id, name, price));

        private static void BuildWeapon(int id, string name, int price, 
            int minDamage, int maxDamage)
        {
            GameItem weapon = new GameItem(GameItem.ItemCategory.Weapon, id, name, price, true);
            weapon.Action = new AttackWithWeapon(weapon, minDamage, maxDamage);
            _standardGameItems.Add(weapon);
        }

        public static GameItem CreateGameItem(int itemTypeId) =>
            _standardGameItems.FirstOrDefault(i => i.ItemTypeId == itemTypeId)?.Clone();

        public static string ItemName(int itemId) =>
            _standardGameItems.FirstOrDefault(i => i.ItemTypeId == itemId)?.Name ?? "";
    }
}