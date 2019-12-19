using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Engine.Models
{
    public abstract class LivingEntity : BaseNotificationClass
    {
        #region Properties

        private string _name;
        private int _currentHitPoints;
        private int _maximumHitPoints;
        private int _gold;
        private int _level;
        private GameItem _currentWeapon;
        private GameItem _currentConsumable;

        public string Name
        {
            get => _name;
            private set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public int CurrentHitPoints
        {
            get => _currentHitPoints;
            private set
            {
                _currentHitPoints = value;
                OnPropertyChanged();
            }
        }

        public int MaximumHitPoints
        {
            get => _maximumHitPoints;
            protected set
            {
                _maximumHitPoints = value;
                OnPropertyChanged();
            }
        }

        public int Level
        {
            get => _level;
            protected set
            {
                _level = value;
                OnPropertyChanged();
            }
        }

        public int Gold
        {
            get => _gold;
            protected set
            {
                _gold = value;
                OnPropertyChanged();
            }
        }

        // Watch for the weapon's actions to raise events
        public GameItem CurrentWeapon
        {
            get => _currentWeapon;
            set
            {
                if (_currentWeapon != null)
                {
                    _currentWeapon.Action.OnActionPerformed -= RaiseActionPerformedEvent;
                }

                _currentWeapon = value;

                if (_currentWeapon != null)
                {
                    _currentWeapon.Action.OnActionPerformed += RaiseActionPerformedEvent;
                }

                OnPropertyChanged();
            }
        }

        public GameItem CurrentConsumable
        {
            get => _currentConsumable;
            set
            {
                if (_currentConsumable != null)
                {
                    _currentConsumable.Action.OnActionPerformed -= RaiseActionPerformedEvent;
                }

                _currentConsumable = value;

                if (_currentConsumable != null)
                {
                    _currentConsumable.Action.OnActionPerformed += RaiseActionPerformedEvent;
                }

                OnPropertyChanged();
            }
        }

        public ObservableCollection<GameItem> Inventory { get; }
        public ObservableCollection<GroupedInventoryItem> GroupedInventory { get; }

        public List<GameItem> Weapons =>
            Inventory.Where(i => i.Category == GameItem.ItemCategory.Weapon).ToList();

        public List<GameItem> Consumables =>
            Inventory.Where(i => i.Category == GameItem.ItemCategory.Consumable).ToList();

        public bool HasConsumable => Consumables.Any();
        public bool IsDead => CurrentHitPoints <= 0;

        #endregion;

        public event EventHandler OnKilled;
        public event EventHandler<string> OnActionPerformed;

        protected LivingEntity(string name, int maximumHitPoints, int currentHitPoints, int gold, int level = 1)
        {
            _name = name;
            _maximumHitPoints = maximumHitPoints;
            _currentHitPoints = currentHitPoints;
            _gold = gold;
            _level = level;

            Inventory = new ObservableCollection<GameItem>();
            GroupedInventory = new ObservableCollection<GroupedInventoryItem>();
        }

        public void UseCurrentWeaponOn(LivingEntity target)
        {
            CurrentWeapon.PerformAction(this, target);
        }

        public void UseCurrentConsumable()
        {
            // currently, we can only use consumables on ourselves
            CurrentConsumable.PerformAction(this,this);
            RemoveItemsFromInventory(CurrentConsumable);
        }

        public void TakeDamage(int damageAmount)
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
            OnPropertyChanged(nameof(Consumables));
            OnPropertyChanged(nameof(HasConsumable));
        }

        public void RemoveItemsFromInventory(GameItem item)
        {
            Inventory.Remove(item);

            GroupedInventoryItem groupedInventoryItemToRemove = item.IsUnique
                ? GroupedInventory.FirstOrDefault(gi => gi.Item == item)
                : GroupedInventory.FirstOrDefault(gi => gi.Item.ItemTypeId == item.ItemTypeId);

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
            OnPropertyChanged(nameof(Consumables));
            OnPropertyChanged(nameof(HasConsumable));
        }

        #region Private functions

        private void RaiseOnKilledEvent() =>
            OnKilled?.Invoke(this, new System.EventArgs());

        private void RaiseActionPerformedEvent(object sender, string result) =>
            OnActionPerformed?.Invoke(this, result);

        #endregion
    }
}