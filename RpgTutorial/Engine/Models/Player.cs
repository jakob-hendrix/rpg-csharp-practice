using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Engine.Models
{
    public class Player : LivingEntity
    {


        #region Properties

        private string _characterClass;
        private string _class;
        private int _experiencePoints;
        private int _level;

        public string Class
        {
            get => _class;
            set
            {
                _class = value;
                OnPropertyChanged(nameof(Class));
            }
        }

        public int ExperiencePoints
        {
            get => _experiencePoints;
            set
            {
                _experiencePoints = value;
                OnPropertyChanged(nameof(ExperiencePoints));
            }
        }

        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                OnPropertyChanged(nameof(Level));
            }
        }

        public ObservableCollection<QuestStatus> Quests { get; set; }

        #endregion

        public Player(string name, string characterClass, int experiencePoints, int maxHitPoints, int currentHitPoints,
            int gold) : base(name, maxHitPoints, currentHitPoints, gold)
        {
            _characterClass = characterClass;
            _experiencePoints = experiencePoints;
            Quests = new ObservableCollection<QuestStatus>();
        }

        public bool HasAllTheseItems(List<ItemQuantity> items)
        {
            foreach (ItemQuantity item in items)
            {
                if (Inventory.Count(i => i.ItemTypeId == item.ItemId) < item.Quantity)
                {
                    return false;
                }
            }

            return true;
        }
    }
}