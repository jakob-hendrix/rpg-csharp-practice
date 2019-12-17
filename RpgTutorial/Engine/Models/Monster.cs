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

        #endregion

        public Monster(string name, string imageName, int maximumHitPoints, int hitPoints, int rewardExperiencePoints,
            int rewardGold, int minimumDamage, int maximumDamage) : base(name, maximumHitPoints, hitPoints, rewardGold)
        {
            ImageName = $"/Engine;component/Images/Monsters/{imageName}";
            MinimumDamage = minimumDamage;
            MaximumDamage = maximumDamage;
            RewardExperiencePoints = rewardExperiencePoints;
        }
    }
}