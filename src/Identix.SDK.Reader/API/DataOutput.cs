using Newtonsoft.Json;

namespace Identix.SDK.API
{
    public class DataOutput
    {
        public Heartbeat Heartbeat { get; set; }
        public Mqtt Mqtt { get; set; }
        public SocketSettings Socket { get; set; }

        [JsonProperty("http_post")]
        public HttpPost HttpPost { get; set; }
    }
}