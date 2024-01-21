namespace Inventory
{
    public static class InventoryExtentions
    {
        public static bool IsEmpty(this InventorySlotData slot)
        {
            return slot.amount <= 0 || string.IsNullOrEmpty(slot.itemID);
        }

        public static void Clean(this InventorySlotData  slot)
        {
            slot.amount = 0;
            slot.itemID = null;
        }
    }
}