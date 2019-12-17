using System.Collections.ObjectModel;

namespace Engine.Models
{
    public class Monster : LivingEntity
    {
        #region Properties

        public string ImageName { get; set; }
        public int MinimumDamage { get; set; }
        public int MaximumDamage { get; set; }
        public int RewardExperiencePoints { get; }
        public int RewardGold { get; set; }

        #endregion

        public Monster(string name, string imageName, int maximumHitPoints, int hitPoints, int rewardExperiencePoints,
            int rewardGold, int minimumDamage, int maximumDamage)
        {
            Name = name;
            ImageName = $"/Engine;component/Images/Monsters/{imageName}";

            MaximumHitPoints = maximumHitPoints;
            CurrentHitPoints = hitPoints;

            MinimumDamage = minimumDamage;
            MaximumDamage = maximumDamage;

            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;
        }
    }
}