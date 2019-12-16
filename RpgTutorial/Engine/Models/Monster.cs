﻿using System;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;

namespace Engine.Models
{
    public class Monster : BaseNotificationClass
    {
        private int _hitPoints;

        public string Name { get; private set; }
        public string ImageName { get; set; }
        public int MaximumHitPoints { get; private set; }
        public int MinimumDamage { get; set; }
        public int MaximumDamage { get; set; }


        public int HitPoints
        {
            get => _hitPoints;
            set
            {
                _hitPoints = value;
                OnPropertyChanged(nameof(HitPoints));
            }
        }

        public int RewardExperiencePoints { get; private set; }
        public int RewardGold { get; set; }

        public ObservableCollection<ItemQuantity> Inventory { get; set; }

        public Monster(string name, string imageName, int maximumHitPoints, int hitPoints, int rewardExperiencePoints, int rewardGold, int minimumDamage, int maximumDamage)
        {
            HitPoints = hitPoints;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;
            MinimumDamage = minimumDamage;
            MaximumDamage = maximumDamage;
            Name = name;
            ImageName = $"/Engine;component/Images/Monsters/{imageName}"; 
            MaximumHitPoints = maximumHitPoints;

            Inventory = new ObservableCollection<ItemQuantity>();
        }

    }
}
