using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    internal static class WorldFactory
    {
        // Internal classes may only be used inside the same project. Only GameSession should be using me
        internal static World CreateWorld()
        {
            World newWorld = new World();

            newWorld.AddLocation(
                0,
                -1,
                "Home",
                "Before you is a ramshackle hut with mold growing along the walls from the steady misting rain.",
                "Home.png"
            );

            newWorld.AddLocation(
                -1,
                -1,
                "Farmer's House",
                "This is the farmer's hut.'",
                "FarmHouse.png"
            );

            newWorld.AddLocation(
                -2,
                -1,
                "Farmer's Field",
                "Rows of corn stretch as far as the eye can see. The wind howls, and from the stalks the sound of crunching emanates.",
                "FarmFields.png"
            );

            newWorld.LocationAt(-2,-1).AddMonster(2,100);

            newWorld.AddLocation(
                -1,
                0,
                "Trading Shop",
                "Ye ole shoppe",
                "Trader.png"
            );

            newWorld.AddLocation(
                0,
                0,
                "Town Square",
                "This is where the town gallows stand.",
                "TownSquare.png"
            );
            newWorld.AddLocation(
                1,
                0,
                "Town Gate",
                "The gate dangles from the walls by a single rusty hinge.",
                "TownGate.png"
            );

            newWorld.AddLocation(
                2,
                0,
                "Spider Forest",
                "What light is not blocked by the tree canopy is captured by the thick layers of gossamer web.",
                "SpiderForest.png"
            );

            newWorld.LocationAt(2, 0).AddMonster(3, 100);

            newWorld.AddLocation(
                0,
                1,
                "Herbalist's Hut",
                "A moldy ramshackle hut, before which an evil looking scarecrow spins in the wind.",
                "HerbalistsHut.png"
            );

            newWorld.LocationAt(0,1).QuestsAvailableHere.Add(QuestFactory.GetQuestById(1));

            newWorld.AddLocation(
                0,
                2,
                "Herbalist's Garden",
                "Many strange flowers grow wild here. You hear the hissing of angry snakes.",
                "HerbalistsGarden.png"
            );

            newWorld.LocationAt(0, 2).AddMonster(1, 100);

            return newWorld;
        }
    }
}
