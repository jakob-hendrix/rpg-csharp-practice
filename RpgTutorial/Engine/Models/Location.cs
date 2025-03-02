﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using Engine.Factories;

namespace Engine.Models
{
    public class Location
    {
        #region Properties

        public int XCoordinate { get; }
        public int YCoordinate { get; }
        public string Name { get; }
        public string Description { get; }
        public string ImageName { get; }
        public List<Quest> QuestsAvailableHere { get; } = new List<Quest>();

        public List<MonsterEncounter> MonstersHere { get; } = new List<MonsterEncounter>();
        public Trader TraderHere { get; set; }

        #endregion

        public Location(int xCoordinate, int yCoordinate, string name, string description, string imageName)
        {
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
            Name = name;
            Description = description;
            ImageName = $"/Engine;component/Images/Locations/{imageName}";
        }

        public void AddMonster(int monsterId, int chanceOfEncountering)
        {
            if (MonstersHere.Exists(m => m.MonsterId == monsterId))
            {
                // this monster has already been added to this location.
                // override the chance to see them with a new value
                MonstersHere.First(m => m.MonsterId == monsterId)
                    .ChanceOfEncountering = chanceOfEncountering;
            }
            else
            {
                // monster new to the area
                MonstersHere.Add(new MonsterEncounter(monsterId, chanceOfEncountering));
            }
        }

        public Monster GetMonster()
        {
            if (!MonstersHere.Any())
            {
                return null;
            }

            int totalEncounterChance = MonstersHere.Sum(m => m.ChanceOfEncountering);
            int rng = RandomNumberGenerator.NumberBetween(1, totalEncounterChance);

            int runningTotal = 0;
            foreach (var monsterEncounter in MonstersHere)
            {
                runningTotal += monsterEncounter.ChanceOfEncountering;

                if (rng <= runningTotal)
                {
                    return MonsterFactory.GetMonster(monsterEncounter.MonsterId);
                }
            }

            return MonsterFactory.GetMonster(MonstersHere.Last().MonsterId);
        }
    }
}