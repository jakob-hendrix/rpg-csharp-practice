using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class GameItem
    {
        public enum ItemCategory
        {
            Miscellaneous,
            Weapon
        }

        #region Properties

        public ItemCategory Category { get; }
        public int ItemTypeId { get; }
        public string Name { get; }
        public int Price { get; }
        public bool IsUnique { get; }
        public int MinDamage { get; }
        public int MaxDamage { get; }

        #endregion

        public GameItem(
            ItemCategory category, int itemTypeId,
            string name, int price,
            bool isUnique = false, int minDamage = 0,
            int maxDamage = 0)
        {
            Category = category;
            ItemTypeId = itemTypeId;
            Name = name;
            Price = price;
            IsUnique = isUnique;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }

        public GameItem Clone() => new GameItem(Category, ItemTypeId, Name, Price, IsUnique);
    }
}