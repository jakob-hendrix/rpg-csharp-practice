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
                "/Engine;component/Images/Locations/Home.png"
            );

            newWorld.AddLocation(
                -1,
                -1,
                "Farmer's House",
                "This is the farmer's hut.'",
                "/Engine;component/Images/Locations/FarmHouse.png"
            );

            newWorld.AddLocation(
                -2,
                -1,
                "Farmer's Field",
                "Rows of corn stretch as far as the eye can see. The wind howls, and from the stalks the sound of crunching emanates.",
                "/Engine;component/Images/Locations/FarmFields.png"
            );

            newWorld.AddLocation(
                -1,
                0,
                "Trading Shop",
                "Ye ole shoppe",
                "/Engine;component/Images/Locations/Trader.png"
            );

            newWorld.AddLocation(
                0,
                0,
                "Town Square",
                "This is where the town gallows stand.",
                "/Engine;component/Images/Locations/TownSquare.png"
            );
            newWorld.AddLocation(
                1,
                0,
                "Town Gate",
                "The gate dangles from the walls by a single rusty hinge.",
                "/Engine;component/Images/Locations/TownGate.png"
            );

            newWorld.AddLocation(
                2,
                0,
                "Spider Forest",
                "What light is not blocked by the tree canopy is captured by the thick layers of gossamer web.",
                "/Engine;component/Images/Locations/SpiderForest.png"
            );
            newWorld.AddLocation(
                0,
                1,
                "Herbalist's Hut",
                "A moldy ramshackle hut, before which an evil looking scarecrow spins in the wind.",
                "/Engine;component/Images/Locations/HerbalistsHut.png"
            );

            newWorld.LocationAt(0,1).QuestsAvailableHere.Add(QuestFactory.GetQuestById(1));

            newWorld.AddLocation(
                0,
                2,
                "Herbalist's Garden",
                "Many strange flowers grow wild here. You hear the hissing of angry snakes.",
                "/Engine;component/Images/Locations/HerbalistsGarden.png"
            );

            return newWorld;
        }
    }
}
