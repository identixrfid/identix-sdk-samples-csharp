using Newtonsoft.Json;

namespace Identix.SDK.API
{
    public class Station
    {
        public bool Connected { get; set; }
        public string Ssid { get; set; }
        public string Bssid { get; set; }
        public int Rssi { get; set; }
        public int Ch { get; set; }

        [JsonProperty("disconnect_reason")]
        public int DisconnectReason { get; set; }

        public string Password { get; set; }

        [JsonProperty("enable_static")]
        public bool EnableStatic { get; set; }

        public string Ip { get; set; }
        public string Gateway { get; set; }
        public string Netmask { get; set; }

        [JsonProperty("primary_dns")]
        public string PrimaryDns { get; set; }

        [JsonProperty("backup_dns")]
        public string BackupDns { get; set; }

        public Security Security { get; set; }
        public Roaming Roaming { get; set; }
    }
}