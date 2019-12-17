using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting;

namespace Engine.Models
{
    public abstract class LivingEntity : BaseNotificationClass
    {
        #region Properties

        private string _name;
        private int _currentHitPoints;
        private int _maximumHitPoints;
        private int _gold;

        public string Name
        {
            get => _name;
            private set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int CurrentHitPoints
        {
            get => _currentHitPoints;
            private set
            {
                _currentHitPoints = value;
                OnPropertyChanged(nameof(CurrentHitPoints));
            }
        }

        public int MaximumHitPoints
        {
            get => _maximumHitPoints;
            private set
            {
                _maximumHitPoints = value;
                OnPropertyChanged(nameof(MaximumHitPoints));
            }
        }

        public int Gold
        {
            get => _gold;
            private set
            {
                _gold = value;
                OnPropertyChanged(nameof(Gold));
            }
        }

        public ObservableCollection<GameItem> Inventory { get; set; }
        public ObservableCollection<GroupedInventoryItem> GroupedInventory { get; set; }
        public List<GameItem> Weapons => Inventory.Where(i => i is Weapon).ToList();
        public bool IsDead => CurrentHitPoints <= 0;

        #endregion;

        public event EventHandler OnKilled;

        protected LivingEntity(string name, int maximumHitPoints, int currentHitPoints, int gold)
        {
            _name = name;
            _maximumHitPoints = maximumHitPoints;
            _currentHitPoints = currentHitPoints;
            _gold = gold;

            Inventory = new ObservableCollection<GameItem>();
            GroupedInventory = new ObservableCollection<GroupedInventoryItem>();
        }

        public  void TakeDamage(int damageAmount)
        {
            CurrentHitPoints -= damageAmount;
            if (IsDead)
            {
                CurrentHitPoints = 0;
                RaiseOnKilledEvent();
            }
        }

        public void Heal(int healedAmount)
        {
            CurrentHitPoints += healedAmount;

            if (CurrentHitPoints > MaximumHitPoints)
            {
                CurrentHitPoints = MaximumHitPoints;
            }
        }

        public void CompletelyHeal()
        {
            CurrentHitPoints = MaximumHitPoints;
        }

        public void ReceiveGold(int amount) => Gold += amount;

        public void SpendGold(int amount)
        {
            if (amount > Gold)
            {
                throw new ArgumentOutOfRangeException($"{Name} only has {Gold} gold, and cannot spend {amount} gold");
            }

            Gold -= amount;
        }

        public void AddItemToInventory(GameItem item)
        {
            Inventory.Add(item);

            if (item.IsUnique)
            {
                GroupedInventory.Add(new GroupedInventoryItem(item, 1));
            }
            else
            {
                if (!GroupedInventory.Any(gi => gi.Item.ItemTypeId == item.ItemTypeId))
                {
                    GroupedInventory.Add(new GroupedInventoryItem(item, 0));
                }

                GroupedInventory.First(gi => gi.Item.ItemTypeId == item.ItemTypeId).Quantity++;
            }

            OnPropertyChanged(nameof(Weapons));
        }

        public void RemoveItemsFromInventory(GameItem item)
        {
            Inventory.Remove(item);

            GroupedInventoryItem groupedInventoryItemToRemove = GroupedInventory.FirstOrDefault(gi => gi.Item == item);

            if (groupedInventoryItemToRemove != null)
            {
                if (groupedInventoryItemToRemove.Quantity == 1)
                {
                    GroupedInventory.Remove(groupedInventoryItemToRemove);
                }
                else
                {
                    groupedInventoryItemToRemove.Quantity--;
                }
            }

            OnPropertyChanged(nameof(Weapons));
        }
        private void RaiseOnKilledEvent() => OnKilled?.Invoke(this, new System.EventArgs());
    }
}