using Newtonsoft.Json;

namespace Identix.SDK.API.RFID
{
    public class Inventory
    {
        public bool Automatic { get; set; }
        
        public bool Running { get; set; }
        
        public InventoryCycle Cycle { get; set; }

        [JsonProperty("smart_buffer")]
        public InventorySmartBuffer SmartBuffer { get; set; }
    }
}
