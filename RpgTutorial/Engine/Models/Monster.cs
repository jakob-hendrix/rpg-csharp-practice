using System.Collections.ObjectModel;

namespace Engine.Models
{
    public class Monster : LivingEntity
    {
        #region Properties

        public string ImageName { get; }
        public int RewardExperiencePoints { get; }

        #endregion

        public Monster(string name, string imageName, int maximumHitPoints,
            int hitPoints, int rewardExperiencePoints, int rewardGold)
            : base(name, maximumHitPoints, hitPoints, rewardGold)
        {
            ImageName = $"/Engine;component/Images/Monsters/{imageName}";
            RewardExperiencePoints = rewardExperiencePoints;
        }
    }
}