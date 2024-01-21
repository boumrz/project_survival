using UnityEngine;

namespace Inventory
{
    public struct InventoryEventArgs
    {
        public string itemID { get; }
        public int amount { get; }
        public Vector2Int inventorySlotCoordinates { get; }
        
        public InventoryEventArgs(string itemID, int amount, Vector2Int inventorySlotCoordinates)
        {
            this.itemID = itemID;
            this.amount = amount;
            this.inventorySlotCoordinates = inventorySlotCoordinates;
        }
    }
}