using System;
using System.Linq;
using UnityEngine;

namespace Inventory
{
    public class InventoryServise
    {
        public event Action<InventoryEventArgs> ItemsAdded;
        public event Action<InventoryEventArgs> ItemsRemoved;
        public event Action<string, int> ItemsDropped;

        private readonly InventoryData inventoryData;
        private readonly InventoryConfig config;

        public InventoryServise(InventoryData inventoryData, InventoryConfig config)
        {
            this.inventoryData = inventoryData;
            this.config = config;
        }
    
        public void Add(string itemID, int amount = 1)
        {
            var remainingAmount = amount;

            AddToSlotWithSameItem(itemID, remainingAmount, out remainingAmount);

            if (remainingAmount <= 0)
            {
                return;
            }

            AddToFirstAvailableSlot(itemID, remainingAmount, out remainingAmount);

            if (remainingAmount > 0)
            {
                InvokeDrop(itemID, remainingAmount);
            }
        }

        public void Add(Vector2Int slotCoordinates, string itemID, int amount = 1)
        {
            var rowLength = config.inventorySize.x;
            var slotIndex = slotCoordinates.x + rowLength * slotCoordinates.y;
            var slot = inventoryData.slots[slotIndex];
            var newValue = slot.amount + amount;

            if (slot.IsEmpty())
            {
                slot.itemID = itemID;
            }

            if (newValue > config.inventorySlotCapacity)
            {
                var remainingItems = newValue - config.inventorySlotCapacity;
                var itemsToAddAmount = config.inventorySlotCapacity - slot.amount;
                slot.amount = config.inventorySlotCapacity;
                
                ItemsAdded?.Invoke(new InventoryEventArgs(itemID, itemsToAddAmount, slotCoordinates));
                
                Add(itemID, remainingItems);
            }
            else
            {
                slot.amount = newValue;
                ItemsAdded?.Invoke(new InventoryEventArgs(itemID, amount, slotCoordinates));
            }
        }

        public bool Remove(string itemID, int amount = 1, bool invokeDrop = true)
        {
            if (!Contains(itemID, amount))
            {
                return false;
            }

            var amountToRemove = amount;
            var size = config.inventorySize;
            var rowLength = size.x;

            for (var i = 0; i < size.x; i++)
            {
                for (var j = 0; j < size.y; j++)
                {
                    var slotCoordinates = new Vector2Int(i, j);
                    var slot = inventoryData.slots[slotCoordinates.x + rowLength * slotCoordinates.y];

                    if (slot.itemID != itemID)
                    {
                        continue;
                    }

                    if (amountToRemove > slot.amount)
                    {
                        amountToRemove -= slot.amount;

                        Remove(slotCoordinates, itemID, slot.amount, invokeDrop);
                    }
                    else
                    {
                        Remove(slotCoordinates, itemID, amountToRemove, invokeDrop);

                        return true;
                    }
                }
            }

            return true;
        }
        
        public bool Remove(Vector2Int slotCoordinates, string itemID, int amount = 1, bool invokeDrop = true)
        {
            var size = config.inventorySize;
            var rowlength = size.x;
            var slot = inventoryData.slots[slotCoordinates.x + rowlength * slotCoordinates.y];

            if (slot.IsEmpty() || slot.itemID != itemID || slot.amount < amount)
            {
                return false;
            }

            slot.amount -= amount;

            if (slot.amount == 0)
            {
                slot.Clean();
            }
            
            ItemsRemoved?.Invoke(new InventoryEventArgs(itemID, amount, slotCoordinates));

            if (invokeDrop)
            {
                InvokeDrop(itemID, amount);
            }

            return true;
        }

        public bool Contains(string itemID, int amount = 1)
        {
            var allSlotsWithItem = inventoryData.slots.Where(s => s.itemID == itemID);
            var sumContains = 0;

            foreach (var slot in allSlotsWithItem)
            {
                sumContains += slot.amount;
            }

            return sumContains >= amount;
        }
        
        private void AddToSlotWithSameItem(string itemID, int amount, out int remainingAmount)
        {
            var size = config.inventorySize;
            var rowLength = size.x;
            remainingAmount = amount;

            for (var i = 0; i < size.x; i++)
            {
                for (var j = 0; j < size.y; j++)
                {
                    var coords = new Vector2Int(i, j);
                    var slot = inventoryData.slots[coords.x + rowLength * coords.y];

                    if (slot.IsEmpty())
                    {
                        continue;
                    }

                    if (slot.amount >= slot.amount + remainingAmount)
                    {
                        continue;
                    }

                    if (slot.itemID != itemID)
                    {
                        continue;
                    }

                    var newValue = slot.amount + remainingAmount;

                    if (newValue > config.inventorySlotCapacity)
                    {
                        remainingAmount = newValue - config.inventorySlotCapacity;
                        var itemsToAddAmount = config.inventorySlotCapacity - slot.amount;
                        slot.amount = config.inventorySlotCapacity;
                        
                        ItemsAdded?.Invoke(new InventoryEventArgs(itemID, itemsToAddAmount, coords));
                    }
                    else
                    {
                        slot.amount = newValue;
                        var itemsToAddAmount = remainingAmount;
                        remainingAmount = 0;
                        
                        ItemsAdded?.Invoke(new InventoryEventArgs(itemID, itemsToAddAmount, coords));
                        return;
                    }
                }
            }
        }
        
        private void AddToFirstAvailableSlot(string itemID, int amount, out int remainingAmount)
        {
            var size = config.inventorySize;
            var rowLength = size.x;
            remainingAmount = amount;

            for (var i = 0; i < size.x; i++)
            {
                for (var j = 0; j < size.y; j++)
                {
                    var coords = new Vector2Int(i, j);
                    var slot = inventoryData.slots[coords.x + rowLength * coords.y];

                    if (!slot.IsEmpty())
                    {
                        continue;
                    }

                    slot.itemID = itemID;
                    var newValue = remainingAmount;

                    if (newValue > config.inventorySlotCapacity)
                    {
                        remainingAmount = newValue - config.inventorySlotCapacity;
                        var itemsToAddAmount = config.inventorySlotCapacity;
                        slot.amount = config.inventorySlotCapacity;
                        
                        ItemsAdded?.Invoke(new InventoryEventArgs(itemID, itemsToAddAmount, coords));
                    }
                    else
                    {
                        slot.amount = newValue;
                        var itemsToAddAmount = remainingAmount;
                        remainingAmount = 0;
                        
                        ItemsAdded?.Invoke(new InventoryEventArgs(itemID, itemsToAddAmount, coords));
                        
                        return;
                    }
                }
            }
        }

        private void InvokeDrop(string itemID, int amount = 1)
        {
            ItemsDropped?.Invoke(itemID, amount);
        }
    }
}