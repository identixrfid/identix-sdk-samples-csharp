using Newtonsoft.Json;

namespace Identix.SDK.API.BLE
{
    public class BLE
    {
        [JsonProperty("enable")]
        public bool Enable { get; set; }

        [JsonProperty("automaticc")]
        public bool Automatic { get; set; }

        [JsonProperty("scanning")]
        public bool Scanning { get; set; }

        [JsonProperty("scan_speed")]
        public int ScanSpeed { get; set; }

        [JsonProperty("filter")]
        public BLEFilter Filter { get; set; }
    }
}