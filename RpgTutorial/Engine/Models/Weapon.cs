using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Weapon : GameItem
    {
        public int DamageDieCount { get; set; }
        public int DamageDieSides { get; set; }

        public Weapon(int itemTypeID, string name, int price, int damageDieCount, int damageDieSides)
            : base(itemTypeID, name, price)
        {
            DamageDieCount = damageDieCount;
            DamageDieSides = damageDieSides;
        }

        public Weapon Clone() => new Weapon(ItemTypeId, Name, Price, DamageDieCount, DamageDieSides);
    }
}
