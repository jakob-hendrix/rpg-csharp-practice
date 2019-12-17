namespace Engine.Models
{
    public class Weapon : GameItem
    {
        #region Properties

        public int MinDamage { get; }
        public int MaxDamage { get; }

        #endregion

        public Weapon(int itemTypeId, string name, int price, int minDamage, int maxDamage)
            : base(itemTypeId, name, price, true)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }

        public new Weapon Clone() => new Weapon(ItemTypeId, Name, Price, MinDamage, MaxDamage);
    }
}