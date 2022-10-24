using Newtonsoft.Json;

namespace Identix.SDK.API.RFID
{
    public class Radio
    {
        public string Model { get; set; }
        public float Temperature { get; set; }
        public string Version { get; set; }

        [JsonProperty("serial_number")]
        public int SerialNumber { get; set; }
        public int Region { get; set; }

        [JsonProperty("enable_inventory_on_high_vswr")]
        public bool EnableInventoryOnHighVswr { get; set; }
    }
}
