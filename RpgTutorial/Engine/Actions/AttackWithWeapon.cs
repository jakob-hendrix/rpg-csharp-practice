using System;
using Engine.Models;

namespace Engine.Actions
{
    public class AttackWithWeapon : IAction
    {
        private readonly GameItem _weapon;
        private readonly int _minimumDamage;
        private readonly int _maximumDamage;

        public event EventHandler<string> OnActionPerformed;

        public AttackWithWeapon(GameItem weapon, int minimumDamage, int maximumDamage)
        {
            if (weapon.Category != GameItem.ItemCategory.Weapon)
            {
                throw new ArgumentException($"{weapon.Name} is not a weapon");
            }

            if (minimumDamage < 0)
            {
                throw new ArgumentException("Min damage must be zero or greater");
            }

            if (maximumDamage < minimumDamage)
            {
                throw new ArgumentException("Maximum cannot be less than the minimum damage");
            }

            _weapon = weapon;
            _minimumDamage = minimumDamage;
            _maximumDamage = maximumDamage;
        }

        public void Execute(LivingEntity actor, LivingEntity target)
        {
            int damage = RandomNumberGenerator.NumberBetween(_minimumDamage, _maximumDamage);

            if (damage == 0)
            {
                ReportResult($"You missed the {target.Name.ToLower()}.");
            }
            else
            {
                ReportResult($"You hit the {target.Name.ToLower()} for {damage} damage.");
                target.TakeDamage(damage);
            }
        }

        private void ReportResult(string result) => OnActionPerformed?.Invoke(this, result);
    }
}
