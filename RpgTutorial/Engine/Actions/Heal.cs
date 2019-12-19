using System;
using Engine.Models;

namespace Engine.Actions
{
    public class Heal : IAction
    {
        private readonly GameItem _item;
        private readonly int _hitPointsToHeal;

        public event EventHandler<string> OnActionPerformed;

        public Heal(GameItem item, int hitPointsToHeal)
        {
            if (item.Category != GameItem.ItemCategory.Consumable)
            {
                throw new ArgumentException($"{item.Name} is not consumable");
            }
            _item = item;
            _hitPointsToHeal = hitPointsToHeal;
        }

        public void Execute(LivingEntity actor, LivingEntity target)
        {
            string actorName = (actor is Player) ? "You" : $"The {actor.Name.ToLower()}";
            string targetName = (target is Player) ? "yourself" : $"the {target.Name.ToLower()}";

            ReportResults(
                $"{actorName} heal {targetName} for {_hitPointsToHeal} pointP{(_hitPointsToHeal > 1 ? "s" : "")}");
            target.Heal(_hitPointsToHeal);
        }

        private void ReportResults(string result) => 
            OnActionPerformed?.Invoke(this, result);
    }
}
