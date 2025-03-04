﻿namespace Engine.Models
{
    public class GroupedInventoryItem : BaseNotificationClass
    {
        private GameItem _item;
        private int _quantity;

        public GameItem Item
        {
            get => _item;
            set
            {
                _item = value;
                OnPropertyChanged(nameof(Item));
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public GroupedInventoryItem(GameItem item, int quantity)
        {
            _item = item;
            _quantity = quantity;
        }
    }
}