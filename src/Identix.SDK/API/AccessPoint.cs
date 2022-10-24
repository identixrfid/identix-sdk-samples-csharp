using Newtonsoft.Json;

namespace Identix.SDK.API
{
    public class AccessPoint
    {
        public string Ssid { get; set; }
        public string Password { get; set; }
        public bool Hidden { get; set; }
        public int Channel { get; set; }

        [JsonProperty("disable_after_sta_connect")]
        public bool DisableAfterStaConnect { get; set; }

        [JsonProperty("disable_count_sec")]
        public int DisableCountSec { get; set; }
    }
}
