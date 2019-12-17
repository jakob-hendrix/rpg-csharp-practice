using System;
using System.Linq;
using Engine.EventArgs;
using Engine.Factories;
using Engine.Models;

namespace Engine.ViewModels
{
    public class GameSession : BaseNotificationClass
    {
        public event EventHandler<GameMessageEventArgs> OnMessageRaised;

        #region Properties

        private Location _currentLocation;
        private Monster _currentMonster;
        private Trader _currentTrader;
        private Player _currentPlayer;

        public World CurrentWorld { get; }

        public Player CurrentPlayer
        {
            get => _currentPlayer;
            set
            {
                if (_currentPlayer != null)
                {
                    _currentPlayer.OnLeveledUp -= OnCurrentPlayerLeveledUp;
                    _currentPlayer.OnKilled -= OnCurrentPlayerKilled;
                }

                _currentPlayer = value;

                if (_currentPlayer != null)
                {
                    _currentPlayer.OnLeveledUp += OnCurrentPlayerLeveledUp;
                    _currentPlayer.OnKilled += OnCurrentPlayerKilled;
                }
            }
        }

        public Location CurrentLocation
        {
            get => _currentLocation;
            set
            {
                _currentLocation = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasLocationToNorth));
                OnPropertyChanged(nameof(HasLocationToEast));
                OnPropertyChanged(nameof(HasLocationToWest));
                OnPropertyChanged(nameof(HasLocationToSouth));

                GivePlayerQuestsAtLocation();
                CompleteQuestAtLocation();
                GetMonsterAtLocation();

                CurrentTrader = CurrentLocation.TraderHere;

