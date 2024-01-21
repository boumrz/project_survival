using System;
using System.Collections.Generic;

namespace Inventory
{
    [Serializable]
    public class InventoryData
    {
        public List<InventorySlotData> slots;
    }
}