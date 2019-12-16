﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Engine.Annotations;
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

        public Player CurrentPlayer { get; set; }
        public World CurrentWorld { get; set; }

        public Location CurrentLocation
        {
            get => _currentLocation;
            set
            {
                _currentLocation = value;
                OnPropertyChanged(nameof(CurrentLocation));
                OnPropertyChanged(nameof(HasLocationToNorth));
                OnPropertyChanged(nameof(HasLocationToEast));
                OnPropertyChanged(nameof(HasLocationToWest));
                OnPropertyChanged(nameof(HasLocationToSouth));

                GivePlayerQuestsAtLocation();
                GetMonsterAtLocation();
            }
        }

        public Monster CurrentMonster
        {
            get => _currentMonster;
            set
            {
                _currentMonster = value;

                OnPropertyChanged(nameof(CurrentMonster));
                OnPropertyChanged(nameof(HasMonster));

                if (CurrentMonster != null)
                {
                    RaiseMessage("");
                    RaiseMessage($"You see a {CurrentMonster.Name} here!");
                }
            }
        }

        public Weapon CurrentWeapon { get; set; }

        public bool HasLocationToNorth => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;
        public bool HasLocationToEast => CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;
        public bool HasLocationToWest => CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;
        public bool HasLocationToSouth => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;
        public bool HasMonster => CurrentMonster != null;
        
        #endregion
        
        public GameSession()
        {
            CurrentPlayer = new Player()
            {
                Name = "Rockjaw",
                Class = "Village Fool",
                Gold = 100000,
                HitPoints = 10,
                ExperiencePoints = 0,
                Level = 1
            };

            CurrentWorld = WorldFactory.CreateWorld();

            CurrentLocation = CurrentWorld.LocationAt(0, 0);

            CurrentPlayer.Inventory.Add(ItemFactory.CreateGameItem(1001));
            CurrentPlayer.Inventory.Add(ItemFactory.CreateGameItem(1002));
            CurrentPlayer.Inventory.Add(ItemFactory.CreateGameItem(1003));
        }

        public void MoveNorth()
        {
            if (HasLocationToNorth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate,CurrentLocation.YCoordinate + 1); 
            }
        }
        public void MoveEast()
        {
            if (HasLocationToEast)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1,CurrentLocation.YCoordinate); 
            }
        }
        public void MoveWest()
        {
            if (HasLocationToWest)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1,CurrentLocation.YCoordinate); 
            }
        }
        public void MoveSouth()
        {
            if (HasLocationToSouth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1); 
            }
        }
        private void GivePlayerQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                if (CurrentPlayer.Quests.All(q => q.PlayerQuest.Id != quest.Id))
                {
                    CurrentPlayer.Quests.Add(new QuestStatus(quest));
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
                CurrentMonster.HitPoints -= damageToMonster;
                RaiseMessage($"You hit the {CurrentMonster.Name} for {damageToMonster} damage.");
            }

            // If the monster is killed, collect rewards etc
            if (CurrentMonster.HitPoints <= 0)
            {
                RaiseMessage("");
                RaiseMessage($"You defeated the {CurrentMonster.Name}!");
                CurrentPlayer.ExperiencePoints += CurrentMonster.RewardExperiencePoints;
                RaiseMessage($"You received {CurrentMonster.RewardExperiencePoints} XP...");
                CurrentPlayer.Gold += CurrentMonster.RewardGold;
                RaiseMessage($"You picked up {CurrentMonster.RewardGold} gold...");

                foreach (ItemQuantity itemQuantity in CurrentMonster.Inventory)
                {
                    GameItem item = ItemFactory.CreateGameItem(itemQuantity.ItemId);
                    CurrentPlayer.AddItemToInventory(item);
                    RaiseMessage($"You found {itemQuantity.Quantity} {item.Name}");
                }

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
                    CurrentPlayer.HitPoints -= damageToPlayer;
                    RaiseMessage($"{CurrentMonster.Name} hits you for {damageToPlayer} damage!");
                }

                // If the player dies, send them home
                if (CurrentPlayer.HitPoints <= 0)
                {
                    RaiseMessage("");
                    RaiseMessage($"The {CurrentMonster.Name} killed you!");

                    CurrentLocation = CurrentWorld.LocationAt(0, -1); // player's home
                    CurrentPlayer.HitPoints = CurrentPlayer.Level * 10;
                }
            }
        }


        private void RaiseMessage(string message) => OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message));

    }
}