                RaiseMessage("");
                RaiseMessage($"You have entered {CurrentLocation.Name}.");
            }
        }

        public Monster CurrentMonster
        {
            get => _currentMonster;
            set
            {
                if (_currentMonster != null)
                {
                    _currentMonster.OnKilled -= OnCurrentMonsterKilled;
                }

                _currentMonster = value;

                if (CurrentMonster != null)
                {
                    _currentMonster.OnKilled += OnCurrentMonsterKilled;

                    RaiseMessage("");
                    RaiseMessage($"You see a {CurrentMonster.Name} here!");
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasMonster));
            }
        }

        public Trader CurrentTrader
        {
            get => _currentTrader;
            set
            {
                _currentTrader = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasTrader));
            }
        }

        public Weapon CurrentWeapon { get; set; }

        public bool HasLocationToNorth =>
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;

        public bool HasLocationToEast =>
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;

        public bool HasLocationToWest =>
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;

        public bool HasLocationToSouth =>
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;

        public bool HasMonster => CurrentMonster != null;
        public bool HasTrader => CurrentTrader != null;

        #endregion

        public GameSession()
        {
            CurrentPlayer = new Player("Rockjaw", "Village Fool", 0, 10, 10, 100000);

            if (!CurrentPlayer.Weapons.Any())
            {
                CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(1001));
            }

            CurrentWorld = WorldFactory.CreateWorld();
            CurrentLocation = CurrentWorld.LocationAt(0, 0);
        }

        public void MoveNorth()
        {
            if (HasLocationToNorth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1);
            }
        }

        public void MoveEast()
        {
            if (HasLocationToEast)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate);
            }
        }

        public void MoveWest()
        {
            if (HasLocationToWest)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate);
            }
        }

        public void MoveSouth()
        {
            if (HasLocationToSouth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1);
            }
        }

        private void CompleteQuestAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                QuestStatus questToComplete =
                    CurrentPlayer.Quests.FirstOrDefault(q => q.PlayerQuest.Id == quest.Id && !q.IsCompleted);

                if (questToComplete != null)
                {
                    if (CurrentPlayer.HasAllTheseItems(quest.ItemsToComplete))
                    {
                        foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
                        {
                            for (int i = 0; i < itemQuantity.Quantity; i++)
                            {
                                CurrentPlayer.RemoveItemsFromInventory(
                                    CurrentPlayer.Inventory.FirstOrDefault(item =>
                                        item.ItemTypeId == itemQuantity.ItemId));
                            }
                        }

                        RaiseMessage("");
                        RaiseMessage($"You completed the quest {quest.Name}!");

                        // give the player the quest rewards
                        RaiseMessage($"You gained {quest.RewardExperiencePoints} XP...");
                        CurrentPlayer.AddExperience(quest.RewardExperiencePoints);

                        RaiseMessage($"You received {quest.RewardGold} gold pieces...");
                        CurrentPlayer.ReceiveGold(quest.RewardGold);

                        foreach (ItemQuantity itemQuantity in quest.RewardItems)
                        {
                            GameItem rewardItem = ItemFactory.CreateGameItem(itemQuantity.ItemId);

                            RaiseMessage($"You receive a {rewardItem.Name}");
                            CurrentPlayer.AddItemToInventory(rewardItem);
                        }

                        questToComplete.IsCompleted = true;
                    }
                }
            }
        }

        private void GivePlayerQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                if (CurrentPlayer.Quests.All(q => q.PlayerQuest.Id != quest.Id))
                {
                    CurrentPlayer.Quests.Add(new QuestStatus(quest));

                    RaiseMessage("");
                    RaiseMessage($"You started the quest {quest.Name}");
                    RaiseMessage(quest.Description);

                    RaiseMessage($"Return with: ");
                    foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
                    {
                        RaiseMessage(
                            $"\t{itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemId).Name}");
                    }

                    RaiseMessage($"And you will receive: ");
                    RaiseMessage($"\t{quest.RewardExperiencePoints} XP");
                    RaiseMessage($"\t{quest.RewardGold} gold");
                    foreach (ItemQuantity itemQuantity in quest.RewardItems)
                    {
                        RaiseMessage(
                            $"\t{itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemId).Name}");
                    }
                }
            }
        }

        private void GetMonsterAtLocation() => CurrentMonster = CurrentLocation.GetMonster();

        public void AttackCurrentMonster()
        {
            if (CurrentWeapon == null)
            {
                RaiseMessage("You must select a weapon before attacking!");
                return;
            }

            // determine damage to monster
            int damageToMonster =
                RandomNumberGenerator.NumberBetween(CurrentWeapon.MinDamage, CurrentWeapon.MaxDamage);

            if (damageToMonster == 0)
            {
                RaiseMessage($"You missed the {CurrentMonster.Name}.");
            }
            else
            {
                RaiseMessage($"You hit the {CurrentMonster.Name} for {damageToMonster} damage.");
                CurrentMonster.TakeDamage(damageToMonster);
            }

            // If the monster is killed, collect rewards etc
            if (CurrentMonster.IsDead)
            {
                GetMonsterAtLocation();
            }
            else
            {
                // If the monster still lives, it attacks!

                int damageToPlayer =
                    RandomNumberGenerator.NumberBetween(CurrentMonster.MinimumDamage, CurrentMonster.MaximumDamage);

                if (damageToPlayer == 0)
                {
                    RaiseMessage($"{CurrentMonster.Name} attacks, but misses.");
                }
                else
                {
                    RaiseMessage($"{CurrentMonster.Name} hits you for {damageToPlayer} damage!");
                    CurrentPlayer.TakeDamage(damageToPlayer);
                }
            }
        }

        private void OnCurrentPlayerKilled(object sender, System.EventArgs e)
        {
            RaiseMessage("");
            RaiseMessage($"You died a horrid death!");

            CurrentLocation = CurrentWorld.LocationAt(0, -1); // player's home
            CurrentPlayer.CompletelyHeal();
        }

        private void OnCurrentMonsterKilled(object sender, System.EventArgs e)
        {
            RaiseMessage("");
            RaiseMessage($"You defeated the {CurrentMonster.Name}!");

            RaiseMessage($"You received {CurrentMonster.RewardExperiencePoints} XP...");
            CurrentPlayer.AddExperience(CurrentMonster.RewardExperiencePoints);

            RaiseMessage($"You picked up {CurrentMonster.Gold} gold...");
            CurrentPlayer.ReceiveGold(CurrentMonster.Gold);

            foreach (GameItem item in CurrentMonster.Inventory)
            {
                RaiseMessage($"You found {item.Name}");
                CurrentPlayer.AddItemToInventory(item);
            }
        }

        private void OnCurrentPlayerLeveledUp(object sender, System.EventArgs e)
        {
            RaiseMessage($"You gained a level! You are now level {CurrentPlayer.Level}.");
        }

        private void RaiseMessage(string message) => OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message));
    }
}