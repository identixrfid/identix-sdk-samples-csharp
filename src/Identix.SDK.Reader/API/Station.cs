using Newtonsoft.Json;

namespace Identix.SDK.API
{
    public class Station
    {
        [JsonProperty("connected")]
        public bool Connected { get; set; }

        [JsonProperty("ssid")]
        public string Ssid { get; set; }

        [JsonProperty("bssid")]
        public string Bssid { get; set; }

        [JsonProperty("rssi")]
        public int Rssi { get; set; }

        [JsonProperty("ch")]
        public int Ch { get; set; }

        [JsonProperty("disconnect_reason")]
        public int DisconnectReason { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("enable_static")]
        public bool EnableStatic { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("Gateway")]
        public string Gateway { get; set; }

        [JsonProperty("netmask")]
        public string Netmask { get; set; }

        [JsonProperty("primary_dns")]
        public string PrimaryDns { get; set; }

        [JsonProperty("backup_dns")]
        public string BackupDns { get; set; }

        [JsonProperty("security")]
        public Security Security { get; set; }

        [JsonProperty("roaming")]
        public Roaming Roaming { get; set; }
    }
}