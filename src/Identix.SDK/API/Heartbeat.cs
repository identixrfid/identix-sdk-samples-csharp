using Newtonsoft.Json;

namespace Identix.SDK.API
{
    public class Heartbeat
    {
        [JsonProperty("period_sec")]
        public int PeriodSec { get; set; }

        [JsonProperty("enable_on_mqtt")]
        public bool EnableOnMqtt { get; set; }

        [JsonProperty("enable_on_socket")]
        public bool EnableOnSocket { get; set; }

        [JsonProperty("enable_on_http_post")]
        public bool EnableOnHttpPost { get; set; }
    }
}