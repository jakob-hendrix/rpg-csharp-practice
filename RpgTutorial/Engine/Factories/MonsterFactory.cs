using System;
using Engine.Models;

namespace Engine.Factories
{
    public static class MonsterFactory
    {
        public static Monster GetMonster(int monsterId)
        {
            switch (monsterId)
            {
                case 1:
                    var snake = new Monster("Snake", "Snake.png", 4, 4, 5, 1);
                    
                    snake.CurrentWeapon = ItemFactory.CreateGameItem(1501);
                    AddLootItem(snake, 9001, 25);
                    AddLootItem(snake, 9002, 75);

                    return snake;

                case 2:
                    var rat = new Monster("Rat", "Rat.png", 5, 5, 5, 1);
                    
                    rat.CurrentWeapon = ItemFactory.CreateGameItem(1502);
                    AddLootItem(rat, 9003, 25);
                    AddLootItem(rat, 9004, 75);

                    return rat;

                case 3:
                    var spider = new Monster("Spider", "Spider.png", 10, 10, 10, 3);
                    
                    spider.CurrentWeapon = ItemFactory.CreateGameItem(1503);
                    AddLootItem(spider, 9005, 25);
                    AddLootItem(spider, 9006, 75);

                    return spider;

                default:
                    throw new ArgumentException($"Monster type {monsterId} does not exist");
            }
        }

        private static void AddLootItem(Monster monster, int itemId, int percentage)
        {
            if (RandomNumberGenerator.NumberBetween(1,100) <= percentage)
            {
                // TODO: add the code to use the percentage to choose the item Id
                monster.AddItemToInventory(ItemFactory.CreateGameItem(itemId));
            }
        }
    }
}
