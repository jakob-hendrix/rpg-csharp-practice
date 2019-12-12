using System.Collections.Generic;
using System.Linq;
using Engine.Models;

namespace Engine.Factories
{
    internal static class QuestFactory
    {
        private static readonly List<Quest> _quests = new List<Quest>();

        static QuestFactory()
        {
            // declare the items needed to complete the quest and it's reward items'
            var itemsToComplete = new List<ItemQuantity>();
            var rewardItems = new List<ItemQuantity>();

            itemsToComplete.Add(new ItemQuantity(9001, 5));
            rewardItems.Add(new ItemQuantity(1002, 1));

            // create the quest
            _quests.Add(
                new Quest(
                    1,
                    "Clear the herb garden!",
                    "Kill all of those horrid snakes in the herbalist\'s garden.",
                    itemsToComplete,
                    25,
                    10,
                    rewardItems
                )
            );
        }

        internal static Quest GetQuestById(int id) => _quests.FirstOrDefault(q => q.Id == id);
    }
}
