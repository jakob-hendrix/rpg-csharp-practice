using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _standardGameItems.Add(new Weapon(1001, "Pointy Stick", 1, 0,2));
            _standardGameItems.Add(new Weapon(1002, "Rusty Sword", 5, 0, 3));
            _standardGameItems.Add(new Weapon(1003, "Broadsword", 20, 0, 6));
            _standardGameItems.Add(new GameItem(9001, "Snake Fang", 1));
            _standardGameItems.Add(new GameItem(9002, "Snakeskin", 2));
            _standardGameItems.Add(new GameItem(9003, "Rat Tail", 1));
            _standardGameItems.Add(new GameItem(9004, "Rat Fur", 2));
            _standardGameItems.Add(new GameItem(9005, "Spider Fang", 1));
            _standardGameItems.Add(new GameItem(9006, "Spider Silk", 3));
        }

        public static GameItem CreateGameItem(int itemTypeId)
        {
            GameItem standardGameItem = _standardGameItems.FirstOrDefault(i => i.ItemTypeId == itemTypeId);

            if (standardGameItem != null)
            {
                if (standardGameItem is Weapon)
                {
                    return (standardGameItem as Weapon).Clone();
                }

                return standardGameItem.Clone();
            }

            return null;
            //return standardGameItem?.Clone();
        }
    }
}
