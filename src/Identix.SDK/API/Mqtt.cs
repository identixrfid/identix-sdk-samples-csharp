using Newtonsoft.Json;

namespace Identix.SDK.API
{
    public class Mqtt
    {
        public bool Enable { get; set; }
        public bool Connected { get; set; }
        public string Url { get; set; }
        public int Port { get; set; }

        [JsonProperty("enable_tls")]
        public bool EnableTls { get; set; }

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        [JsonProperty("inventory_topic")]
        public string InventoryTopic { get; set; }

        [JsonProperty("ble_topic")]
        public string BleTopic { get; set; }

        [JsonProperty("heartbeat_topic")]
        public string HeartbeatTopic { get; set; }

        [JsonProperty("info_topic")]
        public string InfoTopic { get; set; }

        [JsonProperty("command_topic")]
        public string CommandTopic { get; set; }
    }
}
