﻿namespace PizzaOrder.Models
{
    public class StorageItem
    {
        public StorageItem(string itemName)
        {
            ItemName = itemName;
        }

        public string ItemName { get; private set; }

        public int ItemAmount { get; set; }
    }
}
