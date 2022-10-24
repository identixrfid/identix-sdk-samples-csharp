using Newtonsoft.Json;

namespace Identix.SDK.API.BLE
{
    public class BLEFilter
    {
        [JsonProperty("generic")]
        public bool Generic { get; set; }

        [JsonProperty("ibeacon")]
        public bool Ibeacon { get; set; }

        [JsonProperty("eddystone_uid")]
        public bool EddystoneUid { get; set; }

        [JsonProperty("eddystone_url")]
        public bool EddystoneUrl { get; set; }

        [JsonProperty("eddystone_tml")]
        public bool EddystoneTml { get; set; }

        [JsonProperty("altbeacon")]
        public bool Altbeacon { get; set; }

        [JsonProperty("identix")]
        public bool Identix { get; set; }

        [JsonProperty("identix_accelerometer")]
        public bool IdentixAccelerometer { get; set; }

        [JsonProperty("identix_temperature")]
        public bool IdentixTemperature { get; set; }

        [JsonProperty("rssi")]
        public bool RssiFilterEnable { get; set; }

        [JsonProperty("rssi_threshold")]
        public int RssiFilterValue { get; set; }

        [JsonProperty("regex")]
        public bool Regex { get; set; }

        [JsonProperty("regex_string")]
        public string RegexString { get; set; }
    }
}