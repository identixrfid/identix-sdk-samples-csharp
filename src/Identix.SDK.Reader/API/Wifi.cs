using Newtonsoft.Json;

namespace Identix.SDK.API
{
    public class Wifi
    {
        [JsonProperty("access_point")]
        public AccessPoint AccessPoint { get; set; }

        [JsonProperty("station")]
        public Station Station { get; set; }
    }
}
