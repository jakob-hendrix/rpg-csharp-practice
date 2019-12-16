using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Weapon : GameItem
    {
        public int MinDamage { get; }

        public int MaxDamage { get; }

        // implement if you want to use dice based damage
        //public int DamageDieCount { get; set; }
        //public int DamageDieSides { get; set; }

        public Weapon(int itemTypeId, string name, int price, int minDamage, int maxDamage)
            : base(itemTypeId, name, price)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }

        public Weapon Clone() => new Weapon(ItemTypeId, Name, Price, MinDamage, MaxDamage);
    }
}
