using Newtonsoft.Json;

namespace Identix.SDK.API
{
    public class InventoryCommand
    {
        [JsonProperty("inventory")]
        public bool Inventory { get; set; }

        [JsonProperty("ble")]
        public bool BLE { get; set; }

        [JsonProperty("just_inventory")]
        public bool JustInventory { get; set; }

        public InventoryCommand()
        {
            this.BLE = false;
            this.JustInventory = true;
        }
    }
}