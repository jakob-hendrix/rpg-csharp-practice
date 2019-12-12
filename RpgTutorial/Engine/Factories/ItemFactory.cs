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
        private static List<GameItem> _standardGameItems;


        // Populate our list of game items
        static ItemFactory()
        {
            // runs the first time this static class is used
            _standardGameItems = new List<GameItem>();
            _standardGameItems.Add(
                new Weapon(1001, "Pointy Stick", 1, 1,4));
            _standardGameItems.Add(
                new Weapon(1002, "Rusty Sword", 5, 1, 6));
            _standardGameItems.Add(
                new Weapon(1003, "Broadsword", 20, 1, 8));
        }

        public static GameItem CreateGameItem(int itemTypeId)
        {
            GameItem standardGameItem = _standardGameItems.FirstOrDefault(i => i.ItemTypeId == itemTypeId);
            return standardGameItem?.Clone();
        }
    }
}
